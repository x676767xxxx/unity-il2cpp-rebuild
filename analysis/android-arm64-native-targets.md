# Android 1.0 ARM64 native inspection targets

Based on the verified Android 1.0 IL2CppDumper metadata in Actions artifact from run 41169040171. These are Android ELF RVAs only; they are not valid iOS addresses.

| Method | Android ARM64 RVA | Why inspect |
|---|---:|---|
| ScenarioResourceManager.ResolveBackground(uint) | 0x3260EE0 | Core background lookup path |
| ScenarioResourceManager.GetBackgroundTexture(BackgroundResource) | 0x326172C | Background texture retrieval |
| ScenarioResourceManager.GetBackgroundPreviewAsync(BackgroundResource, bool, Action<Texture>) | 0x3261EA0 | Preview loading/cache path |
| ScenarioResourceManager.InitializeAuthoringResources(...) | 0x3264CA8 | Initializes local databases, manifest and overrides |
| AuthoringResourceCatalog.Search(...) | 0x336A1F0 | Resource catalog lookup |
| AuthoringResourceCatalog.Resolve(...) | 0x336B5DC | Resource ID resolution |
| AuthoringResourceCatalog.StageReferencedResources(...) | 0x3372058 | Stages resources referenced by a project |
| AuthoringResourceCatalog.Import(...) | 0x3375FE8 | Imports a local resource |

## Important interpretation

- The ScenarioResourceManager background methods are plausible mobile runtime targets because background lookup/texture retrieval is directly called by scene/gameplay paths in the recovered code.
- The AuthoringResourceCatalog methods form a project/resource authoring API. The presence of Import, Search, Resolve and StageReferencedResources alone does not prove they are required for ordinary gameplay.
- The next task disassembles the selected ARM64 functions from Android 1.0 `libil2cpp.so`. It can reveal native call patterns and dependencies, but does not turn Android code into iOS-compatible code.
- iOS 0.7.9.2 remains the only supplied iOS native binary. A 1.0 iOS build still needs an iOS-compatible implementation and a valid Unity iOS build/signing path.
