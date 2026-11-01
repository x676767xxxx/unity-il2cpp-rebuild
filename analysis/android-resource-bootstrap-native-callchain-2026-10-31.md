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
