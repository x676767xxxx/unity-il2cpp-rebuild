#!/usr/bin/env python3
"""Bounded scan for resource endpoint clues in APK and Windows ZIP files."""
import json, pathlib, re, sys, zipfile

SUFFIXES = (".dex", ".so", ".dll", ".exe", ".json", ".xml", ".config", ".txt", ".bytes", ".manifest")
KEYS = (b"AddrLoadUrl", b"ScenarioResourceManager", b"Addressables", b"assetbundle", b"resourceurl", b"resource_url", b"cdn")
URL = re.compile(rb"https?://[^\x00-\x20\"'<>]{4,300}", re.I)
DOMAIN = re.compile(rb"(?<![A-Za-z0-9_])(?:[A-Za-z0-9-]+\.)+[A-Za-z]{2,}(?::[0-9]{2,5})?", re.I)

def scan(path, label):
    result = {"archive": label, "files_scanned": 0, "large_files_skipped": 0, "url_candidates": [], "keyword_hits": []}
    with zipfile.ZipFile(path) as z:
        candidates = [x for x in z.infolist() if x.filename.lower().endswith(SUFFIXES) and x.file_size]
        print(label, "candidate_files", len(candidates), flush=True)
        for info in candidates:
            if info.file_size > 32 * 1024 * 1024:
                result["large_files_skipped"] += 1
                continue
            try:
                data = z.read(info)
            except Exception:
                continue
            result["files_scanned"] += 1
            for rx in (URL, DOMAIN):
                for m in rx.finditer(data):
                    value = m.group(0).decode("utf-8", "replace").strip(".,);]")
                    row = {"file": info.filename, "value": value}
                    if row not in result["url_candidates"]:
                        result["url_candidates"].append(row)
            low = data.lower()
            for key in KEYS:
                pos = low.find(key.lower())
                if pos >= 0:
                    snippet = data[max(0,pos-100):min(len(data),pos+len(key)+160)]
                    value = re.sub(rb"[^\x20-\x7e]", b" ", snippet).decode("ascii","ignore").strip()
                    result["keyword_hits"].append({"file": info.filename, "keyword": key.decode(), "context": value})
            if result["files_scanned"] % 30 == 0:
                print(label, "scanned", result["files_scanned"], "of", len(candidates), flush=True)
    return result

def main():
    apk, win, out = map(pathlib.Path, sys.argv[1:4])
    data = {"note":"Static leads only; generic asset blobs and files above 32 MiB excluded.", "inputs":[scan(apk,"android-apk"),scan(win,"windows-1.0.1")]}
    out.parent.mkdir(parents=True,exist_ok=True)
    out.write_text(json.dumps(data,indent=2,ensure_ascii=False),encoding="utf-8")
    lines=["# Bounded endpoint string scan",""]
    for item in data["inputs"]:
        lines += ["## "+item["archive"], "- Scanned: "+str(item["files_scanned"]), "- Large skipped: "+str(item["large_files_skipped"]), "- URL/domain candidates:"]
        lines += ["  - "+x["file"]+": "+x["value"] for x in item["url_candidates"]] or ["  - None"]
        lines += ["- Keyword hits:"]
        lines += ["  - "+x["file"]+" ["+x["keyword"]+"]: "+x["context"] for x in item["keyword_hits"]] or ["  - None"]
    out.with_name("endpoint-string-scan.md").write_text("\n".join(lines),encoding="utf-8")
    print("SCAN COMPLETE",flush=True)

if __name__ == "__main__":
    main()
