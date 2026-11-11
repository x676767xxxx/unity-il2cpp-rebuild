# REA Spine resource I/O evidence review — 2026-11-11

## Scope

Reviewed the GitHub Actions artifact `azurearchive-rea-spine-resource-io` from run `42995578057` on branch `progress/azurearchive-ios-port-20261031`, head `0b263f6d69e1c79a36f8bfba35ac945fe502c324`.

The artifact contains five metadata-linked Ghidra pseudocode exports for Android `libil2cpp.so`, plus per-function JSON metadata, a verification manifest, and linked REA evidence.

## Integrity and boundaries

All five exports are marked `extracted_and_hash_verified` in `resource-io-verification.json`. This verifies the exported pseudocode bytes against the recorded hashes; it does not establish complete control-flow recovery.

| Method | ELF RVA | Instructions | Pseudocode chars | SHA-256 |
|---|---:|---:|---:|---|
| RuntimeAtlasTexture.TryPngSize | 0x325c770 | 136 | 3406 | a8230a03ebfc75d706169025ca68c2d9e1d05fb57348d36fa248b0d6933e3223 |
| RuntimeSpineLoader.DestroyOwned | 0x325e994 | 51 | 2809 | 4cd3186e858c6e898a50a8d525b6b8d7cddd0ef9612870cbb9c24e32f5569b83 |
| RuntimeSpineLoader.AtlasPages | 0x325ea60 | 225 | 7137 | 078c9603e9a9451b1fd5f9c38b95467d9b3850bd7a3e58fa7e955bac7d7db4dc |
| RuntimeSpineLoader.Read | 0x325ee08 | 179 | 2833 | 7b80e0f803e175032cef93b6beeb27ec24dda6d6edcf0579d5a16b6844f96b7b |
| RuntimeSpineLoader.ReadAsync | 0x325f1c0 | 69 | 3630 | 36d3133e6d137ce6ef05200fc6de32c91b83332799c802e36e1aa6aec5ccbde3 |

## Preliminary code observations

### RuntimeAtlasTexture.TryPngSize

The pseudocode visibly:
- zeroes the output width and height before parsing;
- checks the 8-byte PNG signature;
- checks for an IHDR chunk marker;
- reads width and height from offsets corresponding to PNG IHDR fields;
- rejects zero/non-positive dimensions and dimensions whose low two bits are set (both dimensions must be divisible by four);
- performs a size/overflow-style arithmetic check before returning success.

This is a preliminary interpretation of the decompiler output. Endianness, exact helper semantics, signedness, and the arithmetic guard must be confirmed from ARM64 instructions and the callee at RVA 0x325e4f4 before porting.

### RuntimeSpineLoader.Read / ReadAsync

The current exports are dominated by IL2CPP static initialization, generic/runtime helper calls, and object/task setup. The ReadAsync export is especially small at its metadata-bounded entry range and does not expose the asynchronous body in a directly readable way. Do not treat either export as a complete reconstruction of file I/O yet.

### RuntimeSpineLoader.AtlasPages

The export shows collection/enumerator-style operations, iteration over entries, and disposal/finalization paths. It also contains numerous Ghidra warnings about removed unreachable blocks and an out-of-line branch target. Its iterator/async state transitions and full CFG are not yet sufficiently validated to translate into source.

### RuntimeSpineLoader.DestroyOwned

The export checks object/state conditions and reaches indirect dispatch paths. It appears to select different cleanup paths depending on runtime state. Exact ownership semantics and the indirect callees remain unresolved; do not implement a guessed destructor from this pseudocode alone.

## Critical limitations

The artifact itself records these limitations:
- metadata-bounded extraction is not proof of complete CFG or LSDA coverage;
- pseudocode is not recovered C# source;
- Android ARM64 RVAs are not iOS addresses;
- no Unity project or IPA is produced.

The per-function JSON also warns that decompiler output may reference out-of-line branch blocks beyond the seeded interval. The Ghidra run used `noanalysis plus explicit metadata-bounded function body`, so a follow-up must not rely solely on these decompiler exports.

## Required next evidence pass

1. Re-import the exact same APK and `global-metadata.dat` pair and record their SHA-256 values in the report.
2. Disassemble each target's ARM64 range and all direct branch targets, including blocks outside the metadata-bounded interval.
3. Run Ghidra analysis on a separate project copy; compare function boundaries and CFG against the no-analysis export.
4. Resolve the callees used by `TryPngSize` (especially RVA 0x325e4f4) and the indirect cleanup dispatch targets in `DestroyOwned`.
5. Recover the compiler-generated iterator/async state-machine types and their MoveNext methods for `ReadAsync` and `AtlasPages`.
6. Only after the above, compare behavior with the legacy iOS 0.7.9.2 implementation and write an implementation spec. Do not copy Android addresses into iOS or claim a working IPA.

## Current conclusion

The artifact is a valid, hash-verified preliminary evidence set. It is useful for narrowing the next reverse-engineering pass, but it is not yet sufficient to implement the Spine resource loader or diagnose the Husk runtime crash by itself.


## Additional metadata cross-check and next target addresses

The linked REA evidence records this exact input pair:
- `libil2cpp.so` (ARM64, 120,315,032 bytes): SHA-256 `7f15b8e7557eb7948fbea746c629fb15d442d05c649ab2db3667ae08aa2c9b6`.
- `global-metadata.dat` (17,132,296 bytes): SHA-256 `717236ea9c650b4fcd373e0afc7c03c25d5949726bebe251d3e8301e4da7a144`.

