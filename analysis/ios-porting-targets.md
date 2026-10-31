# AzureArchive iOS porting targets (metadata/RVA map)

Source: successful IL2CPP dumps from Actions run 41169040171. RVAs are addresses in the original platform binaries, not portable offsets or source code.

## ScenarioResourceManager
- iOS methods: 85; Android ARM64: 115; Windows: 115
- Public Android methods absent from iOS:
  - Android 0x3268CC8: GetCustomBgEffect(uint id); Windows RVA 0x7BD500
  - Android 0x3260EE0: ResolveBackground(uint id); Windows RVA 0x7C2010
  - Android 0x3268CDC: GetBgEffectChoices(); Windows RVA 0x7BC770
  - Android 0x32612C0: EnumerateBackgrounds(); Windows RVA 0x7BB1E0
  - Android 0x3261460: AcquireBackgroundSpine(BackgroundResource resource); Windows RVA 0x7B9820
  - Android 0x326172C: GetBackgroundTexture(BackgroundResource resource); Windows RVA 0x7BC3A0
  - Android 0x3261334: TryGetBackgroundOverridePath(uint id, out string path); Windows RVA 0x7C34C0
  - Android 0x3264864: GetPersistentFilePath(..., extension = ".aap2"); Windows RVA 0x7BE0F0
  - Android 0x3261EA0: GetBackgroundPreviewAsync(...); Windows RVA 0x7BBF60
  - Android 0x3264CA8: InitializeAuthoringResources(...); Windows RVA 0x7BFC10

## Character
- iOS methods: 37; Android ARM64: 38; Windows: 41
- Android-only public method relative to iOS: ApplyDisplayRank(int rank), Android RVA 0x343D058.
- Windows-only public methods relative to Android: get_CurvePlayer() RVA 0x9BF1F0; get_ActionTransform() RVA 0x9BF1C0; PlayCurve(CharacterCurve curve) RVA 0x9BD430.

## CharacterManager
- iOS methods: 32; Android ARM64: 34; Windows: 34.
- Public method GetSkeletonDataAssetAtPath(...) exists in Android/Windows but not iOS.

## AuthoringResourceCatalog
- Absent from iOS 0.7.9.2 metadata; present in Android 1.0 (77 methods) and Windows 1.0.1 (75 methods).
- This is a substantial missing component, with public APIs for catalog search/resolve, project binding, resource import/delete/alias, reference staging, catalog versioning, and verification.
- Selected Android / Windows RVAs:
  - AvailableVoiceKeys: 0x3368E40 / 0x8E4F80
  - UsePreparedCatalog: 0x337D36C / 0x8FBFF0
  - FindScenarioResourceWarnings: 0x337DDA4 / 0x8EC1F0
  - CreateAlias: 0x337B25C / 0x8E7310
  - DeleteLocal: 0x337B980 / 0x8E8460
  - EnumCapabilities: 0x336D084 / 0x8EB920
  - Import: 0x3375FE8 / 0x8EF4C0
  - ReadReference: 0x336CC84 / 0x8F4750
  - Resolve: 0x336B5DC / 0x8F5A30
  - Search: 0x336A1F0 / 0x8F5FD0
  - StageReferencedResources: 0x3372058 / 0x8F7100
  - PrepareVerification: 0x337CDA4 / 0x8F3B80

## CharacterCurve / CharacterCurvePlayer
- Absent from iOS and Android dumps; present only in Windows 1.0.1.
- Treat as Windows-specific until evidence proves otherwise; do not port it into iOS merely because it exists in Windows.

## What this means
1. Android 1.0 and Windows 1.0.1 share much of their resource-management API; iOS 0.7.9.2 is behind in these areas.
2. The immediate mobile porting target is the Android 1.0 resource-management path, not the Windows-only curve editor/player.
3. The dump provides method declarations, fields, native addresses and metadata mapping. It does not recover original C# method bodies. RVAs only locate functions inside their original binaries.
4. Android ELF / Windows PE code cannot be inserted into an iOS Mach-O app. A working iOS 1.0 build still needs iOS-compatible implementations, integration with the iOS Unity runtime, testing and signing.
