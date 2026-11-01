#!/usr/bin/env python3
"""Decode Unity Addressables JSON v1 packed tables and map AssetBundle options."""
import base64
import json
import pathlib
import struct
import sys
import zipfile

def i32(buf, pos):
    if pos < 0 or pos + 4 > len(buf):
        raise ValueError(f"int32 out of range at {pos}/{len(buf)}")
    return struct.unpack_from("<i", buf, pos)[0]

def read_u8_string(buf, pos):
    if pos >= len(buf): raise ValueError("string length byte out of range")
    n=buf[pos]; pos+=1
    end=pos+n
    if end>len(buf): raise ValueError("string payload out of range")
    return buf[pos:end].decode("ascii", errors="replace"), end

def read_key(buf, offset):
    if offset < 4 or offset >= len(buf): raise ValueError(f"key offset out of range: {offset}")
    kind=buf[offset]; p=offset+1
    if kind in (0,1):
        n=i32(buf,p); p+=4
        if n<0 or p+n>len(buf): raise ValueError("invalid key string length")
        raw=buf[p:p+n]
        return raw.decode("ascii" if kind==0 else "utf-16le", errors="replace")
    if kind==2:
        return struct.unpack_from("<H",buf,p)[0]
    if kind==3:
        return struct.unpack_from("<I",buf,p)[0]
    if kind==4:
        return i32(buf,p)
    if kind==5:
        return read_u8_string(buf,p)[0]
    if kind==6:
        return {"serialized_type":read_u8_string(buf,p)[0]}
    if kind==7:
        assembly,p=read_u8_string(buf,p)
        class_name,p=read_u8_string(buf,p)
        n=i32(buf,p); p+=4
        if n<0 or p+n>len(buf): raise ValueError("invalid JSON object length")
        raw=buf[p:p+n]
        try: value=json.loads(raw.decode("utf-16le"))
        except Exception: value={"_raw_utf16":raw.decode("utf-16le",errors="replace")}
        return {"_type":class_name,"_assembly":assembly,**(value if isinstance(value,dict) else {"value":value})}
    raise ValueError(f"unsupported serialized key type byte {kind}")

def read_object(buf, offset):
    if offset < 0: return None
    if offset >= len(buf): raise ValueError(f"extra data offset out of range: {offset}")
    kind=buf[offset]; p=offset+1
    if kind==7:
        assembly,p=read_u8_string(buf,p)
        class_name,p=read_u8_string(buf,p)
        n=i32(buf,p); p+=4
        if n<0 or p+n>len(buf): raise ValueError("invalid extra JSON length")
        raw=buf[p:p+n]
        value=json.loads(raw.decode("utf-16le"))
        if isinstance(value,dict):
            value["_serialized_type"]=class_name
            value["_serialized_assembly"]=assembly
        return value
    if kind==0:
        n=i32(buf,p); p+=4
        return buf[p:p+n].decode("ascii",errors="replace")
    if kind==1:
        n=i32(buf,p); p+=4
        return buf[p:p+n].decode("utf-16le",errors="replace")
    if kind==4: return i32(buf,p)
    if kind==5: return read_u8_string(buf,p)[0]
    return {"_unsupported_type":kind}

def expand_internal_id(value, prefixes):
    if prefixes and "#" in value:
        left,right=value.split("#",1)
        if left.isdigit() and int(left)<len(prefixes):
            return prefixes[int(left)]+right
    return value

def read_catalog(archive, platform):
    with zipfile.ZipFile(archive) as z:
        names=[n for n in z.namelist() if n.lower().endswith("/catalog.json") or n.lower()=="catalog.json"]
        if platform=="android":
            names=[n for n in names if "/assets/aa/" in "/"+n.lower()]
        else:
            names=[n for n in names if "/data/raw/aa/" in "/"+n.lower()]
        if not names: raise RuntimeError(f"{platform}: expected catalog.json not found")
        name=names[0]
        return json.loads(z.read(name).decode("utf-8-sig")),name

