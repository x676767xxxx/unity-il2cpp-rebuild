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
# Produce a compact, deterministic index of SRM methods and the exact class block.
# Keep static-constructor and URL/configuration candidates separate from broad hits.
srm_focus_methods=[]
for idx,item in enumerate(method_arr if isinstance(method_arr,list) else []):
    if not isinstance(item,dict):
        continue
    name=str(item.get("Name", ""))
    if name.startswith("ScenarioResourceManager"):
        tail=name.rsplit("$",1)[-1] if "$" in name else name
        if (tail in (".cctor", ".ctor") or
            any(k in name.lower() for k in ("addrloadurl", "url", "catalog", "initialize", "checkforupdate", "loadremote", "addressable"))):
            srm_focus_methods.append({"index":idx,"name":name,"address":item.get("Address"),"signature":item.get("Signature")})
# Extract the full declaration region for ScenarioResourceManager from dump.cs.
class_blocks=[]
for i,line in enumerate(lines):
    if re.search(r"\b(class|struct)\s+ScenarioResourceManager\b", line):
        depth=0; started=False; block=[]
        for j in range(i,min(len(lines),i+2500)):
            block.append(lines[j])
            depth += lines[j].count("{")-lines[j].count("}")
            if "{" in lines[j]: started=True
            if started and depth<=0: break
        class_blocks.append({"start_line":i+1,"end_line":i+len(block),"declaration":line.strip(),
                             "field_lines":[x.strip() for x in block if re.search(r"\bAddrLoadUrl\b|static\s+.*;",x)],
                             "method_signatures":[x.strip() for x in block if "(" in x and ")" in x and not x.lstrip().startswith("//")],
                             "block":block})
# Save the detailed evidence separately; this is additive to the existing report.
(root/"scenario-resource-manager-focused.json").write_text(json.dumps({
    "static_constructor_and_url_candidates":srm_focus_methods,
    "class_blocks":class_blocks,
    "interpretation":"ScriptMethod.Address is a method address; static field storage still requires class/static-field layout evidence."
},indent=2))
related_methods=[]
for idx,item in enumerate(method_arr if isinstance(method_arr,list) else []):
    if isinstance(item,dict):
        name=str(item.get("Name",""))
        if any(k.lower() in name.lower() for k in ("AddrLoadUrl","ScenarioResourceManager","SetUrl","Url","Addressable","LoadRemote","Resource")):
            related_methods.append({"index":idx,"name":name,"address":item.get("Address"),"signature":item.get("Signature")})
# Inspect schema/layout evidence without assuming Il2CppDumper field names.
def schema_summary(obj):
    if isinstance(obj, dict):
        return {"kind":"object","keys":list(obj.keys())[:80],
                "scalar_sample":{k:v for k,v in obj.items() if isinstance(v,(str,int,float,bool))}}
    if isinstance(obj, list):
        sample=obj[:3]
        return {"kind":"array","length":len(obj),
                "sample":[schema_summary(x) if isinstance(x,(dict,list)) else x for x in sample]}
    return {"kind":type(obj).__name__}
array_schemas={}
if isinstance(data,dict):
    for key,value in data.items():
        if isinstance(value,list):
            array_schemas[key]=schema_summary(value)
# Find records that can link type and field metadata by name or index.
named_records=[]
def find_named_records(node,path="$"):
    if isinstance(node,dict):
        raw=json.dumps(node,ensure_ascii=False)
        if "ScenarioResourceManager" in raw or "AddrLoadUrl" in raw:
            named_records.append({"path":path,"record":node})
        for key,value in node.items():
            if isinstance(value,(dict,list)): find_named_records(value,path+"."+str(key))
    elif isinstance(node,list):
        for n,value in enumerate(node):
            if isinstance(value,(dict,list)): find_named_records(value,path+"["+str(n)+"]")
if data: find_named_records(data)
header=root/"il2cpp.h"
header_hits=[]
if header.exists():
    hlines=header.read_text(errors="replace").splitlines()
    pats=re.compile(r"Il2CppClass|Il2CppMetadataRegistration|Il2CppCodeRegistration|FieldInfo|Il2CppTypeDefinition|Il2CppFieldDefinition|static_fields|fieldOffsets")
    for i,line in enumerate(hlines):
        if pats.search(line):
            header_hits.append({"line":i+1,"context":hlines[max(0,i-3):min(len(hlines),i+14)]})
# Check whether the literal VA falls into any file-backed PT_LOAD mapping.
elf_path_file=root/"libil2cpp-path.txt"
elf_path=None
if elf_path_file.exists():
    raw_path=elf_path_file.read_text().strip()
    candidate=pathlib.Path(raw_path)
    if not candidate.is_absolute(): candidate=pathlib.Path.cwd()/candidate
    if candidate.exists(): elf_path=candidate
