# Background image and preview implementation evidence (2026-10-31)

## Source and quality
- Exact Android 1.0 and Windows 1.0.1 method bodies exported using Cpp2IL + ILSpy.
- Successful export: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41177293557
- Full IL: analysis/full-exact-background-method-il-2026-10-31.md
- Recovered IL is not original source. Unresolved decompiler markers remain and must not be silently interpreted.

## Confirmed behavior

### Override selection
Both platforms' `BackgroundResource.ResolveOverride(id, local, global)` enumerate local overrides first, apply `FirstOrDefault`, return a local match when present, and otherwise enumerate global overrides and return the first match. The predicate closure is partly unresolved in Android IL; the generated predicate body visibly compares a resource ID with the captured requested ID, but exact null/type/source filters remain verification items.

### Resource path safety
Both `BackgroundResource.ResolvePath(root, relative)` bodies include whitespace checks, separator normalization, `Path.IsPathRooted`, path-segment splitting, `Path.Combine`, `Path.GetFullPath`, and a root-prefix `StartsWith` check. Recovered exception messages include:
- `A background resource root and relative path are required.`
- `Background paths must be relative to the resource directory.`
- `Background path escapes its resource directory.`

Windows IL has fewer method-level decompiler markers (10) than Android (36). Exact predicate/comparison details still require verification. A standalone conservative helper was added under `src/AzureArchive.BackgroundCompat`.

### Static custom-image loading
The Android `GetBackgroundTexture(BackgroundResource)` body explicitly calls:
- `BackgroundResource.get_FullPath()`;
- dictionary `TryGetValue` and Unity `Object.op_Equality` checks;
- `File.ReadAllBytes(fullPath)`;
- `DecodeBackgroundImage(bytes, resource.Name)`;
- inserts the decoded texture into `_loadedCustomBackgroundTextures`, keyed by FullPath.

Windows IL corroborates the custom-file branch. Method-level warning counts: Android 17; Windows 2.

### Native Addressables branch
The same method uses `CorrectPath`, `AsyncOperationHandle<T>.WaitForCompletion()`, and an explicit `Addressables.Release(handle)` on an error path. The exact handle-acquisition call is not reliably reconstructed, so this report does not claim that a particular `LoadAssetAsync` overload was proven by this method. The old iOS metadata separately contains Addressables declarations, but runtime behavior is untested.

### Image decode
Both platforms' `DecodeBackgroundImage(byte[], string)` explicitly:
- construct a Texture2D with arguments `2, 2, 4, false` (2x2 initial size, TextureFormat enum value 4 / RGBA32, no mip chain);
- set texture name and wrap mode enum value 1 / Clamp;
- call `ImageConversion.LoadImage(texture, bytes, true)`;
- throw `InvalidDataException("Cannot decode background image: " + name)` when LoadImage returns false.

The cleanup path includes unresolved decompiler markers. A Unity-side candidate source file was added at `src/AzureArchive.UnityCompat/BackgroundImageDecoder.Unity.cs`; it conservatively destroys the newly created texture on failure. It has not been compiled against UnityEngine or run in an iOS player.

### Preview cache and request coalescing
Both `GetBackgroundPreviewAsync` methods construct a key, query `backgroundPreviews`, check Unity object validity and RenderTexture.IsCreated(), consult `Time.frameCount`, then queue work. Both `QueueBackgroundPreview` methods use `pendingBackgroundPreviews`, a key-to-callback-list dictionary, and start a `CoLoadBackgroundPreview` coroutine. This supports in-flight request coalescing, though exact callback and RenderTexture ownership semantics remain partly unresolved.

### Spine-specific cache key
`BackgroundPreviewKey` includes `BackgroundResource.CacheKey` and references `.skel`, `.atlas`, `no-preview`, and `.skel.preview.png`, with a stamp helper. This establishes that Spine sidecar/preview state influences the key; it does not establish that the old iOS Spine runtime can load these assets.

## Implementation order
1. Root-relative path safety utility — implemented and build-tested independently.
2. Static custom-image decoder — Unity C# candidate written, not compiled/integrated.
3. Addressables — wait for exact handle acquisition, ownership and release semantics.
4. Preview — recover coroutine completion, callback fan-out and RenderTexture ownership.
5. Spine — separate 4.2 runtime and iOS converter work; do not send Spine files through the static image decoder.

## Delivery boundary
- The path utility is a build-tested .NET Standard 2.1 library; its GitHub Actions run is https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41177433645.
- The Unity decoder is candidate source only, not a compiled Unity assembly.
- No full Unity project/source tree or macOS/Xcode build host is available.
- No signed or device-verified AzureArchive 1.0 IPA has been produced.
- No Android RVA or machine code is proposed for iOS reuse.


## Native ARM64 preview coroutine and cache lifecycle — verified 2026-10-31

Source: Android 1.0 ARM64 `libil2cpp.so`, matching Il2CppDumper metadata and the corrected disassembly artifact from run [41200411309](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41200411309). The native target is `ScenarioResourceManager.<CoLoadBackgroundPreview>d__28.MoveNext` at Android RVA `0x3270D7C`; this is not an iOS address.

