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


## Follow-up native verification: offline startup URL gate

Run 41192985832 completed successfully and added native disassembly for:
- `AuthoringOfflineResources.TryGetStartupUrl(out string)` at RVA `0x33C5E00`.
- `AuthoringOfflineResources.get_IsActive()` at RVA `0x33C5D68`.
- `AuthoringOfflineResources.ValidateDirectory(...)` at RVA `0x33C6320`.
- `AuthoringCliHost.Bootstrap()` at RVA `0x33838E4`.
- `UserSettings.ReadUserSettings.MoveNext()` at RVA `0x3289908`.
- `UIPatchDownload.CoChangeSource.MoveNext()` at RVA `0x3285C5C`.

The native `get_IsActive()` body reads the static `AuthoringOfflineResources.IsActive` backing flag. `CheckForUpdate.MoveNext()` calls `TryGetStartupUrl` before continuing through the regular update state machine; this is an authoring-offline startup hook, not proof of the ordinary mobile CDN value. The managed implementation of `TryGetStartupUrl` is heavily damaged by unresolved native/indirect operations, so its exact URL source and activation conditions remain under investigation.

The `AuthoringCliHost.Bootstrap()` managed IL confirms the CLI branch uses `UserSettings.SettingData.customSource`, falls back to `https://filev2.foxxlight.top/aares/` only when blank, trims a trailing slash on custom input, and prepares the local `cli-resources` files before calling `InitializeAuthoringResources(...)`. That remains authoring-specific evidence.

The `UserSettings.ReadUserSettings.MoveNext()` and `UIPatchDownload.CoChangeSource.MoveNext()` native bodies are now available for further inspection. Their presence alone does not establish an assignment to `ScenarioResourceManager.AddrLoadUrl`; no such assignment has yet been demonstrated.

Updated disassembly artifact: Run 41192985832, artifact `azurearchive-android-arm64-disassembly-corrected`.

## Follow-up native inspection of settings and source-change coroutines — artifact 13039996236

The corrected Android ARM64 disassembly artifact was downloaded and inspected directly.

### UserSettings.ReadUserSettings.MoveNext (RVA 0x3289908–0x32899e8)
- The coroutine checks file/deserialization-related results, then calls UserSettings.InitializeSettings() (RVA 0x3287388) on its continuation paths.
- This short state machine does not visibly write ScenarioResourceManager.AddrLoadUrl and does not itself reveal a gameplay CDN literal.
- Settings initialization is therefore separate from the still-unresolved mobile Addressables URL provider.

### UIPatchDownload.CoChangeSource.MoveNext (RVA 0x3285c5c–0x3286038)
- The coroutine operates on UI-patch/source-selection state and persists/uses several setting/UI objects.
- Its final state calls a helper at RVA 0x325a97c with values assembled from coroutine fields and local state.
- No direct static-field store to ScenarioResourceManager.AddrLoadUrl is visible in this method body.
- The UI source-change flow is a candidate for further inspection, but this disassembly alone does not prove it sets the gameplay Addressables base URL.

### AuthoringOfflineResources.TryGetStartupUrl (RVA 0x33c5e00–0x33c6320)
- This method performs multi-stage validation/resolution, invokes ValidateDirectory at RVA 0x33c6320, and returns a selected URL/string through its out parameter on a success path.
- CheckForUpdate.MoveNext invokes it at RVA 0x3270040 before proceeding into its update state machine.
- The method belongs to the authoring-offline startup subsystem. Its presence in the normal coroutine does not prove the returned URL is the production CDN; it appears to be an optional offline/manual-startup override gate.
- No direct store to ScenarioResourceManager.AddrLoadUrl is visible in this method body.

### Updated conclusion
The ordinary update coroutine has a verified optional offline-startup URL gate, then performs Addressables catalog update and dependency download. The AddrLoadUrl assignment remains unresolved. Do not hard-code the authoring CLI fallback, and do not treat the manual/offline startup URL as the normal remote source without proving the activation branch and assignment target.

## Native verification of UI source submission callback — run 41196234346

Artifact `azurearchive-android-arm64-disassembly-corrected`, ID `13040812483`, completed successfully. The exact callback declaration `UIPatchDownload.<>c.<CoChangeSource>b__13_1(string source)` is at RVA `0x3285AB0`.

Direct ARM64 inspection shows:
- The incoming `source` argument is preserved in X19.
- The callback stores that string at object offset `0x90` on the `UserSettings.SettingData` object reached through the singleton/settings object chain.
- Android `dump.cs` identifies `UserSettings.SettingData.<customSource>k__BackingField` at offset `0x90`; it is not the `ScenarioResourceManager.AddrLoadUrl` static field.
- The callback later calls `PopupManager.ShowPopupOk(...)` at RVA `0x325A46C`, consistent with the UI source-change confirmation flow.
- Therefore this callback proves persistence/update of the authoring/UI `customSource` setting; it does **not** prove assignment of the normal mobile Addressables base URL. Do not wire `customSource` into `AddrLoadUrl` based on this callback.

The Android metadata separately declares `UserSettings.SettingData.catalogUrlOverride` at field offset `0x18`, but no runtime use/assignment has yet been proven. The actual mobile `AddrLoadUrl` writer remains unresolved.

### Follow-up methods queued for direct disassembly
The existing Android native-disassembly workflow was extended (no new workflow or source file) to inspect:
- `UserSettings.WriteUserSettings` (RVA `0x3284FB0`)
- `UserSettings.InitializeSettings` (RVA `0x3287388`)
- `UserSettings.SettingData.set_catalogUrlOverride` (RVA `0x3287DAC`)
- `ScenarioResourceManager.Awake` (RVA `0x3264724`)
- `ScenarioResourceManager.Initialize` (RVA `0x3264A00`)
- `ScenarioResourceManager.UpdateAddressable` (RVA `0x3265514`)

The resulting run is [41196441783](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41196441783), currently queued at checkpoint creation.
