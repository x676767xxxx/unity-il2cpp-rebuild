# Android 1.0 background runtime evidence from full IL — 2026-10-31

Source: Android 1.0 `Assembly-CSharp.dll` full IL exported in GitHub Actions Run 41191415958 artifact `azurearchive-raw-il-ios-port-fixed`. The IL is Cpp2IL-derived and contains unresolved native-memory loads and method placeholders; observations below are limited to recovered method declarations and explicit call references.

## Confirmed runtime flow

### Startup / cache
- `ScenarioResourceManager.Initialize()` configures a Unity cache using `UserSettings.CachePath` when the configured path is nonempty and exists; it calls Unity cache APIs (`GetCacheByPath`, `AddCache`, `set_currentCacheForWriting`, `MoveCacheBefore`).
- It starts `CheckForUpdate()` as a coroutine.
- `InitializeAfterSplash()` is a separate coroutine entry point. Its state-machine body requires additional native-level recovery to reconstruct exact order.

### Background resolution
- `ResolveBackground(uint32)` exists in Android 1.0 (RVA 0x436330).
- Its recovered IL explicitly calls `BackgroundResource.ResolveOverride(id, LocalOverrideData, GlobalOverrideData)`.
- It reads the scenario background-name database via `ScenarioBgNameDbSchema.GetExcel()` and `ScenarioBGNameExcel.get_BGFileName()`.
- `TryGetBackgroundOverridePath(uint32, out string)` calls `ResolveBackground(id)` and then reads `BackgroundResource.FullPath`.
- `GetBackgroundType(uint32)` also calls `ResolveBackground(id)`.

### Synchronous background texture
- `GetBackgroundTexture(uint32)` resolves the resource then delegates to `GetBackgroundTexture(BackgroundResource)`.
- The resource overload first checks a texture cache keyed by `BackgroundResource.FullPath`.
- The recovered method contains a local-file path using `File.ReadAllBytes(FullPath)` and `DecodeBackgroundImage(bytes, name)`, then caches the texture.
- It also contains an Addressables operation handle `WaitForCompletion()` path and releases the handle with `Addressables.Release`.
- Because parts of this method contain unresolved Cpp2IL values, the exact branch condition and complete local-vs-Addressables precedence cannot be certified from pseudo-IL alone.

### Asynchronous previews
- `GetBackgroundTextureAsync(uint32, bool, Action<Texture>)` resolves the resource and delegates to `GetBackgroundPreviewAsync`.
- `GetBackgroundPreviewAsync` builds a preview cache key and checks the preview cache before queuing work.
- `QueueBackgroundPreview` deduplicates in-flight requests, stores callbacks and starts `CoLoadBackgroundPreview`.
- The coroutine state-machine body is present, but its MoveNext currently contains decompiler gaps. Exact remote/local fallback sequence needs native-level validation.

### Custom Spine backgrounds
- `AcquireBackgroundSpine(BackgroundResource)` checks `BackgroundResource.IsCustomSpine` and uses `RuntimeSpineLoader.Load(FullPath, ...)`.
- It retains the resulting `RuntimeSpineAsset`, caches it, calls `SpineBackgroundVisual.ValidatePlayback`, and trims the skeleton cache.
- This is a separate resource path from ordinary background image textures and must be tested independently.

### Local override storage
- `GetResourceOverrideRoot()` delegates to `GetPersistentFilePath(...)`.
- `InitializeAuthoringResources` reads the resource override root, loads global override data, and initializes authoring database/manifest inputs. This is authoring setup and must not be confused with normal mobile startup.
- `LoadGlobalResourceOverrides()` reads/writes override JSON under the override root and calls `ClearBackgroundResources()` after applying changes.

## Porting consequences

1. The first playable iOS target should prioritize the mobile gameplay path: background ID resolution, local override selection, texture loading/cache/release, and custom Spine loading.
2. Keep the existing `BackgroundOverrideResolver` as a unit-testable primitive, but do not claim it fully reproduces `BackgroundResource.ResolveOverride` until all matching and merge semantics are recovered.
3. Keep `AddressableInternalIdResolver` injectable. The exact runtime value/source of `ScenarioResourceManager.AddrLoadUrl` is still unresolved; the authoring CLI fallback URL is not proof of the game's mobile CDN.
4. Implement image decoding with Unity's supported image APIs on iOS, and verify texture formats, color/alpha handling, mipmaps, ownership, and release semantics on device.
5. Implement the custom Spine route separately. The Android IL proves a dependency on `RuntimeSpineLoader.Load` and `SpineBackgroundVisual.ValidatePlayback`, but not that the existing iOS 0.7.9.2 Spine runtime is API- or binary-compatible with Android 1.0.
6. Do not transplant Android RVAs or Cpp2IL pseudo-IL into iOS. Use this report as a behavioral specification and validate against the iOS Mach-O/metadata and actual catalog/assets.

## Still blocking an IPA

- No complete Unity project/source tree is present in the repository.
- No verified mobile runtime assignment for `AddrLoadUrl`.
- No exact target Addressables package/catalog runtime integration test.
- No validated Spine 4.2 iOS integration.
- No Unity iOS export/Xcode build pipeline has produced an app bundle or IPA.

This report records recovered behavior; it is not a source reconstruction and does not claim that an IPA exists.
