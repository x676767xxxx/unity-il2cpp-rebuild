# AzureArchive iOS port progress — local-first background override helper

Date: 2026-10-31

## Newly implemented helper
Added `BackgroundOverrideResolver.TryResolve<T>` under `src/AzureArchive.BackgroundCompat`.
- Searches the local override sequence first and returns its first matching item.
- Searches global overrides only when no local item matches.
- Allows either sequence to be null/empty.
- Requires a non-null caller-supplied predicate.
- Keeps resource-ID matching policy outside this helper because the recovered Android predicate has unresolved decompiler details.

This follows the recovered Android 1.0 / Windows 1.0.1 local-before-global override selection order. It is a reusable compatibility primitive, not yet connected to ScenarioResourceManager or any Unity gameplay entry point.

## Tests added
Added five checks for local precedence, global fallback, null sequences, no match, and null predicate. The existing 13 path-safety checks remain.

## CI
- Run [41184475166](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41184475166) was in progress when this note was written.
- No pass/fail result is claimed until it completes.

## Current 1.0 feature implementation assessment
Implemented/build-tested standalone helpers:
- root-relative background path validation;
- candidate static image decode source (compiled only against test stubs, not real Unity);
- local-first/global-fallback override selection (CI pending).

Not implemented/integrated:
- ScenarioResourceManager / ResolveBackground;
- Addressables native background load and handle ownership;
- texture cache keyed by resource identity;
- preview coroutine, request coalescing, callback fan-out, RenderTexture cache/eviction;
- RuntimeSpineLoader .skel/.atlas/page texture loading;
- Spine 4.2 runtime integration and iOS converter integration into the game;
- AuthoringResourceCatalog and other Android 1.0-specific gameplay/UI systems;
- complete Unity project/Xcode target, app packaging and IPA.

## Evidence boundary
Latest Unity decoder contract-test workflow [41184314103](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41184314103) succeeded, but its UnityEngine surface is a local stub harness. It does not prove compatibility with the actual Unity 2023.2.20f1c1 player or iOS device. No full AzureArchive Android 1.0 feature parity or installable IPA is claimed.
