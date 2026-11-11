# Spine P-code call-target ownership review — run 20

Date: 2026-11-11

## Run and artifact

- Branch: `progress/azurearchive-ios-port-20261031`
- Run: [43005245179](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43005245179)
- Run commit: `23560233828e20af9b44bc7327038ed594693aa2`
- Artifact ID: `13848695096`
- Artifact: `azurearchive-rea-spine-flow-recovery`, 4,514,956 bytes
- SHA-256: `2372d56b25b66e59bf225b4ba331d66a0c89d0432acec24d06a19a0e098705f9`
- Archive: 145 entries; verification JSON lists 34 targets; no target reports a decode failure or reachable/decoded count mismatch.

## P-code address export

The three forensic TSVs now contain actual tab/newline delimiters and parse as 622, 1,306 and 188 rows respectively. Each row has 14 tab-separated fields, but the header has only 13 fields: the newly added `flow_target_owners` column is missing from the header. Data columns after `flow_targets` are therefore shifted by one when parsed against the header.

The data-column order is:
`pcode_address, memory_block, initialized, executable, instruction_present, mnemonic, instruction_text, bytes_hex, flow_type, flow_targets, flow_target_owners, containing_function_entry, containing_function_name, target_body`.

The address-level result remains consistent with run 19: all 2,116 exported addresses (188 + 1,306 + 622) are initialized, executable and have an instruction present. This is not proof that all addresses belong to the corresponding target method.

## Direct call-target owner resolution

The script calls Ghidra `getFunctionContaining(target)` for each instruction flow target. The new `flow_target_owners` data encodes each flow target as `target=containingFunctionEntry:functionName`; an empty value after `=` means no Ghidra function currently contains that target. This is a Ghidra function-ownership result, not a proof that the machine-code target is invalid.

| Method | Direct calls (`bl`) | Computed calls | Named function targets found |
|---|---:|---:|---|
| `RuntimeSpineLoader.Load` | 104 | 1 | 1 call to `RuntimeSpineLoader.Read` at `0x0335ee08` |
| `RuntimeSpineAsset.Retain` | 226 | 30 | 0 |
| `RuntimeSpineAsset.get_EstimatedTextureBytes` | 105 | 20 | 0 |

For Load, the other direct call target addresses do not currently resolve to a containing named Ghidra function. Retain and get_EstimatedTextureBytes have no named function owner among the exported call targets. Computed calls may have no statically enumerated flow target, so they should not be counted as unresolved direct-call destinations.

## Code correction committed after run 20

The 14-field row / 13-field header mismatch was confirmed in `scripts/ghidra/PrepareFlowFunction.java`. A one-line header correction was committed to the same branch:
- Commit: `f6849c8f0e3212dd9e41c587e27feaa114e5c5f6`
- Change: add `flow_target_owners` to the TSV header between `flow_targets` and `owner_entry`.

Run 21 should verify the corrected header and re-export the three tables. Do not infer that the target methods are fully recovered from this schema fix.

## Interpretation and next work

1. The direct call evidence confirms that most statically visible call destinations in these three decompiler P-code exports are not covered by a Ghidra function definition. Load has one clear named target: `RuntimeSpineLoader.Read`.
2. This narrows the next task to candidate function-entry recovery for repeated helper/runtime call targets, plus checking their actual prologues, references, metadata entries and overlap with existing function bodies.
3. Do not create functions for every P-code address. A call target is a better candidate than an arbitrary interior instruction, but still needs validation against code boundaries, branch structure and method metadata.
4. Re-decompile only after validated helper boundaries are established. The current bounded reachable-flow counts remain limited evidence and do not prove complete CFG or source equivalence.
5. No iOS port, IPA, or successful runtime build is established by these results.
