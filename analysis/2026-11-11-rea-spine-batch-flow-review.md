# REA Spine batch flow recovery review — 2026-11-11

## Run and artifact

- Repository: `x676767xxxx/unity-il2cpp-rebuild`
- Branch: `progress/azurearchive-ios-port-20261031`
- Commit: `f926d7d640f36ef6722cc70752be67e0b15ef315`
- Actions run: [43001799392](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43001799392)
- Artifact: `azurearchive-rea-spine-flow-recovery`, ID `13846143207`, 4,419,063 bytes
- Artifact SHA-256: `afbff3a02c3d2856d7d43cdfd27b77a1190d0b623d53fa0c458929a802cf9626`
- Ghidra 12.1.4, Java 21, AARCH64:LE:64:v8A:default.

## Batch verification

The workflow completed successfully. The artifact contains 34 target rows, 34 C pseudocode exports, 34 per-function JSON records, the target TSV, linked REA evidence, and batch logs. All 34 rows have status `flow_decompiled_and_hash_verified`. Aggregate reachable/decoded instruction starts: 5,301 / 5,301; decode failures: 0. Every JSON record reports the Ghidra function-body instruction-start set equal to the bounded reachable decode set (no missing or extra body starts).

This validates the bounded reachable-flow extraction and exported pseudocode hashes. It does **not** establish full CFG completeness: the extractor follows reachable direct control flow, does not follow calls, and explicitly warns that indirect jump tables and exception LSDA edges may be incomplete. The P-code address accounting also reveals several significant attribution anomalies below.

## Key Spine methods

| Method | Loaded entry | Reachable/decoded starts | P-code addresses outside recovered body | Pseudocode bytes/chars |
|---|---:|---:|---:|---:|
| RuntimeSpineLoader.Create | 0x0335f32c | 509 / 509 | 13 | 8,808 |
| RuntimeSpineLoader.Load | 0x0335f2dc | 20 / 20 | 185 | 8,982 |
| PageTextureLoader.Load | 0x0336004c | 94 / 94 | 1 | 1,957 |
| PageTextureLoader.Unload | 0x033601c4 | 1 / 1 | 0 | 53 |
| RuntimeSpineAsset.Own | 0x0335fc74 | 1,531 / 1,531 | 128 | 29,730 |
| RuntimeSpineAsset.Retain | 0x03360490 | 25 / 25 | 1,295 | 51,777 |
| RuntimeSpineAsset.Dispose | 0x0335fd1c | 170 / 170 | 22 | 4,542 |

### Direct observations

- `PageTextureLoader.Load` resolves an atlas page path through a managed dictionary-like lookup, stores the resulting texture handle in the page object, lazily queries width/height if either dimension is zero, then selects one of two texture application/upload paths based on a page flag. Failure falls through an error/logging path. Some external callees remain unnamed.
- `PageTextureLoader.Unload` decompiles to an immediate return (one reachable instruction). This is evidence that this method is currently a no-op in this Android binary, not proof that texture memory is globally unmanaged; ownership may be handled elsewhere.
- `RuntimeSpineAsset.Dispose` decrements a field at object offset +0x18 and only enters the cleanup path when it reaches zero. The recovered body then iterates owned items, calls `RuntimeSpineLoader_DestroyOwned`, and performs further cleanup/release operations. The exact managed field names and all ownership semantics still require metadata/field-layout correlation.
- `RuntimeSpineLoader.Create` has a substantial recovered body (509 reachable instruction starts, no decode failures). Its pseudocode contains initialization, exception/cleanup paths, calls to ownership helpers, and asset construction logic. It is suitable for focused semantic annotation, but the low-level external calls and compiler-generated exception paths still need cross-reference validation.
- `RuntimeSpineLoader.Load` is a serious decompiler-attribution warning: only 20 reachable instruction starts belong to its bounded body, while its pseudocode is 8,982 characters and 185 P-code addresses are outside that body. The exported C listing must not be treated as a clean isolated decompilation of Load until this discrepancy is resolved.
- `RuntimeSpineAsset.Own` and `Retain` also have material P-code/body attribution discrepancies (128 and 1,295 external P-code addresses respectively). In particular, Retain has only 25 reachable body starts but a 51,777-character pseudocode listing. Its pseudocode is not presently reliable as an isolated method listing.
- `Dispose` has 22 P-code addresses outside its recovered body. Use its pseudocode as a provisional structural lead, not a complete method-level control-flow proof.

## Next analysis actions

1. Resolve P-code provenance for Load, Own, Retain, and Dispose. Export each P-code op's sequence address, instruction address, containing function, and call-target context; compare against the bounded body and metadata method-entry table.
2. Re-decompile suspect methods in a clean isolated Ghidra program/function context, preserving original function boundaries and recording why P-code addresses fall outside the body. Do not silently widen a function body to absorb neighboring methods.
3. Reconcile Create/Load/Read/ReadAsync call relationships against IL2CPP metadata signatures and callsites; annotate the meaningful external callees only after their own entry points are verified.
4. Inspect indirect branch/jump-table and exception landing-pad recovery for Create, Own, Retain, and Dispose.
5. Only after these checks, produce a semantic resource-lifecycle map and compare it with the old iOS 0.7.9.2 implementation.

## Scope boundary

No Unity project restoration, Android-to-iOS port, IPA build, or runtime compatibility result is established by this artifact. The current result is a batch of bounded ARM64 reachable-flow disassemblies and Ghidra pseudocode exports with explicit remaining attribution/control-flow limitations.
