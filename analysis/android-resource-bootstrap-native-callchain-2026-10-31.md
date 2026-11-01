# Android 1.0 resource bootstrap native call-chain checkpoint — 2026-10-31

## Reproducible disassembly

Source: Android 1.0 ARM64 `libil2cpp.so`, exact RVAs verified against `dump-android-arm64/dump.cs` from artifact `azurearchive-ios-il2cpp-comparison`.

- Native disassembly [Run 41192374000](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41192374000), artifact `azurearchive-android-arm64-disassembly-corrected`.
- Follow-up state-machine disassembly [Run 41192427431](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41192427431) adds compiler-generated coroutine `MoveNext` bodies; its result is pending at the time of this checkpoint.

## Verified method entry points and relationships

| Method | ARM64 RVA | Directly observed relationship |
|---|---:|---|
| `ScenarioResourceManager.Awake()` | `0x3264724` | Performs singleton/runtime setup and calls `InitializeAfterSplash()` at `0x326498C` |
| `ScenarioResourceManager.InitializeAfterSplash()` | `0x326498C` | Coroutine factory; creates/schedules generated iterator `<InitializeAfterSplash>d__89` |
| `<InitializeAfterSplash>d__89.MoveNext()` | `0x32771DC` | Added to follow-up disassembly to inspect coroutine body |
| `ScenarioResourceManager.Initialize()` | `0x3264A00` | Performs Addressables/resource setup and calls `CheckForUpdate()` at `0x32646B0` near completion |
| `ScenarioResourceManager.CheckForUpdate()` | `0x32646B0` | Coroutine factory; creates/schedules generated iterator `<CheckForUpdate>d__87` |
| `<CheckForUpdate>d__87.MoveNext()` | `0x326FDF0` | Added to follow-up disassembly to inspect catalog/hash/update URL use |
| `ScenarioResourceManager.UpdateAddressable()` | `0x3265514` | Method body is effectively empty at this RVA in the dump; not a useful initialization hook by itself |

## What this establishes

The normal mobile resource initialization is a real native path through `Awake`, the splash coroutine, `Initialize`, and the update-check coroutine. This is the correct path to inspect for the actual remote source and Addressables catalog initialization. It is distinct from the authoring CLI fallback described in `analysis/android-url-source-evidence-2026-10-31.md`.

The first disassembly pass does not show a direct `AddrLoadUrl` field write in these wrapper methods. The actual work is in the generated coroutine `MoveNext` bodies and lower-level Addressables/ResourceManager calls, which are being inspected next.

## Porting consequence

Do not inject the authoring CLI fallback into iOS. The iOS port needs to initialize the URL provider before the mobile `CheckForUpdate` / catalog load path runs, but the provider's value and precedence still need to be recovered from the coroutine/native calls or runtime configuration. Once proven, install the Addressables transform at the earliest safe point in this mobile bootstrap path.

No Unity project, iOS UnityFramework integration, iOS archive, or IPA has been produced.

## Follow-up: recovered managed IL and Addressables consumer path

The full recovered Android ScenarioResourceManager.il was inspected directly from artifact azurearchive-raw-il-ios-port-fixed (Run 41191415958).

### Startup and cache setup

- <InitializeAfterSplash>d__89.MoveNext (managed IL RVA 0x688598) waits for the startup splash condition, then calls ScenarioResourceManager.Initialize().
- Initialize() (native ARM64 RVA 0x3264A00) performs a cache-path branch using UserSettings.CachePath. When a configured cache directory exists, it checks/adds a Unity Cache and sets it as the current writing cache; otherwise it starts CheckForUpdate() as a coroutine.
- CheckForUpdate() (native ARM64 RVA 0x32646B0) creates/schedules the update coroutine. Its recovered managed MoveNext is heavily damaged by an unresolved indirect jump, so the full catalog/hash URL construction is not recoverable from the IL export alone.

### Post-update Addressables asset consumers

The <OnUpdateComplete>d__93.MoveNext recovered body establishes a post-update consumer sequence:
- constructs AddressableFlatBufferLoader instances for static FlatBuffer/TextAsset data, including FlatData/bgmadditionalinfoexceltable.bytes;
- calls AddressableFlatBufferLoader.LoadAll(callback);
- after the loader stage it calls ScenarioResourceManager.LoadGlobalResourceOverrides().

