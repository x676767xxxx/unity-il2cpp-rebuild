# REA Android 1.0 Spine and resource method index map — 2026-11-01

## Source and confidence

- REA run: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41307085864
- Evidence artifact: azurearchive-rea-il2cpp-evidence, ID 13083434524.
- Metadata SHA-256: 717236ea9c650b4fcd373e0afc7c03c25d5949726bebe251d3e8301e4da7a144; metadata v29; complete inventory retained.
- Evidence contains 77 assembly images, 16,912 types, 152,789 methods, 77,092 fields, 28,462 properties, 284 events, 182,036 parameters and 22,462 string literals.
- All type and method indices below are observed positions in REA's decoded global-metadata.dat. They are metadata indices, not native RVAs.
- Native RVAs noted below are independently sourced from existing Android ARM64 disassembly/call-graph reports. They have not yet been linked by REA.

## Primary gameplay resource types

All selected types belong to Assembly-CSharp.dll (image index 0; type range 0–2048).

| Type index | Type | Method index range | Key methods |
|---:|---|---:|---|
| 315 | BackgroundResource | 2643–2658 | FullPath, CacheKey, ResolveOverride, Overrides, Validate, ResolvePath |
| 1217 | BgSpineOverrideData | 25192–25213 | SpinePath, Animation, Loop, Skin, PreviewPath, PreviewTime, Scale, OffsetX, OffsetY, Id |
| 1221 | ResourceOverrideData | 25164–25183 | BgOverrides, BgSpineOverrides, other override collections, IsEmpty, AdaptPathSeparator |
| 1227 | PageTextureLoader | 25299–25301 | Load, Unload |
| 1231 | RuntimeSpineLoader | 25291–25297 | DestroyOwned, AtlasPages, Read, ReadAsync, Load, Create, CanLoadPngAsDxt5 |
| 1233 | RuntimeSpineAsset | 25309–25317 | Asset, HasConsumers, EstimatedTextureBytes, Own, Retain, Materials, Dispose |
| 1288 | ScenarioResourceManager | 25332–25446 | Background resolution, texture and preview, Spine acquisition, cache cleanup, resource updates and override loading |
| 1306 | SpineBackgroundVisual | 25717–25729 | 13 methods; playback validation/visual integration target |
| 1242 | AddressableFlatBufferLoader | 25448–25451 | Clear, Load, LoadAll |
| 1869 | MetadataTextureLoader | 31002–31004 | Load, Unload |
| 1870 | GlobalCatalogSnapshot | 31005 | constructor |
| 1873 | PreparedCatalog | 31009–31013 | CatalogVersion, Timings, Dispose |
| 1908 | AzureArchive.Automation.AuthoringResourceCatalog | 30925–31001 | authoring catalog, native manifest, Spine background staging and resource import |
| 1996 | AzureArchive.Automation.AuthoringOfflineResources | 31577–31581 | IsActive, TryGetStartupUrl, ValidateDirectory, FileDigest |

## ScenarioResourceManager method indices