### Coroutine state and branch evidence
The Android dump gives the generated iterator fields:
- `Texture texture`, `bool addressable`, `string full`;
- `AsyncOperationHandle<Texture> handle`;
- `Task<byte[]> read`;
- `Task<RuntimeSpineLoader.Files> read`;
- original resource, owner, thumbnail preference and preview key.

The ARM64 body has a six-state dispatch (states 0–5). Native call targets resolve to:
- `ScenarioResourceManager.CorrectPath(string)`;
- `File.Exists(string)` and `File.ReadAllBytesAsync(string, CancellationToken)`;
- `ScenarioResourceManager.DecodeBackgroundImage(byte[], string)`;
- `RuntimeSpineLoader.ReadAsync(string)` and `RuntimeSpineLoader.Create(...)`;
- `SpineBackgroundVisual.RenderPreview(resource, asset, 1024)`;
- `BackgroundPreviewCachePath(resource)` and `SaveBackgroundPreview(RenderTexture, path)`;
- `Graphics.Blit(Texture, RenderTexture)`;
- `LimitBackgroundPreviewSize(ref Texture, ref bool, 1024)`;
- `ReleaseBackgroundPreview(Object)` on a cleanup path;
- `ReportBackgroundError(Exception)` on an error path.

This establishes that preview generation is not just a single static-image decode: the coroutine contains separate static-image, Addressables-handle and Spine-preview paths, and it uses async file reads. It also confirms a 1024-pixel maximum-size argument at the observed preview render/limit call sites. Exact ordering of every await/error branch still needs careful state-by-state review before writing replacement C#.

### Cache ownership and eviction
The Android `ScenarioResourceManager.BackgroundPreview` metadata declares:
- `Texture` at offset 0x10;
- `Addressable` at 0x18;
- `Users` at 0x1C;
- `LastUse` at 0x20;
- `Resource` at 0x28;
- `PreferThumbnail` at 0x30.

Native helper disassembly verifies:
- `ReleaseBackgroundPreview` finds the preview record for the supplied texture, updates the user count and last-use bookkeeping, and calls `TrimBackgroundPreviews(false)`.
- The trim filter is exactly `Users == 0`.
- The trim sort key is `LastUse`, supporting least-recently-used eviction among unused previews.
- `TrimBackgroundPreviews(bool allUnused)` branch conditions are now reconstructed from the raw ARM64 loop:
  - candidates are the unused-preview sequence sorted by ascending `LastUse`;
  - when `allUnused == true`, each candidate goes directly to destruction, bypassing both normal thresholds;
  - otherwise, trimming continues while the current total preview bytes are **strictly greater than 0x02000000 (32 MiB)** OR the current cache entry count is **at least 25**;
  - after each destruction, the returned size is subtracted from the running byte total and the cache count is re-read on the next iteration.
- A pure `BackgroundPreviewTrimPlanner` now models those decision rules and takes already-LRU-ordered candidates with their runtime-reported byte sizes. It selects keys only; Unity/Addressables destruction remains the runtime adapter's responsibility. Boundary tests cover exactly 32 MiB, 32 MiB + 1 byte, 25 entries, and forced trim.
- `DestroyBackgroundPreview` branches on the `Addressable` flag, using the Addressables release path for addressable textures and the Unity object-destruction path for owned textures.

### Implementation consequence
The tested `BackgroundPreviewRequestCoalescer<TTexture>` covers only the pending-key/callback-list part of `QueueBackgroundPreview`. A real adapter still needs the recovered six-state coroutine flow, both async read types, native handle ownership, RenderTexture lifecycle, 1024 sizing, preview cache record/ref-count/last-use fields, LRU trim and error cleanup. Do not replace those with a generic image-only cache.


### Resource-release helper details — verified from native helper bodies

The corrected full disassembly also exposes the helper bodies, not only their call sites:

- `ReleaseBackgroundPreview(texture)` searches the cache record by texture identity. If no matching record exists it takes the no-match return path. For a match, it decrements `Users`, updates `LastUse` to the current frame, and invokes `TrimBackgroundPreviews(false)`. The method returns whether a matching preview record was found.
- `DestroyBackgroundPreview(preview)` first checks Unity object validity. If the texture is already a destroyed/null Unity object, it exits without issuing a second destruction/release. Otherwise it branches on the record's `Addressable` flag: addressable records go through `Addressables.Release(texture)`; non-addressable records go through the Unity object destruction path.
- The normal trim loop enumerates unused records, orders them by `LastUse`, and evaluates the running byte total and current cache count before each deletion. After deletion it subtracts the size returned by the size helper and re-reads the cache count on the next iteration.
- This establishes a useful adapter contract: decrement/update first; never evict an entry with positive users; make Unity-object release idempotent with respect to already-destroyed objects; choose exactly one release mechanism from the stored ownership flag; and only then continue the trim loop.

### Coroutine reconstruction boundary

