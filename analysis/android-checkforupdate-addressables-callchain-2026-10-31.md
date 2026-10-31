# Android 1.0 CheckForUpdate native call-chain — 2026-10-31

## Evidence

- Android ARM64 disassembly artifact: Run 41192667235, artifact `azurearchive-android-arm64-disassembly-corrected`.
- Android metadata: `dump-android-arm64/dump.cs` from Run 41169040171, artifact `azurearchive-ios-il2cpp-comparison`.
- Inspected coroutine body: `ScenarioResourceManager.CheckForUpdate.MoveNext()`, RVA `0x326FDF0` through `0x3270D08`.

## New verified Addressables calls

The native call targets resolve against Android `dump.cs`:

| Call target RVA | Resolved API | Evidence / role |
|---|---|---|
| `0x634D710` | `Addressables.CheckForCatalogUpdates(bool autoReleaseHandle = true)` | Starts catalog update check |
| `0x634DC14` | `Addressables.UpdateCatalogs(bool autoCleanBundleCache, IEnumerable<string> catalogs, bool autoReleaseHandle = true)` | Applies catalog updates |
| `0x6347F50` | `Addressables.GetDownloadSizeAsync(string key)` | Calculates required download size |
| `0x634917C` | `Addressables.DownloadDependenciesAsync(object key, bool autoReleaseHandle = false)` | Downloads required dependencies |
| `0x51AB288` / `0x51AC624` | `AsyncOperationHandle<long/object>.get_Result()` | Reads async operation results |
| `0x51AC688` | `AsyncOperationHandle<object>.get_Status()` | Checks async operation status |
| `0x66090D4` | `AsyncOperationHandle.GetDownloadStatus()` | Reads download progress |
| `0x32654A0` | `ScenarioResourceManager.OnUpdateComplete()` | Starts post-update resource initialization |

## Reconstructed high-level flow

The coroutine state machine performs the following operations:

1. Checks whether the configured manual-install path exists and whether its structure version is compatible.
2. If a manual install is accepted, it records/uses that install path and enters the completion path.
3. Otherwise, it invokes `Addressables.CheckForCatalogUpdates()`.
4. It awaits the returned operation and inspects its result.
5. When catalog updates are returned, it invokes `Addressables.UpdateCatalogs(...)` and awaits completion.
6. It invokes `Addressables.GetDownloadSizeAsync(string key)` and awaits the required download size.
7. It invokes `Addressables.DownloadDependenciesAsync(object key)` for the required dependency key and awaits completion/progress.
8. It enters `ScenarioResourceManager.OnUpdateComplete()` and later the previously documented TextAsset/FlatBuffer loading and local override path.

The generated state machine contains progress/status and error branches, including a popup path, download-progress reporting, and a cache-clear path. This confirms that the update path is standard Addressables catalog/dependency orchestration, rather than a custom raw HTTP download loop.

## What is not proven

This is a substantial recovery of the resource-update call chain, but it does not expose the concrete catalog URL or prove where the static field `ScenarioResourceManager.AddrLoadUrl` is assigned. The Addressables API calls consume the catalog/locator configuration established elsewhere. Do not infer the production CDN from these call targets alone.

## Next investigation

1. Locate all native references/writes to the static `ScenarioResourceManager.AddrLoadUrl` field and identify its initialization order.
2. Inspect the settings/catalog provider setup and determine how the token `{ScenarioResourceManager.AddrLoadUrl}` is substituted into catalog and hash URLs.
3. Compare Android and iOS provider configuration and platform-specific catalog/hash settings.
4. Only after the source value and precedence are proven, implement an iOS-compatible URL provider before `CheckForCatalogUpdates()`.
5. Validate catalog update, size calculation, dependency download, then post-update TextAsset loading on iOS.

## Porting implication

The safest iOS integration point is before the first `CheckForCatalogUpdates()` call in the mobile startup path. Preserve the existing sequence (catalog check -> catalog update -> dependency size -> dependency download -> OnUpdateComplete) and avoid bypassing it with a hard-coded URL or manually downloading individual bundles.

No complete iOS UnityFramework, archive, or IPA has been built yet.
