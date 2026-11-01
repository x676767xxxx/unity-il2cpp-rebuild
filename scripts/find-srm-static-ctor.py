#!/usr/bin/env python3
import pathlib, re, json, sys
root=pathlib.Path(sys.argv[1])
p=root/"dump.cs"
if not p.exists():
    p=next(root.rglob("dump.cs"),None)
if p is None:
    raise SystemExit("dump.cs not found")
lines=p.read_text(errors="replace").splitlines()
start=next((i for i,x in enumerate(lines) if "class ScenarioResourceManager" in x),None)
if start is None:
    raise SystemExit("ScenarioResourceManager class not found")
end=len(lines)
for i in range(start+1,len(lines)):
    if re.search(r"^\s*(public|private|internal|protected).*\bclass\s+",lines[i]):
        end=i
        break
out=[]
for i in range(start,end):
    if ".cctor" in lines[i] or "ScenarioResourceManager(" in lines[i]:
        lo=max(start,i-5)
        hi=min(end,i+4)
        block="\n".join(lines[lo:hi])
        rva=re.search(r"// RVA:\s*(0x[0-9a-fA-F]+)",block)
        token=re.search(r"// Token:\s*(0x[0-9a-fA-F]+)",block)
        out.append({"line":i+1,"signature":lines[i].strip(),"rva":rva.group(1) if rva else None,"token":token.group(1) if token else None,"context":lines[lo:hi]})
result={"source":str(p),"class_line":start+1,"methods":out}
pathlib.Path("work/out/scenario-resource-manager-ctor.json").write_text(json.dumps(result,indent=2))
print(json.dumps(result))
