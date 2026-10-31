# Reviewed Android 1.0 background call graph findings

Source artifact: `azurearchive-android-background-callgraph`, GitHub Actions run 41173095659. The workflow completed successfully and uploaded the artifact (102,071,499 bytes; includes a 649 MB full ARM64 disassembly text file and a 27 KB call graph report).

Important QA correction: the generated report's target-finding script has two symbol-selection errors:
- `EnumerateBackgrounds` was matched on its `IteratorStateMachine` attribute and incorrectly assigned ResolveBackground's RVA 0x3260EE0. Its actual iterator body is a compiler-generated state-machine type; do not use that reported caller list.
- `InitializeAuthoringResources` was matched on a generated lambda `<InitializeAuthoringResources>b__1` at 0x326FD5C. The actual method declaration is at RVA 0x3264CA8, visible as the caller of GetResourceOverrideRoot (0x3265028) and ReadOverrides (0x3265084). Do not use the false 0-caller result for the lambda as the caller count for the actual initializer.

The other target declarations and direct BL edges below were cross-checked against the report and the dump's method RVAs. Direct BL edges are a lower bound; BLR, virtual dispatch, delegates and runtime registration are not covered.

## Verified direct-call relationships

| Method | Android ARM64 RVA | Direct BL callers | Observed caller / use |
|---|---:|---:|---|
| ResolveBackground(uint) | 0x3260EE0 | 8 | TryGetBackgroundOverridePath; GetBackgroundType; GetBackgroundTexture(uint); GetBackgroundTextureAsync; state-machine MoveNext; Clear; LoadBGAsync; ScenarioUtil.ParseScript |
| TryGetBackgroundOverridePath(uint, out string) | 0x3261334 | 0 | Calls ResolveBackground; used by reverse path lookup |
| AcquireBackgroundSpine(BackgroundResource) | 0x3261460 | 1 | BackgroundResource.Prepare |
| GetBackgroundTexture(BackgroundResource) | 0x326172C | 1 | BackgroundResource.Prepare |
| GetBackgroundPreviewAsync(...) | 0x3261EA0 | 2 | GetBackgroundTextureAsync; LoadBGAsync |
| GetCustomBgEffect(uint) | 0x3268CC8 | 1 | SetEffect |
| GetBgEffectChoices() | 0x3268CDC | 2 | RefreshEffects; ImportSelectedBundle |
| BackgroundPreviewKey(...) | 0x3262048 | 2 | GetBackgroundPreviewAsync; preview coroutine MoveNext |
| QueueBackgroundPreview(...) | 0x32623D0 | 2 | GetBackgroundPreviewAsync; OnApplicationFocus |
| DecodeBackgroundImage(byte[], string) | 0x3261A5C | 4 | GetBackgroundTexture(BackgroundResource); preview coroutine state machines |

## Implementation details established from ARM64 call instructions

### ResolveBackground
Observed calls include:
- `BackgroundResource.ResolveOverride(id, local, global)`
- `ScenarioResourceManager.GetExcel()`
- scenario background name/type getters
- `Path.GetFileNameWithoutExtension`
- BackgroundResource constructor carrying id, name, type, source, path, root and Spine override settings.

This supports the earlier conclusion that resolution combines override precedence with scenario Excel metadata before constructing a resource descriptor.

### GetBackgroundTexture
Observed sequence includes:
- `BackgroundResource.get_FullPath()`
- `File.ReadAllBytes(path)`
- `DecodeBackgroundImage(bytes, name)`
- `CorrectPath(path)`
- Unity Object equality checks and error paths.

### DecodeBackgroundImage
Observed Unity API calls:
- create Texture2D with width/height and texture format arguments
- set texture name
- set wrap mode
- `ImageConversion.LoadImage(texture, bytes, markNonReadable)`
- cleanup via `DestroyOwned` on error/lifetime paths.

This gives a concrete iOS implementation target using Unity's own Texture2D/ImageConversion APIs, but parameter values, format choices and complete error semantics still need method-level analysis.

### GetBackgroundPreviewAsync
Observed calls:
- BackgroundPreviewKey(resource, preferThumbnail)
- Unity Object validity checks
- RenderTexture.IsCreated()
- Time.frameCount
- QueueBackgroundPreview(...)

QueueBackgroundPreview starts a coroutine through `StartCoroutine`; it is also called from OnApplicationFocus. This indicates preview work is refreshed after app focus changes.

### AcquireBackgroundSpine
Observed calls:
- resource.IsCustomSpine and FullPath
- RuntimeSpineAsset.Load(basePath, material, blend materials, quiet)
- SkeletonData retrieval
- ValidatePlayback(override settings, skeleton data)
- Retain and TrimBackgroundSkeletonCache
- frame-count based cache/lifetime handling.

This is a distinct dependency path: a background implementation that only handles static textures will not cover Spine-based backgrounds.

### Background effects
- GetCustomBgEffect is called by SetEffect.
- GetBgEffectChoices is called by RefreshEffects and ImportSelectedBundle.
- The observed method reads BackgroundEffectResource overrides and scenario BG effect Excel fields, including effect path/name information.
These callers suggest effect-choice APIs may be tied to authoring/import UI flows; verify whether SetEffect is used in ordinary scenario playback before prioritizing.

## Porting order recommendation

1. BackgroundResource resolution and local/global override precedence.
2. Texture path normalization, byte loading and Texture2D decoding.
3. Preview coroutine/cache lifecycle and focus refresh.
4. Spine asset loading, skeleton validation, retain/release and cache trimming.
5. Background effect selection only after tracing SetEffect into actual gameplay paths.
6. Authoring-resource initialization and catalog only if the mobile runtime depends on them.

## Limits

- This is Android ARM64 direct-call analysis, not iOS implementation code.
- Android RVAs must not be applied to iOS.
- The direct BL graph is incomplete for indirect/virtual/delegate calls.
- The call graph is not a recovered source project; no IPA is produced by this analysis.
