#!/usr/bin/env python3
"""Extract ScenarioResourceManager and AddrLoadUrl evidence from Il2CppDumper output."""
import json, pathlib, re, sys
if len(sys.argv) != 3:
    raise SystemExit("usage: extract-il2cpp-targets.py DUMP_DIR OUTPUT_JSON")
root=pathlib.Path(sys.argv[1]); out=pathlib.Path(sys.argv[2])
dump=root/"dump.cs"
if not dump.exists():
    candidates=list(root.rglob("dump.cs"))
    if not candidates: raise SystemExit("dump.cs not found; Il2CppDumper did not produce a managed dump")
    dump=candidates[0]
lines=dump.read_text(errors="replace").splitlines()
hits=[]
for i,line in enumerate(lines):
    if re.search(r"ScenarioResourceManager|AddrLoadUrl",line,re.I):
        hits.append({"line":i+1,"text":line.strip(),"context":lines[max(0,i-18):min(len(lines),i+25)]})
methods=[]
for i,line in enumerate(lines):
    if re.search(r"ScenarioResourceManager|AddrLoadUrl",line,re.I):
        start=max(0,i-40); end=min(len(lines),i+80)
        block="\n".join(lines[start:end])
        for m in re.finditer(r"// RVA:\s*(0x[0-9A-Fa-f]+).*?// Offset:\s*(0x[0-9A-Fa-f]+).*?// Token:\s*(0x[0-9A-Fa-f]+)",block,re.S):
            methods.append({"rva":m.group(1),"offset":m.group(2),"token":m.group(3),"near_line":i+1})

# Inspect Il2CppDumper script.json for exact type/field metadata. Keep only
# compact parent records around the named type/field to avoid copying the dump.
script=root/"script.json"
metadata_matches=[]
data={}
if script.exists():
    try:
        data=json.loads(script.read_text(errors="replace"))
        def walk(node,path="$"):
            if isinstance(node,dict):
                vals=[v for v in node.values() if isinstance(v,str)]
                joined=" | ".join(vals)
                if "ScenarioResourceManager" in joined or "AddrLoadUrl" in joined:
                    metadata_matches.append({"path":path,"record":node})
                for k,v in node.items():
                    if isinstance(v,(dict,list)): walk(v,path+"."+str(k))
            elif isinstance(node,list):
                for n,v in enumerate(node):
                    if isinstance(v,(dict,list)): walk(v,path+"["+str(n)+"]")
        walk(data)
    except Exception as exc:
        metadata_matches=[{"error":str(exc)}]
(root/"il2cpp-script-metadata.json").write_text(json.dumps(metadata_matches,indent=2))
addr_matches=[m for m in metadata_matches if "AddrLoadUrl" in json.dumps(m.get("record",{}),ensure_ascii=False)]
srm_matches=[m for m in metadata_matches if "ScenarioResourceManager" in json.dumps(m.get("record",{}),ensure_ascii=False)]
def compact(m):
    rec=m.get("record",{})
    return {"path":m.get("path"),"keys":list(rec.keys())[:24],
            "values":{k:v for k,v in rec.items() if isinstance(v,(str,int,float,bool))}}


# Emit focused source evidence for the exact AddrLoadUrl placeholder.
addr_dump_lines=[{"line":i+1,"text":line.strip(),
                  "context":lines[max(0,i-12):min(len(lines),i+18)]}
                 for i,line in enumerate(lines)
                 if "AddrLoadUrl" in line]
script_top_keys=list(data.keys()) if isinstance(data,dict) else []
report={"dump_file":str(dump),"line_count":len(lines),"hit_count":len(hits),"hits":hits,"nearby_method_headers":methods,"script_metadata_matches":len(metadata_matches),"addrloadurl_dump_lines":addr_dump_lines,"script_top_level_keys":script_top_keys}
out.write_text(json.dumps(report,indent=2))
md=["# Android IL2CPP target evidence","","Generated from Il2CppDumper dump.cs. RVAs are static metadata addresses, not proof of runtime values.","",f"- Source: {dump}","- Hits: {len(hits)}",""]
for h in hits:
    md += [f"## dump.cs:{h['line']}","\x60\x60\x60csharp",*h["context"],"\x60\x60\x60",""]
(out.with_suffix(".md")).write_text("\n".join(md))
# Summarize the type/field metadata arrays and methods related to URL loading.
def top_array_preview(key, needle="ScenarioResourceManager"):
    arr=data.get(key,[]) if isinstance(data,dict) else []
    if not isinstance(arr,list): return {"type":type(arr).__name__}
    selected=[]
    for idx,item in enumerate(arr):
        raw=json.dumps(item,ensure_ascii=False)
        if needle in raw:
            selected.append({"index":idx,"keys":list(item.keys())[:30] if isinstance(item,dict) else [],
                             "preview":item if isinstance(item,dict) else str(item)[:500]})
    return {"total":len(arr),"matches":len(selected),"preview":selected[:30]}
method_arr=data.get("ScriptMethod",[]) if isinstance(data,dict) else []
srm_methods=[]
for idx,item in enumerate(method_arr if isinstance(method_arr,list) else []):
    if isinstance(item,dict) and str(item.get("Name","")).startswith("ScenarioResourceManager"):
        srm_methods.append({"index":idx,"name":item.get("Name"),"address":item.get("Address"),"signature":item.get("Signature")})
related_methods=[]
for idx,item in enumerate(method_arr if isinstance(method_arr,list) else []):
    if isinstance(item,dict):
        name=str(item.get("Name",""))
        if any(k.lower() in name.lower() for k in ("AddrLoadUrl","ScenarioResourceManager","SetUrl","Url","Addressable","LoadRemote","Resource")):
            related_methods.append({"index":idx,"name":name,"address":item.get("Address"),"signature":item.get("Signature")})
print(json.dumps({"hits":len(hits),"nearby_method_headers":len(methods),"dump_file":str(dump),"script_metadata_matches":len(metadata_matches),"addrloadurl_metadata_matches":len(addr_matches),"scenariorm_metadata_matches":len(srm_matches),"addrloadurl_metadata_preview":[compact(m) for m in addr_matches[:60]],"addrloadurl_dump_lines":addr_dump_lines,"script_top_level_keys":script_top_keys,"scenariorm_metadata_preview":[compact(m) for m in srm_matches[:12]],"script_metadata_type_preview":top_array_preview("ScriptMetadata"),"script_metadata_method_preview":top_array_preview("ScriptMetadataMethod"),"scenario_resource_manager_methods":srm_methods,"related_script_methods":related_methods[:200]},ensure_ascii=False))