The metadata links the relevant declaring types and return types as follows:
- `RuntimeSpineLoader.Read` returns `RuntimeSpineLoader.Files`.
- `RuntimeSpineLoader.ReadAsync` returns `Task<RuntimeSpineLoader.Files>`.
- `RuntimeSpineLoader.AtlasPages` returns `string[]`.
- `RuntimeSpineLoader.Files` is a nested type with a constructor.
- `RuntimeSpineLoader.PageTextureLoader` has `Load` and `Unload` methods.
- `RuntimeAtlasTexture` has a broader cache/texture pipeline: `Read`, `Load`, `CreateTexture`, `DestroyTexture`, `TryReadCache`, `QueueWrite`, `WriteCache`, `IsCacheFileError`, `Checksum`, `ReadChecksum`, `FinishPendingWritesAsync`, `CanLoadPngAsDxt5`, `DxtBytes`, `TryPngSize`, and `EstimateTextureBytes`.

Exact next ARM64 method-pointer RVAs recovered from the same metadata evidence (not iOS addresses):

| Method | Method index | ELF RVA |
|---|---:|---:|
| RuntimeAtlasTexture.Read | 25270 | 0x325c600 |
| RuntimeAtlasTexture.Load | 25271 | 0x325d080 |
| RuntimeAtlasTexture.CreateTexture | 25272 | 0x325d49c |
| RuntimeAtlasTexture.TryReadCache | 25274 | 0x325c9f8 |
| RuntimeAtlasTexture.QueueWrite | 25275 | 0x325d5f4 |
| RuntimeAtlasTexture.WriteCache | 25276 | 0x325daf0 |
| RuntimeAtlasTexture.ReadChecksum | 25279 | 0x325d8f0 |
| RuntimeAtlasTexture.FinishPendingWritesAsync | 25280 | 0x325e2e4 |
| RuntimeAtlasTexture.CanLoadPngAsDxt5 | 25281 | 0x325e484 |
| RuntimeAtlasTexture.DxtBytes | 25282 | 0x325d94c |
| RuntimeAtlasTexture.EstimateTextureBytes | 25284 | 0x325e56c |
| RuntimeSpineLoader.Load | 25295 | 0x325f2dc |
| RuntimeSpineLoader.Create | 25296 | 0x325f32c |
| PageTextureLoader.Load | 25300 | 0x326004c |
| PageTextureLoader.Unload | 25301 | 0x32601c4 |
| RuntimeSpineAsset.Dispose | 25316 | 0x325fd1c |

Priority for the next decompilation pass:
1. `RuntimeAtlasTexture.Read/Load/TryReadCache/CreateTexture` to establish the PNG/cache/texture path and its memory cost.
2. `PageTextureLoader.Load/Unload` and `RuntimeSpineAsset.Dispose` to establish page texture ownership and release behavior.
3. `RuntimeSpineLoader.Load/Create` to connect file parsing to Spine runtime construction.
4. `RuntimeAtlasTexture.FinishPendingWritesAsync/WriteCache/QueueWrite` to understand background cache writes and task completion.

The current artifact does not contain these next functions' pseudocode; the RVAs above are metadata-derived target pointers only. They must be independently disassembled, CFG-checked, and decompiled before drawing behavioral conclusions.


## Follow-up evidence review — runs 43000801629, 43001386531, and 43001483524

Checked the current branch on 2026-11-11. The branch head is [0e4fd512caa1f96ea7c5f7d6a8a2868f49217dd6](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/0e4fd512caa1f96ea7c5f7d6a8a2868f49217dd6), titled “Batch Spine texture and ownership analysis in one Ghidra import.” No pull requests were returned for this repository.

### Successful six-target bounded-flow artifact — run 43000801629

[Run 43000801629](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43000801629) completed successfully on commit `f284da5992c863056cb36e6248f5c45636b822e6`; all 11 steps passed. Artifact [13846211764](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43000801629/artifacts/13846211764) is 4,355,124 bytes, with GitHub ZIP digest `sha256:dc85d8ca5783191ed02c191864da8c87fc172a37d753ea5ccdc6e50a98ed296b`. I downloaded and inspected the ZIP: it contains 29 files, including six pseudocode exports, six JSON sidecars, six per-target logs, the target TSV, input hashes, linked REA evidence, and a verification manifest.

The input pair is unchanged:
- ARM64 `libil2cpp.so`: 120,315,032 bytes; SHA-256 `7f15b8e7557eb7948fbea746c629fb15d442d05c649ab2db3667ae08aa2c9b6`.
- `global-metadata.dat`: 17,132,296 bytes; metadata v29; SHA-256 `717236ea9c650b4fcd373e0afc7c03c25d5949726bebe251d3e8301e4da7a144`.

The Ghidra import base is `0x100000`; the following are Android ELF RVAs, not iOS addresses:

| Method | Android ELF RVA | Reachable / decoded instruction starts | Pseudocode chars |
|---|---:|---:|---:|
| `RuntimeAtlasTexture.TryPngSize` | `0x325c770` | 136 / 136 | 3,406 |
| `RuntimeAtlasTexture.<TryPngSize>g__Read\|19_0` | `0x325e4f4` | 30 / 30 | 4,698 |
| `RuntimeSpineLoader.DestroyOwned` | `0x325e994` | 51 / 51 | 2,809 |
| `RuntimeSpineLoader.AtlasPages` | `0x325ea60` | 225 / 225 | 7,137 |
| `RuntimeSpineLoader.Read` | `0x325ee08` | 179 / 179 | 2,833 |
| `RuntimeSpineLoader.ReadAsync` | `0x325f1c0` | 329 / 329 | 3,630 |

