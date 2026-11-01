#!/usr/bin/env python3
"""Targeted IL2CPP metadata/native binary string scan for AzureArchive."""
import json, re, sys, zipfile
from pathlib import PurePosixPath

MAX_BYTES = 256 * 1024 * 1024
TARGET_NAMES = {"global-metadata.dat", "gameassembly.dll", "libil2cpp.so", "libmain.so", "assembly-csharp.dll"}
TOKENS = [b"ScenarioResourceManager", b"AddrLoadUrl", b"resourceurl", b"resource_url",
          b"Addressables", b"AssetBundle", b"catalog_", b"cdn", b"https://", b"http://"]
URL_RE = re.compile(rb"https?://[^\x00-\x20\"'<>]{5,240}", re.I)
STR_RE = re.compile(rb"[\x20-\x7e]{6,}")

def scan_archive(path, label):
    results = {"archive": label, "targets": [], "large_targets_skipped": []}
    with zipfile.ZipFile(path) as z:
        candidates = []
        for info in z.infolist():
            base = PurePosixPath(info.filename).name.lower()
            if base in TARGET_NAMES:
                candidates.append(info)
        print(f"{label} targeted_candidates {len(candidates)}", flush=True)
        for info in candidates:
            if info.file_size > MAX_BYTES:
                results["large_targets_skipped"].append({"file": info.filename, "bytes": info.file_size})
                continue
            data = z.read(info)
            strings = [m.group().decode("utf-8", "replace") for m in STR_RE.finditer(data)]
            hits = []
            seen = set()
            for value in strings:
                low = value.lower()
                if any(t.decode().lower() in low for t in TOKENS):
                    key = value[:500]
                    if key not in seen:
                        seen.add(key); hits.append(key)
            urls = []
            for m in URL_RE.finditer(data):
                v = m.group().decode("utf-8", "replace")
                if v not in urls: urls.append(v)
            results["targets"].append({
                "file": info.filename, "bytes": info.file_size,
                "token_string_hits": hits[:1000], "url_candidates": urls[:500],
                "contains_addrloadurl": b"AddrLoadUrl" in data,
                "contains_scenarioresourcemanager": b"ScenarioResourceManager" in data
            })
            print(f"{label} scanned {info.filename} ({info.file_size} bytes), token_hits={len(hits)}, urls={len(urls)}", flush=True)
    return results

def main():
    if len(sys.argv) != 4:
        raise SystemExit("usage: scan-il2cpp-runtime-strings.py APK WINDOWS_ZIP OUTPUT_JSON")
    out = {"note": "Static strings only; candidates require code-reference validation.",
           "inputs": [scan_archive(sys.argv[1], "android-apk"),
                      scan_archive(sys.argv[2], "windows-1.0.1")]}
    with open(sys.argv[3], "w", encoding="utf-8") as f:
        json.dump(out, f, ensure_ascii=False, indent=2)
    print("TARGETED IL2CPP SCAN COMPLETE", flush=True)

if __name__ == "__main__":
    main()
