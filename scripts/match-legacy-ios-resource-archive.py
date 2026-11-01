#!/usr/bin/env python3
"""Match the legacy iOS resource archive's actual bundle files to parsed Addressables metadata."""
import json, pathlib, sys, zipfile, hashlib, tempfile, os
map_path=pathlib.Path(sys.argv[1]); chunks=[pathlib.Path(x) for x in sys.argv[2:6]]; out=pathlib.Path(sys.argv[6])
out.parent.mkdir(parents=True,exist_ok=True)
missing=[str(p) for p in chunks if not p.is_file()]
if missing: raise SystemExit("Missing archive chunks: "+", ".join(missing))
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
    for name,row in sorted(ios.items()):
        # AddressablesTools exposes the bundle's internal filename stem in bundle_name.
        candidates=[name, name+".bundle"]
        found=next((byname[x] for x in candidates if x in byname),None)
        if found is None:
            missing_names.append({"bundle_name":row["bundle_name"],"hash":row.get("hash"),"primary_key":row.get("primary_key")})
        else:
            rec={"bundle_name":row["bundle_name"],"archive_path":found.filename,"archive_bytes":found.file_size,"catalog_bytes":row.get("size"),"catalog_hash":row.get("hash")}
            matches.append(rec)
            if row.get("size") is not None and found.file_size!=row["size"]: size_mismatches.append(rec)
    result={"source":"legacy iOS 0.7.9.2 resource archive; not Android 1.0","archive_bytes":sum(p.stat().st_size for p in chunks),"archive_sha256":h.hexdigest(),"zip_entries":len(infos),"archive_bundle_files":len(bundles),"catalog_ios_bundle_entries":len(ios),"matched_by_bundle_filename":len(matches),"missing_from_archive":len(missing_names),"matched_size_mismatches":len(size_mismatches),"missing_samples":missing_names[:200],"size_mismatch_samples":size_mismatches[:200],"matches":matches}
out.write_text(json.dumps(result,indent=2,ensure_ascii=False),encoding="utf-8")
print(json.dumps({k:v for k,v in result.items() if k not in ("matches","missing_samples","size_mismatch_samples")},ensure_ascii=False))
