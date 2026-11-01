#!/usr/bin/env python3
"""Generate consistent CSV manifests and Markdown summary from decoded bundle map JSON."""
import csv
import json
import pathlib
import sys

src=pathlib.Path(sys.argv[1])
out=pathlib.Path(sys.argv[2])
out.mkdir(parents=True,exist_ok=True)
data=json.loads(src.read_text(encoding="utf-8"))
rows=data["comparison"]

fields=["bundle_name","logical_path","ios_hash","android_hash","ios_size","android_size","hash_equal","size_equal"]
same=[]; changed=[]
for r in rows:
    i=r["ios"]; a=r["android"]
    logical=i.get("internal_id","").split("}/",1)[-1]
    item={
        "bundle_name":r["bundle_name"],
        "logical_path":logical,
        "ios_hash":i.get("hash") or "",
        "android_hash":a.get("hash") or "",
        "ios_size":i.get("size"),
        "android_size":a.get("size"),
        "hash_equal":bool(r["hash_equal"]),
        "size_equal":i.get("size")==a.get("size"),
    }
    (same if item["hash_equal"] else changed).append(item)

def write_csv(name,items):
    with (out/name).open("w",newline="",encoding="utf-8-sig") as f:
        w=csv.DictWriter(f,fieldnames=fields); w.writeheader(); w.writerows(items)

write_csv("same-hash-bundles.csv",same)
write_csv("changed-hash-bundles.csv",changed)
write_csv("all-bundle-comparison.csv",same+changed)
counts=data["counts"]
md=[
"# iOS 0.7.9.2 vs Android 1.0 decoded Addressables bundle comparison","",
"Generated from the native packed-table decoder output. This report supersedes older pre-decoder hash-scan notes.","",
f"- iOS Bundle rows: {counts['ios_bundle_entries']}",
f"- Android Bundle rows: {counts['android_bundle_entries']}",
f"- Common logical names: {counts['common_names']}",
f"- iOS-only / Android-only: {counts['ios_only']} / {counts['android_only']}",
f"- Same Addressables hash: {counts['same_hash']}",
f"- Different Addressables hash: {counts['different_hash']}","",
"## Interpretation","",
"Equal Addressables hashes are strong catalog evidence for the same bundle version, but are not a byte-for-byte comparison of Android and iOS payloads and do not prove runtime compatibility. Different hashes must not be substituted without obtaining and validating the corresponding payloads.","",
"## Manifests","",
"- `same-hash-bundles.csv`: same catalog hash; candidate legacy-resource reuse only.",
"- `changed-hash-bundles.csv`: different catalog hashes; replacement/update required for exact Android 1.0 content.",
"- `all-bundle-comparison.csv`: complete joined inventory.","",
"## Next requirements","",
"The Android APK supplies the catalog but not the complete remote bundle payload set. Obtain the Android 1.0 Addressables payloads and validate actual bytes, Unity bundle format, and platform-specific content before reuse. A full Unity project plus macOS/Xcode remains necessary for a standard iOS player build."
]
(out/"ios-0792-vs-android-1.0-catalog-diff.md").write_text("\n".join(md)+"\n",encoding="utf-8")
print(json.dumps({"same_hash":len(same),"different_hash":len(changed),"output":str(out)},ensure_ascii=False))
