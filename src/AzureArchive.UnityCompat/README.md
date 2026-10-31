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

The standalone .NET Standard path utility in `../AzureArchive.BackgroundCompat` is independently build-tested, but it is not connected to this decoder yet.

## Addressables adapter

`AddressablesRuntimeAdapter.Load<TAsset>(key, onLoaded, onFailed)` wraps the Addressables `LoadAssetAsync<TAsset>(object key)` completion event. On success it transfers ownership of the completed operation handle to an `AddressableAssetLease`; the consumer must dispose the lease exactly once when finished. On a failed operation, the adapter releases the failed handle before reporting the error. If the consumer's success callback throws, the adapter disposes the newly created lease before rethrowing.

The adapter is compiled in CI against isolated API stubs that mirror the old iOS metadata surface. This verifies C# structure and ownership/error paths, not binary compatibility with the exact Addressables package, Unity player execution, or runtime catalog/provider behavior. Before gameplay integration, compile against the actual Unity 2023.2.20f1c1/Addressables assemblies and test a real catalog key and bundle on device.


### Catalog internal-ID transform hook

`AddressablesInternalIdTransformHook.Install(getAddrLoadUrl)` registers a transform through `Addressables.InternalIdTransformFunc`. It chains any previously registered transform, rewrites only IDs beginning with the verified `{ScenarioResourceManager.AddrLoadUrl}` token, and leaves unrelated IDs unchanged. Disposing restores the previous delegate only if this hook still owns the global slot, avoiding overwriting a later subsystem's registration.

Install it before the relevant catalog-backed loads begin. The supplied URL callback must read the actual game runtime configuration; do not pass the authoring CLI fallback URL without proving it is the mobile runtime value. The standalone tests use an isolated API stub; compile and test against the exact Unity 2023.2.20f1c1 / Addressables assemblies before shipping.
