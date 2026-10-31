# Spine runtime API comparison: old iOS vs Android 1.0 / Windows 1.0.1

## Evidence source
The platform-specific Il2CppDumper outputs in artifact azurearchive-cross-platform-il2cpp-comparison from run 41172721121 were inspected directly:
https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41172721121

These are metadata-derived declarations, not original source bodies. Their declarations can establish API presence/signature differences, but not exact runtime behavior or a buildable project.

## High-confidence API differences

### 1. Android 1.0 adds a RuntimeSpineLoader/RuntimeSpineAsset layer absent from old iOS
- Android and Windows dump.cs contain RuntimeSpineLoader.Files, RuntimeSpineLoader.PageTextureLoader, RuntimeSpineLoader and RuntimeSpineAsset.
- Old iOS 0.7.9.2 dump.cs has zero occurrences of RuntimeSpineLoader and RuntimeSpineAsset.
- Android RuntimeSpineLoader declarations include:
  - AtlasPages(string text)
  - Read(string basePath, bool supportsDxt5 = false)
  - ReadAsync(string basePath)
  - Load(string basePath, Material material, BlendModeMaterials.TemplateMaterials blends, bool quiet = true)
  - Create(RuntimeSpineLoader.Files files, Material material, BlendModeMaterials.TemplateMaterials blends, bool quiet = true)
  - CanLoadPngAsDxt5(byte[] data)
- RuntimeSpineAsset owns Unity Objects, retains/releases references, exposes SkeletonDataAsset and Materials, and tracks estimated texture bytes.
- This is a custom game-level runtime loading/cache layer, not the standalone SpineSkeletonDataConverterLib CLI tool.

### 2. Spine core API generation differs
- Old iOS Spine.SkeletonBinary is declared directly and does not inherit SkeletonLoader in the dump. Its declaration includes the older compact bone/slot timeline constants and methods such as ReadSkeletonData(string path) and ReadSkeletonData(Stream file).
- Android/Windows Spine.SkeletonBinary inherits Spine.SkeletonLoader. Their declaration includes newer timeline/data constants such as BONE_TRANSLATEX, BONE_TRANSLATEY, BONE_SCALEX/Y, BONE_INHERIT, ATTACHMENT_SEQUENCE and PHYSICS_*.
- Android/Windows also contain the abstract Spine.SkeletonLoader base type; the old iOS dump does not.
- This is strong evidence the old iOS Spine API generation is older/incompatible with the Android 1.0 API surface. The exact old/new Spine semantic version still needs identification; do not claim an exact version from these declarations alone.

### 3. Spine Unity asset APIs differ
- Old iOS SkeletonDataAsset has CreateRuntimeInstance overloads based on ScriptableTextAsset and AtlasAssetBase, and does not declare SetRawContent/GetRawContent or the raw-byte CreateRuntimeInstance overload.
- Android/Windows SkeletonDataAsset adds SetRawContent(byte[]), GetRawContent(), CreateRuntimeInstance(byte[] skeletonRawContent, string skelName, AtlasAssetBase[] atlasAssets, ...), and SetupRuntimeBlendModeMaterials(...).
- Old iOS SpineAtlasAsset CreateRuntimeInstance overloads do not accept the custom TextureLoader factory parameter.
- Android/Windows SpineAtlasAsset overloads accept Func<SpineAtlasAsset, TextureLoader>, and GetAtlas(bool onlyMetaData = false) differs from old iOS GetAtlas().
- Android/Windows SkeletonAnimation adds ISpineComponent, OnAnimationRebuild, UpdateTiming, UnscaledTime, ApplyAnimation and AfterAnimationApplied members not present in the old iOS declaration.

## Relevance of the native converter library
The separately inspected Android/Windows SpineSkeletonDataConverterLib exports only:
- CastJsonSkeletonToBinary
- CastSkeletonDataToDstVersion
- FreeBuffer

Its help strings describe .json <-> .skel conversion and version migration across Spine 3.5.x through 4.2.x. This is a conversion utility, not the Spine animation renderer. It is not the main missing runtime dependency for in-game background playback. The custom RuntimeSpineLoader/RuntimeSpineAsset and matching Spine core/Unity API generation are the more relevant compatibility gaps.

## Porting decision
- Do not attempt to load Android 1.0 skeleton assets through old iOS Spine APIs without a version/format compatibility test.
- A thin wrapper around old iOS SkeletonDataAsset is insufficient: required byte-content APIs, texture loader hooks and the custom RuntimeSpineLoader layer are missing at the declaration level.
- Preferred route: obtain the matching Spine runtime source/package and version used by Android 1.0, then build it into a proper Unity iOS project using a compatible Unity editor/Xcode toolchain. If the license/source package is unavailable, first prototype a narrowly scoped adapter only after confirming skeleton binary version, atlas page loading, blend modes, and animation features.
- Keep the native converter out of the first gameplay port unless a real gameplay caller is found; its exposed purpose is file conversion.

## Current buildability status
- API comparison: completed from platform-matched dump.cs declarations.
- Compilable iOS Spine compatibility layer: not available; no buildable Unity source project.
- iOS 1.0 IPA: not built or verified.
