# iOS 0.7.9.2 API feasibility for the first background compatibility phase (2026-10-31)

## Source evidence
- iOS 0.7.9.2 IL2CPP dump.cs from cross-platform comparison run 41172721121, artifact 13032345439.
- Android 1.0 recovered IL and exact declarations from runs 41171917303 and 41175531020.
- This report distinguishes a managed API declaration in the iOS metadata from a tested runtime behavior. The dump is not source code and does not prove successful execution on an actual device.

## Confirmed iOS APIs available in the old package

### Image decoding
The iOS 0.7.9.2 dump contains UnityEngine.ImageConversionModule.dll and the following managed API declaration:
- UnityEngine.ImageConversion.LoadImage(Texture2D tex, byte[] data, bool markNonReadable) -> bool
- An overload LoadImage(Texture2D tex, byte[] data) -> bool

The iOS dump also contains UnityEngine.Texture2D and UnityEngine.TextureFormat types. This is stronger evidence than searching strings in UnityFramework: the ImageConversion method is present in the platform-matched IL2CPP metadata declaration set.

### Addressables
The iOS dump contains Unity.Addressables and Unity.ResourceManager types, including:
- Addressables.LoadAssetAsync<TObject>(IResourceLocation location)
- Addressables.LoadAssetAsync<TObject>(object key)
- Addressables.Release<TObject>(TObject obj)
- Addressables.Release<TObject>(AsyncOperationHandle<TObject> handle)
- AsyncOperationHandle completion/wait APIs are present in the Unity.ResourceManager metadata surface.

Therefore, the core Unity primitives used by Android 1.0's static background loader—ImageConversion.LoadImage and Addressables load/release—are represented in the old iOS runtime metadata. This makes a narrow static-image adapter technically plausible, subject to exact overload/generic constraints and runtime tests.

## What is still missing from old iOS game code
- The exact Android 1.0 game-level methods ResolveBackground, GetBackgroundTexture(BackgroundResource), DecodeBackgroundImage, GetBackgroundPreviewAsync and related resource descriptor classes are not present in old iOS 0.7.9.2 metadata.
- The old iOS package does not include the Android 1.0 BackgroundResource/RuntimeSpineLoader/RuntimeSpineAsset/RuntimeAtlasTexture game-level implementation.
- The old iOS Spine runtime API is older than Android 1.0's Spine 4.2-oriented surface; see analysis/spine-runtime-compatibility-gap-ios-vs-android-2026-10-31.md.

## Proposed narrow first-phase adapter
1. Implement a new iOS-compatible resource descriptor and override resolver, preserving local/global override semantics and the native/custom distinction.
2. For custom image overrides, read bytes from the app's writable sandbox path, create a Texture2D on the Unity main thread, call ImageConversion.LoadImage, set name/wrap mode and destroy the texture if decode fails.
3. For native Addressables resources, normalize the key/path, use the iOS runtime's supported LoadAssetAsync overload, track the AsyncOperationHandle, and release it exactly once on failure or when the owner is done.
4. Cache only live Unity Texture objects and keep ownership separate between Addressables-owned assets and textures created by the adapter.
5. Add tests for PNG/JPEG, malformed bytes, alpha, path normalization, repeated load/release, app backgrounding and memory pressure before connecting to gameplay.

## Buildability boundary
- The proposed adapter is design-level behavior, not compilable C#.
- A buildable Unity project/source tree and compatible iOS build environment are still absent.
- No iOS runtime test has been performed; metadata declarations only establish that the required Unity APIs exist in the old binary's type/method surface.
- No IPA has been built or signed.
