#!/usr/bin/env python3
"""Inspect Android Spine converter ELF symbols without conflating symtab and dynsym."""
import json, shutil, subprocess, sys, zipfile, tempfile, os
from pathlib import PurePosixPath

NAMES = ("CastJsonSkeletonToBinary", "CastSkeletonDataToDstVersion")

def readelf(path, *args):
    if not shutil.which("readelf"):
        return ""
    p = subprocess.run(["readelf", *args, path], capture_output=True, text=True)
    return p.stdout[:300000]

def matching_lines(text):
    return [line.strip() for line in text.splitlines()
            if any(name in line for name in NAMES)]

def is_defined_global(line):
    fields = line.split()
    # readelf symbol rows: Num Value Size Type Bind Vis Ndx Name
    return len(fields) >= 8 and fields[4] == "GLOBAL" and fields[6] != "UND"

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
                   "elf_class": data[4] if len(data) > 5 else None,
                   "endianness": data[5] if len(data) > 5 else None}
            fd, path = tempfile.mkstemp(suffix=".so")
            os.close(fd)
            try:
                with open(path, "wb") as f:
                    f.write(data)
                row["elf_header"] = readelf(path, "-h")
                row["symbol_table_matches"] = matching_lines(readelf(path, "-Ws"))
                row["dynamic_symbol_matches"] = matching_lines(readelf(path, "--dyn-syms", "-W"))
                row["defined_global_symbol_table_matches"] = [
                    x for x in row["symbol_table_matches"] if is_defined_global(x)]
                row["defined_global_dynamic_symbol_matches"] = [
                    x for x in row["dynamic_symbol_matches"] if is_defined_global(x)]
                row["dynamic"] = readelf(path, "-d")
                row["dynamic_dependencies"] = [
                    line.strip() for line in row["dynamic"].splitlines()
                    if "(NEEDED)" in line or "(SONAME)" in line]
                row["strings_matches"] = []
                if shutil.which("strings"):
                    p = subprocess.run(["strings", "-a", path], capture_output=True, text=True)
                    row["strings_matches"] = [x for x in p.stdout.splitlines()
                                               if any(name in x for name in NAMES)]
            finally:
                os.unlink(path)
            rows.append(row)
    result = {
        "library_count": len(rows),
        "libraries": rows,
        "interpretation": {
            "function_names": list(NAMES),
            "note": "A symbol present only in .symtab is not a dynamic export. ELF static symbols and Android dependencies do not establish iOS ABI compatibility."
        }
    }
    with open(out, "w") as f:
        json.dump(result, f, indent=2)
    print(json.dumps({
        "library_count": len(rows),
        "symbols": {x["path"]: {
            "symtab": x["defined_global_symbol_table_matches"],
            "dynsym": x["defined_global_dynamic_symbol_matches"],
            "dependencies": x["dynamic_dependencies"]
        } for x in rows}
    }), flush=True)

if __name__ == "__main__":
    main()
