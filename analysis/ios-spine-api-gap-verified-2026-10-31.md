# Verified iOS Spine API gap (2026-10-31)

## Source and confidence
- Uses actual Il2CppDumper `dump.cs` outputs from [cross-platform comparison run 41172721121](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41172721121), generated with each platform's own IL2CPP binary and matching metadata.
- The artifact contains iOS dump.cs (40,835,508 bytes), Android dump.cs (47,862,364 bytes), and Windows dump.cs (42,438,955 bytes).
- This is metadata declaration evidence. It does not establish successful runtime behavior, and a missing type can in principle be affected by stripping. The differences below are multiple coordinated API-surface changes, not isolated string misses.

## iOS 0.7.9.2: Spine runtime exists, but is an older API surface

Present in iOS metadata:
- `Spine.SkeletonBinary` (TypeDefIndex 13501)
- `Spine.SkeletonJson` (13507)
- `Spine.Atlas` (13457)
- `Spine.SkeletonData` (13505)
- `Spine.Unity.SkeletonDataAsset` (13524)
- `Spine.Unity.SpineAtlasAsset` (13530)
- `Spine.Unity.BlendModeMaterials` (13521)

However, the iOS signatures differ from the Android 1.0 / Windows 1.0.1 runtime:

### SkeletonBinary
- iOS `SkeletonBinary` does not inherit the newer `SkeletonLoader` base type.
- Its constants and methods are from an older feature surface: it has BONE_TRANSLATE/BONE_SCALE/BONE_SHEAR and SLOT_COLOR/SLOT_TWO_COLOR; the new runtime has X/Y-specific translate/scale/shear, BONE_INHERIT, sequences, and physics-constraint timelines.
- The old iOS class has no `ReadSequence` method or newer sequence/physics parsing method surface.

### SkeletonDataAsset
Old iOS exposes:
- `CreateRuntimeInstance(ScriptableTextAsset, AtlasAssetBase, bool, float)`
- `CreateRuntimeInstance(ScriptableTextAsset, AtlasAssetBase[], bool, float)`
- `GetSkeletonData(bool)`, `Clear()`, `FillStateData()`

New Android/Windows exposes additional APIs:
- `SetRawContent(byte[])` and `GetRawContent()`
- `CreateRuntimeInstance(byte[] skeletonRawContent, string skelName, AtlasAssetBase[], bool, float, bool quiet)`
- `SetupRuntimeBlendModeMaterials(bool, BlendModeMaterials.TemplateMaterials)`
- `FillStateData(bool quiet = false)`

The new RuntimeSpineLoader calls the byte-array CreateRuntimeInstance overload and SetupRuntimeBlendModeMaterials; those signatures are absent from old iOS.

### SpineAtlasAsset and texture-loader APIs
Old iOS:
- `CreateRuntimeInstance(TextAsset, Material[], bool)`
- `GetAtlas()`
- no `customTextureLoader` field on the class declaration;
- no `Func<SpineAtlasAsset, TextureLoader>` callback parameter on the factory.

New Android/Windows:
- `CreateRuntimeInstance(TextAsset, Material[], bool, Func<SpineAtlasAsset, TextureLoader>)`
- `GetAtlas(bool onlyMetaData = false)`
- `customTextureLoader` field;
- `Spine.Unity.NoOpTextureLoader` type.

The new loader depends on those newer APIs for metadata-only atlas-page enumeration and custom texture loading.

## Converter: absent from old iOS metadata
The old iOS dump does not contain:
- namespace `Native.SpineConverter`;
- `Native.SpineConverter.Converter`;
- `Native.SpineConverter.SpineVersion`;
- `RuntimeSpineLoader`;
- `RuntimeSpineAsset`;
- `RuntimeAtlasTexture`;
- `BackgroundResource`.

The iOS dump does contain an unrelated .NET `System.ComponentModel.Converter`; that is not the Spine converter.

Android 1.0 / Windows 1.0.1 contain:
- `SpineVersion.Version35=0`, `Version36=1`, `Version37=2`, `Version38=3`, `Version40=4`, `Version41=5`, `Version42=6`, `Invalid=-1`.
- `Native.SpineConverter.Converter` with native extern declarations:
  - `CastSkeletonDataToDstVersionNative(byte[] inBuf, int inLen, int dstVersion, out IntPtr outPtr, out int outLen)`
  - `FreeBufferNative(IntPtr p)`
  - `CastJsonSkeletonToBinary(string input, out IntPtr outPtr, out int outLen)`
  - managed wrappers `CastJsonToBinary(string)` and `CastSkeletonDataToDstVersion(byte[], SpineVersion)`.
- `DLL_NAME_WIN_LNX_MAC_ANDROID = "SpineSkeletonDataConverterLib"`.
- The new runtime calls `CastSkeletonDataToDstVersion(..., Version42)` before creating the SkeletonDataAsset.

Native binary evidence:
- Android package: ARM64 ELF `libSpineSkeletonDataConverterLib.so`.
- Windows package: x86-64 PE `SpineSkeletonDataConverterLib.dll`.
- Both expose `CastJsonSkeletonToBinary`, `CastSkeletonDataToDstVersion`, and `FreeBuffer`.
- The old iOS app inventory has no separate converter dylib/framework. The UnityFramework string scan did not find the converter identifiers; static linking/stripping cannot be excluded solely from this.

## Porting consequence

This is not just a missing converter binary. The old iOS Spine runtime has a materially older managed API surface than the 1.0 Android/Windows runtime. Adding only a native iOS converter would not supply:
- the new SkeletonBinary/SkeletonLoader feature set;
- byte-array SkeletonDataAsset construction;
- runtime Spine atlas custom texture-loader callback;
- NoOpTextureLoader and newer atlas metadata API;
- runtime asset ownership/cache classes.

Therefore a direct source-level port into the old iOS 0.7.9.2 runtime is not currently feasible by adding only a few adapter methods.

## Viable next routes
1. Obtain a newer iOS 1.0 build or the original Unity project/source and matching Spine 4.2 packages; this is the cleanest route.
2. If no newer iOS build exists, port the compatible Spine 4.2 C# and Spine-Unity runtime API surface plus the native converter to arm64-apple-ios, then integrate it into the Unity project. Original converter source or a fully verified reimplementation is required; Android ARM64/Windows x64 binaries cannot be reused on iOS.
3. A fallback converter target of Spine 3.8 would require proving that all 1.0 skeleton data and required features can be represented without 4.0/4.1/4.2 features. Current evidence does not establish that; do not assume it is safe.

## Delivery boundary
- This is verified API-gap analysis, not a compiled Spine runtime port.
- No original Unity project/source tree, original converter C++ source, or macOS/Xcode build host is available in the current repository workflow.
- No IPA has been built or device-tested.
- No Android RVA or machine code is proposed for iOS reuse.