The iterator metadata and native call graph establish six states and the separate async operations (file-byte task, Spine-files task and Addressables handle), but the generated state machine has several compiler/runtime helper calls whose high-level exception and callback semantics are not yet fully mapped. In particular, do not yet claim a complete ordering for: Addressables handle acquisition vs completion, callback-list fan-out vs cache registration, temporary RenderTexture ownership vs final Texture ownership, and exception fan-out to every pending callback. Those are the remaining reverse-engineering targets before a production Unity coroutine adapter is written.


### Additional raw MoveNext observations from corrected ARM64 disassembly (artifact 13041984071)

The complete selected-disassembly text was re-opened and checked directly, rather than relying on the earlier summary.

- The state-machine dispatch reads the integer state at object offset `0x10`, rejects values above 5, and dispatches through a six-entry jump table. This confirms six dispatch arms; the current text-only artifact does not expose the jump-table bytes needed to assign every arm index to a semantic label with certainty.
- The iterator stores the source resource at offset `0x20`, thumbnail preference at `0x30`, working/final texture references in fields around `0x40` and `0x50`, a task/async working reference around `0x78`, and another async/temporary reference around `0x80`. These are state-machine field offsets, not managed C# field names.
- One path obtains the resource path, reads file bytes asynchronously, then calls `DecodeBackgroundImage`; the returned texture is stored in the iterator's texture field and the byte-task field is cleared before control advances.
- A separate branch obtains the resource's Addressables-related value and invokes completion/result helpers before the preview-generation path continues. The exact generic helper identities and exception/result semantics are not symbolized in this artifact.
- The preview generation branch selects a target dimension of 512 when the thumbnail flag is set and 1024 otherwise, then calls the native method at RVA `0x3263310` (previously identified as `LimitBackgroundPreviewSize`). It subsequently checks the resulting texture/RenderTexture state and enters the preview-save/render operations.
- The state machine contains native exception landing pads and cleanup branches. Their presence is confirmed, but the exception object mapping, exact task-await exception propagation, and whether every failure path fans out the same exception to all queued callbacks are not established by this disassembly alone.

**Implementation consequence:** the static-file decode ordering and 512/1024 sizing choice can now be treated as evidence-backed. Do not yet implement a single shared failure callback path or assert exact Addressables-vs-file fallback order; those require the jump-table bytes plus symbolized helper/type metadata or a controlled runtime trace.


### Callback fan-out and callback-exception handling — additional control-flow evidence

A second pass over the full `CoLoadBackgroundPreview.MoveNext` body resolves one previously open item:

- After the preview result has been prepared, the success path walks a linked collection of callback/delegate records. Each record's callable target is loaded and invoked indirectly; the loop advances through the collection until its end. This is direct machine-code evidence of callback fan-out, not merely the presence of a callback-list field.
- The delegate invocation is enclosed by native exception landing-pad paths. A callback-thrown exception is caught, placed into a temporary exception collection, and a separate exception-processing call is made before the loop continues. Therefore callback invocation errors are not simply allowed to unwind immediately through the whole fan-out loop.
- The coroutine also has distinct exception landing pads around earlier asynchronous/load stages. These paths include exception-object construction/throw and calls into helper routines, but the selected disassembly does not identify the semantic names or complete contract of those helpers. It does not yet prove whether every load-stage failure is delivered to every queued callback, whether a null preview is delivered, or whether a failed operation is silently removed from the pending map.
- The exact cache registration ordering is partially visible: preview result/record construction occurs before the callback iteration, and the constructed record carries the texture, ownership/thumbnail metadata and cache-related fields. However, the full managed cache insertion point and its relationship to the callback collection's source object still require type-layout confirmation.

**Porting consequence:** the Unity adapter must preserve multi-subscriber fan-out and isolate exceptions thrown by an individual subscriber so one callback cannot prevent later subscribers from being considered. Keep load-stage failure fan-out as an explicit unresolved contract; do not assume the success fan-out loop proves failure fan-out behavior.

### Clarification after instruction-level re-review: exception landing pads are not yet semantically named

The previous callback-exception paragraph was too specific about the purpose of the temporary stack collection. The disassembly does show catch handlers, exception-type checks, temporary storage of caught exception object/type-related pointers, and follow-up virtual/helper calls before control returns to the coroutine's main flow. However, the helper targets at 0x3261D74, 0x2F2DC8C, 0x5AD7AD8, and 0x66B9AD8 are not symbolized in this selected text. It is therefore not safe to label that stack area an exception collection or to claim that it aggregates subscriber callback failures.

What is safe to state:
- The iterator has multiple compiler-generated exception landing pads covering separate portions of the coroutine.
- Some landing pads catch an exception, inspect its runtime type, store an exception-related pointer temporarily, then call further runtime/helper routines.
- The delegate invocation loop and these exception handlers coexist in the same method, but the current artifact does not prove that every such handler belongs specifically to subscriber callbacks rather than await/continuation/runtime cleanup.
- No production exception policy should be copied from these handlers until the helper symbols and managed type metadata are resolved.

This clarification supersedes any stronger interpretation in the earlier Callback fan-out and callback-exception handling section. The confirmed callback loop remains evidence of callback iteration; the exception policy remains unresolved.
