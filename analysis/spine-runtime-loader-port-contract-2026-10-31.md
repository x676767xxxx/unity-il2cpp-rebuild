# Spine runtime loader port contract (2026-10-31)

## Evidence sources
- Android/Windows exact recovered IL: `analysis/full-exact-background-method-il-2026-10-31.md` and raw targeted IL artifact from [run 41175531020](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41175531020).
- Converter native binary inspection: [run 41174332705](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41174332705).
- Managed converter wrapper surface: `analysis/spine-converter-managed-pinvoke-abi-2026-10-31.md`.
- Old iOS runtime baseline: [run 41173880844](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41173880844).
- This document describes recovered IL, not original source or a compiled Unity implementation.

## Reconstructed loading sequence

### 1. Read files and atlas page names
Android 1.0 and Windows 1.0.1 `RuntimeSpineLoader.Read(basePath, supportsDxt5)`:
1. Canonicalizes the base path with `Path.GetFullPath`.
2. Reads `basePath + ".atlas"` using `File.ReadAllText`.
3. Reads `basePath + ".skel"` using `File.ReadAllBytes`.
4. Parses atlas page names by constructing `Spine.Atlas(TextReader, directory, Spine.Unity.NoOpTextureLoader)`, then enumerating atlas pages.
5. Resolves each page relative to the resource directory using `BackgroundResource.ResolvePath`.
6. Builds a page-source dictionary using `RuntimeAtlasTexture.Read(path, supportsDxt5)`.
7. Throws `InvalidDataException("Spine atlas contains no texture pages: " + ...)` if no page sources were found.

The Android recovered method has substantially more unresolved decompiler markers than Windows; Windows IL independently confirms the same core file/atlas/page-resolution sequence.

### 2. Asynchronous file read
`RuntimeSpineLoader.ReadAsync(basePath)` checks `SystemInfo.SupportsTextureFormat` before dispatching the synchronous read through `Task.Run`. Do not move Unity object creation to this worker thread. The observed asynchronous path is for reading/parsing file data and source records; Unity textures/materials are created later.

### 3. Atlas page image loading
`RuntimeAtlasTexture.Read(path, supportsDxt5)`:
- canonicalizes the file path and reads bytes;
- obtains PNG dimensions via `TryPngSize`;
- computes a checksum;
- checks a `.dxt` cache record via `TryReadCache`.

`RuntimeAtlasTexture.Load(source, name)` has two branches:
- DXT5 cache branch: requires `SystemInfo.SupportsTextureFormat(TextureFormat.DXT5)`; creates a texture, calls `LoadRawTextureData`, then `Apply`.
- Image fallback: creates a texture, calls `ImageConversion.LoadImage(texture, bytes, true)`, inspects resulting texture format/dimensions/raw data, and may queue a DXT cache write.

The recovered IL uses enum literal 12 for the DXT5 format and 4 for the fallback texture format. It also contains cache/checksum/format fallback logic. Do not use the static background image decoder as a replacement for this path: atlas pages have their own DXT5 cache and Spine texture-loader behavior.

### 4. Runtime Spine object construction
`RuntimeSpineLoader.Create(files, material, blendModeTemplates, ...)`:
1. Loads each page texture through `RuntimeAtlasTexture.Load` and registers owned Unity objects.
2. Creates page materials and assigns their main textures.
3. Creates a `Spine.Unity.SpineAtlasAsset` through `CreateRuntimeInstance`, with a texture-loader callback.
4. Gets the Spine `Atlas` instance from the runtime atlas asset.
5. Calls `Native.SpineConverter.Converter.CastSkeletonDataToDstVersion(files.Skeleton, enumValue: 6)`.
6. Calls `SkeletonDataAsset.CreateRuntimeInstance` with the converted skeleton bytes, base path ending in `.skel`, the runtime atlas asset array, and observed flags/scale arguments (the recovered IL includes `false`, `0.01f`, `true`).
7. Gets `SkeletonData`, calls `SetupRuntimeBlendModeMaterials`, and validates that the runtime Spine material is present.

Several branches in this method contain unresolved Cpp2IL decompiler markers. The call order and named APIs are visible, but not every error/cleanup branch is reconstructed.

### 5. Ownership and cleanup
`RuntimeSpineAsset` tracks owned Unity objects and provides retain/dispose behavior. Its disposal path disposes the Spine atlas and destroys owned objects through `RuntimeSpineLoader.DestroyOwned`. The recovered disposal IL has unresolved indirect calls, so exact reference-count transitions and all object-destruction ordering still require validation.

## Converter dependency and iOS gap

- Android package contains `lib/arm64-v8a/libSpineSkeletonDataConverterLib.so` (ELF AArch64).
- Windows package contains `SpineSkeletonDataConverterLib.dll` (PE x86-64).
- The Android and Windows native binaries export `CastJsonSkeletonToBinary`, `CastSkeletonDataToDstVersion`, and `FreeBuffer`.
- The Windows BepInEx interop proxy exposes managed wrapper methods for conversion and buffer release. These IL2CPP proxy signatures are not themselves the native C ABI.
- The managed field name `DLL_NAME_WIN_LNX_MAC_ANDROID` suggests the named platform family, but is not conclusive proof that no iOS-specific implementation exists.
- The old iOS app inventory has no separate Spine converter dylib/framework, and the UnityFramework string scan did not find the converter identifiers. Static linking or stripped symbols cannot be excluded from these observations alone.
- Android/Windows call `CastSkeletonDataToDstVersion` with literal enum value 6. The enum's exact semantic label must be confirmed from the full metadata dump; do not simply label it Spine 4.2 based on the converter's supported-version strings.

## Porting decision gates

