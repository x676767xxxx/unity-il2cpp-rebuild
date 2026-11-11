# Spine orphaned P-code address forensics — run 19 (2026-11-11)

## Run and artifact

- Branch: `progress/azurearchive-ios-port-20261031`
- Source commit: `10748b4f057aa8d651463c71aa4af86f9bb1cd7d`
- Actions run: [43004776783](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43004776783)
- Result: success; 34 bounded-flow targets recovered and verified.
- Artifact: `azurearchive-rea-spine-flow-recovery`, ID `13847538595`, 4,514,081 bytes.
- SHA-256: `3b8706fb64c93ebaf7bab49c476d98145622fddbd1edb49f430862948fee7452`.
- Archive inspection: 145 entries; includes three additional address-forensics exports for Load, Retain and get_EstimatedTextureBytes.

## Main finding: orphaned addresses are real executable instructions

Every distinct P-code source address exported for the three priority methods was checked against the Ghidra memory map and listing. All were in initialized executable memory blocks and all had a decoded ARM64 instruction at the address.

| Target | Distinct P-code addresses | Initialized executable | Instruction present | Ghidra owner absent |
|---|---:|---:|---:|---:|
| RuntimeSpineLoader.Load | 188 | 188 | 188 | 185 |
| RuntimeSpineAsset.Retain | 1,306 | 1,306 | 1,306 | 1,295 |
| RuntimeSpineAsset.get_EstimatedTextureBytes | 622 | 622 | 622 | 572 |

The counts are distinct P-code sequence target addresses, not P-code operation rows and not necessarily unique semantic operations. A single machine instruction may emit multiple P-code operations; this export deduplicates by address.

### Address distribution

- Load: all 188 addresses in memory block `il2cpp`; 109 address clusters when a gap greater than 4 bytes starts a new cluster; address span `0x0335f300–0x05dbc718`.
- Retain: 1,294 addresses in `il2cpp`, 12 in `.text`; 356 address clusters; span `0x0302d9a4–0x03e3e6e0`.
- get_EstimatedTextureBytes: all 622 addresses in `il2cpp`; 152 address clusters; span `0x033602ec–0x05f73528`.

These are widely separated code regions, not one contiguous method body. This strongly narrows the issue to Ghidra function ownership / decompiler control-flow attribution, rather than addresses landing in non-executable data or failing instruction decode.

## Direct-flow observations

- Load's exported instruction set includes 104 direct calls; the most frequent target is `0x0302d9f8` (26 calls), followed by `0x0302da0c` (9), `0x0302dc94` (9), and `0x0302dc84` (7).
- Retain includes 226 direct calls and 30 computed calls; frequent direct targets include `0x03066ebc` (32), `0x03066bc8` (28), `0x0302d9f8` (20), and `0x0302da0c` (18).
- get_EstimatedTextureBytes includes 105 direct calls and 20 computed calls; frequent direct targets include `0x0302d9f8` (25) and `0x03066ebc` (20).

The run-19 forensic TSV writer mistakenly emitted escaped delimiter sequences (`\\t` and `\\n`) rather than actual TSV separators/newlines. The raw content was still parseable by splitting on those literal sequences, and the address/instruction counts above were independently computed from it. The output format defect is corrected in the next commit; the next run also adds direct-flow target owner information.

## Interpretation and limits

1. These addresses are valid executable ARM64 instructions. They are not merely malformed P-code addresses, unmapped memory, or undecodable bytes.
2. The large P-code address sets are distributed across hundreds of disjoint code clusters. The current decompiler output is therefore not a clean, isolated method-level listing for Load, Retain or get_EstimatedTextureBytes.
3. This does not prove every exported address is semantically part of the target method. Potential explanations still include incomplete/misleading function boundaries, shared cold/error/exception blocks, tail-merged code, bad indirect-flow recovery, and Ghidra decompiler attribution behavior.
4. The bounded flow verifier still proves only its own 5,301 reachable/decoded instruction starts across 34 targets; it does not prove complete CFG or source equivalence.
5. No iOS build, IPA, or runtime port is established by this evidence.

## Next step — run 20

The next run corrects TSV serialization and exports owner information for each direct flow target. Use those results to identify which direct call targets already have Ghidra functions, which are thunks/runtime helpers, and which executable regions are currently unowned. Then add function definitions only for validated function entries and re-decompile the three priority methods. Do not bulk-create functions at every P-code address: many addresses are ordinary interior instructions, not function entries.
