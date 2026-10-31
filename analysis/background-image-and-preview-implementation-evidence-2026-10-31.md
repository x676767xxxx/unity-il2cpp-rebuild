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
