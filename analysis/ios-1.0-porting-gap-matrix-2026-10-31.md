# AzureArchive iOS 1.0 porting gap matrix — 2026-10-31

Inputs: iOS 0.7.9.2 IPA, Android 1.0 APK, Windows 1.0.1 ZIP, Android ARM64 disassembly and call-site report.

This is an evidence-based porting plan, not a source reconstruction or a build.

## Important correction to earlier iOS method-presence claims

The first iOS inspection established that the 0.7.9.2 IPA contains:
- ARM64 UnityFramework, 82,150,064 bytes.
- IL2CPP global-metadata.dat, 15,946,676 bytes.
- Unity runtime string 2023.2.20f1c1.
- Addressables catalog and related files.

The inspection also found strings including ScenarioResourceManager, TryGetBackgroundOverridePath, GetBackgroundTexture and GetBackgroundTextureAsync. String presence is not proof of a complete method declaration or callable implementation.

The first Cpp2IL attempt against the iOS .app directory failed during path resolution because the tool did not locate an executable in the supplied directory. Therefore, previous statements that specific methods are absent from iOS metadata must be treated as unverified until a platform-aware Mach-O + metadata dump succeeds.

A new cross-platform Il2CppDumper run is being used to compare each platform's own native binary with its matching global-metadata.dat. Il2CppDumper documents support for Mach-O, ELF and PE; its published release documents Unity support through 2022.2, while this IPA reports Unity 2023.2.20f1c1. Compatibility is therefore being tested, not assumed.

## Priority 0 — establish a buildable iOS base

- The only supplied iOS native baseline is 0.7.9.2.
- No Unity project, original C# source, or iOS 1.0 Mach-O was supplied.
- Android ELF and Windows PE code and their RVAs are not reusable as iOS machine code.
- The old iOS UnityFramework is a candidate baseline to investigate, not yet a proven 1.0-compatible build base.
- Before implementation, establish a safe way to extend the existing iOS UnityFramework or obtain a matching Unity project/source.

## Priority 1 — background runtime (likely core mobile behavior)

Android 1.0 analysis identified background-related methods:
- ResolveBackground
- EnumerateBackgrounds
- GetBackgroundTexture
- AcquireBackgroundSpine
- TryGetBackgroundOverridePath
- GetCustomBgEffect
- GetBgEffectChoices
- GetBackgroundPreviewAsync
- InitializeAuthoringResources

Android direct-call scan found:
- ResolveBackground: 8 direct BL sites
- GetBackgroundTexture: 1 direct BL site
- GetBackgroundPreviewAsync: 2 direct BL sites
- InitializeAuthoringResources: 1 direct BL site

The direct BL scan is incomplete: it does not cover virtual dispatch, delegates, function pointers or runtime registration.

Porting decision:
- Treat background resolution, texture acquisition and override lookup as high-priority investigation targets.
- Treat preview as medium priority until UI entry points are confirmed.
- Reimplement against iOS-compatible Unity APIs and iOS resource storage; do not transplant Android machine code.
- Verify data formats and resource ownership/lifetime before implementation.

## Priority 2 — authoring resource catalog (feature scope not established)

Android 1.0 and Windows 1.0.1 recovery reported an AuthoringResourceCatalog type with methods including Search, Resolve, StageReferencedResources and Import. Earlier call-site analysis found direct call counts Search 1, Resolve 5, Import 2, StageReferencedResources 0.

These counts are partial call-graph evidence only. Zero direct BL sites does not prove a method is unused.

Porting decision:
- Do not implement the entire catalog yet.
- Identify the Android screens and ordinary gameplay paths that invoke catalog search/resolve/import.
- If only local authoring/import tools use it, defer it from the first playable iOS build.

## Priority 3 — smaller API differences to verify

- Character.ApplyDisplayRank(int): verify declaration and UI/gameplay use across all three platforms before porting.
- CharacterManager.GetSkeletonDataAssetAtPath: verify declaration and content dependency.
- Android persistent file extension default reportedly differs (.aap2 vs .aap); determine whether this is a file-format/version distinction or platform storage convention before changing iOS persistence.

## Exclude unless new evidence appears

