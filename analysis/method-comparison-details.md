# AzureArchive deep method comparison (2026-10-31)

Inputs: XR1 release assets. Successful IL2CppDumper v6.7.46 run 41169040171. The original full dump files are in artifact azurearchive-ios-il2cpp-comparison.

## Verified method declaration counts

| Type | iOS 0.7.9.2 | Android ARM64 1.0 | Windows 1.0.1 |
|---|---:|---:|---:|
| ScenarioResourceManager | 85 RVA entries / 57 public | 115 / 65 | 115 / 65 |
| AuthoringResourceCatalog | absent | 77 / 20 | 75 / 20 |
| CharacterManager | 32 / 26 | 34 / 26 | 34 / 26 |
| Character | 37 / 23 | 38 / 24 | 41 / 27 |

## Android additions in ScenarioResourceManager absent from iOS

- ResolveBackground(uint id)
- EnumerateBackgrounds()
- AcquireBackgroundSpine(BackgroundResource)
- GetBackgroundTexture(BackgroundResource)
- GetBackgroundPreviewAsync(BackgroundResource, bool, Action<Texture>)
- InitializeAuthoringResources(string databasePath, string fileNamesPath, string bgmInfoPath)
- GetCustomBgEffect(uint id)
- GetBgEffectChoices()
- Background preview cache, decode, trim and lifecycle helpers
- Persistent file extension default differs: iOS .aap; Android .aap2

Some Android-only entries are private implementation helpers rather than public API. This points to a substantial resource-management update, not necessarily 30 additional user-visible features.

## AuthoringResourceCatalog

This type is absent from the iOS 0.7.9.2 dump and present in both 1.0 builds. Public methods include Search, Resolve, ReadReference, EnumCapabilities, StageReferencedResources, Import, CreateAlias, DeleteLocal, PrepareVerification, UsePreparedCatalog, and FindScenarioResourceWarnings.

The naming and API shape suggest a local authoring/custom-resource workflow. It should not be assumed to be required for basic gameplay until call sites/UI entry points are checked.

## Interpretation and limits

- Android 1.0 and Windows 1.0.1 share the 1.0 resource-manager API surface in this comparison.
- Windows-only CharacterCurve functionality is absent from Android too; it is not currently a mobile port target.
- Dumper output is metadata-derived declarations and RVAs. Method bodies in dump.cs are empty stubs. RVAs are only valid in their original platform binary.
- Android ELF and Windows PE machine code cannot be copied into an iOS Mach-O UnityFramework.
- Next technical test: Cpp2IL managed-code recovery on Android 1.0, Windows 1.0.1 and iOS 0.7.9.2. Cpp2IL's own documentation describes decompiled CIL as messy and best-effort; recovered code must be validated. If it can reconstruct useful method bodies, compare Android 1.0 implementations to the old iOS declarations and estimate what can be ported.
- A buildable IPA still requires an iOS-compatible Unity project/runtime integration, iOS implementations for missing code, full testing, and code signing.
