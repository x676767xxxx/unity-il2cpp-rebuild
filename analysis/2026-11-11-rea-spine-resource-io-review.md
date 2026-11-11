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
