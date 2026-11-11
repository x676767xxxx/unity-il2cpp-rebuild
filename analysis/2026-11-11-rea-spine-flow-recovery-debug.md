# REA Spine control-flow recovery — failure analysis and corrected workflow (2026-11-11)

## Scope and provenance

- Repository: `x676767xxxx/unity-il2cpp-rebuild`
- Branch: `progress/azurearchive-ios-port-20261031`
- Branch head after workflow corrections: `38cf09f704f086f8f19b7246fa55089bc30e3ad6`
- Baseline resource-I/O run: [42995578057](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42995578057), successful.
- First reachable-flow run: [42996125270](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42996125270), failed at output verification.
- Second reachable-flow run: [42996956698](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42996956698), failed during Ghidra launch after the first workflow correction.
- First flow artifact: [azurearchive-rea-spine-flow-recovery, artifact 13844780081](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42996125270/artifacts/13844780081).
- Latest workflow correction commit: [38cf09f](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/38cf09f704f086f8f19b7246fa55089bc30e3ad6).

## Verified facts

### Input pair and metadata

The failed flow artifact reuses the same Android 1.0 input pair as the successful resource-I/O run:

- ARM64 `libil2cpp.so`: 120,315,032 bytes; SHA-256 `7f15b8e7557eb7948fbea746c629fb15d442d05c649ab2db3667ae08aa2c9b6`.
- `global-metadata.dat`: 17,132,296 bytes; metadata v29; SHA-256 `717236ea9c650b4fcd373e0afc7c03c25d5949726bebe251d3e8301e4da7a144`.
- REA linked 152,789 method pointers; the run did not request generic method instances.

### The one function that reached decompilation

The first flow artifact contains a new `RuntimeAtlasTexture.TryPngSize` export:

- Android ARM64 ELF RVA: `0x325c770`; Ghidra import address: `0x335c770`.
- Reachable instructions: 162; decoded instructions: 162; decode failures: 0.
- Pseudocode length: 3,400 characters.
- Pseudocode SHA-256: `48847146bc41d411a5a9dec5a5eae36d46ff080f4f13067a678eea57d8204adb`.
- The earlier metadata-bounded export reported 136 seeded instructions. The reachable-flow pass therefore decoded 26 additional instructions for this function. This is an observed expansion, not proof of complete CFG recovery.

The pseudocode includes calls to the helper at Ghidra address `0x335e4f4` (ELF RVA `0x325e4f4`) for reading PNG fields. It checks the PNG signature and IHDR marker, reads width/height, rejects dimensions not divisible by four, rejects non-positive dimensions, and applies a calculated upper-bound check. The exact helper semantics, endianness and guard arithmetic still require instruction-level review.

The artifact contains no flow pseudocode/JSON for the other four targets. Its file listing contains only the TryPngSize flow export plus shared metadata/log files.

### Metadata signatures and Android ARM64 RVAs

The paired REA metadata provides the following declaration-level signatures. These are metadata-derived declarations and Android ELF RVAs, not recovered C# bodies or iOS addresses.

| Method | Signature | Android ARM64 RVA |
|---|---|---:|
| RuntimeAtlasTexture.Read | `Source Read(string path, bool supportsDxt5)` | `0x325c600` |
| RuntimeAtlasTexture.Load | `Texture2D Load(Source source, string name)` | `0x325d080` |
| RuntimeAtlasTexture.CreateTexture | `Texture2D CreateTexture(int width, int height, TextureFormat format, string name)` | `0x325d49c` |
| RuntimeAtlasTexture.TryReadCache | `byte[] TryReadCache(Source source)` | `0x325c9f8` |
| RuntimeAtlasTexture.QueueWrite | `void QueueWrite(Source source, int width, int height, byte[] bytes)` | `0x325d5f4` |
| RuntimeAtlasTexture.WriteCache | `void WriteCache(Source source, int width, int height, byte[] bytes)` | `0x325daf0` |
| RuntimeAtlasTexture.FinishPendingWritesAsync | `Task FinishPendingWritesAsync()` | `0x325e2e4` |
| RuntimeAtlasTexture.CanLoadPngAsDxt5 | `bool CanLoadPngAsDxt5(byte[] bytes)` | `0x325e484` |
| RuntimeAtlasTexture.DxtBytes | `long DxtBytes(int width, int height)` | `0x325d94c` |
| RuntimeAtlasTexture.EstimateTextureBytes | `long EstimateTextureBytes(Texture2D texture)` | `0x325e56c` |
| RuntimeAtlasTexture.TryPngSize | `bool TryPngSize(byte[] data, out int width, out int height)` | `0x325c770` |
| RuntimeSpineLoader.Read | `Files Read(string basePath, bool supportsDxt5)` | `0x325ee08` |
| RuntimeSpineLoader.ReadAsync | `Task<Files> ReadAsync(string basePath)` | `0x325f1c0` |
| RuntimeSpineLoader.AtlasPages | `string[] AtlasPages(string text)` | `0x325ea60` |
| RuntimeSpineLoader.Load | `RuntimeSpineAsset Load(string basePath, Material material, TemplateMaterials blends, bool quiet)` | `0x325f2dc` |
| RuntimeSpineLoader.Create | `RuntimeSpineAsset Create(Files files, Material material, TemplateMaterials blends, bool quiet)` | `0x325f32c` |
| PageTextureLoader.Load | `void Load(AtlasPage page, string path)` | `0x326004c` |
| PageTextureLoader.Unload | `void Unload(object texture)` | `0x32601c4` |
| RuntimeSpineAsset.Dispose | `void Dispose()` | `0x325fd1c` |

