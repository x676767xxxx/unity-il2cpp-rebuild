#!/usr/bin/env python3
"""Trace selected IL2CPP ARM64 methods, callees, direct BL callers and ADRP-based memory accesses."""
import json,pathlib,struct,sys
from elftools.elf.elffile import ELFFile
from capstone import Cs,CS_ARCH_ARM64,CS_MODE_ARM
if len(sys.argv)!=3: raise SystemExit("usage: script libil2cpp.so output-dir")
elf_path=pathlib.Path(sys.argv[1]); out=pathlib.Path(sys.argv[2]); out.mkdir(parents=True,exist_ok=True)
targets={"ScenarioResourceManager.Initialize":0x3264A00,"ScenarioResourceManager.CheckForUpdate":0x32646B0,"ScenarioResourceManager.InitializeAfterSplash":0x326498C,"ScenarioResourceManager.OnUpdateComplete":0x32654A0,"ScenarioResourceManager..cctor":0x3438D28,"UserSettings.SettingData.get_catalogUrlOverride":0x3287DA4,"UserSettings.SettingData.set_catalogUrlOverride":0x3287DAC}
callees={"helper_2f2d9f8":0x2F2D9F8,"helper_2f2dc84":0x2F2DC84,"CheckForUpdate_callback":0x326FDC4,"InitializeAfterSplash_callback":0x32771B0,"OnUpdateComplete_callback":0x32773F4,"write_barrier_2f2d9a4":0x2F2D9A4}
pages={0x6E66000,0x6E64000,0x6E65000,0x72C2000}; md=Cs(CS_ARCH_ARM64,CS_MODE_ARM)
def sx(v,n): return (v^(1<<(n-1)))-(1<<(n-1))
report={"targets":{},"callees":{},"direct_bl_xrefs":{},"global_page_refs":[]}
with elf_path.open("rb") as f:
 e=ELFFile(f); report["elf_header"]={"class":e.elfclass,"machine":e.header["e_machine"],"type":e.header["e_type"]}
 if e.header["e_machine"]!="EM_AARCH64": raise SystemExit("Expected AArch64")
 loads=[s for s in e.iter_segments() if s["p_type"]=="PT_LOAD"]; execs=[s for s in loads if int(s["p_flags"])&1]; blob=elf_path.read_bytes()
 def off_for(a):
  for s in loads:
   b=int(s["p_vaddr"]); z=b+int(s["p_filesz"])
   if b<=a<z:return int(s["p_offset"])+a-b
 def window(a,n):
  o=off_for(a); return {"rva":hex(a),"file_offset":hex(o) if o is not None else None,"instructions":[{"address":hex(i.address),"bytes":i.bytes.hex(),"mnemonic":i.mnemonic,"op_str":i.op_str} for i in md.disasm(blob[o:o+n],a)] if o is not None else []}
 for k,a in targets.items(): report["targets"][k]=window(a,0x180)
 for k,a in callees.items(): report["callees"][k]=window(a,0x100)
 xrefs={k:[] for k in list(targets)+list(callees)}; refs=[]
 for s in execs:
  base=int(s["p_vaddr"]); pos0=int(s["p_offset"]); end=pos0+int(s["p_filesz"])
  for p in range(pos0,end-3,4):
   w=struct.unpack_from("<I",blob,p)[0]; pc=base+p-pos0
   if w&0xFC000000==0x94000000:
    d=pc+(sx(w&0x3FFFFFF,26)<<2)
    for k,a in list(targets.items())+list(callees.items()):
     if d==a:xrefs[k].append({"caller":hex(pc),"destination":hex(d)})
   if w&0x9F000000==0x90000000:
    imm=((w>>5)&0x7FFFF)<<2|((w>>29)&3); pg=(pc&~0xFFF)+(sx(imm,21)<<12)
    if pg in pages: refs.append({"address":hex(pc),"page":hex(pg),"register":f"x{w&31}","next_words":[hex(struct.unpack_from("<I",blob,q)[0]) for q in (p+4,p+8,p+12) if q<end-3]})
 report["direct_bl_xrefs"]=xrefs; report["global_page_refs"]=refs
 targeted=[]
 for sec in ("targets","callees"):
  for name,win in report[sec].items():
   ins=win["instructions"]
   for idx,insn in enumerate(ins):
    if insn["mnemonic"]!="adrp": continue
    parts=insn["op_str"].split(",",1)
    if len(parts)!=2: continue
    reg=parts[0].strip()
    try: page=int(parts[1].strip().lstrip("#"),16)
    except ValueError: continue
    accesses=[]
    for nxt in ins[idx+1:idx+17]:
     mnemonic=nxt["mnemonic"]; operands=nxt["op_str"]
     if "["+reg in operands:
      kind="write" if mnemonic.startswith("str") or mnemonic in ("stp","stur","stxr","stlxr") else "read"
      accesses.append({"address":nxt["address"],"mnemonic":mnemonic,"op_str":operands,"access":kind})
     # Stop when an instruction overwrites the ADRP register; preserve a load/store
     # that uses it as the base before clobbering it.
     dest=operands.split(",",1)[0].strip() if operands else ""
     writes_dest=(mnemonic not in ("str","strb","strh","stp","sturb","sturh","stxr","stlxr","cmp","cmn","tst","tbz","tbnz","cbz","cbnz","b","bl","br","blr","ret","nop","dmb"))
     if dest==reg and writes_dest: break
    if accesses: targeted.append({"section":sec,"method":name,"adrp_address":insn["address"],"register":reg,"page":hex(page),"memory_accesses":accesses})
 report["targeted_global_accesses"]=targeted
 report["limits"]="ADRP-based accesses stop when the base register is overwritten. Read/write labels describe instruction direction only; class-static field identity is not proven. Direct BL scans cover file-backed executable PT_LOAD segments; indirect calls and runtime-resolved pointers remain unresolved."