All six traversals reported zero decode failures, and the exact Ghidra function-body instruction-start sets matched the script's bounded traversal sets. This is useful bounded-reachability evidence. The artifact does not export a direct-branch edge table or a complete CFG, and the script's explicit limits remain: indirect jump tables may be unresolved, exception/LSDA edges may be incomplete, and reachable blocks do not prove a complete source-level CFG.

### Decompiler coverage diagnostic — important qualification

The same JSON sidecars compare unique instruction addresses represented in Ghidra HighFunction P-code with the bounded traversal set. The reported values are diagnostic counts, not a literal percentage of machine instructions translated:

| Method | P-code instruction addresses | Traversed starts missing from P-code | P-code addresses outside traversed set |
|---|---:|---:|---:|
| `TryPngSize` | 106 | 40 | 10 |
| `TryPngSize` local reader | 141 | 3 | 114 |
| `DestroyOwned` | 83 | 25 | 57 |
| `AtlasPages` | 176 | 89 | 40 |
| `Read` | 40 | 166 | 27 |
| `ReadAsync` | 11 | 319 | 1 |

The exact function-body set check passed, but the P-code address sets still differ materially. In particular, the local reader has 114 P-code addresses outside its 30-instruction traversal set; `ReadAsync` has only 11 unique P-code addresses against 329 traversed starts; and `Read` has 40 against 179. These results do not prove the extra P-code addresses are invalid, nor that missing P-code addresses are unreachable; they do mean the decompiler output cannot be accepted as a complete semantic representation without address-by-address disassembly and edge attribution. The workflow verifier currently records these mismatches but does not fail on them.

### Preliminary observations from the new local-reader export

The `TryPngSize` pseudocode calls metadata-linked helper `RuntimeAtlasTexture.<TryPngSize>g__Read|19_0` at RVA `0x325e4f4`. Its exported C-like pseudocode appears to bounds-check a byte-array read and assemble four bytes in big-endian order. The caller appears to check the PNG signature, IHDR length/marker, reads dimensions at offsets `0x10` and `0x14`, requires positive dimensions divisible by four, and applies an upper-bound arithmetic guard. These remain decompiler-derived preliminary interpretations: the helper's P-code/body mismatch and lack of a raw ARM64 branch-target audit mean endianness, unsigned-overflow behavior, and exact allocation arithmetic are not yet instruction-verified.

`DestroyOwned` still contains unresolved indirect dispatch/jump-table warnings. `AtlasPages` and `Read` retain PIC/unreachable-block warnings. `ReadAsync` remains a very short wrapper-like pseudocode export despite its 329-instruction bounded traversal; metadata identifies its return type as `Task<RuntimeSpineLoader.Files>`, but this artifact does not establish the downstream task implementation or a corresponding Spine-loader state machine.

### Expanded 34-target metadata inventory — run 43001483524

The next workflow revision [run 43001483524](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43001483524), on head commit `0e4fd512caa1f96ea7c5f7d6a8a2868f49217dd6`, successfully:
- downloaded/extracted the Android 1.0 APK inputs;
- reproduced both input SHA-256 values above;
- linked the REA metadata;
- generated a 34-row `flow-targets.tsv`.

The TSV and linked evidence are preserved in partial artifact [13846152215](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43001483524/artifacts/13846152215), 4,331,291 bytes, digest `sha256:8996e713b79fc3108fa450fda59ade712daebf99087cf5bf80139dc39fdc65bc`. I downloaded and inspected it. It contains only `batch-flow-recovery.log`, `flow-targets.tsv`, `input-sha256.txt`, `linked-evidence.json`, and `metadata-link.log`; it contains no per-target pseudocode, no per-target Ghidra JSON, and no flow verification manifest.

The 34-row target inventory does include the requested cache/texture, loader, page-texture, and asset-lifecycle targets. Metadata-derived Android ELF RVAs are:

