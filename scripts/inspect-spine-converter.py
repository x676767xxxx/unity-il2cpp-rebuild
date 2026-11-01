#!/usr/bin/env python3
"""Inspect Android Spine converter ELF libraries and report exported symbols."""
import json, shutil, subprocess, sys, zipfile
from pathlib import PurePosixPath

def main():
    if len(sys.argv) != 3:
        raise SystemExit("usage: inspect-spine-converter.py APK OUTPUT_JSON")
    apk, out = sys.argv[1:]
    rows=[]
    with zipfile.ZipFile(apk) as z:
        libs=[i for i in z.infolist() if PurePosixPath(i.filename).name == "libSpineSkeletonDataConverterLib.so"]
        for info in libs:
            data=z.read(info)
            row={"path":info.filename,"bytes":info.file_size,"elf_magic":data[:4].hex(),"elf_class":data[4] if len(data)>5 else None,"endianness":data[5] if len(data)>5 else None}
            import tempfile, os
            fd,path=tempfile.mkstemp(suffix=".so")
            os.close(fd)
            try:
                with open(path,"wb") as f:f.write(data)
                for cmd,key in [(["readelf","-h",path],"elf_header"),(["readelf","-Ws",path],"symbols"),(["readelf","-d",path],"dynamic")]:
                    if shutil.which(cmd[0]):
                        p=subprocess.run(cmd,capture_output=True,text=True)
                        row[key]=p.stdout[:200000]
                        if p.returncode: row[key+"_error"]=p.stderr[:2000]
                strings=subprocess.run(["strings","-a",path],capture_output=True,text=True) if shutil.which("strings") else None
                if strings:
                    row["interesting_strings"]=[x for x in strings.stdout.splitlines() if any(k in x.lower() for k in ("spine","skeleton","convert","input","output","json","skel","error","version"))][:2000]
            finally: os.unlink(path)
            rows.append(row)
    result={"library_count":len(rows),"libraries":rows,"note":"ELF exports and strings are static evidence, not proof of iOS ABI compatibility."}
    with open(out,"w") as f:json.dump(result,f,indent=2)
    print(json.dumps({"library_count":len(rows),"paths":[x["path"] for x in rows]}),flush=True)
if __name__=="__main__":main()