Additional mapped methods in the same metadata inventory include `RuntimeSpineLoader.DestroyOwned` at `0x325e994`, `RuntimeAtlasTexture.ReadChecksum` at `0x325d8f0`, and `RuntimeAtlasTexture.WriteCache`/cache helpers listed above. The exact method index and type/parameter records are in the linked evidence artifact.

## Workflow failures and corrections

### Run 42996125270

The job successfully installed Ghidra, downloaded the APK, linked metadata, and ran the reachable-flow script for the first target. The verification step then failed with:

`RuntimeAtlasTexture.TryPngSize: identity/entry mismatch`

Cause confirmed from the artifact: the script serializes the Ghidra address as hexadecimal text `0335c770`, while the target TSV stores the same address as decimal `53856112`. The verifier compared the strings directly instead of comparing parsed numeric values.

The same run also attempted to import `libil2cpp.so` repeatedly into one existing Ghidra project. Logs show `Found conflicting program file in project: /libil2cpp.so` on later targets. Consequently, only the first function export was present. The workflow did not correctly fail fast on those Ghidra import errors.

### Run 42996956698

The first correction introduced separate project directories per target and explicit checks for the Ghidra success marker and output files, but the shell variable expressions were accidentally escaped in the workflow text. Ghidra rejected the resulting project path with:

`Path element contains invalid character: '}'`

This is a workflow-script quoting error, not a failure of the Android binary or metadata.

### Current workflow state

Commit `e59886a` introduced per-target project isolation and numeric entry comparison. Commit `38cf09f` corrected the shell variable expansion. The current workflow file was re-read from the branch and verified to contain:

- per-target project paths using expanded `${stem}`;
- hexadecimal parsing of the metadata entry before comparison with the decimal target address;
- per-target log marker and output-file checks.

No successful run of this corrected revision has yet been observed. The available GitHub integration supports reading runs and logs but does not expose a workflow-dispatch action in this session. A new run therefore remains unverified and requires a manual `Run workflow` from the [flow-recovery Actions page](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/workflows/rea-ghidra-spine-flow-recovery.yml).

## Preliminary interpretation — not yet port-ready

- `TryPngSize`'s alignment and size arithmetic are consistent with checking dimensions for block-compressed texture allocation, possibly DXT5. The helper at RVA `0x325e4f4` and the caller `CanLoadPngAsDxt5` must be inspected before concluding this is the actual conversion policy.
- The declaration graph establishes a path from `Read/ReadAsync` to a `Files` value, then `Load/Create` to `RuntimeSpineAsset`; the texture pipeline exposes cache reads/writes, DXT sizing, and texture byte estimation. These are signature-level relationships, not proof of exact call order or ownership semantics.
- The old iOS 0.7.9.2 metadata comparison reports that `RuntimeSpineLoader`, `RuntimeSpineAsset`, `RuntimeAtlasTexture`, and the new page texture-loader API surface are absent from the old iOS build. The older iOS Spine API also lacks the newer byte-array SkeletonDataAsset creation and custom texture-loader hooks. See [verified iOS Spine API gap](https://github.com/x676767xxxx/unity-il2cpp-rebuild/blob/progress/azurearchive-ios-port-20261031/analysis/ios-spine-api-gap-verified-2026-10-31.md) and [iOS/Android/Windows method diff](https://github.com/x676767xxxx/unity-il2cpp-rebuild/blob/progress/azurearchive-ios-port-20261031/analysis/ios-android-windows-exact-method-diff-2026-10-31.md).
- No old-iOS runtime body-level comparison has been completed for this texture/cache pipeline. The older API gap makes a thin wrapper-only port unlikely, but matching Spine 4.2 behavior still needs source/runtime and device-level validation.

## Unresolved issues

1. Run the corrected flow workflow and confirm all five target exports are present.
2. Record direct branch targets, out-of-range blocks, decode failures, skipped method-entry edges, and any instruction/span caps per target.
3. Resolve helper RVA `0x325e4f4` and the `CanLoadPngAsDxt5` caller; verify PNG endianness, DXT5 block-byte arithmetic, alignment and overflow checks from ARM64 instructions.
4. Locate generated async state-machine types and `MoveNext` methods for `ReadAsync` and any iterator state machine behind `AtlasPages`.
5. Resolve indirect dispatch in `DestroyOwned`, and trace `PageTextureLoader.Load/Unload` plus `RuntimeSpineAsset.Dispose` for ownership and release semantics.
6. Decompile the texture cache path and compare with the old iOS 0.7.9.2 implementation using the platform-matched iOS binary/metadata, not Android addresses.

## Delivery boundary

This report documents metadata, a single bounded reachable-flow export, workflow failures and a workflow-only correction. It does not claim complete CFG recovery, original C# recovery, a reconstructed Unity project, an iOS build, or an IPA.