def decode(catalog, platform):
    keys_buf=base64.b64decode(catalog["m_KeyDataString"])
    buckets_buf=base64.b64decode(catalog["m_BucketDataString"])
    entries_buf=base64.b64decode(catalog["m_EntryDataString"])
    extra_buf=base64.b64decode(catalog["m_ExtraDataString"])
    key_count=i32(keys_buf,0)
    bucket_count=i32(buckets_buf,0)
    if key_count!=bucket_count:
        raise ValueError(f"{platform}: key/bucket count mismatch {key_count}/{bucket_count}")
    keys=[]
    bucket_rows=[]
    pos=4
    for _ in range(bucket_count):
        key_offset=i32(buckets_buf,pos); entry_count=i32(buckets_buf,pos+4); pos+=8
        if entry_count<0 or entry_count>len(entries_buf)//4:
            raise ValueError("invalid bucket entry count")
        ids=[]
        for _ in range(entry_count):
            ids.append(i32(buckets_buf,pos)); pos+=4
        keys.append(read_key(keys_buf,key_offset))
        bucket_rows.append(ids)
    entry_count=i32(entries_buf,0)
    if entry_count<0 or 4+entry_count*28>len(entries_buf):
        raise ValueError(f"{platform}: invalid entry table count {entry_count}")
    internal_ids=catalog["m_InternalIds"]
    provider_ids=catalog["m_ProviderIds"]
    resource_types=catalog["m_resourceTypes"]
    prefixes=catalog.get("m_InternalIdPrefixes") or []
    legacy_keys=catalog.get("m_Keys")
    locations=[]
    for n in range(entry_count):
        vals=struct.unpack_from("<7i",entries_buf,4+n*28)
        iid,pidx,depkey,dephash,dataidx,primaryidx,typeidx=vals
        if not (0<=iid<len(internal_ids) and 0<=pidx<len(provider_ids)):
            raise ValueError(f"{platform}: invalid entry index at {n}: {vals}")
        internal_id=expand_internal_id(internal_ids[iid],prefixes)
        data=read_object(extra_buf,dataidx)
        primary=(legacy_keys[primaryidx] if legacy_keys is not None else keys[primaryidx])
        locations.append({"entry_index":n,"internal_id":internal_id,"provider_id":provider_ids[pidx],
                          "dependency_key_index":depkey,"dependency_hash":dephash,
                          "data_index":dataidx,"primary_key":primary,"resource_type_index":typeidx,
                          "resource_type":resource_types[typeidx] if 0<=typeidx<len(resource_types) else None,
                          "data":data})
    rows={}
    bundle_rows=0
    for key,entry_ids in zip(keys,bucket_rows):
        for idx in entry_ids:
            if not 0<=idx<len(locations): raise ValueError(f"{platform}: bucket references invalid entry {idx}")
            loc=locations[idx]; data=loc["data"] or {}
            name=data.get("m_BundleName") if isinstance(data,dict) else None
            provider=loc["provider_id"]
            if not name and "AssetBundleProvider" not in provider: continue
            if not name:
                import re
                m=re.search(r"([^/]+\\.bundle)(?:[?#].*)?$",loc["internal_id"],re.I)
                if m: name=m.group(1)
            if not name: continue
            bundle_rows+=1
            row={"platform":platform,"bundle_name":name,"hash":data.get("m_Hash"),
                 "crc":data.get("m_Crc"),"size":data.get("m_BundleSize"),
                 "internal_id":loc["internal_id"],"provider_id":provider,
                 "primary_key":loc["primary_key"],"catalog_key":key,
                 "entry_index":idx,"data":data}
            rows[name.lower()]=row
    return rows,{"key_count":key_count,"bucket_count":bucket_count,"entry_count":entry_count,
                 "location_count":len(locations),"bundle_location_count":bundle_rows,
                 "keys_with_locations":sum(bool(x) for x in bucket_rows)}

def main():
    ios_path,android_path,out_path=sys.argv[1:4]
    ios,ipath=read_catalog(ios_path,"ios"); android,apath=read_catalog(android_path,"android")
    a,astats=decode(ios,"ios"); b,bstats=decode(android,"android")
    common=set(a)&set(b)
    same=sum((a[k]["hash"] or "").lower()==(b[k]["hash"] or "").lower() for k in common)
    report={"parser":"native Python Addressables JSON v1 packed-table decoder",
      "sources":{"ios":ipath,"android":apath},"decode_stats":{"ios":astats,"android":bstats},
      "counts":{"ios_bundle_entries":len(a),"android_bundle_entries":len(b),
        "common_names":len(common),"ios_only":len(set(a)-set(b)),
        "android_only":len(set(b)-set(a)),"same_hash":same,"different_hash":len(common)-same},
      "ios":sorted(a.values(),key=lambda x:x["bundle_name"].lower()),
      "android":sorted(b.values(),key=lambda x:x["bundle_name"].lower()),
      "comparison":[{"bundle_name":a[k]["bundle_name"],"ios":a[k],"android":b[k],
        "hash_equal":(a[k]["hash"] or "").lower()==(b[k]["hash"] or "").lower()} for k in sorted(common)]}
    pathlib.Path(out_path).write_text(json.dumps(report,indent=2,ensure_ascii=False),encoding="utf-8")
    print(json.dumps({"counts":report["counts"],"decode_stats":report["decode_stats"]},ensure_ascii=False))
if __name__=="__main__": main()
