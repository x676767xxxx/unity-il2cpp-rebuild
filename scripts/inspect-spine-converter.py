#!/usr/bin/env python3
"""Inspect Android Spine converter ELF ABI, dynamic exports, and CLI/API clues."""
import json, shutil, subprocess, sys, zipfile, tempfile, os
from pathlib import PurePosixPath

APP_SYMBOLS = ("CastJsonSkeletonToBinary", "CastSkeletonDataToDstVersion")

def run_readelf(path, *args):
    if not shutil.which("readelf"):
        return ""
    p = subprocess.run(["readelf", *args, path], capture_output=True, text=True)
    return p.stdout[:300000]

def main():
    if len(sys.argv) != 3:
        raise SystemExit("usage: inspect-spine-converter.py APK OUTPUT_JSON")
    apk, out = sys.argv[1:]
    rows = []
    with zipfile.ZipFile(apk) as z:
        libs = [i for i in z.infolist()
                if PurePosixPath(i.filename).name == "libSpineSkeletonDataConverterLib.so"]
        for info in libs:
            data = z.read(info)
            row = {"path": info.filename, "bytes": info.file_size,
                   "elf_magic": data[:4].hex(),
                   "elf_class": data[4] if len(data) > 5 else None,
                   "endianness": data[5] if len(data) > 5 else None}
            fd, path = tempfile.mkstemp(suffix=".so")
            os.close(fd)
            try:
                with open(path, "wb") as f:
                    f.write(data)
                row["elf_header"] = run_readelf(path, "-h")
                row["symbols"] = run_readelf(path, "-Ws")
                row["dynamic_symbols"] = run_readelf(path, "--dyn-syms", "-W")
                row["dynamic"] = run_readelf(path, "-d")
                dyn = row["dynamic_symbols"].splitlines()
                row["converter_dynamic_exports"] = [
                    line.strip() for line in dyn
                    if any(name in line for name in APP_SYMBOLS)
                    and " GLOBAL " in line and " UND " not in line
                ]
                row["converter_export_names"] = [
                    name for name in APP_SYMBOLS
                    if any(name in line for line in row["converter_dynamic_exports"])
                ]
                strings = subprocess.run(["strings", "-a", path],
                                          capture_output=True, text=True) if shutil.which("strings") else None
                if strings:
                    row["interesting_strings"] = [
                        x for x in strings.stdout.splitlines()
                        if any(k in x.lower() for k in
                               ("spine", "skeleton", "convert", "input", "output",
                                "json", "skel", "error", "version"))
                    ][:2000]
            finally:
                os.unlink(path)
            rows.append(row)
    result = {
        "library_count": len(rows),
        "libraries": rows,
        "interpretation": {
            "converter_api_symbols": list(APP_SYMBOLS),
            "note": "Dynamic exports and CLI strings are static evidence. They do not prove iOS ABI compatibility or source-code availability."
        }
    }
    with open(out, "w") as f:
        json.dump(result, f, indent=2)
    print(json.dumps({
        "library_count": len(rows),
        "exports": {x["path"]: x.get("converter_export_names", []) for x in rows}
    }), flush=True)

if __name__ == "__main__":
    main()
