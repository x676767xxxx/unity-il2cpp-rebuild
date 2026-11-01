#!/usr/bin/env python3
import pathlib,re,json,sys
root=pathlib.Path(sys.argv[1]); p=root/"dump.cs"
if not p.exists(): p=next(root.rglob("dump.cs"),None)
if p is None: raise SystemExit("dump.cs not found")
lines=p.read_text(errors="replace").splitlines()
hits=[i for i,x in enumerate(lines) if "ScenarioResourceManager" in x]
if not hits: raise SystemExit("ScenarioResourceManager not found")
start=hits[0]
# Include a generous window; Il2CppDumper may render methods outside the class block.
lo=max(0,start-15); hi=min(len(lines),start+260)
window=lines[lo:hi]
methods=[]
for j,x in enumerate(window):
    if re.search(r"\.cctor|ScenarioResourceManager\s*\(",x):
        context=window[max(0,j-4):min(len(window),j+5)]
        block="\n".join(context)
        rv=re.search(r"// RVA:\s*(0x[0-9a-fA-F]+)",block)
        tk=re.search(r"// Token:\s*(0x[0-9a-fA-F]+)",block)
        methods.append({"line":lo+j+1,"signature":x.strip(),"rva":rv.group(1) if rv else None,"token":tk.group(1) if tk else None,"context":context})
# Keep all lines with method metadata in the window for diagnosis.
metadata=[{"line":lo+j+1,"text":x.strip()} for j,x in enumerate(window) if "RVA:" in x or "Token:" in x or "AddrLoadUrl" in x]
result={"source":str(p),"first_hit_line":start+1,"window_start":lo+1,"window_end":hi,"constructor_candidates":methods,"metadata_lines":metadata,"window":window}
pathlib.Path("work/out/scenario-resource-manager-ctor.json").write_text(json.dumps(result,indent=2))
print(json.dumps({"first_hit_line":start+1,"constructor_candidates":methods,"metadata_lines":metadata}))
