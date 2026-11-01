#!/usr/bin/env python3
"""Decode Unity Addressables JSON catalogs and export bundle metadata."""
import json, pathlib, sys, zipfile
from AddressablesTools import parse_json

def catalog_from_archive(path, platform):
    with zipfile.ZipFile(path) as z:
        matches=[n for n in z.namelist() if n.lower().endswith("/catalog.json") or n.lower()=="catalog.json"]
        if platform=="android":
            matches=[n for n in matches if "/assets/aa/" in "/"+n.lower()]
        else:
            matches=[n for n in matches if "/data/raw/aa/" in "/"+n.lower()]
        if not matches: raise RuntimeError(f"{platform}: catalog.json not found")
        return parse_json(z.read(matches[0]).decode("utf-8-sig")), matches[0]

def extract(catalog, platform):
    rows={}
    for key, locations in catalog.Resources.items():
        for loc in locations:
            wrapped=getattr(loc, "Data", None)
            obj=getattr(wrapped, "Object", None)
            if obj is None: continue
            bundle_name=getattr(obj, "BundleName", None)
            bundle_hash=getattr(obj, "Hash", None)
            if not bundle_name and not (isinstance(key,str) and key.lower().endswith(".bundle")): continue
            name=bundle_name or str(key)
            rows[name.lower()] = {
                "platform":platform, "key":str(key), "bundle_name":name,
                "hash":bundle_hash, "crc":getattr(obj,"Crc",None),
                "size":getattr(obj,"BundleSize",None),
                "internal_id":getattr(loc,"InternalId",None),
                "provider_id":getattr(loc,"ProviderId",None),
                "primary_key":getattr(loc,"PrimaryKey",None)
            }
    return rows

def main():
    ios_path, android_path, out_path=sys.argv[1:4]
    ios, ipath=catalog_from_archive(ios_path,"ios")
    android, apath=catalog_from_archive(android_path,"android")
    a=extract(ios,"ios"); b=extract(android,"android")
    common=set(a)&set(b)
    same=sum(1 for k in common if (a[k]["hash"] or "").lower()==(b[k]["hash"] or "").lower())
    changed=len(common)-same
    report={
      "parser":"AddressablesToolsPy (read-only catalog parser)",
      "sources":{"ios":ipath,"android":apath},
      "counts":{"ios_bundle_entries":len(a),"android_bundle_entries":len(b),
        "common_names":len(common),"ios_only":len(set(a)-set(b)),
        "android_only":len(set(b)-set(a)),"same_hash":same,"different_hash":changed},
      "ios":sorted(a.values(),key=lambda x:x["bundle_name"].lower()),
      "android":sorted(b.values(),key=lambda x:x["bundle_name"].lower()),
      "comparison":[{"bundle_name":a[k]["bundle_name"],"ios":a[k],"android":b[k],
        "hash_equal":(a[k]["hash"] or "").lower()==(b[k]["hash"] or "").lower()}
        for k in sorted(common)]
    }
    pathlib.Path(out_path).write_text(json.dumps(report,indent=2,ensure_ascii=False),encoding="utf-8")
    print(json.dumps(report["counts"],ensure_ascii=False))
if __name__=="__main__": main()
