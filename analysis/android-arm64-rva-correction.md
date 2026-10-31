# Corrected Android 1.0 ARM64 function map

Checked directly against the method declarations in `dump-android-arm64/dump.cs` from Actions run 41169040171. This supersedes any RVA assignments inferred by searching only the preceding 300 characters.

| Exact method | Verified Android ARM64 RVA |
|---|---:|
| ScenarioResourceManager.InitializeAuthoringResources(string, string, string) | 0x3264CA8 |
| ScenarioResourceManager.ResolveBackground(uint) | 0x3260EE0 |
| ScenarioResourceManager.GetBackgroundTexture(BackgroundResource) | 0x326172C |
| ScenarioResourceManager.GetBackgroundPreviewAsync(BackgroundResource, bool, Action<Texture>) | 0x3261EA0 |
| AuthoringResourceCatalog.Search(string, string, string[], string, int, string) | 0x336A1F0 |
| AuthoringResourceCatalog.Resolve(string, string, string, bool) | 0x336B5DC |
| AuthoringResourceCatalog.StageReferencedResources(ProjectData, string, string) | 0x3372058 |
| AuthoringResourceCatalog.Import(ProjectData, JObject, bool, string) | 0x3375FE8 |

## Correction to previous disassembly artifact

The first disassembly workflow selected an RVA by looking backward only 300 characters from a method name. In large IL2CPP classes, the matching RVA annotation can be farther away, so several methods were assigned the previous method's address. The run itself succeeded, but those sections were mislabelled and must not be used as evidence for the named methods.

The corrected workflow uses the exact RVAs above, each verified against its method declaration in dump.cs, and uploads an artifact named `azurearchive-android-arm64-disassembly-corrected`.

These are Android ELF RVAs only. They cannot be reused as iOS addresses, and disassembly alone does not provide an iOS-compatible implementation.
