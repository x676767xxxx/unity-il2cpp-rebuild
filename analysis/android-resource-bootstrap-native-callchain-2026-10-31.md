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