1. Complete the corrected iOS/Android/Windows Il2CppDumper Spine API comparison. The first attempt failed because the runner lacked .NET 6; the corrected run is [41178006518](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41178006518). It must produce non-empty `dump.cs` for each platform before its API comparison is trusted.
2. Verify whether old iOS has the required `SkeletonBinary`, `SkeletonDataAsset.CreateRuntimeInstance`, `SpineAtlasAsset.CreateRuntimeInstance`, `NoOpTextureLoader`, and converter types/signatures.
3. If iOS Spine APIs are older/incompatible, choose one of:
   - build a matching arm64-apple-ios converter from original native converter source (not currently present);
   - implement/port the converter in managed code against a matching Spine runtime (requires a compatible Spine 4.2 runtime source/API);
   - obtain a newer iOS build with the converter/runtime already included, if one exists.
4. Only after the converter and Spine API gates pass, implement and test the atlas page loader, DXT cache fallback, runtime atlas/material creation, and object ownership on iOS.

## Current delivery status
- This is an evidence-based port contract, not a Unity patch.
- No original converter C++ source, complete Unity project/source tree, or macOS/Xcode build host is currently available in the repository workflow.
- No compilable Spine compatibility layer, signed IPA, or device-verified build has been produced.
- No Android ARM64 RVA or machine code is proposed for iOS reuse.


## Verified old-iOS API incompatibility (additional evidence)
The actual three-platform `dump.cs` artifact from [run 41172721121](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41172721121) was inspected directly. Old iOS 0.7.9.2 has Spine.SkeletonBinary, SkeletonDataAsset and SpineAtlasAsset types, but not the newer API used by the 1.0 loader:
- No `Native.SpineConverter.Converter` or `Native.SpineConverter.SpineVersion` in the old iOS namespace.
- No `RuntimeSpineLoader`, `RuntimeSpineAsset`, `RuntimeAtlasTexture` or `BackgroundResource`.
- Old iOS `SkeletonDataAsset.CreateRuntimeInstance` takes `ScriptableTextAsset`; it lacks the byte[]/name overload and `SetupRuntimeBlendModeMaterials`.
- Old iOS `SpineAtlasAsset.CreateRuntimeInstance` lacks the custom texture-loader callback; `GetAtlas()` has no metadata-only bool overload.
- `Spine.Unity.NoOpTextureLoader` is not present in the old iOS dump.
- Old iOS `SkeletonBinary` has a visibly older feature surface; new Android/Windows include the newer `SkeletonLoader`, sequence and physics-constraint APIs.
- Android/Windows enum confirms `SpineVersion.Version42 = 6`; the new loader's literal 6 is therefore Spine 4.2.

See the full declaration-by-declaration evidence in `analysis/ios-spine-api-gap-verified-2026-10-31.md`.

**Consequence:** porting only the native converter or writing a small wrapper is insufficient. The old iOS Spine C# / Spine-Unity API surface also needs upgrading or replacing. A safe Spine 3.8 fallback is not proven because the new runtime uses Spine 4.2-era features.

## Additional exact Android 1.0 lifecycle evidence (2026-10-31)

The matching Android 1.0 dump.cs from cross-platform artifact run [41172721121](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41172721121) gives the following concrete layouts:

### File-read payload
RuntimeSpineLoader.Files contains:
- BasePath at 0x10
- AtlasText at 0x18
- Skeleton byte array at 0x20
- Pages dictionary (string -> RuntimeAtlasTexture.Source) at 0x28

This confirms the asynchronous read result is a managed data container. It carries file bytes and page source records; it is not itself a Unity texture/material container.

### Runtime asset ownership record
RuntimeSpineAsset contains:
- owned: HashSet<UnityEngine.Object> at 0x10
- references: integer at 0x18
- Asset backing field (SkeletonDataAsset) at 0x20

Its public Retain() implementation increments the reference count only when the count is already positive; the zero-count path throws rather than resurrecting a released asset. Dispose() is reference-counted: it decrements a positive count and enters the cleanup path when the count reaches zero. The cleanup path traverses owned Unity objects and invokes the loader's DestroyOwned helper, while releasing associated Spine atlas/material resources and clearing the asset reference. The exact platform-specific destruction behavior must remain in the Unity adapter; do not replace this with a generic managed Dispose that merely drops references.

### Cache limits and consumer semantics
The manager declaration confirms:
- background preview cache entry limit: 24;
- background preview cache byte budget: 33,554,432 bytes (32 MiB);
- background skeleton cache byte budget: 805,306,368 bytes (768 MiB);
- backgroundSkeletons maps keys to RuntimeSpineAsset;
- backgroundSkeletonAccess separately tracks access values;
- RuntimeSpineAsset.HasConsumers and EstimatedTextureBytes are used by the manager's trimming logic.

The previously implemented 32 MiB / 25-entry preview texture trim planner is not the skeleton cache policy. Keep these caches separate: preview textures use the 32 MiB / 24-entry cap, while retained Spine skeleton assets use a distinct 768 MiB budget and consumer/access tracking.

### Adapter requirements refined
1. Keep file I/O and atlas-page source parsing off the Unity main thread only where the recovered async worker does so.
2. Perform Unity texture, material, Spine atlas and SkeletonDataAsset creation on the Unity main thread.
3. Track every Unity object created by the loader in the asset's owned-object set.
4. A retained asset cannot be resurrected after its reference count reaches zero.
5. Evict only assets with no consumers; release the complete owned-object/atlas dependency graph once the final reference is released.
6. Preserve the independent preview texture and skeleton-asset cache budgets above.

These are Android 1.0 behavior findings. They do not provide iOS-compatible addresses or machine code.
