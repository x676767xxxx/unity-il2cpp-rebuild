#!/usr/bin/env python3
"""Match legacy iOS Addressables entries to archive bundle filenames."""
import json, pathlib, sys, zipfile, hashlib, tempfile, re

map_path=pathlib.Path(sys.argv[1])
chunks=[pathlib.Path(x) for x in sys.argv[2:6]]
out=pathlib.Path(sys.argv[6])
out.parent.mkdir(parents=True,exist_ok=True)
missing=[str(p) for p in chunks if not p.is_file()]
if missing:
    raise SystemExit("Missing archive chunks: "+", ".join(missing))
archive=pathlib.Path(tempfile.gettempdir())/"IOS_202408202213.6.reassembled.zip"
h=hashlib.sha256()
with archive.open("wb") as dst:
    for p in chunks:
        with p.open("rb") as src:
            while True:
                b=src.read(8*1024*1024)
                if not b: break
                dst.write(b); h.update(b)
data=json.loads(map_path.read_text(encoding="utf-8"))
ios={x["bundle_name"].lower():x for x in data["ios"]}
with zipfile.ZipFile(archive) as z:
    infos=[i for i in z.infolist() if not i.is_dir()]
    bundles=[i for i in infos if i.filename.lower().endswith(".bundle")]
    byname={pathlib.PurePosixPath(i.filename).name.lower():i for i in bundles}
    matches=[]; missing_names=[]; size_mismatches=[]
    for _,row in sorted(ios.items()):
        candidates=[]
        pk=row.get("primary_key")
        if isinstance(pk,str) and pk:
            base=pathlib.PurePosixPath(pk).name.lower()
            candidates.append((base,"primary_key_basename"))
            # Addressables key appends a 32-hex content hash before .bundle;
            # legacy archive names commonly retain only the logical filename.
            stripped=re.sub(r"_[0-9a-f]{32}(\.bundle)$",r"\1",base)
            if stripped!=base:
                candidates.append((stripped,"primary_key_without_hash"))
        iid=row.get("internal_id")
        if isinstance(iid,str) and iid:
            # Internal IDs may include a runtime URL prefix; only the path basename is relevant.
            base=pathlib.PurePosixPath(iid.split("}")[-1]).name.lower()
            if base.endswith(".bundle"):
                candidates.append((base,"internal_id_basename"))
        key=row.get("key")
        if isinstance(key,str) and key.lower().endswith(".bundle"):
            candidates.append((pathlib.PurePosixPath(key).name.lower(),"key_basename"))
        found=None; method=None
        for candidate,source in candidates:
            if candidate in byname:
                found=byname[candidate]; method=source; break
        if found is None:
            missing_names.append({"bundle_name":row["bundle_name"],"hash":row.get("hash"),"primary_key":pk,"internal_id":iid,"key":key,"candidate_filenames":[x[0] for x in candidates]})
        else:
            rec={"bundle_name":row["bundle_name"],"archive_path":found.filename,"match_method":method,"archive_bytes":found.file_size,"catalog_bytes":row.get("size"),"catalog_hash":row.get("hash")}
            matches.append(rec)
            if row.get("size") is not None and found.file_size!=row["size"]:
                size_mismatches.append(rec)
    result={
      "source":"legacy iOS 0.7.9.2 resource archive; not Android 1.0",
      "archive_bytes":sum(p.stat().st_size for p in chunks),
      "archive_sha256":h.hexdigest(),"zip_entries":len(infos),
      "archive_bundle_files":len(bundles),"catalog_ios_bundle_entries":len(ios),
      "matched_by_normalized_addressables_filename":len(matches),
      "missing_from_archive":len(missing_names),
      "matched_size_mismatches":len(size_mismatches),
      "archive_bundle_filename_samples":[pathlib.PurePosixPath(i.filename).name for i in bundles[:200]],
      "catalog_candidate_filename_samples":[{"primary_key":x.get("primary_key"),"internal_id":x.get("internal_id"),"key":x.get("key"),"bundle_name":x.get("bundle_name"),"hash":x.get("hash")} for x in data["ios"][:100]],
      "missing_samples":missing_names[:200],"size_mismatch_samples":size_mismatches[:200],
      "match_method_counts":{k:sum(1 for m in matches if m["match_method"]==k) for k in sorted({m["match_method"] for m in matches})},
      "matches":matches
    }
    out.write_text(json.dumps(result,indent=2,ensure_ascii=False),encoding="utf-8")
    print(json.dumps({k:v for k,v in result.items() if k not in ("matches","missing_samples","size_mismatch_samples")},ensure_ascii=False))