- CharacterCurve / CharacterCurvePlayer was reported only in Windows 1.0.1, not Android 1.0. Treat as desktop-specific unless new mobile evidence appears.

## Next concrete analysis tasks

1. Complete the platform-matched Mach-O / ELF / PE metadata dump.
2. Compare ScenarioResourceManager, AuthoringResourceCatalog, Character and CharacterManager declarations using each platform's own metadata.
3. Map Android background method callers to enclosing methods and UI/gameplay entry points.
4. Classify AuthoringResourceCatalog callers as gameplay-critical vs optional authoring.
5. Export raw IL for selected Android/Windows methods; treat ILSpy pseudo-C# as analysis aid, not buildable source.
6. Only then write platform-neutral behavior specifications and implement iOS equivalents.

## Confidence and limitations

- Metadata declaration presence does not prove runtime behavior.
- Decompiled C# may contain invalid IL stack placeholders and unresolved tokens; it is not buildable source.
- Android ARM64 RVAs must not be applied to iOS Mach-O.
- Current evidence supports a porting investigation plan, not full feature parity.
- No iOS 1.0 IPA has been built yet.


## Verified exact method diff (completed 2026-10-31)

A platform-matched Il2CppDumper extraction subsequently succeeded for all three packages. Exact top-level class-scope parsing confirms:
- ScenarioResourceManager: iOS has 3/11 selected declarations; Android 1.0 and Windows 1.0.1 have all 11. The eight additional Android/Windows declarations are ResolveBackground, EnumerateBackgrounds, AcquireBackgroundSpine, GetBackgroundTexture(BackgroundResource), GetBackgroundPreviewAsync, InitializeAuthoringResources, GetCustomBgEffect and GetBgEffectChoices.
- AuthoringResourceCatalog: absent from the iOS 0.7.9.2 dump; present in Android and Windows with matching Search, Resolve, StageReferencedResources (two overloads) and Import declarations.
- Character.ApplyDisplayRank(int): absent in old iOS; present in Android and Windows.
- CharacterManager.GetSkeletonDataAssetAtPath exists on all platforms, but Android/Windows have an extra optional quiet parameter.

Full signatures and limitations: [exact method diff](ios-android-windows-exact-method-diff-2026-10-31.md). These are metadata declarations, not recovered source bodies or proof of runtime behavior. The Android dump completed but logged a protection warning, so native offsets/call paths need independent verification.

## Implementation checkpoint — standalone compatibility primitives (2026-10-31)

- Added a root-confined background image file decoder overload in `BackgroundImageDecoder.Unity.cs`; the overload uses `SafeResourcePath.ResolveUnderRoot` before reading resource-manifest paths.
- Added the missing `SafeResourcePath.cs` compile include to the Unity compatibility test project and regression tests for normal nested paths, parent traversal and absolute paths.
- The compatibility CI passed on [run 41191825872](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41191825872): library build, path safety tests, Unity decoder stub tests, Addressables adapter tests and artifact upload all succeeded. Artifact: `azurearchive-background-path-compat-library` (14,814 bytes).
- This is still a standalone .NET compatibility library tested against isolated Unity API stubs. It is not integrated into the actual UnityFramework or a Unity project, and the tests do not establish runtime behavior on iOS.
- Addressables token resolver and generic handle-lease adapter exist as separate primitives; the actual Addressables ResourceManager internal-ID transform hook is not yet connected to the game catalog.
- Spine 4.2 runtime integration remains blocked by the old iOS 0.7.9.2 API gap. The existing native bridge is only an ABI shell; the matching iOS converter implementation and compatible Spine runtime are not present.
- No iOS 1.0 Unity project/source tree, complete runtime port, Xcode archive or IPA has been produced.

## Next implementation gates

1. Verify the iOS 2023.2 Addressables `InternalIdTransformFunc` API from platform-matched metadata and implement a guarded registration/restore adapter; test that only the verified token is rewritten and all unrelated IDs preserve existing transforms.
2. Keep the first playable milestone static-background-only; defer Spine backgrounds until a compatible Spine runtime and iOS converter source are available.
3. Establish an actual iOS integration base: obtain the matching Unity 1.0 project/source or prove a safe, reproducible way to extend the old iOS UnityFramework. Do not call a standalone library an IPA.
