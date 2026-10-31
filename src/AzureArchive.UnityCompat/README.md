# Unity compatibility source candidates

`BackgroundImageDecoder.Unity.cs` is a Unity-side source candidate based on the recovered Android 1.0 / Windows 1.0.1 IL and the old iOS 0.7.9.2 metadata API surface.

## Evidence mapping
- Initial texture: 2 x 2.
- Format: `TextureFormat.RGBA32` (Unity enum value 4).
- Mip chain: false.
- Wrap mode: `TextureWrapMode.Clamp` (enum value 1).
- Decode: `ImageConversion.LoadImage(texture, bytes, true)`.
- False return: `InvalidDataException` with the recovered message prefix.

## Filesystem adapter

`BackgroundImageDecoder.DecodeFile(path)` reads a custom image file synchronously and delegates to `Decode`. The overload `DecodeFile(resourceRoot, relativePath)` first confines manifest/override-provided paths with `SafeResourcePath.ResolveUnderRoot`, then reads and decodes the file. Prefer the root-confined overload for any path originating outside trusted code. Call either overload only on Unity's main thread because it creates a Texture2D. The file read itself is synchronous; callers needing asynchronous I/O should read bytes off-thread and marshal only the Decode call back to the Unity main thread.

## Status
This is candidate Unity C# source, not an integrated game patch. It has **not** been compiled against UnityEngine assemblies, run in Unity, or tested on iOS. Unity texture creation and destruction must occur on the Unity main thread. The cleanup behavior on decoder exceptions is a conservative implementation choice; the corresponding original IL contains unresolved decompiler regions.

The overload `BackgroundImageDecoder.DecodeCustomResource(BackgroundResourceDescriptor)` consumes the descriptor's root-confined `FullPath` from `../AzureArchive.BackgroundCompat`; callers should use this descriptor-based entry point for override resources.

## Addressables adapter

`AddressablesRuntimeAdapter.Load<TAsset>(key, onLoaded, onFailed)` wraps the Addressables `LoadAssetAsync<TAsset>(object key)` completion event. On success it transfers ownership of the completed operation handle to an `AddressableAssetLease`; the consumer must dispose the lease exactly once when finished. On a failed operation, the adapter releases the failed handle before reporting the error. If the consumer's success callback throws, the adapter disposes the newly created lease before rethrowing.

The adapter is compiled in CI against isolated API stubs that mirror the old iOS metadata surface. This verifies C# structure and ownership/error paths, not binary compatibility with the exact Addressables package, Unity player execution, or runtime catalog/provider behavior. Before gameplay integration, compile against the actual Unity 2023.2.20f1c1/Addressables assemblies and test a real catalog key and bundle on device.


### Catalog internal-ID transform hook

`AddressablesInternalIdTransformHook.Install(getAddrLoadUrl)` registers a transform through `Addressables.InternalIdTransformFunc`. It chains any previously registered transform, rewrites only IDs beginning with the verified `{ScenarioResourceManager.AddrLoadUrl}` token, and leaves unrelated IDs unchanged. Disposing restores the previous delegate only if this hook still owns the global slot, avoiding overwriting a later subsystem's registration.

Install it before the relevant catalog-backed loads begin. The supplied URL callback must read the actual game runtime configuration; do not pass the authoring CLI fallback URL without proving it is the mobile runtime value.

The platform-matched iOS 0.7.9.2 dump confirms both `ScenarioResourceManager.AddrLoadUrl` (static string field) and `Addressables.InternalIdTransformFunc` (public static `Func<IResourceLocation, string>` property). `InstallFromRuntimeType(Type)` can bind the hook to a supplied game type, and `InstallFromGameRuntime()` locates the global `ScenarioResourceManager` in `Assembly-CSharp` or the loaded assemblies and reads `AddrLoadUrl` lazily. The test suite covers lazy URL changes and rejects a missing/wrongly typed field.

This is a source-level integration adapter, not yet called from the shipped game's startup path. The standalone tests use an isolated API stub; compile and test against the exact Unity 2023.2.20f1c1 / Addressables assemblies before shipping.


## Static background runtime cache

`BackgroundImageRuntimeCache` mirrors the recovered Android `ScenarioResourceManager.GetBackgroundTexture(BackgroundResource)` split:

- Custom Image: decode the descriptor's safe `FullPath`, cache by full path, and destroy only cache-owned custom textures on `Dispose`.
- Native Image: load as Unity `Texture` (not only Texture2D), apply the game's existing `CorrectPath(string)`, deduplicate concurrent requests, and retain the Addressables operation in an idempotent lease.
- The cache can bind a live manager instance with `CreateFromGameRuntime(manager)`, or resolve the inherited `Singleton<ScenarioResourceManager>.Instance` with `CreateFromGameRuntimeSingleton(typeof(ScenarioResourceManager))`. Call the singleton factory only after the game manager has initialized.
- Dispose the cache on the Unity main thread when the owning manager/presenter is destroyed. In-flight Addressables requests cannot be cancelled; if they finish after disposal, the cache releases their leases without publishing the asset.
- Non-Image resources are rejected. Spine skeleton/atlas playback must use the separate Spine runtime path and is not implemented by this class.

CI [run 41199317839](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41199317839) passed the isolated Unity API-stub compile/tests, including native/custom cache ownership, CorrectPath binding, singleton lookup, and pending-load disposal. This is not a Unity Editor or device test.


## Spine owned-object cleanup

`SpineOwnedObjectCleanup.Unity.cs` mirrors the Android 1.0 `RuntimeSpineLoader.DestroyOwned` split: ignore Unity-null objects, call deferred `Object.Destroy` while `Application.isPlaying`, and call `Object.DestroyImmediate` otherwise. `Rollback` consumes the partial object snapshot returned by `SpineAssetConstructionLedger.Abort`. Both operations must run on Unity's main thread.

CI [run 41205621240](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41205621240) now explicitly includes this source in the UnityCompat test compilation and passes play-mode destruction, edit-mode destruction, null-object handling, batch rollback, and null-collection validation against isolated Unity API stubs. This confirms the source compiles against the stubs and its branch behavior is exercised; it is not yet compiled against the actual Unity assemblies or executed in the Unity player.
