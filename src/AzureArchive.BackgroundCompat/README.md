# AzureArchive background and Addressables compatibility primitives

This project contains standalone, testable C# primitives recovered or derived from the Android 1.0 / Windows 1.0.1 behavior and the old iOS 0.7.9.2 metadata surface.

## Components

- `SafeResourcePath`: prevents rooted paths and traversal outside a trusted resource root.
- `BackgroundOverrideResolver`: applies local-before-global override precedence; caller supplies the verified ID matcher.
- `AddressableInternalIdResolver`: replaces the exact catalog prefix `{ScenarioResourceManager.AddrLoadUrl}` with a runtime-supplied HTTP(S) base URL after validating the suffix and origin. It deliberately does not supply or guess the CDN URL.
- `AddressableAssetLease`: owns one successful Addressables operation handle and releases it exactly once through a supplied callback.
- `SpineAtlasPageNames` / `SpineAtlasPageResourceResolver`: extracts atlas page names and maps them to caller-provided resource keys.
- `SpineAtlasPageCache`: tracks texture ownership across active leases and defers release until the final lease is returned.

## Unity-side adapter

`AzureArchive.UnityCompat/AddressablesRuntimeAdapter.Unity.cs` wraps `Addressables.LoadAssetAsync<T>(object key)` and transfers successful handles into `AddressableAssetLease`. The adapter is compiled against isolated API stubs in CI, not the exact Unity/Addressables binaries from the target project.

## Validation

The GitHub Actions workflow `Build background path compatibility utility` builds the .NET Standard 2.1 library and runs deterministic unit-style checks. A passing workflow validates these standalone components only; it does not prove integration with the game catalog, its `ScenarioResourceManager`, a real Addressables provider/catalog, Spine 4.2, or an iOS player.

## Missing integration inputs

- Exact implementation/configuration source for `ScenarioResourceManager.AddrLoadUrl`.
- Unity project/source tree and actual target Addressables assemblies.
- Real catalog/provider runtime test and bundle access configuration.
- Newer Spine 4.2 runtime or compatible source/packages for iOS.
- macOS/Xcode Unity iOS build environment.

No IPA is produced by this standalone compatibility project.
