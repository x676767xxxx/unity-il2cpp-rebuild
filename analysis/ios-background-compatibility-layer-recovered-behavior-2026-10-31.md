# Background compatibility layer: recovered Android 1.0 behavior and iOS port plan

Date: 2026-10-31

## Evidence and provenance
- Platform-matched IL2CPP metadata comparison: Actions run 41172721121.
- Android 1.0 managed-code recovery/decompilation output: successful Cpp2IL/ILSpy recovery artifacts previously produced for this repository (Android 1.0 APK from release XR1).
- Cross-platform declaration comparison: analysis/ios-android-windows-exact-method-diff-2026-10-31.md.
- Reviewed Android native call graph: analysis/android-background-callgraph-reviewed-2026-10-31.md.
- Runtime baseline: analysis/ios-runtime-baseline-verified-findings-2026-10-31.md.
- This report describes recovered/decompiled behavior, not original source. Cpp2IL output contains invalid IL notes, placeholder values, and unresolved indirect calls. Treat control-flow details as partial evidence and verify against raw IL/native disassembly where critical.

## 1. Background resource identity and path semantics
The recovered Android 1.0 BackgroundResource model carries:
- Id, Name, Type (Image/Spine), Source (Native or custom/override), Root, Path, and optional Spine override data.
- IsCustomSpine is true when Spine override metadata exists.
- FullPath returns Path directly for Native resources; non-Native resources resolve Root + Path through ResolvePath.
- CacheKey includes source/resource identity data; the full getter body is decompiler-noisy and should not be reimplemented byte-for-byte from this output.

Recovered ResolveBackground(id) begins with BackgroundResource.ResolveOverride(id, LocalOverrideData, GlobalOverrideData). If no override matches, it queries the native scenario background-name database and constructs a Native image/background resource using the database filename, filename stem and background type. The body has unresolved expression-tree code and should be checked against the raw IL before reproducing edge cases.

Implication: an iOS adapter must preserve resource source and root/path semantics. It must not flatten every resource to a single path or treat custom Spine resources as ordinary images.

## 2. Static image loading path
Recovered GetBackgroundTexture(BackgroundResource) indicates:
- Reject missing resources and non-Image types; Spine resources require the presenter/Spine path, with a separate async static preview path.
- For non-Native images: use FullPath, check a path-keyed texture cache, read bytes with File.ReadAllBytes, then decode.
- For Native images: normalize the asset path with CorrectPath, call Addressables LoadAssetAsync, wait for completion, track the loaded object, and release the handle on failure.
- DecodeBackgroundImage(bytes, name) creates a 2x2 RGBA32 Texture2D without mipmaps, sets name and Clamp wrap mode, calls ImageConversion.LoadImage(..., markNonReadable: true), and throws if decoding returns false.

iOS runtime baseline confirms UnityFramework contains ImageConversion/LoadImage implementation references. This is not yet proof that every required image format or Addressables provider path works in an iOS build. Required validation: PNG/JPEG and actual background samples; verify alpha, dimensions, color space, orientation, memory use and iOS texture format support.

## 3. Preview request coalescing and cache behavior
Recovered GetBackgroundPreviewAsync / QueueBackgroundPreview indicates:
- A null callback is ignored.
- A cache key is derived from resource identity and preferThumbnail.
- Cached Unity objects are returned only if still valid; RenderTexture-backed previews are additionally checked with IsCreated().
- Concurrent requests for the same key are coalesced into a list of callbacks in pendingBackgroundPreviews.
- Only the first request starts a coroutine (CoLoadBackgroundPreview); later callers join the callback list.
- Preview key construction for custom Spine resources incorporates stamps for .skel, .atlas, optional preview metadata and .skel.preview.png. A persisted preview path is FullPath + ".skel.preview.png".
- The preview eviction code indicates bounds around 25 entries / 32 MiB, evicts least-recently-used candidates, and destroys owned Unity objects. The exact byte accounting and ordering need verification because Cpp2IL output has unresolved iterator/call sites.

Implication: preserve main-thread Unity object lifetime, callback fan-out, cache invalidation and handle ownership. Do not replace this with an arbitrary background thread that constructs Texture2D/RenderTexture objects.

## 4. Spine runtime file contract
Recovered RuntimeSpineLoader.Read(basePath, supportsDxt5) indicates a file-set contract:
- Read text from basePath + ".atlas".
- Read binary skeleton from basePath + ".skel".
- Parse atlas page names using AtlasPages(atlasText).
- Resolve each atlas page relative to the base directory using BackgroundResource.ResolvePath.
- Load each page through RuntimeAtlasTexture.Read(path, supportsDxt5) and retain a page-source dictionary.
- ReadAsync runs this file read in Task.Run and determines DXT5 support using SystemInfo.SupportsTextureFormat(TextureFormat.DXT5).
- Create(files, material, blends, quiet) creates the Spine runtime asset and supplies a custom TextureLoader using page-to-material mapping. The decompiled body is incomplete and should not be treated as executable source.

This establishes that the 1.0 custom runtime expects a .skel + .atlas + one or more page textures, and contains explicit texture-page loading logic. It does not establish that the loose files are present in the release; they may be stored in overrides or generated by another pipeline.

## 5. Proposed implementation order (not yet code)
### Phase A — iOS static image compatibility
1. Recreate resource identity/source/path model and override-first resolution.
2. Implement a Native Addressables path and a custom-file path separately.
3. Implement image decode using the Unity 2023.2.20f1c1-compatible iOS runtime API.
4. Add a path/asset-key cache with explicit Addressables handle ownership and release.
5. Validate with actual Native image resources and custom image overrides.

### Phase B — preview pipeline
1. Preserve one in-flight operation per preview key.
2. Fan out callbacks on Unity main thread.
3. Validate cached Unity objects and RenderTexture.IsCreated().
4. Add persistent .skel.preview.png read/write only after its cache stamp format is reconstructed.
5. Enforce memory/count limits and destroy only objects owned by this cache.

### Phase C — Spine playback
1. Identify exact Spine core and spine-unity assembly versions in Android 1.0 and old iOS.
2. Recover the complete Android RuntimeSpineLoader, RuntimeSpineAsset, RuntimeAtlasTexture and presenter lifecycle contracts from raw IL / verified decompilation.
3. Obtain matching Spine runtime source/package or establish a narrow binary-format/API adapter.
4. Test skeleton version, atlas page loading, blend-mode materials, animation state, disposal/retention and cache eviction.
5. Only then connect custom Spine backgrounds to the iOS presenter.

## 6. What is and is not delivered
- Analysis: delivered in this report.
- Pseudocode: the phase descriptions above are design-level pseudocode/behavior summaries.
- Compilable code: none. UnityEngine/Addressables/Spine references and a matching Unity project are unavailable.
- IPA: not built, signed or device-verified.

## 7. Blocking inputs
- A buildable Unity project/source tree matching the iOS target, or authorized source/recovery route to recreate the game code.
- Matching Spine runtime source/package and license/version information for Android 1.0.
- Unity 2023.2.20f1c1-compatible iOS build toolchain (Unity Editor, Xcode/macOS build host and signing).
- Real Android 1.0 resource samples or a reproducible method to extract the relevant Addressables bundles/overrides and test them.

## Safety of conclusions
No Android ARM64 RVA, pointer, calling convention or machine code is proposed for iOS reuse. All RVA values remain diagnostic references to their original Android binary only.
