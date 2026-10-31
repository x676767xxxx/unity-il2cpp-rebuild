# Android 1.0 CheckForUpdate native call-chain — 2026-10-31

## Evidence

- Android ARM64 disassembly artifact: Run 41192667235, artifact `azurearchive-android-arm64-disassembly-corrected`.
- Android metadata: `dump-android-arm64/dump.cs` from Run 41169040171, artifact `azurearchive-ios-il2cpp-comparison`.
- Full Android managed IL: artifact `azurearchive-raw-il-ios-port-fixed`.
- Inspected coroutine: `ScenarioResourceManager.CheckForUpdate.MoveNext()`, ARM64 RVA `0x326FDF0` through `0x3270D08`.

## Resolved Addressables calls

The native call targets resolve against Android `dump.cs`:

| Call target RVA | Resolved API |
|---|---|
| `0x634D710` | `Addressables.CheckForCatalogUpdates(bool autoReleaseHandle = true)` |
| `0x634DC14` | `Addressables.UpdateCatalogs(bool autoCleanBundleCache, IEnumerable<string> catalogs, bool autoReleaseHandle = true)` |
| `0x6347F50` | `Addressables.GetDownloadSizeAsync(string key)` |
| `0x634917C` | `Addressables.DownloadDependenciesAsync(object key, bool autoReleaseHandle = false)` |
| `0x51AB288` / `0x51AC624` | `AsyncOperationHandle<long/object>.get_Result()` |
| `0x51AC688` | `AsyncOperationHandle<object>.get_Status()` |
| `0x66090D4` | `AsyncOperationHandle.GetDownloadStatus()` |
| `0x32654A0` | `ScenarioResourceManager.OnUpdateComplete()` |

The coroutine's state machine also calls `Directory.Exists`, `Directory.Delete`, `UserSettings.WriteUserSettings()`, `ScenarioResourceManager.OnUpdateComplete()`, and `MonoBehaviour.StartCoroutine()`. It includes download progress, error/popup, and cache-clear branches.

## Reconstructed high-level flow

1. Inspect a configured manual-install directory and its resource-structure version.
2. If a compatible manual install is accepted, use the manual-install completion path.
3. Otherwise call `Addressables.CheckForCatalogUpdates()`.
4. Await and inspect the catalog-update result.
5. When catalogs need updating, call `Addressables.UpdateCatalogs(...)` and await it.
6. Call `Addressables.GetDownloadSizeAsync(string key)` to calculate required download size.
7. Call `Addressables.DownloadDependenciesAsync(object key)` and monitor progress.
8. Continue into `ScenarioResourceManager.OnUpdateComplete()`, which loads static TextAsset/FlatBuffer data and later local override manifests.

This confirms that the normal update path is Addressables catalog/dependency orchestration, not a bespoke raw HTTP download loop.

## New distinction: authoring source versus normal mobile source

The recovered full IL shows:
- `UserSettings.SettingData.customSource` is read in `AuthoringCliHost.Bootstrap()`; when blank, that authoring bootstrap selects `https://filev2.foxxlight.top/aares/`, and when nonblank it trims the trailing slash and uses the custom value.
- The authoring bootstrap then prepares `cli-resources` files and calls `ScenarioResourceManager.InitializeAuthoringResources(...)`.
- The string `{ScenarioResourceManager.AddrLoadUrl}/` appears in `AuthoringOfflineResources` prefix-validation helpers.
- The managed IL search does not show a normal mobile gameplay assignment to `ScenarioResourceManager.AddrLoadUrl`. The `customSource` backing field is not evidence that the mobile game uses this setting as its CDN source.
- `UserSettings.SettingData.catalogUrlOverride` has accessors in the dump, but no confirmed runtime call site was recovered in this IL pass.

The normal mobile coroutine calls `AuthoringOfflineResources.TryGetStartupUrl(out url)` as part of its startup state machine. That helper belongs to the authoring-offline subsystem; its exact effect and activation conditions are being checked separately. Its presence alone does not prove the runtime CDN URL.

## What remains unproven

The exact value and assignment site of the normal mobile `ScenarioResourceManager.AddrLoadUrl` remain unresolved. The Addressables calls consume catalog/locator configuration established elsewhere. Do not infer the production CDN from the authoring CLI fallback or from catalog token placeholders.

## Next investigation

1. Inspect native bodies for `AuthoringOfflineResources.TryGetStartupUrl`, `get_IsActive`, `ValidateDirectory`, and `AuthoringCliHost.Bootstrap`.
2. Inspect `UserSettings.ReadUserSettings.MoveNext` and `UIPatchDownload.CoChangeSource.MoveNext` to distinguish settings persistence/UI behavior from the runtime source assignment.
3. Trace the static field write to `ScenarioResourceManager.AddrLoadUrl` and its initialization order.
4. Compare Android/iOS provider configuration and token substitution before implementing an iOS URL provider.
5. Preserve the sequence catalog check -> catalog update -> dependency-size check -> dependency download -> post-update data loading.

## Porting status

The Android resource-update flow is now substantially mapped. The URL provider is not yet proven, and no complete iOS UnityFramework, archive, or IPA has been produced.
