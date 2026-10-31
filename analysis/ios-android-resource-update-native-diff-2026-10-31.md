# AzureArchive iOS 0.7.9.2 vs Android 1.0 resource-update native comparison — 2026-10-31

## Inputs verified

- iOS artifact: GitHub Actions run 41194368222, artifact `azurearchive-ios-il2cpp-method-map`.
- Android artifact: GitHub Actions run 41192985832, artifact `azurearchive-android-arm64-disassembly-corrected`.
- Both artifacts contain platform-matched IL2CPP dump/disassembly outputs. iOS was decoded from the actual arm64 Mach-O `UnityFramework`; Android from the actual arm64 ELF `libil2cpp.so`.
- The iOS method-map workflow now maps offsets through the Mach-O `__TEXT` segment and Capstone decodes ARM64 instructions. The resulting iOS file contains method ranges and disassembly.
- Version comparison: iOS 0.7.9.2 versus Android 1.0. These are not same-version binaries.

## Verified native state-machine ranges

| Method | iOS 0.7.9.2 | Android 1.0 |
|---|---:|---:|
| `ScenarioResourceManager.<CheckForUpdate>d__48.MoveNext()` | `0x3D746AC–0x3D751A0` | `0x326FDF0–0x3270D08` |
| `ScenarioResourceManager.<OnUpdateComplete>d__52.MoveNext()` | `0x3D776C0–0x3D77BE4` | `0x3277420–0x32779E8` |

These are platform-specific addresses. They must not be interchanged.

## Decoded-range comparison

The bounded decoded ranges have the following byte lengths and direct/indirect call sites:

| Method | Platform | Range bytes | `BL` sites | `BLR` sites |
|---|---|---:|---:|---:|
| CheckForUpdate.MoveNext | iOS | 2,804 | 144 | 10 |
| CheckForUpdate.MoveNext | Android | 3,864 | 149 | 11 |
| OnUpdateComplete.MoveNext | iOS | 1,316 | 72 | 0 |
| OnUpdateComplete.MoveNext | Android | 1,480 | 72 | 0 |

Interpretation:
- Both versions contain native coroutine state machines for the resource update path.
- The update state machines are not byte-for-byte or layout-equivalent; the bounded native code sizes differ.
- The OnUpdateComplete ranges have equal direct-call counts, but equal call counts do not prove equal targets or behavior.
- This comparison is a native structure check, not a recovered C# source diff. Calls must be resolved to metadata methods and relevant branch/state transitions before any patch is selected.

## Important iOS integration API confirmation

The iOS 0.7.9.2 platform-matched `dump.cs` explicitly confirms:
- `ScenarioResourceManager.AddrLoadUrl` is a static string field.
- `UnityEngine.AddressableAssets.Addressables.InternalIdTransformFunc` is a public static `Func<IResourceLocation, string>` property, with getter and setter.
- `ScenarioResourceManager.UpdateAddressable()`, `LoadGlobalResourceOverrides()`, `GetResourceOverrideRoot()`, `TryGetBackgroundOverridePath(uint, out string)`, `GetBackgroundTexture(uint)`, and `GetBackgroundTextureAsync(uint, bool, Action<Texture>)` exist in the old iOS binary.

This confirms the old iOS Addressables API surface needed by the repository's token-transform adapter exists at metadata level. It does not prove that the adapter is installed at the correct lifecycle point or that it can be injected into the shipped UnityFramework without a project/build integration path.

## Confirmed feature gap remains

The platform-matched class-level declaration diff still stands:
- Old iOS 0.7.9.2 has 3 of the 11 selected `ScenarioResourceManager` declarations.
- Android 1.0 and Windows 1.0.1 have all 11.
- Eight Android/Windows declarations are absent from old iOS: `ResolveBackground`, `EnumerateBackgrounds`, `AcquireBackgroundSpine`, `GetBackgroundTexture(BackgroundResource)`, `GetBackgroundPreviewAsync`, `InitializeAuthoringResources`, `GetCustomBgEffect`, and `GetBgEffectChoices`.

The old iOS binary therefore provides a useful runtime baseline for existing update/catalog behavior, but it does not already contain the full Android 1.0 background API.

## Next implementation gate

1. Resolve the direct-call targets in both state machines against each platform's own `dump.cs`; compare the calls around Addressables initialization, catalog update, dependency-size checks, downloads, and completion callbacks.
2. Confirm which Android 1.0 background APIs are used by normal gameplay and which are editor/authoring-only.
3. Connect the verified `AddrLoadUrl` token transform at a proven early iOS lifecycle point; preserve and chain any existing transform.
4. Implement the first static-background path against the old iOS runtime's existing Addressables APIs; keep Spine-specific loading deferred until its runtime/API compatibility is verified.
5. Establish a reproducible Unity/Xcode integration base before attempting a full IPA build.

## Delivery boundary

This report is a verified analysis milestone. It does not reconstruct missing C# method bodies, add the Android 1.0 gameplay features to the shipped binary, or produce an installable IPA. The repository still lacks a complete Unity project/source tree or a proven full-game Xcode integration target.
