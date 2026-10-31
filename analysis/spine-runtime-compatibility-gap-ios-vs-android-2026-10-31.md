# Spine runtime compatibility gap: old iOS vs Android 1.0 (2026-10-31)

## Evidence sources
- Platform-matched IL2CPP dump.cs outputs from Actions run 41172721121, artifact 13032345439.
- Targeted recovered IL from Actions run 41175531020, artifact 13032865917.
- Native Spine converter inventory from Actions run 41174332705, artifact 13032532232.
- Old iOS package is AzureArchive 0.7.9.2; Android package is AzureArchive 1.0.

## Key result
The old iOS build contains Spine runtime types, so it is inaccurate to say that it has no Spine support. However, its Spine API surface is materially older than Android 1.0's runtime. The Android 1.0 custom-background loader also calls a native skeleton converter targeting Spine 4.2. The old iOS metadata has no Native.SpineConverter declarations, and its IPA contains only UnityFramework.framework under Frameworks (no separate converter dylib/framework was inventoried).

## Exact API differences in the IL2CPP metadata

### SkeletonBinary
- Old iOS has the older SkeletonBinary shape: it does not derive from SkeletonLoader in the dump, and its ReadVertices takes an int vertexCount. It has no Sequence type/API.
- Android 1.0 and Windows 1.0.1 derive SkeletonBinary from SkeletonLoader, add Sequence parsing, use ReadVertices(input, bool weighted), and add the newer timeline methods.
- Android/Windows also have Spine.Sequence and SequenceTimeline types; these types are absent from old iOS metadata.

### SkeletonData and SkeletonDataAsset
- Old iOS SkeletonData has no PhysicsConstraints / ReferenceScale members. Android/Windows include PhysicsConstraints, FindPhysicsConstraint and ReferenceScale.
- Old iOS SkeletonDataAsset has only TextAsset-like/ScriptableTextAsset runtime factory overloads and no SetRawContent/GetRawContent or byte[]-based CreateRuntimeInstance overload.
- Android/Windows add SetRawContent(byte[]), GetRawContent(), byte[] CreateRuntimeInstance(...), SetupRuntimeBlendModeMaterials(...), FillStateData(bool quiet) and stream-based skeleton parsing.

### SpineAtlasAsset and blend materials
- Old iOS SpineAtlasAsset CreateRuntimeInstance overloads do not accept the custom TextureLoader factory callback; GetAtlas() has no onlyMetaData parameter.
- Android/Windows CreateRuntimeInstance overloads accept Func<SpineAtlasAsset, Spine.TextureLoader>, and GetAtlas(bool onlyMetaData) exists.
- Old iOS BlendModeMaterials exposes only the basic state properties, ApplyMaterials and region-cloning method. Android/Windows add blend-mode detection and runtime material creation helpers.

These are exact metadata declaration differences, not runtime test results. They show that simply calling the Android 1.0 Spine API against the old iOS runtime will not compile or link without adaptation or a newer runtime.

## Native converter requirement
- Android ARM64 package contains libSpineSkeletonDataConverterLib.so (AArch64 ELF; SHA256 0775f4442c0ee7d63453319255694c9ee9c3bd7f6dd264559b47c6b417714cf8).
- Windows package contains SpineSkeletonDataConverterLib.dll (PE32+ x86-64; SHA256 8196e6aa6c04377585e3029df7791f3de1560c8b8bfe623c63a81e70bbd51653).
- Both export a function named CastSkeletonDataToDstVersion.
- Android 1.0 RuntimeSpineLoader.Create IL directly calls Native.SpineConverter.Converter.CastSkeletonDataToDstVersion(bytes, SpineVersion). The argument is integer 6; the recovered enum maps 6 to Version42. Thus the custom runtime converts the skeleton bytes to Spine 4.2 before creating the SkeletonDataAsset.
- Old iOS dump.cs has zero Native.SpineConverter references. The old iOS package inventory did not find a standalone iOS converter library. This does not prove there is no statically linked equivalent, but no matching iOS converter implementation is evidenced in the supplied old IPA.

## Consequences for the port
1. Ordinary PNG/JPEG background images remain the best first porting target; their IL call chain uses Unity Texture2D/ImageConversion and does not require the Spine converter.
2. Do not route Android 1.0 custom Spine .skel files directly into old iOS SkeletonBinary. Its metadata/API is older and lacks newer Sequence/Physics structures.
3. A Spine port needs one of:
   - the matching Spine 4.2 C# runtime and Unity integration package, plus an iOS-compatible native converter built from its source; or
   - a verified managed conversion/parser implementation that removes the native converter dependency and a complete port of the required Spine 4.2 API surface.
4. The repository currently contains no Unity project, no Spine 4.2 source/package, no converter C++ source, and no iOS converter binary. These are concrete blockers, not just missing configuration.
5. The existing old iOS UnityFramework's Unity 2023.2.20f1c1 runtime is only patch-level older than Android's Unity 2023.2.22f1; the much larger Spine runtime/API gap is the more immediate obstacle for custom Spine backgrounds.

## Recommended implementation order
- Phase 1: implement and test static background file loading on iOS using existing ImageConversion/Texture2D APIs, preserving resource source/path and cache ownership.
- Phase 2: recover exact BackgroundResource.ResolveOverride and preview coroutine/cache behavior.
- Phase 3: obtain Spine 4.2 runtime and converter source, produce an iOS arm64 converter, then validate actual .skel/.atlas/page texture samples.
- Keep AuthoringResourceCatalog out of the first playable target unless gameplay call sites prove it is required.

## Delivery status
- This is an evidence-based analysis report, not a source implementation.
- No C# compatibility layer has been compiled.
- No Unity iOS project/Xcode build host is available in the repository workflow.
- No AzureArchive 1.0 IPA has been built, signed or device-verified.
- No Android ARM64 RVA or machine code is proposed for iOS reuse.
