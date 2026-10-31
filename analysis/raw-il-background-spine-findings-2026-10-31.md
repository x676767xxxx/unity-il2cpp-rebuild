 # Raw IL findings for Android 1.0 background and Spine paths (2026-10-31)

## Sources
- Raw IL export artifact: run 41171917303, artifact 13031387716 (1,287,548 bytes).
- Inputs: Android 1.0 and Windows 1.0.1 Cpp2IL recovered Assembly-CSharp.dll outputs from run 41169615291.
- These are recovered IL listings, not original C# source. Cpp2IL reports unresolved native instructions and missing method bodies; use call relationships only where the IL is explicit and corroborated.

## Verified IL-level behavior

### BackgroundResource resolution
Android and Windows ScenarioResourceManager.ResolveBackground(uint) both explicitly call:
- BackgroundResource.ResolveOverride(id, localOverrides, globalOverrides).
- The scenario background-name database (ScenarioBGNameDbSchema.GetExcel / ScenarioBGNameExcel).
- Filename stem extraction via Path.GetFileNameWithoutExtension.
- Construction of BackgroundResource with ID, name, background type, resource source/type, path/root and optional Spine override data.

The recovered method has unresolved expression-tree and memory-load sections (Android 40 warning markers; Windows 19). Exact precedence/fallback details must be confirmed from the separate BackgroundResource.ResolveOverride export and original call sites before implementing.

### Static texture loading
The GetBackgroundTexture(BackgroundResource) IL on both platforms contains:
- BackgroundResource.FullPath lookup.
- Texture-cache dictionary lookup and Unity object validity check.
- Custom-file path: File.ReadAllBytes(FullPath) followed by DecodeBackgroundImage.
- Native path: CorrectPath, Addressables async handle completion (WaitForCompletion), and handle release on failure.
- Cache insertion and explicit error paths.

This is stronger than string presence: the IL includes direct calls to File.ReadAllBytes, DecodeBackgroundImage, Addressables.WaitForCompletion and Addressables.Release. It still does not prove the old iOS 0.7.9.2 build has the same managed path or that all file formats decode identically.

### Image decode
Android and Windows DecodeBackgroundImage(byte[], string) IL explicitly calls:
- Texture2D(int width, int height, TextureFormat, bool mipChain).
- Set texture name and wrap mode.
- UnityEngine.ImageConversion.LoadImage(texture, bytes, markNonReadable).
- RuntimeSpineLoader.DestroyOwned on failure cleanup.
- InvalidDataException for decode failure.

The method body has decompiler-warning markers (Android 11; Windows 3), so constructor arguments and exception/control-flow details need raw native verification. ImageConversion.LoadImage is a direct IL call, not merely a binary string hit.

### Preview cache
GetBackgroundPreviewAsync(resource, preferThumbnail, callback) on both platforms explicitly:
- Builds BackgroundPreviewKey(resource, preferThumbnail).
- Looks up cached Unity Texture objects.
- Validates Unity object liveness and RenderTexture.IsCreated().
- Uses Time.frameCount in preview freshness logic.
- Calls QueueBackgroundPreview to join/start pending work.

The Android method has 35 warning markers; Windows has 23. Preserve callback fan-out and main-thread Unity object lifetime, but reconstruct exact pending-operation semantics from the coroutine/state-machine exports before coding.

### Spine acquisition: a notable platform discrepancy
Android AcquireBackgroundSpine(resource) recovered IL calls:
- RuntimeSpineLoader.Load(fullPath, material, blendModeMaterials, quiet).
- SkeletonDataAsset.GetSkeletonData(...).
- SpineBackgroundVisual.ValidatePlayback(...).
- RuntimeSpineAsset.Retain() and TrimBackgroundSkeletonCache().

Windows AcquireBackgroundSpine(resource) instead visibly calls:
- SystemInfo.SupportsTextureFormat(TextureFormat).
- RuntimeSpineLoader.Read(fullPath, supportsDxt5).
- RuntimeSpineLoader.Create(files, material, blendModeMaterials, quiet).
- Then the same skeleton validation / retain / cache trimming family.

This difference may represent platform-specific conditional compilation, an Android wrapper method, or a Cpp2IL recovery gap. It is not safe to assume one platform's implementation is byte-for-byte identical. The next targeted export inspects RuntimeSpineLoader, RuntimeSpineAsset, RuntimeAtlasTexture, and Spine runtime types directly for both platforms.

## Decompiler quality warning counts
Counts below are marker occurrences in each method's recovered IL, not percentages of incorrect code.

| Method | Android markers | Windows markers |
|---|---:|---:|
| ResolveBackground | 40 | 19 |
| GetBackgroundTexture(BackgroundResource) | 20 | 2 |
| DecodeBackgroundImage | 11 | 3 |
| GetBackgroundPreviewAsync | 35 | 23 |
| AcquireBackgroundSpine | 43 | 16 |

Android's recovered IL is more degraded for these methods. Prefer cross-platform corroboration and native disassembly for uncertain branches.

## Implementation consequence
1. Static image loading is the best first compatibility-layer target: the call chain and Unity APIs are explicit and small enough to validate with tests.
2. Background resolution should wait for the exact ResolveOverride and BackgroundResource property/constructor behavior.
3. Preview caching should wait for pending callback and coroutine state-machine details.
4. Spine porting should wait for the targeted type export to explain Android Load vs Windows Read/Create, and for the exact Spine runtime type/version surface.

## Delivery boundary
- This report is analysis based on recovered IL.
- No source patch is supplied.
- No Unity project or matching Spine package is available in the repository artifacts as a buildable project.
- No compilable iOS compatibility code or IPA has been produced.
- No Android RVA or machine code is proposed for iOS reuse.
