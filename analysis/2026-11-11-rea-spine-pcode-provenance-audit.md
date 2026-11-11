# REA Spine P-code provenance audit — 2026-11-11

## Inputs

- Branch: `progress/azurearchive-ios-port-20261031`
- Provenance-schema run: [43003746737](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43003746737)
- Source commit used by run 17: `720bb62a37bf1700fbb51a86edde7bf0501dcbbb`
- Artifact: `azurearchive-rea-spine-flow-recovery`, ID `13847442265`
- Artifact size: 4,486,566 bytes
- Artifact digest: `sha256:840040dfdac2e95bb8f5d959eb1cb2eda7727baf1e66025ff99da47e2a3a940c`

## Mechanical validation

The downloaded ZIP was opened and parsed locally.

- 34 target records and 34 per-function JSON files were present.
- All 34 per-function JSON records contained a `pcode_provenance_file` pointer resolving to a TSV present in the archive.
- All 34 TSV files parsed with the expected five-column header.
- Across the TSVs there were 55,634 P-code operation rows: 18,706 rows had a containing Ghidra function, and 36,928 had no containing function.
- These are P-code operation rows, not unique machine instructions or unique code addresses. Repeated rows can refer to the same instruction address.

## Important schema defect discovered

The run-17 column named `inside_recovered_body` did **not** test whether the P-code address belonged to the target method's recovered body. The implementation called `owner.getBody().contains(pcodeAddress)`, where `owner` is the function returned by `getFunctionContaining(pcodeAddress)`. Consequently, the value was effectively true whenever an owner existed and false when no owner existed. It did not resolve the earlier target-body attribution question.

Examples include:
- `RuntimeSpineLoader.Load`: 5,516 P-code operation rows; 5,440 have no containing Ghidra function.
- `RuntimeSpineAsset.Retain`: 18,328 rows; 18,055 have no containing Ghidra function.
- `RuntimeSpineAsset.get_EstimatedTextureBytes`: 6,153 rows; 5,405 have no containing Ghidra function.
- `RuntimeSpineAsset.Dispose`: 1,012 rows; all have a containing function, but that alone does not establish that every P-code op belongs to Dispose.

The no-owner rows are unresolved attribution, not proof of wrong code, a bad call edge, or incorrect decompilation. The row counts also cannot be read as percentages of instructions because P-code emits multiple operations per instruction.

## Corrective source change

Commit `c8b75e85d8e00144bc442fc0fe977a9d05fb95ca` changes the provenance TSV schema to separate:
- `inside_target_recovered_body`: tests `function.getBody().contains(pcodeAddress)` for the method currently being decompiled.
- `containing_function_body_contains_address`: tests the body of the function returned by Ghidra's function manager.

This removes the previous ambiguous/misleading field. The next successful run must validate both columns and compare target-body membership with the per-method bounded instruction-start set. The correction is committed; a new Actions run and its artifact still need verification.

## Interpretation and next work

1. Re-run the batch using the corrected two-body provenance columns.
2. For `Load`, `Retain`, and `get_EstimatedTextureBytes`, group P-code operations by distinct instruction address and separately record target-body membership, containing function entry, and no-owner addresses.
3. Inspect no-owner addresses against the method-entry map, instruction listing, and direct/indirect callsites before attributing them.
4. Only then decide whether a clean isolated re-decompilation or more accurate function definitions are needed.
5. Keep the previously recovered bounded-flow pseudocode provisional where attribution remains unresolved.

## Scope

This audit verifies archive structure and TSV parsing. It does not prove full CFG recovery, complete P-code attribution, source-level equivalence, an iOS port, or a working IPA.
