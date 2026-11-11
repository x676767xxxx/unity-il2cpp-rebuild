# REA Spine P-code provenance audit — run 18 (2026-11-11)

## Run and artifact

- Branch: `progress/azurearchive-ios-port-20261031`
- Source commit: `c8b75e85d8e00144bc442fc0fe977a9d05fb95ca`
- Actions run: [43004125957](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43004125957)
- Result: success; all 34 targets recovered and verification step passed.
- Artifact: `azurearchive-rea-spine-flow-recovery`, ID `13847786518`, 4,490,417 bytes.
- Artifact SHA-256: `137f1033f136e36d3ac63dd32a6a12ddb606d79ede0b1813d4b112de2c771e93`.
- Downloaded ZIP parsed locally: 142 entries; 34 pseudocode files, 34 JSON records, 34 P-code provenance TSVs, and verification JSON.

## Batch-level verification

- All 34 target records are present.
- Every target reports reachable instruction count = decoded instruction count and zero decode failures.
- Summed reachable/decoded instruction starts: 5,301 / 5,301.
- The workflow's verification checks pseudocode SHA-256, character count, symbol name and entrypoint metadata.
- This verifies bounded reachable-flow recovery and artifact integrity, not complete CFG recovery, source equivalence, or an iOS build.

## Corrected provenance schema — important interpretation

The new TSV has six columns:
`pcode_address`, `containing_function_entry`, `containing_function_name`, `inside_target_recovered_body`, `containing_function_body_contains_address`, `opcode`.

The corrected target-body flag now tests membership in the function currently being decompiled. The owner-body flag tests membership in the function returned by Ghidra's function manager. When an owner exists, that second flag is expected to be true by construction; it is a consistency check, not independent proof.

Counts below use **distinct P-code sequence target addresses**, not raw P-code operation rows. A single instruction can emit many P-code ops. The raw TSV row count is therefore not a machine-instruction count.

## High-priority findings

| Method | Bounded reachable starts | Distinct P-code addresses | In target body | Other named owner body | No Ghidra owner |
|---|---:|---:|---:|---:|---:|
| RuntimeSpineLoader.Load | 20 | 188 | 3 | 0 | 185 |
| RuntimeSpineAsset.Retain | 25 | 1,306 | 11 | 0 | 1,295 |
| RuntimeSpineAsset.get_EstimatedTextureBytes | 123 | 622 | 50 | 0 | 572 |
| RuntimeSpineAsset.Own | 1,531 | 212 | 84 | 22 | 106 |
| RuntimeSpineAsset.Dispose | 170 | 128 | 106 | 22 | 0 |
| RuntimeSpineLoader.Create | 509 | 186 | 173 | 0 | 13 |
| PageTextureLoader.Load | 94 | 55 | 54 | 0 | 1 |
| PageTextureLoader.Unload | 1 | 1 | 1 | 0 | 0 |

### What this means

1. **Load, Retain, and get_EstimatedTextureBytes remain attribution blockers.** Their decompiler P-code address sets are much larger than the bounded method bodies, and almost all distinct P-code addresses have no Ghidra function owner. This is not proof that the decompiler is wrong: inline-expanded code, runtime thunks, missing function definitions, or address-space/analysis artifacts remain possible. But it means their current pseudocode must not be treated as a clean, isolated method listing.
2. **Own and Dispose show a useful positive control.** Each has 22 distinct P-code addresses owned by the named `RuntimeAtlasTexture_CanLoadPngAsDxt5` / `RuntimeSpineLoader_CanLoadPngAsDxt5` functions, outside the target body's range. This is consistent with Ghidra's decompiler incorporating code from separately identified callees/inlined regions. It demonstrates why “outside target body” alone is not an error signal.
3. **The large Own listing is still provisional.** Its P-code includes 106 unowned addresses and 22 addresses attributed to other named functions. The target body itself has 1,531 bounded instruction starts, so the listing should be read alongside its CFG and callee mapping, not as a standalone source reconstruction.
4. **Dispose is more interpretable than Retain:** 106/128 distinct P-code addresses lie in its target body, with 22 attributed to the two named helper functions and no unowned addresses. It is still not proof of complete exceptional-flow or ownership semantics.
5. **PageTextureLoader.Unload remains a one-instruction immediate-return implementation in this Android binary.** This supports describing this particular method as a no-op at the recovered entrypoint, not concluding that all texture memory is unmanaged or never released elsewhere.

## Additional examples of provenance behavior

- `RuntimeAtlasTexture.TryPngSize`: 136 bounded starts; 106 distinct P-code addresses, of which 96 are in target body and 10 unowned.
- `RuntimeSpineLoader.Create`: 509 bounded starts; 173/186 distinct P-code addresses in target body, 13 unowned.
- `RuntimeSpineLoader.ReadAsync`: 329 bounded starts; 10/11 in target body, one unowned.
- `RuntimeSpineLoader.ReadAsync` callback: 4 bounded starts; 40 distinct P-code addresses, 13 attributed to another named function and 27 unowned. The callback's decompiler result also needs separate scrutiny.

## Next required work

1. Inspect the disassembly and memory blocks for representative unowned addresses from `Load`, `Retain`, and `get_EstimatedTextureBytes`; determine whether they are executable code, runtime stubs, literal/data regions, or address-space artifacts.
2. Resolve direct calls and indirect callsites from each bounded method body to their actual target addresses. Do not infer a call graph from P-code sequence targets alone.
3. Add missing Ghidra function definitions for confirmed executable helper/thunk regions, then rerun decompilation and provenance. Keep the old artifact for comparison.
4. Reconstruct `Retain`/consumer-count/ownership/disposal semantics only after mapping the real field offsets, callees, and exceptional exits.
5. Compare the resulting Android behavior with the old iOS 0.7.9.2 binary/resource format. Spine Essential licensing permits use within its license terms but does not itself resolve Unity/iOS API, asset format, or runtime compatibility.

## Scope and non-claims

This run does **not** prove full CFG completeness, complete exception handling, source-level equivalence, successful Android-to-iOS porting, IPA generation, or runtime success on iPhone. The evidence is a reproducible bounded-flow decompilation plus per-address provenance audit.