Exact loader declarations in Android dump.cs:
- AddressableFlatBufferLoader.Load(): RVA 0x326D3A8.
- AddressableFlatBufferLoader.LoadAll(Action): RVA 0x326D520.
- ScenarioResourceManager.LoadGlobalResourceOverrides(): RVA 0x326A4F8.
- ScenarioResourceManager.GetResourceOverrideRoot(): RVA 0x3265028.
- ScenarioResourceManager.<OnUpdateComplete>b__93_1(AsyncOperationHandle<TextAsset>): RVA 0x326BE84.

Recovered IL for AddressableFlatBufferLoader.Load() references the Unity.Addressables.Addressables type and registers an AsyncOperationHandle<TextAsset>.Completed callback. This confirms that the app's post-update static-data path consumes Addressables TextAssets; it does not yet reveal the URL provider used by the catalog.

### Local override manifest is a separate path

LoadGlobalResourceOverrides() begins by invalidating the global authoring catalog, calls GetResourceOverrideRoot(), combines that root with manifest.json, checks for the file, and reads/deserializes it into ResourceOverrideData. This is a local override manifest path, not evidence for the remote Addressables CDN base URL.

### Current inference and remaining gap

The integration point for an iOS resource loader is now more concrete: preserve the mobile startup order, initialize the Addressables URL transform before the update/catalog coroutine begins, and retain the later TextAsset/FlatBuffer and local override stages. However, the exact runtime URL assignment and catalog-update call chain remain unproven because the key CheckForUpdate coroutine contains unresolved native/indirect operations in the managed IL export.

The next native disassembly pass adds the Addressables loader and local override methods to distinguish remote catalog setup from local override processing. No production CDN is hard-coded and no IPA has been produced.

## Follow-up native disassembly checkpoint — 2026-11-01

The previously generated Android ARM64 disassembly artifact from workflow [41200328434](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41200328434), artifact ID `13041957634`, was downloaded and inspected directly. This adds native evidence but does not resolve the mobile URL assignment.

### Confirmed startup coroutine behavior

- `ScenarioResourceManager.InitializeAfterSplash()` at RVA `0x326498C` is a coroutine factory. Its generated `MoveNext` is at `0x32771DC`.
- The startup iterator reaches `ScenarioResourceManager.Initialize()` at RVA `0x3264A00`; it does not establish an `AddrLoadUrl` assignment.
- The generated `ScenarioResourceManager.CheckForUpdate()` iterator has its native `MoveNext` at RVA `0x326FDF0`. Within its state-machine body, the code calls `AuthoringOfflineResources.TryGetStartupUrl(out string)` at RVA `0x33C5E00` before continuing through Addressables/catalog-related calls. This confirms the optional startup/offline URL helper is consulted in the mobile update coroutine. It does not prove that the returned value is assigned to `ScenarioResourceManager.AddrLoadUrl`.
- `AuthoringOfflineResources.TryGetStartupUrl` reads its own helper/configuration state and has separate calls for validation/catalog handling. The helper is not a verified production CDN provider.
- `ScenarioResourceManager.UpdateAddressable()` at RVA `0x3265514` is a single `ret` instruction in this Android build; it is not the missing URL initialization method.
- `UserSettings.ReadUserSettings.MoveNext()` at RVA `0x3289908` deserializes settings through `SettingData.Deserialize` and then invokes `InitializeSettings`. The inspected body does not show a direct write to the static `AddrLoadUrl` field.

### Decision

No production URL has been established. Keep the URL provider injectable and do not hard-code the authoring CLI fallback `https://filev2.foxxlight.top/aares/`. The next useful reverse-engineering step is a metadata/native cross-reference of the static field itself (including reflection/native field writes), not another guessed URL adapter.

No C# source was changed in this checkpoint because the required runtime value and precedence are still unknown. Existing compatibility modules remain intact; no duplicate files were added. This checkpoint does not produce a Unity/Xcode build or IPA.


## Field-reference narrowing — 2026-11-01

A second pass inspected the complete recovered Android `Assembly-CSharp.full.il`, the native `TryGetStartupUrl` body and the settings-field layout rather than treating the call site alone as proof of the runtime URL source.

### New evidence