| Method | Metadata index | Android ELF RVA |
|---|---:|---:|
| `RuntimeAtlasTexture.Read` | 25270 | `0x325c600` |
| `RuntimeAtlasTexture.Load` | 25271 | `0x325d080` |
| `RuntimeAtlasTexture.CreateTexture` | 25272 | `0x325d49c` |
| `RuntimeAtlasTexture.DestroyTexture` | 25273 | `0x325d558` |
| `RuntimeAtlasTexture.TryReadCache` | 25274 | `0x325c9f8` |
| `RuntimeAtlasTexture.QueueWrite` | 25275 | `0x325d5f4` |
| `RuntimeAtlasTexture.WriteCache` | 25276 | `0x325daf0` |
| `RuntimeAtlasTexture.IsCacheFileError` | 25277 | `0x325d988` |
| `RuntimeAtlasTexture.Checksum` | 25278 | `0x325c990` |
| `RuntimeAtlasTexture.ReadChecksum` | 25279 | `0x325d8f0` |
| `RuntimeAtlasTexture.FinishPendingWritesAsync` | 25280 | `0x325e2e4` |
| `RuntimeAtlasTexture.CanLoadPngAsDxt5` | 25281 | `0x325e484` |
| `RuntimeAtlasTexture.DxtBytes` | 25282 | `0x325d94c` |
| `RuntimeAtlasTexture.TryPngSize` | 25283 | `0x325c770` |
| `RuntimeAtlasTexture.EstimateTextureBytes` | 25284 | `0x325e56c` |
| `RuntimeAtlasTexture.<TryPngSize>g__Read\|19_0` | 25286 | `0x325e4f4` |
| `Source.get_HasDxtCache` | 25287 | `0x325e898` |
| `<>c__DisplayClass11_0.<QueueWrite>b__0` | 25290 | `0x325e8a8` |
| `RuntimeSpineLoader.DestroyOwned` | 25291 | `0x325e994` |
| `RuntimeSpineLoader.AtlasPages` | 25292 | `0x325ea60` |
| `RuntimeSpineLoader.Read` | 25293 | `0x325ee08` |
| `RuntimeSpineLoader.ReadAsync` | 25294 | `0x325f1c0` |
| `RuntimeSpineLoader.Load` | 25295 | `0x325f2dc` |
| `RuntimeSpineLoader.Create` | 25296 | `0x325f32c` |
| `RuntimeSpineLoader.CanLoadPngAsDxt5` | 25297 | `0x325ffc8` |
| `PageTextureLoader.Load` | 25300 | `0x326004c` |
| `PageTextureLoader.Unload` | 25301 | `0x32601c4` |
| `<>c__DisplayClass4_0.<ReadAsync>b__0` | 25306 | `0x3260250` |
| `<>c__DisplayClass6_0.<Create>b__0` | 25308 | `0x3260260` |
| `RuntimeSpineAsset.get_HasConsumers` | 25311 | `0x32602dc` |
| `RuntimeSpineAsset.get_EstimatedTextureBytes` | 25312 | `0x32602ec` |
| `RuntimeSpineAsset.Own` | 25313 | `0x325fc74` |
| `RuntimeSpineAsset.Retain` | 25314 | `0x3260490` |
| `RuntimeSpineAsset.Dispose` | 25316 | `0x325fd1c` |

These are metadata-linked method-pointer RVAs from the same ARM64 ELF/metadata pair. In this failed run they are target mappings only, not newly disassembled or decompiled evidence.

### Confirmed cause of the 34-target batch failure

Run 43001483524 failed in step 9, “Recover all reachable control-flow targets in one Ghidra import.” The current workflow file's TSV reader uses a double-escaped tab literal in its `IFS` assignment, so the shell receives a literal backslash plus `t` rather than a tab delimiter. The six tab-separated columns are therefore not split. The Ghidra script receives malformed arguments; the archived log shows examples where the first script argument contains fragments of the TSV row, followed by:

`java.lang.NumberFormatException` at `PrepareFlowFunction.java:36`.

The same malformed loop appears again in the verification step, which was skipped after step 9 failed. This is a confirmed workflow quoting/TSV parsing defect, not a Ghidra binary-analysis failure and not evidence that the 34 methods lack code. No source or workflow file was modified during this review.

A preceding run, [43001386531](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43001386531), was cancelled during metadata linking after a newer push; its partial artifact [13847005216](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43001386531/artifacts/13847005216) is not a successful flow-recovery result.

## Updated evidence classification

### Verified facts
- The current branch head is `0e4fd512caa1f96ea7c5f7d6a8a2868f49217dd6`; no PRs are open/returned.
- The same Android 1.0 ARM64 ELF and metadata hashes are reproduced in the latest run.
- Run 43000801629 produced six bounded reachable-flow exports and matching function-body instruction-start sets, with zero decode failures.
- The expanded run 43001483524 generated and archived 34 metadata-linked target rows, but failed before producing any new target pseudocode.
- The 430014 partial artifact's hash validates the ZIP bytes only; it does not validate CFG recovery or any target body.

### Preliminary inferences
- The local reader helper appears to read 32-bit PNG fields in big-endian order.
- The caller's four-byte dimension alignment and size guard may relate to compressed-texture constraints, but the exact DXT5 policy and allocation size are not established.
- `ReadAsync` appears wrapper-like; no associated Spine-loader state-machine implementation has been proven.

### Unresolved
- Direct branch target inventory, instruction-by-instruction ARM64 disassembly, full CFG including indirect branches and LSDA/exception edges.
- Attribution of the P-code addresses outside each bounded function body and the missing P-code addresses.
- Actual cache hit/miss path, checksum semantics, PNG-to-DXT5 conversion, texture memory formula, and pending-write task completion.
- `PageTextureLoader.Load/Unload`, `RuntimeSpineAsset.Own/Retain/Dispose`, and `DestroyOwned` ownership/release behavior.
- The relationship between `ReadAsync` and any compiler-generated state machine or downstream async helper.
- A body-level comparison against the old iOS 0.7.9.2 binary. Existing metadata/API-gap evidence says the old iOS build lacks the new runtime loader/asset/texture-loader API surface; Android RVAs must not be reused as iOS addresses.

### Next evidence pass
1. Correct the TSV delimiter in the workflow and validate the parsed six fields before invoking Ghidra; do not modify this report's evidence status until a new artifact contains target C files and JSON sidecars.
2. Make P-code/body-set discrepancies a visible acceptance gate or explicitly classify/attribute each mismatch; retain the exact body-start equality check.
3. Export per-instruction disassembly and direct branch/fall-through edges, including the exact skipped method-entry edges and cap/termination reason.
4. Prioritize `RuntimeAtlasTexture.Read/Load/TryReadCache/CreateTexture`, `CanLoadPngAsDxt5/DxtBytes/EstimateTextureBytes`, then `WriteCache/FinishPendingWritesAsync`.
5. Trace `PageTextureLoader.Load/Unload` and `RuntimeSpineAsset.Own/Retain/Dispose` together with `DestroyOwned` before making ownership claims.
6. Recover the actual `ReadAsync` downstream task/state-machine path and compare behavior with old iOS only using the matching iOS binary/metadata.