| Method index | Name | Existing Android ARM64 RVA, if independently verified |
|---:|---|---:|
| 25332 | ResolveBackground | 0x3260EE0 |
| 25333 | EnumerateBackgrounds | not mapped here |
| 25334 | TryGetBackgroundOverridePath | 0x3261334 |
| 25335 | GetBackgroundType | not mapped here |
| 25338 | CoTryPreloadBackgroundTexture | not mapped here |
| 25339 | AcquireBackgroundSpine | 0x3261460 |
| 25340–25341 | GetBackgroundTexture overloads | resource overload 0x326172C; overload-to-index association pending |
| 25342 | DecodeBackgroundImage | 0x3261A5C |
| 25343 | GetBackgroundTextureAsync | not mapped here |
| 25344 | GetBackgroundPreviewAsync | 0x3261EA0 |
| 25345 | BackgroundPreviewKey | 0x3262048 |
| 25346 | QueueBackgroundPreview | 0x32623D0 |
| 25347 | BackgroundPreviewCachePath | not mapped here |
| 25348 | SaveBackgroundPreview | not mapped here |
| 25349 | CoLoadBackgroundPreview | coroutine state machine; MoveNext not mapped here |
| 25350 | ReleaseBackgroundPreview | not mapped here |
| 25351 | TrimBackgroundPreviews | not mapped here |
| 25354 | ClearBackgroundResources | not mapped here |
| 25355 | TrimBackgroundSkeletonCache | not mapped here |
| 25369 | CheckForUpdate | not mapped here |
| 25371 | InitializeAfterSplash | not mapped here |
| 25372 | Initialize | not mapped here |
| 25373 | InitializeAuthoringResources | 0x3264CA8 |
| 25374 | OnAddressablesExceptionRaised | not mapped here |
| 25375 | OnUpdateComplete | not mapped here |
| 25376 | UpdateAddressable | not mapped here |
| 25410 | LoadGlobalResourceOverrides | not mapped here |
| 25411 | LoadLocalResourceOverrides | not mapped here |
| 25413 | GetTextureFromPersistentDataAsync | not mapped here |
| 25416 | CorrectPath | called from texture retrieval path; own RVA not mapped here |
| 25421 | ReleaseAsset | not mapped here |

The method index sequence makes the important resource path easy to query without rescanning all 152,789 methods. The native address association remains a separate step.

## Spine loader method indices

| Type index | Type | Method index | Method |
|---:|---|---:|---|
| 1231 | RuntimeSpineLoader | 25291 | DestroyOwned |
| 1231 | RuntimeSpineLoader | 25292 | AtlasPages |
| 1231 | RuntimeSpineLoader | 25293 | Read |
| 1231 | RuntimeSpineLoader | 25294 | ReadAsync |
| 1231 | RuntimeSpineLoader | 25295 | Load |
| 1231 | RuntimeSpineLoader | 25296 | Create |
| 1231 | RuntimeSpineLoader | 25297 | CanLoadPngAsDxt5 |
| 1227 | PageTextureLoader | 25300 | Load |
| 1227 | PageTextureLoader | 25301 | Unload |
| 1233 | RuntimeSpineAsset | 25313 | Own |
| 1233 | RuntimeSpineAsset | 25314 | Retain |
| 1233 | RuntimeSpineAsset | 25316 | Dispose |

These names confirm that Android 1.0 has a dedicated Spine loader, atlas-page loader, and owned/ref-counted runtime asset abstraction. This is stronger than inferring Spine behavior from the existence of .skel files alone. It does not establish exact body semantics, native addresses or compatibility with the old iOS runtime.

## Authoring-only candidates to keep separate

AuthoringResourceCatalog is a distinct type in the same Android metadata image. Its method list includes ReadCustomSkeleton, CaptureForCompilation, StageReferencedResources, StageSpineBackground, Import, PrepareVerification, UsePreparedCatalog and manifest operations. The existing cross-platform comparison shows this subsystem absent from the old iOS 0.7.9.2 Assembly-CSharp. Do not assume all of these are required for normal gameplay; trace callers before prioritizing.

## Next reverse-engineering steps

1. Run the separate REA native-link workflow without generic-instance expansion. It attempts to add method addresses and type information while staying below the fixed report budget.
2. If the linked Evidence fits the budget, generate Ghidra seeds and inspect native functions for RuntimeSpineLoader.Load/Create, PageTextureLoader.Load/Unload, and SpineBackgroundVisual validation.
3. Cross-check REA method indices and any linked addresses against existing Cpp2IL/ARM64 reports before changing port code.
4. Keep the first iOS implementation focused on gameplay resource loading: BackgroundResource, RuntimeSpineLoader, PageTextureLoader, RuntimeSpineAsset and ScenarioResourceManager. Keep authoring catalog tools separate until their runtime call sites are proven.
5. Do not copy Android ARM64 machine code or RVAs into iOS. Reimplement observed behavior against the iOS Unity runtime and matching Spine runtime, subject to license terms.

## Limits

This artifact is a complete metadata inventory, not a recovered source project. Metadata-only REA has no native addresses, no field/parameter/return type resolution against the binary, and no runtime behavior. No complete Unity project, IPA or device test is produced by this step.