- In the complete Android `Assembly-CSharp.full.il`, the only two managed uses of the `ScenarioResourceManager.AddrLoadUrl` field are in generated delegates `AuthoringOfflineResources/<>c::<ValidateDirectory>b__5_2` and `<ValidateDirectory>b__5_3`. Both use the value only to recognize/strip the `{AddrLoadUrl}/` prefix while validating offline resource paths.
- The native `CheckForUpdate` coroutine does call `AuthoringOfflineResources.TryGetStartupUrl(out string)` at RVA `0x33C5E00`, but the helper's recovered managed IL enumerates command-line arguments and explicitly looks for `--aap-offline-resources`. Its generated predicate `<TryGetStartupUrl>b__4_1` compares each argument with that exact flag. The call site therefore does not establish that the offline mode is active during ordinary gameplay.
- `AuthoringOfflineResources.get_IsActive()` at RVA `0x33C5D68` returns an internal static active-state flag. The helper is an authoring/offline integration, not a proven normal mobile CDN provider.
- The Android `UserSettings.SettingData` class contains both `catalogUrlOverride` (field offset `0x18`) and `customSource` (field offset `0x90`). The `UIPatchDownload` submit callback writes the submitted string into the settings object at offset `0x90` and then calls `UserSettings.WriteUserSettings()`; this proves persistence of the custom source setting, but not that it is copied into `AddrLoadUrl` during normal mobile startup.
- `UserSettings.WriteUserSettings()` serializes the settings data. The recovered IL/native evidence still does not show a verified mobile-runtime assignment from `customSource` or `catalogUrlOverride` to `ScenarioResourceManager.AddrLoadUrl`.

### Corrected conclusion

The earlier wording that the offline URL helper is simply part of the ordinary mobile URL initialization path was too strong. The native update coroutine calls the helper, but the helper is gated by an explicit authoring/offline command-line mode. The ordinary gameplay URL source remains unresolved.

Do not use `https://filev2.foxxlight.top/aares/` as a presumed production default, and do not assume `customSource` or `catalogUrlOverride` is the effective source until the native startup dataflow proves the precedence and assignment. No runtime source code was changed because a guessed assignment would risk breaking catalog and bundle resolution.


## Follow-up: classify the customSource references — 2026-11-01

The full-IL offsets for `customSource` were mapped back to their enclosing method. All three reads at IL offsets `0x07E9`, `0x0863`, and `0x088F` belong to `AzureArchive.Automation.AuthoringCliHost.Bootstrap()` (method RVA `0x5D32C8`), not to `UserSettings.InitializeSettings` or the ordinary game startup coroutine.

This materially narrows the earlier result:

- The recovered Android IL shows `customSource` being consumed by the authoring CLI bootstrap path, alongside the CLI's authoring/offline setup and the literal fallback `https://filev2.foxxlight.top/aares/`.
- It does not show ordinary gameplay startup reading `customSource` and assigning it to `ScenarioResourceManager.AddrLoadUrl`.
- The managed IL still contains no `ldsfld` or `stsfld` instruction for `ScenarioResourceManager.AddrLoadUrl`; only its declaration and the two authoring offline path-prefix string uses are visible.

**Updated conclusion:** `customSource` is now positively classified as an authoring CLI input in the recovered managed path. It must not be used as the iOS gameplay URL provider by assumption. The production mobile resource URL source and assignment remain unresolved; the next useful evidence is the Android native field/static-storage cross-reference or an observed runtime configuration response. No production URL adapter was changed.


## Cross-platform decisive comparison — Windows authoring CLI assignment — 2026-11-01

The Windows 1.0.1 full IL export was checked against Android 1.0. The Windows build contains an explicit assignment that is absent from the Android managed IL export:

- Enclosing method: `AzureArchive.Automation.AuthoringCliHost.Bootstrap()`, method RVA `0x5D32C8` in the Windows build.
- It reads `UserSettings.data.customSource`. If blank, it uses the literal `https://filev2.foxxlight.top/aares/`; otherwise it trims trailing slash characters and normalizes one trailing slash.
- It appends the platform suffix `win`, then executes `stsfld string ScenarioResourceManager::AddrLoadUrl` at IL offset `0x068F`.
- This authoring CLI bootstrap is gated by the `AZUREARCHIVE_CLI_DIRECTORY` environment variable and sets `AuthoringCliHost.IsActive`; its surrounding method then prepares local `cli-resources` files and invokes authoring services.

This is now direct evidence for the Windows authoring-tool URL rule, not a guess. It is **not** evidence that Android/iOS ordinary gameplay uses the same default, the `customSource` setting, or the `/win` suffix. In the Android full IL export, the corresponding field assignment is absent and `customSource` reads map to the authoring CLI bootstrap only; the Android offline helper is separately gated by `--aap-offline-resources`.

### Porting consequence

