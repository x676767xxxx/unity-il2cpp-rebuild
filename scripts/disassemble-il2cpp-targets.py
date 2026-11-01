#!/usr/bin/env python3
"""Disassemble selected IL2CPP ARM64 methods and find direct BL cross-references."""
import json, pathlib, sys
from elftools.elf.elffile import ELFFile
from capstone import Cs, CS_ARCH_ARM64, CS_MODE_ARM

if len(sys.argv) != 3:
    raise SystemExit("usage: disassemble-il2cpp-targets.py libil2cpp.so output-dir")
elf_path=pathlib.Path(sys.argv[1]); out=pathlib.Path(sys.argv[2]); out.mkdir(parents=True,exist_ok=True)
targets={
 "ScenarioResourceManager.Initialize":0x3264A00,
 "ScenarioResourceManager.CheckForUpdate":0x32646B0,
 "ScenarioResourceManager.InitializeAfterSplash":0x326498C,
 "ScenarioResourceManager.OnUpdateComplete":0x32654A0,
 "UserSettings.SettingData.get_catalogUrlOverride":0x3287DA4,
 "UserSettings.SettingData.set_catalogUrlOverride":0x3287DAC,
}
md=Cs(CS_ARCH_ARM64,CS_MODE_ARM); md.detail=True
report={"elf":str(elf_path),"targets":{},"direct_bl_xrefs":{},"warnings":[]}
with elf_path.open("rb") as f:
    e=ELFFile(f)
    report["elf_header"]={"class":e.elfclass,"little_endian":e.little_endian,"machine":e.header["e_machine"],"type":e.header["e_type"]}
    if e.header["e_machine"]!="EM_AARCH64": raise SystemExit("Expected AArch64 libil2cpp.so")
    loads=[s for s in e.iter_segments() if s["p_type"]=="PT_LOAD"]
    def file_offset(addr):
        for s in loads:
            start=int(s["p_vaddr"]); end=start+int(s["p_filesz"])
            if start <= addr < end: return int(s["p_offset"])+(addr-start)
        return None
    blob=elf_path.read_bytes()
    for name,addr in targets.items():
        off=file_offset(addr)
        item={"rva":hex(addr),"file_offset":hex(off) if off is not None else None,"instructions":[]}
        if off is None:
            item["error"]="RVA not covered by a file-backed PT_LOAD segment"
        else:
            for ins in md.disasm(blob[off:off+0x180],addr):
                item["instructions"].append({"address":hex(ins.address),"bytes":ins.bytes.hex(),"mnemonic":ins.mnemonic,"op_str":ins.op_str})
        report["targets"][name]=item
    # Bounded first pass: inspect calls inside selected method windows only.
    # A whole-.text scan is deferred to an optimized follow-up pass.
    xrefs={name:[] for name in targets}
    for name,item in report["targets"].items():
        for ins in item["instructions"]:
            if ins["mnemonic"]=="bl":
                try: dest=int(ins["op_str"].split()[0].replace("#",""),0)
                except (ValueError,IndexError): continue
                for target_name,target_addr in targets.items():
                    if dest==target_addr:
                        xrefs[target_name].append({"caller":ins["address"],"within":name,"instruction":"bl "+ins["op_str"]})
    report["direct_bl_xrefs"]=xrefs
    report["scan_note"]="Bounded first pass: direct BL calls originating inside selected method windows only. No whole-.text caller scan, indirect calls or static-field data-flow resolution."
(out/"arm64-il2cpp-disassembly.json").write_text(json.dumps(report,indent=2))
lines=["# ARM64 IL2CPP native disassembly","",f"- ELF: {elf_path}",f"- ELF header: {report['elf_header']}","- Method windows: 0x180 bytes from each RVA","- Cross-reference scan: direct BL inside selected method windows only",""]
for name,item in report["targets"].items():
    lines += [f"## {name} — {item['rva']}",f"- File offset: {item['file_offset'] or 'not mapped'}"]
    if item.get("error"): lines.append("- "+item["error"])
    lines += ["","| Address | Instruction |","|---|---|"]
    for ins in item["instructions"]:
        lines.append(f"| {ins['address']} | `{ins['mnemonic']} {ins['op_str']}` |")
    lines += ["","Direct BL callers:"]
    callers=report["direct_bl_xrefs"][name]
    lines += [f"- {x['caller']} ({x['section']})" for x in callers] if callers else ["- None found"]
    lines.append("")
lines += ["## Interpretation limits","",report["scan_note"],"","A missing direct BL result does not prove the method is unused; callers outside these method windows, indirect calls, function pointers, virtual dispatch and data-flow references require further analysis."]
(out/"arm64-il2cpp-disassembly.md").write_text("\n".join(lines))
print(json.dumps({"machine":report["elf_header"],"mapped":sum(bool(x["file_offset"]) for x in report["targets"].values()),"direct_bl_xrefs":{k:len(v) for k,v in xrefs.items()}}))
