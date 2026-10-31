# AzureArchive iOS 1.0 porting gap matrix — 2026-10-31

Inputs: iOS 0.7.9.2 IPA, Android 1.0 APK, Windows 1.0.1 ZIP, Android ARM64 disassembly and call-site report.
This is an evidence-based porting plan, not a source-code reconstruction or build.

## Priority 0 — establish a buildable iOS base
- The only supplied iOS native baseline is 0.7.9.2.
- No Unity project, original C# source, or iOS 1.0 Mach-O was supplied.
- Android ELF/Windows PE code and their RVAs are not reusable as iOS machine code.
- Before implementation, obtain a matching Unity project/source or establish a safe way to extend the existing iOS UnityFramework. Do not patch Android addresses into iOS.

## Priority 1 — background runtime (likely core mobile behavior)
Evidence:
- iOS metadata lacks Android/Windows 1.0 public APIs: ResolveBackground, EnumerateBackgrounds, GetBackgroundTexture, AcquireBackgroundSpine, TryGetBackgroundOverridePath, GetCustomBgEffect, GetBgEffectChoices, GetBackgroundPreviewAsync, InitializeAuthoringResources.
- Android direct-call scan found: ResolveBackground 8 sites; GetBackgroundTexture 1; GetBackgroundPreviewAsync 2; InitializeAuthoringResources 1.
- ARM64 disassembly confirms ResolveBackground performs resource/database lookups and error/null branches; GetBackgroundTexture has resource-state and cache-like checks; preview method has asynchronous/callback/cache behavior.
Porting decision:
- Treat background resolution, texture acquisition, and override lookup as high priority.
- Treat preview as medium priority until UI entry points are confirmed.
- Reimplement against iOS Unity APIs and iOS resource storage; do not transplant Android machine code.
- Verify data formats and resource ownership/lifetime before writing code.

## Priority 2 — authoring resource catalog (feature scope not yet established)
Evidence:
- AuthoringResourceCatalog is absent from iOS 0.7.9.2 metadata, present in Android 1.0 (77 methods) and Windows 1.0.1 (75 methods).
- Direct-call scan: Search 1, Resolve 5, Import 2, StageReferencedResources 0.
- Decompiled C# is known to contain invalid IL stack placeholders and unresolved method tokens. It is not buildable source.
Porting decision:
- Do not implement the entire catalog yet.
- First identify the Android screens and ordinary gameplay paths that invoke catalog search/resolve/import.
- If only local authoring/import tools use it, defer it from the first playable iOS 1.0 build.
- StageReferencedResources having zero direct BL hits does not prove it is unused; indirect calls and metadata registration are not covered.

## Priority 3 — smaller API differences
- Character.ApplyDisplayRank(int) exists in Android 1.0 but not iOS 0.7.9.2. Confirm its UI/gameplay use before porting.
- CharacterManager.GetSkeletonDataAssetAtPath exists in Android/Windows but not iOS. Confirm whether 1.0 content requires it.
- Android persistent file path default extension differs (.aap2 vs .aap). Determine whether this is a file-format/version distinction or platform storage convention before changing iOS persistence.

## Exclude unless new evidence appears
- CharacterCurve / CharacterCurvePlayer appears only in Windows 1.0.1, not Android 1.0. Treat as desktop-specific, not a mobile port target.

## Next concrete analysis tasks
1. Map each of the eight ResolveBackground call sites to its enclosing managed method and UI/gameplay entry point.
2. Trace the single GetBackgroundTexture caller and both preview callers.
3. Trace InitializeAuthoringResources caller and determine when/how it is initialized.
4. For catalog Search/Resolve/Import callers, identify screens and classify as gameplay-critical vs optional authoring.
5. Export and inspect raw IL for the selected methods; reject ILSpy pseudo-C# as an implementation source.
6. Only after the above, write platform-neutral behavior specs and implement iOS equivalents.

## Confidence and limitations
- Method presence/counts come from IL2CPP metadata declarations, not proof of runtime behavior.
- Direct BL counts are a partial call graph; virtual dispatch, delegates, function pointers and runtime registration can be missed.
- Current evidence supports a porting priority list, not a complete feature parity claim.
- No iOS 1.0 IPA has been built yet.