Do not port the Windows `/win` suffix or its default URL into the iOS player. The remaining target is to identify the Android/iOS gameplay-side resource root and catalog selection path independently. The Windows CLI method is useful as a reference for the authoring tool only. No runtime URL code was changed.


## iOS-vs-Android field and settings reference audit — 2026-11-01

A targeted scan of the Android full IL and both Android/iOS IL2CPP metadata dumps adds the following constraints:

1. Android full IL references to `catalogUrlOverride` resolve to its generated getter/setter, the settings-data constructor/serialization region, and the UI patch-source completion callback. No recovered ordinary-startup method reads the property to select an Addressables catalog. The generated getter/setter existing is not evidence that the value is consumed.
2. Android `customSource` references resolve to its generated getter/setter, settings-data serialization, and `AuthoringCliHost.Bootstrap()`. The authoring CLI bootstrap is the previously identified consumer; no ordinary startup consumer was found in the managed IL.
3. Android and iOS metadata dumps both contain `ScenarioResourceManager.AddrLoadUrl` and the `SettingData.catalogUrlOverride` / `customSource` properties. However, the iOS metadata dump contains no `AuthoringCliHost` type, consistent with that authoring CLI path being absent from the iOS player.
4. These are bounded negative findings from the recovered managed IL and metadata symbol inventory. They do not exclude native/reflection/server-driven writes, stripped methods, or runtime-generated catalog configuration.

**Porting consequence:** neither setting is currently a verified gameplay catalog source. The iOS player does have the static `AddrLoadUrl` field, but the assignment site is still not identified. Do not bind the new iOS Addressables transform to either setting until an actual runtime assignment/consumer is established. Next investigate native references to the static field and catalog-load callsites, plus the iOS startup binary's resource configuration.


## iOS player metadata layout confirmation — 2026-11-01

The exact iOS IL2CPP dump confirms `ScenarioResourceManager.AddrLoadUrl` is present as a public static string field in the iOS player metadata. The iOS `ScenarioResourceManager` instance layout differs from Android (for example, Android has the background-skeleton/preview cache fields before the query fields; iOS does not), so Android instance offsets must not be reused when interpreting iOS native code. The static field is reported at metadata offset `0x0`, which is not its runtime static-storage address.

The iOS dump contains no `AuthoringCliHost` class, while the Android dump does. Therefore the Windows/Android authoring CLI implementation is not available as an iOS player class. This confirms that the iOS player needs its own assignment mechanism or server/config-driven initialization. Static field presence alone does not prove the field is assigned before Addressables requests.

No gameplay URL was guessed or inserted. No code was changed in this checkpoint; it records verified metadata facts for the next native cross-reference pass.


## Addressables catalog entry-point audit — 2026-11-01

A full-file scan of the recovered Android `Assembly-CSharp.full.il` was used to distinguish game-code catalog initialization from ordinary Addressables asset usage:

- No managed calls were found to `Addressables.LoadContentCatalogAsync`, `Addressables.UpdateCatalogs`, `Addressables.InitializeAsync`, `Addressables.LoadResourceLocationsAsync`, or `Addressables.InternalIdTransformFunc` in this game assembly.
- Five calls to `Addressables.DownloadDependenciesAsync` were found. Their enclosing generated methods are background preload coroutines for Spine, BGM, popup images, sounds, and voice. The arguments are the respective resource keys/paths; these calls do not establish the catalog URL.
- The game assembly declares `ScenarioResourceManager.AddrLoadUrl`, but its recovered managed IL contains no field read/write instruction for that field. The only two string references to its name are offline resource path-prefix templates in `AuthoringOfflineResources.ValidateDirectory`.
- This does not rule out Addressables package auto-initialization, serialized Addressables settings/catalog data, native/reflection writes, or configuration supplied by the player build. The game-assembly IL alone cannot reveal those.

**Updated technical conclusion:** the resource URL is not being selected by an explicit game-assembly `LoadContentCatalogAsync` call. The next evidence target is the player-side Addressables settings/catalog data and native startup initialization, not the background `DownloadDependenciesAsync` callsites. In particular, inspect the iOS IPA's `Data/StreamingAssets/aa` content (if present), `AddressablesLink/link.xml`, `settings.json`, catalog JSON/binary, and the iOS executable's IL2CPP native field references. The currently available iOS artifact in this repository is an older IPA release and must be treated as a comparison baseline, not as proof of the Android 1.0 runtime configuration.

No production code or guessed URL was added in this audit.