No original C# source, restored Unity project, iOS compilation, or IPA is established by these artifacts.
\\t' read -r index type method symbol stem entry; do
```

The file has two literal backslashes before `t`, not the single ANSI-C escape needed to produce a tab delimiter. Therefore the shell does not split the tab-separated target rows into six columns. The Ghidra script receives malformed arguments; the archived log shows examples where the first script argument contains fragments of the TSV row, followed by:

`java.lang.NumberFormatException` at `PrepareFlowFunction.java:36`.

The same malformed loop appears again in the verification step, which was skipped after step 9 failed. This is a confirmed workflow quoting/TSV parsing defect, not a Ghidra binary-analysis failure and not evidence that the 34 methods lack code. No source or workflow file was modified during this review.

A preceding run, [43001386531](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43001386531), was cancelled during metadata linking after a newer push; its partial artifact [13847005216](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43001386531/artifacts/13847005216) is not a successful flow-recovery result.

## Updated evidence classification

### Verified facts
- The current branch head is `0e4fd512caa1f96ea7c5f7d6a8a2868f49217dd6`; no PRs are open/returned.
- The same Android 1.0 ARM64 ELF and metadata hashes are reproduced in the latest run.
- Run 43000801629 produced six bounded reachable-flow exports and matching function-body instruction-start sets, with zero decode failures.
- The expanded run 43001483524 generated and archived 34 metadata-linked target rows, but failed before producing any new target pseudocode.
- The 430014 partial artifact's hash validates the ZIP bytes only; it does not validate CFG recovery or any target body.

### Preliminary inferences
- The local reader helper appears to read 32-bit PNG fields in big-endian order.
- The caller's four-byte dimension alignment and size guard may relate to compressed-texture constraints, but the exact DXT5 policy and allocation size are not established.
- `ReadAsync` appears wrapper-like; no associated Spine-loader state-machine implementation has been proven.

### Unresolved
- Direct branch target inventory, instruction-by-instruction ARM64 disassembly, full CFG including indirect branches and LSDA/exception edges.
- Attribution of the P-code addresses outside each bounded function body and the missing P-code addresses.
- Actual cache hit/miss path, checksum semantics, PNG-to-DXT5 conversion, texture memory formula, and pending-write task completion.
- `PageTextureLoader.Load/Unload`, `RuntimeSpineAsset.Own/Retain/Dispose`, and `DestroyOwned` ownership/release behavior.
- The relationship between `ReadAsync` and any compiler-generated state machine or downstream async helper.
- A body-level comparison against the old iOS 0.7.9.2 binary. Existing metadata/API-gap evidence says the old iOS build lacks the new runtime loader/asset/texture-loader API surface; Android RVAs must not be reused as iOS addresses.

### Next evidence pass
1. Correct the TSV delimiter in the workflow and validate the parsed six fields before invoking Ghidra; do not modify this report's evidence status until a new artifact contains target C files and JSON sidecars.
2. Make P-code/body-set discrepancies a visible acceptance gate or explicitly classify/attribute each mismatch; retain the exact body-start equality check.
3. Export per-instruction disassembly and direct branch/fall-through edges, including the exact skipped method-entry edges and cap/termination reason.
4. Prioritize `RuntimeAtlasTexture.Read/Load/TryReadCache/CreateTexture`, `CanLoadPngAsDxt5/DxtBytes/EstimateTextureBytes`, then `WriteCache/FinishPendingWritesAsync`.
5. Trace `PageTextureLoader.Load/Unload` and `RuntimeSpineAsset.Own/Retain/Dispose` together with `DestroyOwned` before making ownership claims.
6. Recover the actual `ReadAsync` downstream task/state-machine path and compare behavior with old iOS only using the matching iOS binary/metadata.

No original C# source, restored Unity project, iOS compilation, or IPA is established by these artifacts.


## Corrected 34-target batch completed — run 43001799392

The TSV delimiter defect described above was corrected by workflow commit [f926d7d640f36ef6722cc70752be67e0b15ef315](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/f926d7d640f36ef6722cc70752be67e0b15ef315). The corrected shell loop now uses a single ANSI-C tab escape in `IFS`. The resulting [run 43001799392](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43001799392), on that commit, completed successfully; all 11 steps passed, including target generation, the single Ghidra import, output verification, and artifact upload.

Artifact [13846143207](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43001799392/artifacts/13846143207) is 4,419,063 bytes; GitHub ZIP digest is `sha256:afbff3a02c3d2856d7d43cdfd27b77a1190d0b623d53fa0c458929a802cf9626`. I downloaded and inspected the ZIP. It contains 108 files: 34 pseudocode C exports, 34 per-target JSON sidecars, 34 skipped-method-entry lists, the 34-row target TSV, the aggregate verification JSON, batch log, metadata-link log, linked REA evidence, and input hashes. All 34 targets have a non-empty pseudocode export and JSON sidecar. Every sidecar reports zero decode failures, and every exact Ghidra function-body instruction-start set matches its bounded traversal set. The two input SHA-256 values match the earlier runs exactly.

### What is now established

The 34-target inventory from the preceding section now has actual bounded-flow pseudocode exports, rather than only metadata pointers. It includes every requested primary target:
- `RuntimeSpineLoader.Read/ReadAsync/AtlasPages/Load/Create`;
- `RuntimeAtlasTexture.Read/Load/TryReadCache/CreateTexture/WriteCache/FinishPendingWritesAsync/TryPngSize`;
- `PageTextureLoader.Load/Unload`;
- `RuntimeSpineAsset.Dispose`.

It also includes related helpers and dependencies: `DestroyTexture`, `QueueWrite`, `IsCacheFileError`, `Checksum/ReadChecksum`, `CanLoadPngAsDxt5`, `DxtBytes`, `EstimateTextureBytes`, `Source.get_HasDxtCache`, the QueueWrite/ReadAsync/Create closures, `RuntimeSpineAsset.Own/Retain`, and texture-byte/consumer getters.

The workflow's “success” is specifically bounded traversal + function-body assignment + non-empty pseudocode/JSON + file hash verification. It is not a complete CFG acceptance test. The script records P-code mismatch metrics in each sidecar, but does not fail when P-code addresses are missing from or outside the bounded set. No per-instruction direct-branch edge list is exported. The script also explicitly does not guarantee indirect jump-table or exception/LSDA recovery.

### P-code/body mismatch remains the primary quality gate

The following selected sidecar metrics compare bounded instruction starts with unique addresses represented by Ghidra HighFunction P-code. They are diagnostic counts, not a literal percentage of machine instructions translated:

| Target | Bounded starts | P-code addresses | Starts missing from P-code | P-code addresses outside bounded set |
|---|---:|---:|---:|---:|
| `RuntimeAtlasTexture.Read` | 90 | 9 | 82 | 1 |
| `RuntimeAtlasTexture.Load` | 245 | 234 | 120 | 109 |
| `RuntimeAtlasTexture.CreateTexture` | 47 | 88 | 33 | 74 |
| `RuntimeAtlasTexture.TryReadCache` | 379 | 272 | 107 | 0 |
| `RuntimeAtlasTexture.WriteCache` | 385 | 299 | 160 | 74 |
| `RuntimeAtlasTexture.FinishPendingWritesAsync` | 100 | 73 | 42 | 15 |
| `RuntimeAtlasTexture.TryPngSize` | 136 | 106 | 40 | 10 |
| `RuntimeAtlasTexture.<TryPngSize>g__Read|19_0` | 30 | 141 | 3 | 114 |
| `RuntimeAtlasTexture.DxtBytes` | 15 | 15 | 0 | 0 |
| `RuntimeAtlasTexture.EstimateTextureBytes` | 125 | 115 | 37 | 27 |
| `RuntimeSpineLoader.AtlasPages` | 225 | 176 | 89 | 40 |
| `RuntimeSpineLoader.Read` | 179 | 40 | 166 | 27 |
| `RuntimeSpineLoader.ReadAsync` | 329 | 11 | 319 | 1 |
| `RuntimeSpineLoader.Load` | 20 | 188 | 17 | 185 |
| `RuntimeSpineLoader.Create` | 509 | 186 | 336 | 13 |
| `PageTextureLoader.Load` | 94 | 55 | 40 | 1 |
| `PageTextureLoader.Unload` | 1 | 1 | 0 | 0 |
| `RuntimeSpineAsset.Own` | 1,531 | 212 | 1,447 | 128 |
| `RuntimeSpineAsset.Retain` | 25 | 1,306 | 14 | 1,295 |
| `RuntimeSpineAsset.Dispose` | 170 | 128 | 64 | 22 |
| `RuntimeSpineAsset.get_EstimatedTextureBytes` | 123 | 622 | 73 | 572 |

The mismatches are not automatically proof of incorrect decompilation: out-of-set P-code can arise from inlined/callee or exception-related code, and instructions may not all yield ordinary P-code. They do, however, require address attribution before treating a pseudocode export as a clean body. Examples are particularly severe for `ReadAsync`, `Read`, `Load`, `Own`, `Retain`, and `get_EstimatedTextureBytes`. The `RuntimeAtlasTexture.Load` sidecar's out-of-set sample begins at the `CreateTexture` entry, and `RuntimeSpineLoader.Load`'s begins at the `Create` entry; whether these are decompiler inlining/tail-call expansion or function-scope artifacts remains unresolved.

### Preliminary texture/cache findings from the exported pseudocode

These observations are useful leads, not source-level facts:

1. **DXT byte count.** `RuntimeAtlasTexture.DxtBytes` has the cleanest metric in this batch (15 bounded starts, 15 P-code addresses, no address mismatch). Its pseudocode computes a rounded-up count of 4×4 blocks and multiplies by 16 bytes per block. This is consistent with DXT5 block sizing. The overflow/rounding branches still need ARM64 instruction confirmation.

2. **Texture memory estimate.** `EstimateTextureBytes` branches on apparent Unity texture-format enum values: `12` uses 16 bytes per 4×4 block, `10` uses 8 bytes per block, `3` uses 3 bytes per pixel, and other formats use 4 bytes per pixel. The result appears to be doubled when the texture's mipmap flag is set. Mapping the numeric formats to Unity enum names and verifying the mipmap multiplier from the exact instructions remains required.

3. **Cache header and validation.** `TryReadCache` appears to require a file length of at least `0x5c` (92 bytes), compare an eight-byte little-endian value corresponding to the ASCII marker `AAADXT5` plus a terminator, check a version value of `2`, compare width/height, validate a checksum, and require the payload length to match the DXT byte estimate plus the `0x5c` header. `WriteCache` appears to emit the same marker/version/dimension/checksum fields and payload. This suggests a custom DXT5 cache container; the precise field offsets, checksum algorithm, and failure/delete policy still need disassembly and call-target mapping.

4. **Queued writes.** `QueueWrite` appears to capture the source, dimensions, and byte buffer in a closure, validate that the buffer length matches the DXT5 block-size calculation, then enqueue work. The metadata-linked `<QueueWrite>b__0` callback calls `WriteCache`. `FinishPendingWritesAsync` appears to enumerate pending task objects and await/aggregate them, but the exact task-completion semantics and cancellation/error behavior are not established.

5. **PNG-to-DXT5 eligibility.** Both `RuntimeAtlasTexture.CanLoadPngAsDxt5` and `RuntimeSpineLoader.CanLoadPngAsDxt5` visibly delegate to `TryPngSize`. `RuntimeAtlasTexture.Load` additionally checks a DXT5/format-support path before creating a texture. The exact platform support predicate and fallback format must be confirmed.

### Preliminary loader and ownership findings

- `RuntimeAtlasTexture.Read` decompiles to a short initialization/delegation stub despite 90 bounded starts; it is not accepted as the full read implementation.
- `RuntimeSpineLoader.ReadAsync` remains a wrapper-like stub despite 329 bounded starts. Metadata identifies a `<ReadAsync>b__0` closure under `RuntimeSpineLoader`, but no compiler-generated `<ReadAsync>d__...` state-machine type is nested under that class. The closure's downstream work and exact task construction still need a call-target trace.
- `RuntimeSpineLoader.Load` has only 20 bounded starts, while its pseudocode is roughly 9 KB and P-code addresses are mostly outside its bounded set. Its output visibly starts with a call to `Read` and then resembles the `Create` path. Do not treat the current `Load` export as an independent complete body.
- `RuntimeSpineLoader.Create` has 509 bounded starts and visibly performs substantial object/material construction and calls `RuntimeSpineAsset.Own`; 336 starts are absent from the P-code address set, so the full create path remains provisional.
- `PageTextureLoader.Load` appears to obtain a texture through an unresolved helper, store it in a page field, fill width/height from the returned texture when absent, and update a material/texture property. `PageTextureLoader.Unload` exports as a single return instruction. This does not prove that no cleanup exists elsewhere.
- `RuntimeSpineAsset.Retain` visibly increments a field at object offset `+0x18` when it is nonzero; `Dispose` decrements the same field and enters cleanup when it reaches zero. This is consistent with reference-counted lifetime management. The `Own` and `Retain` P-code/body mismatches are too large to accept the full ownership graph yet.
- `Dispose` visibly enumerates owned objects and calls `RuntimeSpineLoader.DestroyOwned`, and includes an atlas disposal path. The exact ordering, exception behavior, and indirect destruction dispatch remain unresolved; do not equate this with a proven complete resource-release contract.

### Updated evidence classification after run 43001799392

**Verified facts**
- The TSV delimiter fix commit [f926d7d](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/f926d7d640f36ef6722cc70752be67e0b15ef315) triggered a successful 34-target run.
- The artifact contains 34 pseudocode exports and 34 JSON sidecars; all bounded walks report zero decode failures and all assigned function-body instruction-start sets match the traversed sets.
- `DxtBytes` has a 15/15 P-code-address match; the other selected metrics are recorded above.
- The cache/texture, page-loader, loader, and asset-lifecycle target RVAs remain Android ELF RVAs derived from the same metadata/binary pair.

**Preliminary inferences**
- The cache is a custom `AAADXT5`-marked container with a 92-byte header and DXT5 block payload.
- Texture-size estimation is format-aware and appears to include mipmap overhead.
- Asset lifetime is likely reference-counted; disposal appears to drain owned resources.

**Unresolved**
- Complete CFG, direct branch-target export, indirect jump tables, exception/LSDA edges, and attribution of every P-code address outside each bounded function body.
- Exact checksum algorithm, cache field layout, file-corruption recovery, DXT5 capability/fallback, and texture allocation calls.
- The actual async task/state-machine path for `ReadAsync` and pending cache writes.
- The complete `Own/Retain/Dispose/DestroyOwned` ownership graph and proof that every page texture is released exactly once.
- A binary-level comparison with legacy iOS 0.7.9.2.

The next pass should prioritize address-level disassembly and call/branch-edge attribution for `TryReadCache`, `WriteCache`, `Load`, `CreateTexture`, `EstimateTextureBytes`, `FinishPendingWritesAsync`, `RuntimeSpineAsset.Dispose`, and `DestroyOwned`. Do not promote the preliminary pseudocode interpretations to implementation requirements until those checks pass. No original C# source, restored Unity project, iOS compilation, or IPA is established.


## P-code provenance follow-up — runs 43002455898 and 43002856588

Commit [7fca6b7](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/7fca6b7bebba9dafa6727465e42160d4b495b657) added a per-P-code-operation provenance export. The first run, [43002455898](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43002455898), completed successfully and uploaded artifact [13846951669](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43002455898/artifacts/13846951669) (4,487,966 bytes; ZIP digest `sha256:45f139db1044373f358659b970628aefe3aed2c68c4caf10f842e057fa048f06`). That first provenance export used literal backslash-t/backslash-n separators in both the header and rows, so its files were not valid TSV.

Commit [5f8e6a1](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/5f8e6a11aafe32e6c13f666a4f90c17b0f7b2b7f) corrected the row formatter to emit actual tab separators and added `pcode_provenance_file` to each JSON sidecar. The resulting [run 43002856588](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43002856588) completed successfully; all 11 steps passed. Artifact [13846468132](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43002856588/artifacts/13846468132) is 4,486,758 bytes, digest `sha256:f3c441cc19c1e281a78983f7dd7c14d4d648eb2b1c16079bbc6119e5b1bd463c`. I downloaded and inspected it: 142 files, including 34 C exports, 34 JSON sidecars, 34 P-code provenance files, 34 skipped-entry lists, and the shared metadata/verification files.

The 34 pseudocode hashes and bounded-flow counts are unchanged from run 43001799392. This change adds provenance diagnostics; it does not add new machine-code traversal, branch edges, or CFG recovery.

### Provenance header is still malformed

The data rows in the 430028 provenance files now use actual tabs, but the header initializer in the current `PrepareFlowFunction.java` still contains double-escaped tab and newline sequences. In all 34 files:
- the header contains literal backslash-t and backslash-n characters rather than tab/newline delimiters;
- the first data row is concatenated onto that same physical line after the literal backslash-n;
- subsequent data rows are tab-delimited and can be parsed.

The JSON sidecars point to the provenance files, but the workflow verification step still checks only the C export and JSON identity/hash; it does not parse or validate the provenance TSV. Therefore the successful workflow status does not validate this TSV header. This is a confirmed output-format defect in the latest observed script revision.

### Function-owner attribution is order-dependent

The provenance script asks Ghidra for the function containing each P-code address while it is decompiling the current target. However, the workflow creates and decompiles targets sequentially in TSV order. Later target functions have not yet been created when earlier provenance files are written. The 430028 data confirms this ordering effect:

| Target | P-code addresses | Outside current body | Owner attribution for outside addresses |
|---|---:|---:|---|
| `RuntimeAtlasTexture.TryReadCache` | 272 | 0 | All 272 unique P-code addresses belong to its own recovered body |
| `RuntimeAtlasTexture.Load` | 234 | 109 | All 109 outside addresses are unowned at capture time |
| `RuntimeSpineLoader.AtlasPages` | 176 | 40 | All 40 outside addresses are unowned at capture time |
| `RuntimeSpineLoader.Load` | 188 | 185 | All 185 outside addresses are unowned at capture time |
| `RuntimeSpineLoader.ReadAsync` closure | 40 | 40 | 13 addresses are owned by the already-created `RuntimeSpineLoader.Read`; 27 are unowned |
| `RuntimeSpineAsset.Own` | 212 | 128 | 22 are owned by earlier DXT5 eligibility functions; 106 are unowned |
| `RuntimeSpineAsset.Retain` | 1,306 | 1,295 | All 1,295 outside addresses are unowned at capture time |
| `RuntimeSpineAsset.Dispose` | 128 | 22 | 14 belong to `RuntimeAtlasTexture.CanLoadPngAsDxt5`; 8 belong to `RuntimeSpineLoader.CanLoadPngAsDxt5` |
| `RuntimeSpineAsset.get_EstimatedTextureBytes` | 622 | 572 | All 572 outside addresses are unowned at capture time |

“Unowned” means that Ghidra's function manager returned no containing function at the time this target's provenance file was written. It does not mean the address is invalid or unreachable. For example, the target order places `RuntimeAtlasTexture.CreateTexture` after `RuntimeAtlasTexture.Load`, `RuntimeSpineLoader.Read` after `RuntimeSpineLoader.AtlasPages`, and `RuntimeSpineLoader.Create` after `RuntimeSpineLoader.Load`; their functions are therefore not yet available to the earlier provenance pass. The `ReadAsync` closure's 13 addresses attributed to `Read` are consistent with cross-function/inlined callback content, but do not by themselves prove the call graph.

The provenance export should be made two-phase: first create and verify all target function bodies, then decompile and record P-code ownership after every target function exists. A final attribution pass over the completed Ghidra program would also work. The TSV header must be fixed and the verification step should validate the file format and row counts.

### What provenance does and does not establish

- It helps distinguish P-code addresses inside the current recovered body, addresses owned by another Ghidra function, and addresses with no function owner at capture time.
- `RuntimeAtlasTexture.TryReadCache` is currently the cleanest high-priority target by this diagnostic: 272 unique P-code addresses are all inside its recovered body, with zero P-code addresses outside. It still has 107 bounded instruction starts absent from P-code, so this is not a complete-CFG proof.
- `RuntimeAtlasTexture.DxtBytes` remains the cleanest small method: 15 bounded starts and 15 P-code addresses, with no reported mismatch.
- The `Read/ReadAsync/AtlasPages/Load/Create` and asset-lifetime exports remain provisional because their P-code/body mismatches are large or order-dependent.
- Provenance records P-code instruction addresses, containing function, body membership, and opcode. It is not a raw ARM64 disassembly or a direct-branch edge list, and it does not recover indirect jump tables or exception/LSDA edges.

### Next evidence pass

1. Fix the header's Java string escaping and make the verifier parse every provenance file, including checking that the first data row is not merged with the header.
2. Split target processing into function-body creation and decompilation/provenance phases so future target functions are already known when owners are attributed.
3. Compare each P-code address against the final function-body set and attribute every outside address to a target function, non-target function, or genuinely unowned block.
4. Export raw ARM64 instruction bytes/disassembly and direct branch/fall-through edges; separately resolve indirect jump tables and exception/LSDA paths.
5. Revisit the cache and ownership findings only after the provenance map is complete. No code or workflow files were changed during this evidence review.
