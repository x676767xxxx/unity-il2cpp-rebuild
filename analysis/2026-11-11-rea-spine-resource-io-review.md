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
