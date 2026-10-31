# iOS 1.0 background compatibility implementation specification

Status: design specification only. No Unity project/source or iOS 1.0 Mach-O is available, so this document does not claim to be compilable source or an IPA.

Evidence:
- Android 1.0 ARM64 disassembly and reviewed call graph.
- Platform-matched IL2CPP metadata comparison.
- Existing iOS 0.7.9.2 package as the candidate runtime baseline.

## Objective

Implement the Android 1.0 background runtime behavior on top of a compatible iOS Unity runtime, while preserving existing iOS resource conventions. Do not transplant Android ARM64 instructions or addresses.

## Module A — resource resolution

Required behavior:
1. Accept a scenario background ID.
2. Resolve local override first and global override according to the original BackgroundResource.ResolveOverride semantics.
3. If no override resolves, query scenario background metadata for file name and background type.
4. Normalize the path and construct a resource descriptor containing ID, name, type, source, full path/root and optional Spine override data.
5. Preserve null/missing-resource behavior and diagnostic/error distinctions.

Required evidence before coding:
- Full IL for ResolveBackground and BackgroundResource.ResolveOverride.
- Exact local/global override precedence and fallback behavior.
- Scenario BG Excel schema and resource-root rules.
- Whether resource descriptors are serialized or referenced by other systems.

Acceptance tests:
- no override -> Excel-backed resource
- local override -> local path wins when defined
- global override -> global path used when local override absent
- missing ID / missing file -> same observable fallback/error as Android
- path separators and case handling match iOS filesystem behavior

## Module B — static background texture loading

Required behavior:
1. Obtain resource FullPath.
2. Apply the same path correction/normalization rules.
3. Read bytes with bounded error handling.
4. Decode using Unity Texture2D and ImageConversion.LoadImage semantics.
5. Set texture name/wrap mode and ownership consistently.
6. Destroy partially created Unity objects on failure and avoid leaks/double-destroy.

Acceptance tests:
- supported image formats decode
- malformed and empty files fail safely
- texture dimensions and alpha match Android output
- repeated loads do not leak Unity native objects
- path correction works for packaged and override resources

Important: native disassembly shows calls to File.ReadAllBytes, DecodeBackgroundImage, Texture2D construction, ImageConversion.LoadImage and DestroyOwned. Exact constructor arguments, image flags and wrap mode must be recovered from IL/native control flow before implementing.

## Module C — asynchronous preview and cache

Required behavior:
1. Build a stable preview key from resource identity and preferThumbnail.
2. Validate cached Unity Object and RenderTexture state before reuse.
3. Check frame-age/refresh policy.
4. Queue coroutine work on cache miss/stale cache.
5. Complete callback exactly once, including failure/cancel paths.
6. Refresh or invalidate preview state when application focus changes.

Acceptance tests:
- cache hit returns the existing valid preview
- destroyed/uncreated RenderTexture triggers refresh
- thumbnail preference produces a distinct cache key
- app focus regain refreshes stale previews
- callback is not lost or invoked twice
- scene unload/backgrounding does not retain invalid Unity objects

## Module D — Spine background resources

Required behavior:
1. Detect custom Spine resource mode.
2. Resolve the Spine asset base path and material/blend-material dependencies.
3. Load RuntimeSpineAsset.
4. Obtain SkeletonData and validate playback against override settings.
5. Retain/release resources and trim cache consistently.
6. Handle missing skeleton/material/animation without corrupting the static-background path.

Acceptance tests:
- standard Spine background loads and animates
- override Spine background uses the expected skeleton and materials
- invalid override falls back or errors exactly as intended
- cache retain/release is balanced across scene changes
- memory pressure / focus changes do not leave stale skeleton data

Dependency gate: verify the old iOS 0.7.9.2 build contains a compatible Spine runtime and matching asset format. If it does not, a simple C# wrapper cannot supply the missing native/runtime implementation.

## Module E — background effects

Required behavior:
- Resolve effect data and available choices from the same resource override and scenario metadata.
- Separate runtime playback effect selection from authoring/import UI choices.

Priority gate: GetBgEffectChoices was called by RefreshEffects and ImportSelectedBundle in the inspected Android binary. Confirm whether SetEffect is reached by ordinary scenario playback before adding this to the first playable iOS build.

## Module F — authoring resource initialization

Android initializer consumes:
- SQLite database
- file-name manifest
- FlatBuffers BGM additional-info data
- local/global override data

Do not port this initializer blindly. First establish whether standard mobile gameplay needs the authoring catalog. If only authoring/import screens need it, defer it from the first playable build.

## Build integration strategy

Preferred sequence:
1. Obtain a legitimate buildable Unity iOS project/source or establish a supported extension point in the existing iOS 0.7.9.2 app.
2. Confirm Unity version/API compatibility and Spine runtime compatibility.
3. Implement modules A and B first, then C and D.
4. Build for iOS ARM64 using a compatible Unity/Xcode toolchain.
5. Integrate resources and validate bundle identifiers, entitlements, signing and app lifecycle.
6. Test on a real iPhone; compare background behavior with Android 1.0.

Do not patch Android/Windows native code into the iOS Mach-O. The old IPA's UnityFramework is not by itself a Unity project and cannot be assumed to be relinkable with newly decompiled code.

## Go / no-go gates

- Gate 1: can we legally and technically build/extend the old iOS app? If no, stop feature implementation and obtain a source project or a supported plugin/extension mechanism.
- Gate 2: does the old iOS runtime include compatible Spine and image APIs? If no, quantify the missing runtime dependency before coding.
- Gate 3: can the resource data formats be read on iOS? If no, implement/port parsers before feature code.
- Gate 4: build a minimal iOS test app that loads one background image and one Spine background before attempting full 1.0 parity.

## Current state

- Reverse-engineering and declaration/call-graph analysis: completed for the selected targets, with stated direct-call limitations.
- iOS implementation: not started; no buildable source tree has been established.
- IPA: not built.
