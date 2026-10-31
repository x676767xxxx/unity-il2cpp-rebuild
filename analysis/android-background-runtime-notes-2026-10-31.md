# AzureArchive Android 1.0 background runtime — preliminary reverse-engineering notes

Evidence used:
- Android 1.0 ARM64 selected disassembly artifact from run 41170866048.
- Android 1.0 IL2CPP dump.cs from cross-platform extraction run 41172721121.
- Earlier direct-BL caller scan from run 41171027793.
- New expanded call-graph workflow: run 41173095659 (in progress at time of writing).

These notes describe observed native call edges and metadata signatures. They are not reconstructed source code.

## 1. ResolveBackground(uint id)

Verified declaration RVA: 0x3260EE0.

Observed direct call sequence includes:
- `BackgroundResource.ResolveOverride(uint id, ResourceOverrideData local, ResourceOverrideData global)`
- `ScenarioResourceManager.GetExcel()`
- `ScenarioBGNameExcel.get_BGFileName()`
- `ScenarioBGNameExcel.get_BGType()`
- `Path.GetFileNameWithoutExtension(...)`
- `BackgroundResource..ctor(uint id, string name, ScenarioBGType type, ResourceType source, string path, string root, BgSpineOverrideData spine)`

Interpretation: background resolution is not merely an ID-to-texture lookup. It first consults local/global override data, then uses scenario background metadata (file name and type), then constructs a resource descriptor carrying source/path/root and Spine override information. A compatible iOS port needs the same precedence and metadata semantics.

Earlier direct BL scan found 8 call sites to this method. That is a lower bound; virtual/delegate/function-pointer edges are not included.

## 2. GetBackgroundTexture(BackgroundResource resource)

Verified declaration RVA: 0x326172C.

Observed calls include:
- `BackgroundResource.get_FullPath()`
- `File.ReadAllBytes(path)`
- `DecodeBackgroundImage(byte[] bytes, string name)`
- `ScenarioResourceManager.CorrectPath(string path)`
- UnityEngine.Object equality checks
- exception construction for error branches

Interpretation: the resource object provides a full path; the native routine reads bytes and decodes a Texture2D. It also checks Unity object/cache state and normalizes paths. The iOS port must preserve path normalization, decode behavior, texture lifetime and error handling. The method does not appear to simply return a preloaded asset.

Earlier direct BL scan found 1 call site to the id-based GetBackgroundTexture overload; this count should not be generalized to every overload.

## 3. GetBackgroundPreviewAsync(BackgroundResource resource, bool preferThumbnail, Action<Texture> callback)

Verified declaration RVA: 0x3261EA0.

Observed calls include:
- `BackgroundPreviewKey(resource, preferThumbnail)`
- UnityEngine.Object inequality check
- `RenderTexture.IsCreated()`
- `Time.get_frameCount()`
- `QueueBackgroundPreview(resource, preferThumbnail, key, callback)`

Interpretation: preview retrieval uses a cache key that depends on the resource and thumbnail preference, checks cached Unity texture/render-target validity and frame age, and queues asynchronous work when the cached preview cannot be reused. A port must reproduce cache invalidation, callback completion and Unity main-thread/lifetime constraints.

Earlier direct BL scan found 2 call sites to this method.

## 4. InitializeAuthoringResources(databasePath, fileNamesPath, bgmInfoPath)

Verified declaration RVA: 0x3264CA8.

Observed calls include:
- `AuthoringResourceCatalog.InvalidateGlobalCatalog()`
- SQLite connection constructor with `SQLiteOpenFlags`
- `File.ReadAllText(fileNamesPath)`
- `SetFileManifest(FileNameInfo)`
- `File.ReadAllBytes(bgmInfoPath)`
- FlatBuffers `BGMAdditionalInfoExcelTable.GetRootAsBGMAdditionalInfoExcelTable(ByteBuffer)`
- `GetResourceOverrideRoot()`
- generated `ReadOverrides(root)`
- initialization of resource/override state

Interpretation: this initialization routine joins three distinct input streams: a SQLite database, a JSON/text file manifest, and a FlatBuffers BGM info file, then loads override data and invalidates the authoring catalog cache. The iOS implementation must match database open mode/schema, manifest schema, FlatBuffers schema/version and override precedence.

Earlier direct BL scan found 1 call site to this method.

## 5. What remains unverified

- Full caller hierarchy and enclosing UI/gameplay methods for each call site.
- Indirect calls through virtual dispatch, delegates, reflection, function pointers or IL2CPP runtime registration.
- Exact resource schema and precedence for global/local overrides.
- Full implementations of `QueueBackgroundPreview`, `BackgroundPreviewKey`, `DecodeBackgroundImage`, `AcquireBackgroundSpine`, `GetCustomBgEffect` and `GetBgEffectChoices`.
- Whether all authoring/catalog functionality is required for ordinary mobile gameplay.

## 6. Recommended port order

1. Implement resource descriptor + override resolution semantics.
2. Implement background path resolution, byte loading and image decode.
3. Implement preview cache and async callback lifecycle.
4. Implement SQLite/manifest/FlatBuffers initialization only if ordinary mobile screens require it.
5. Defer AuthoringResourceCatalog import/staging until its actual UI/gameplay callers are identified.

Do not transplant Android ARM64 RVAs or native instructions into iOS. These are Android-only addresses and platform-specific code.


## 7. Native preview coroutine/cache findings (run 41200411309)

The exact compiler-generated iterator has Android ARM64 RVA `0x3270D7C`. Its metadata fields include an Addressables `AsyncOperationHandle<Texture>`, a `Task<byte[]>`, a `Task<RuntimeSpineLoader.Files>`, a texture, an addressable flag and a full path. Native call resolution confirms calls to `File.ReadAllBytesAsync`, `DecodeBackgroundImage`, `RuntimeSpineLoader.ReadAsync/Create`, `SpineBackgroundVisual.RenderPreview(..., 1024)`, `BackgroundPreviewCachePath`, `SaveBackgroundPreview`, `Graphics.Blit`, `LimitBackgroundPreviewSize(..., 1024)`, and cleanup/error helpers.

The `BackgroundPreview` record has Texture, Addressable, Users, LastUse, Resource and PreferThumbnail fields. The native trim predicate is `Users == 0`; its sort selector returns `LastUse`. The trim body contains a 32 MiB threshold and a 25-entry count check. Destroy branches on Addressable to choose release vs Unity destruction. These findings establish a reference-counted, last-use-aware preview cache; the exact trim condition should be preserved from the native disassembly when implementing it.

This is Android-only behavioral evidence. The generated ILSpy `MoveNext` pseudo-IL remains incomplete; native ARM64 instructions were used to resolve the call targets. No Android RVA or machine code is suitable for reuse in iOS.
