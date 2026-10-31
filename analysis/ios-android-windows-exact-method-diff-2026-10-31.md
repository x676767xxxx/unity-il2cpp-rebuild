# AzureArchive exact IL2CPP method declaration comparison (2026-10-31)

Source: artifact `azurearchive-cross-platform-il2cpp-comparison`, GitHub Actions run 41172721121. The artifact contains actual Il2CppDumper `dump.cs` outputs for iOS, Android and Windows, generated using each platform's own native binary and matching metadata. The class parser was independently rerun against the three dump.cs files and bounded by each exact top-level TypeDefIndex class body. This report records declaration presence/signatures only; it does not establish runtime behavior or provide buildable source.

## Extraction status

- iOS: Metadata v29, Il2Cpp v29, Il2CppDumper completed; `dump.cs` 40,835,508 bytes.
- Android ARM64: Metadata v29, Il2Cpp v29, completed with warning `This file may be protected`; nevertheless generated `dump.cs` 47,862,364 bytes. Treat Android method results as high-confidence declarations but verify native code/call paths where protection could affect registration/offsets.
- Windows: Metadata v29, Il2Cpp v29, completed; `dump.cs` 42,438,955 bytes.
- All three outputs contain actual dump.cs and DummyDll/Assembly-CSharp.dll; this is no longer just a string scan.

## Exact declarations by class

### ScenarioResourceManager

| Method | iOS 0.7.9.2 | Android 1.0 | Windows 1.0.1 |
|---|---|---|---|
| ResolveBackground(uint id) | — | Yes | Yes |
| EnumerateBackgrounds() | — | Yes | Yes |
| TryGetBackgroundOverridePath(uint id, out string ...) | Yes | Yes | Yes |
| AcquireBackgroundSpine(BackgroundResource) | — | Yes | Yes |
| GetBackgroundTexture(uint id) | Yes | Yes | Yes |
| GetBackgroundTexture(BackgroundResource) | — | Yes | Yes |
| GetBackgroundTextureAsync(uint id, bool, Action<Texture>) | Yes | Yes | Yes |
| GetBackgroundPreviewAsync(BackgroundResource, bool, Action<Texture>) | — | Yes | Yes |
| InitializeAuthoringResources(string, string, string) | — | Yes | Yes |
| GetCustomBgEffect(uint id) | — | Yes | Yes |
| GetBgEffectChoices() | — | Yes | Yes |

Interpretation: iOS 0.7.9.2 has three of the eleven listed declarations; Android 1.0 and Windows 1.0.1 match for all eleven. Eight declarations are present in Android/Windows but not in the old iOS class. This is the clearest first porting target, but each missing method still needs dependency and call-site analysis before implementation.

### AuthoringResourceCatalog

- iOS 0.7.9.2: type not found in the dumped Assembly-CSharp metadata.
- Android 1.0 and Windows 1.0.1: same five declarations:
  - `Search(string kind, string query, string[] ids, string cursor, int limit, string expectedCatalogVersion)`
  - `Resolve(string resourceId, string expectedKind, string expectedCatalogVersion, bool includeCapabilities)`
  - `StageReferencedResources(ProjectData, string stagingResourcesDir, string expectedCatalogVersion)`
  - internal overload `StageReferencedResources(ProjectData, string, Snapshot)`
  - `Import(ProjectData, JObject request, bool inboxOnly, string expectedCatalogVersion)`

Interpretation: this is a real metadata-level platform gap, not just an earlier nearby-string false positive. It may be an optional authoring/import subsystem rather than required gameplay. Do not port it before confirming user-facing gameplay entry points.

### Character

- iOS 0.7.9.2: `ApplyDisplayRank(int rank)` not found in Character class.
- Android 1.0 and Windows 1.0.1: `public void ApplyDisplayRank(int rank)` present.

### CharacterManager

- All platforms: `GetSkeletonDataAssetAtPath(string fullPath, bool useCache = true)` present.
- Android/Windows include an additional `bool quiet = true` parameter; old iOS does not. This is a signature difference requiring call-site review, not proof the underlying behavior is missing.

## Prioritized implementation plan

1. **Background runtime first.** Trace the eight missing methods and their data types, native call sites, UI/gameplay callers, resource file formats, cache ownership and async callback behavior. Reimplement against iOS-compatible Unity APIs; never copy Android ARM64 RVAs into iOS.
2. **Character display rank second.** Trace where `ApplyDisplayRank` is called and what visual/state change it makes. Small surface area, likely a tractable patch if its dependencies already exist in iOS.
3. **AuthoringResourceCatalog deferred.** Find actual UI/gameplay entry points. If limited to editor/authoring/import features, keep out of the first playable iOS 1.0 build.
4. **CharacterManager quiet parameter.** Inspect Android/Windows call sites and implementation behavior; old iOS may only need a compatibility overload or call-site adaptation.

## Buildability boundary

The generated DummyDll and dump.cs are metadata/declaration reconstructions. They do not contain original C# method bodies that can simply be recompiled into an iOS IPA. Decompiled C# and IL2CPP metadata do not recreate the Unity project, scenes, serialized assets, build settings or iOS native implementations. No iOS 1.0 IPA has been built.
