# iOS 0.7.9.2 IL2CPP method map — 2026-10-31

## Source and verification

- Source IPA: repository release `XR1/AzureArchive_IOS_0.7.9.2.ipa`.
- Unity binary extracted from `Payload/*/UnityFramework.framework/UnityFramework`.
- Matching `global-metadata.dat` extracted from the same IPA.
- Il2CppDumper: v6.7.46, executed against the actual iOS Mach-O and its matching metadata.
- Successful workflow: Run 41193566903, artifact `azurearchive-ios-il2cpp-method-map` (30,326,393 bytes).
- The artifact contains a 40,835,508-byte iOS `dump.cs`, method search output, and selected method map.

## Verified iOS-specific method RVAs

The iOS `dump.cs` directly identifies these addresses. These are not Android RVAs:

| Method | iOS RVA |
|---|---:|
| `ScenarioResourceManager.CheckForUpdate()` | `0x3D6931C` |
| `ScenarioResourceManager.Awake()` | `0x3D693C8` |
| `ScenarioResourceManager.Initialize()` | `0x3D69690` |
| `ScenarioResourceManager.OnUpdateComplete()` | `0x3D699A4` |
| `ScenarioResourceManager.UpdateAddressable()` | `0x3D69A50` |
| `ScenarioResourceManager.LoadGlobalResourceOverrides()` | `0x3D6EB54` |
| `ScenarioResourceManager.GetResourceOverrideRoot()` | `0x3D6EE30` |
| `ScenarioResourceManager.<CheckForUpdate>d__48.MoveNext()` | `0x3D746AC` |
| `ScenarioResourceManager.<OnUpdateComplete>d__52.MoveNext()` | `0x3D776C0` |

The state-machine method end boundaries in the metadata are:
- CheckForUpdate MoveNext: `0x3D746AC` to the next method at `0x3D751A0`.
- OnUpdateComplete MoveNext: `0x3D776C0` to the next method at `0x3D77BE4`.

## Key finding

The old iOS 0.7.9.2 build contains the same managed resource-manager class and compiler-generated update state machines by name. This gives a sound, metadata-derived iOS target for direct native comparison with Android 1.0.

The iOS addresses must be treated independently from Android. Android's previously analyzed CheckForUpdate MoveNext begins at `0x326FDF0`; that address must never be applied to the iOS UnityFramework.

## Remaining work

1. Disassemble the iOS MoveNext methods using the actual Mach-O address mapping and confirm the instruction ranges.
2. Compare the iOS and Android state machines by resolved call targets, branch conditions, and field access.
3. Identify version-specific changes and any missing iOS 1.0 code/resource assets.
4. Implement only evidence-backed compatibility changes.
5. Build, install, and launch-test the resulting iOS IPA.

## Delivery status

This is a verified analysis milestone, not a completed port. No Android 1.0 iOS binary has been built, packaged, or tested on a device yet.