(out/"arm64-il2cpp-disassembly.json").write_text(json.dumps(report,indent=2))
lines=["# ARM64 IL2CPP trace","","Direct BL callers are scanned across executable file-backed PT_LOAD segments.",""]
for sec in ("targets","callees"):
 for k,v in report[sec].items():
  lines += [f"## {k} — {v['rva']} (file offset {v['file_offset']})","| Address | Instruction |","|---|---|"]
  lines += [f"| {i['address']} | `{i['mnemonic']} {i['op_str']}` |" for i in v["instructions"]]
  lines.append("")
lines += ["## Direct BL callers"]
for k,v in xrefs.items(): lines += [f"### {k}"]+[f"- {z['caller']}" for z in v] if v else [f"### {k}","- None found"]
lines += ["## Targeted ADRP memory accesses"]
for z in targeted:
 lines.append(f"- {z['method']} {z['adrp_address']} page {z['page']} ({z['register']}): "+", ".join(f"{a['address']} {a['access']} {a['mnemonic']} {a['op_str']}" for a in z["memory_accesses"]))
lines += ["",report["limits"]]
(out/"arm64-il2cpp-disassembly.md").write_text("\n".join(lines))
summary={"elf":report["elf_header"],"mapped_targets":sum(bool(v["file_offset"]) for v in report["targets"].values()),"mapped_callees":sum(bool(v["file_offset"]) for v in report["callees"].values()),"bl_callers":{k:len(v) for k,v in xrefs.items()},"global_page_refs":len(refs),"targeted_access_groups":len(targeted),"targeted_reads":sum(a["access"]=="read" for z in targeted for a in z["memory_accesses"]),"targeted_writes":sum(a["access"]=="write" for z in targeted for a in z["memory_accesses"])}
print(json.dumps(summary)); print("TARGETED_GLOBAL_ACCESSES",json.dumps(targeted))
for sec in ("targets","callees"):
 for k,v in report[sec].items():
  print("\\n===",k,v["rva"],"===")
  for i in v["instructions"][:96]:print(i["address"],i["mnemonic"],i["op_str"])