import struct
elf_mapping={"path":str(elf_path) if elf_path else None,"literal_va":"0x7096470","segments":[]}
if elf_path:
    blob=elf_path.read_bytes()
    if blob[:4]==b"\x7fELF" and blob[4]==2:
        endian="<" if blob[5]==1 else ">"
        eh=struct.unpack_from(endian+"HHIQQQIHHHHHH",blob,16)
        phoff,phentsize,phnum=eh[4],eh[8],eh[9]
        for n in range(phnum):
            p=struct.unpack_from(endian+"IIQQQQQQ",blob,phoff+n*phentsize)
            p_type,p_flags,p_offset,p_vaddr,p_paddr,p_filesz,p_memsz,p_align=p
            if p_type==1:
                va=0x7096470
                contains=p_vaddr <= va < p_vaddr+p_filesz
                elf_mapping["segments"].append({"type":"PT_LOAD","flags":p_flags,"offset":hex(p_offset),"vaddr":hex(p_vaddr),"filesz":hex(p_filesz),"memsz":hex(p_memsz),"literal_file_backed":contains,"literal_file_offset":hex(p_offset+va-p_vaddr) if contains else None})
        # Capture bytes at the mapped literal address so the next pass can
        # verify the actual string and inspect adjacent pointer/table data.
        mapped=next((x for x in elf_mapping["segments"] if x["literal_file_backed"]),None)
        if mapped:
            file_off=int(mapped["literal_file_offset"],16)
            sample=blob[file_off:file_off+256]
            nul=sample.find(b"\\x00")
            cstr=sample[:nul if nul >= 0 else len(sample)]
            elf_mapping["literal_bytes_hex"]=sample.hex()
            elf_mapping["literal_ascii_preview"]=cstr.decode("utf-8",errors="replace")
            elf_mapping["literal_cstring_terminated"]=nul >= 0
            elf_mapping["literal_va_file_offset_delta"]=hex(0x7096470-file_off)
# Search the actual UTF-8 literal in the ELF and Il2CppDumper string table.
# ScriptString.Address can point to a registration/metadata slot rather than
# the bytes of the string, so do not assume it is a direct ELF VA.
literal_text=b"{ScenarioResourceManager.AddrLoadUrl}/"
literal_search={"needle":literal_text.decode(),"elf_file_offsets":[],"stringliteral_json_matches":[]}
if elf_path:
    blob=elf_path.read_bytes()
    pos=0
    while True:
        pos=blob.find(literal_text,pos)
        if pos < 0: break
        literal_search["elf_file_offsets"].append(hex(pos))
        pos+=1
stringliteral=root/"stringliteral.json"
if stringliteral.exists():
    try:
        sl=json.loads(stringliteral.read_text(errors="replace"))
        if isinstance(sl,list):
            for idx,item in enumerate(sl):
                if isinstance(item,dict) and literal_text.decode() in str(item.get("Value","")):
                    literal_search["stringliteral_json_matches"].append({"index":idx,"record":item})
    except Exception as exc:
        literal_search["error"]=str(exc)
elf_mapping["literal_search"]=literal_search
report["script_array_schemas"]=array_schemas
report["named_metadata_records"]=named_records[:200]
report["il2cpp_header_hits"]=header_hits[:100]
report["elf_literal_mapping"]=elf_mapping
out.write_text(json.dumps(report,indent=2))
print(json.dumps({"focused_report":{"class_block_count":len(class_blocks),"class_blocks":[{"start_line":b["start_line"],"end_line":b["end_line"],"declaration":b["declaration"],"field_lines":b["field_lines"],"method_signatures":b["method_signatures"][:180]} for b in class_blocks],"static_constructor_and_url_candidates":srm_focus_methods[:180]},"hits":len(hits),"nearby_method_headers":len(methods),"dump_file":str(dump),"script_metadata_matches":len(metadata_matches),"addrloadurl_metadata_matches":len(addr_matches),"scenariorm_metadata_matches":len(srm_matches),"addrloadurl_metadata_preview":[compact(m) for m in addr_matches[:20]],"addrloadurl_dump_lines":addr_dump_lines,"script_top_level_keys":script_top_keys,"script_array_schemas":array_schemas,"scenariorm_metadata_preview":[compact(m) for m in srm_matches[:12]],"script_metadata_type_preview":top_array_preview("ScriptMetadata"),"script_metadata_method_preview":top_array_preview("ScriptMetadataMethod"),"scenario_resource_manager_methods":srm_methods,"related_script_methods":related_methods[:200],"elf_literal_mapping":elf_mapping,"il2cpp_header_hit_count":len(header_hits),"il2cpp_header_preview":header_hits[:30]},ensure_ascii=False))
