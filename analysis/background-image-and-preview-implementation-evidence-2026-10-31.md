# Background image and preview implementation evidence (2026-10-31)

## Source and quality
- Exact method bodies were exported from Android 1.0 and Windows 1.0.1 Cpp2IL recovery outputs using ILSpy.
- Successful workflow: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41177293557
- Full IL report: analysis/full-exact-background-method-il-2026-10-31.md
- The recovered IL is not original source. The methods retain NoteDecompilerIssue/unmanaged-load markers; method bodies must be treated conservatively.

## Confirmed behavior

### 1. Override selection
Both Android and Windows `BackgroundResource.ResolveOverride(id, local, global)`:
1. Enumerate the local override data using `BackgroundResource.Overrides(local, ResourceType)`.
2. Apply a `FirstOrDefault` predicate.
3. Return a local match if non-null.
4. Otherwise enumerate global overrides and return the first match.

The predicate closure is partly unresolved in Android IL; a generated predicate body visibly compares a BackgroundResource ID with the captured requested ID, but do not infer all null/type/source rules from this alone. Implement local-before-global ordering, but keep the precise filter contract as an explicit verification item.

### 2. Safe resource path resolution
Android and Windows `BackgroundResource.ResolvePath(root, relative)` contain:
- whitespace validation;
- slash normalization;
- `Path.IsPathRooted` checks;
- path-segment splitting and an `Any` predicate;
- `Path.Combine` and `Path.GetFullPath`;
- a root-prefix `String.StartsWith` check;
- explicit `InvalidDataException` messages:
  - `A background resource root and relative path are required.`
  - `Background paths must be relative to the resource directory.`
  - `Background path escapes its resource directory.`

Windows recovered IL is less degraded here (10 decompiler markers versus 36 on Android). The security intent is clear, but the exact predicate and platform-specific comparison semantics should still be checked before claiming byte-for-byte equivalence.

### 3. Custom image loading and texture cache
Android `ScenarioResourceManager.GetBackgroundTexture(BackgroundResource)` explicitly contains:
- `BackgroundResource.FullPath` lookup;
- custom texture cache lookup and Unity Object liveness check;
- `File.ReadAllBytes(FullPath)`;
- `DecodeBackgroundImage(bytes, resource.Name)`;
- insertion into `_loadedCustomBackgroundTextures`, keyed by FullPath.

Windows IL corroborates the same custom-file branch. The Android method has 17 decompiler-warning markers; Windows has 2. This is direct IL evidence, not a string-only inference.

### 4. Native Addressables branch
The same method's native-resource path calls `CorrectPath`, uses an `AsyncOperationHandle<T>.WaitForCompletion()`, and has an explicit `Addressables.Release(handle)` call on an error path. The exact handle acquisition call is not reliably reconstructed in the IL, so do not state that a specific `LoadAssetAsync` overload was confirmed from this method. The old iOS metadata independently contains Addressables load/release declarations, but actual iOS behavior remains untested.

### 5. Image decode parameters
Android and Windows `DecodeBackgroundImage(byte[], string)` explicitly:
- construct a Texture2D with IL arguments `2, 2, 4, false` (2x2 initial texture, TextureFormat enum value 4, no mip chain);
- set texture name;
- set wrap mode enum value 1;
- call `ImageConversion.LoadImage(texture, bytes, true)`;
- throw `InvalidDataException("Cannot decode background image: " + name)` if LoadImage returns false.

The constructor arguments and LoadImage call are visible in IL. Cleanup calls/control flow around the exception contain unresolved method markers; exact destruction/ownership on every failure path is not yet proven.

### 6. Preview cache and request coalescing
Both platforms' `GetBackgroundPreviewAsync`:
- construct a preview key;
- query `backgroundPreviews`;
- inspect Unity object validity and RenderTexture creation state;
- use `Time.frameCount` for freshness;
- enqueue work through `QueueBackgroundPreview`.

Both `QueueBackgroundPreview` methods use `pendingBackgroundPreviews`, a dictionary from key to callback list, and start a coroutine via `StartCoroutine(CoLoadBackgroundPreview(...))`. This supports the interpretation that concurrent requests for the same key share one in-flight preview operation and collect callbacks. Some callback invocation and RenderTexture ownership details remain unresolved.

### 7. Spine-specific preview key
The key builder references `.skel`, `.atlas`, `no-preview`, and `.skel.preview.png`, plus the resource cache key and a stamp helper. This is evidence that Spine sidecar files and preview availability affect cache identity. It is not proof that the old iOS runtime can parse those files.

## Implementation order supported by evidence
1. Build a standalone, tested root-relative path utility first; do not silently accept absolute paths or traversal.
2. Implement the static custom-image path using File.ReadAllBytes + Texture2D + ImageConversion.LoadImage, with main-thread Unity object creation and explicit ownership.
3. Add Addressables loading only after exact handle-acquisition and release semantics are verified for the iOS runtime.
4. Reconstruct preview request coalescing and RenderTexture lifecycle before connecting preview rendering.
5. Keep Spine preview/playback out of the static image adapter; it requires the separate Spine 4.2/runtime-converter work.

## Delivery boundary
- This report is evidence-based analysis, not an integrated Unity patch.
- The old iOS 0.7.9.2 metadata has the Unity image and Addressables API declarations, but no runtime test has been run on iPhone.
- The repository still lacks the Unity project/source tree and matching Spine 4.2/converter source.
- No compilable Unity C# compatibility layer, signed IPA, or device-verified build has been produced.
- No Android RVA or machine code is proposed for iOS reuse.
