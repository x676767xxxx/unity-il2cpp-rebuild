# AzureArchive iOS 1.0 port — implemented feature status (2026-11-01)

This status distinguishes source implementation, isolated CI validation, integration, and real-device validation. It is not a claim of 1.0 feature parity.

## Implemented source components

| Feature area | What exists in this repository | Validation level | Not yet proven |
|---|---|---|---|
| Background resource model | Resource kind/origin descriptors, safe path handling, scenario background resolution, local-before-global override precedence | Standalone .NET tests | Actual game's manifest deserialization and end-to-end scenario selection |
| Custom static backgrounds | Root-confined file read and Unity Texture2D decode candidate; RGBA32, no mipmaps, Clamp | Unity API stub tests | Exact target Unity assembly compile, real image corpus, iOS device |
| Native static backgrounds | Addressables load adapter, CorrectPath callback binding, in-flight request coalescing, operation-handle lease and cache disposal | Unity API stub tests | Real Addressables catalog/provider and actual mobile startup |
| Addressables URL transform | Exact AddrLoadUrl token resolver; hook chains prior transform; bootstrap candidate installs before scene load | Standalone tests and source inspection | Correct runtime value of AddrLoadUrl; actual game startup integration |
| Background preview support | Request coalescing, callback order/error isolation, ownership ledger and trim planner | Standalone tests | Actual preview UI and Unity texture destruction wiring |
| Spine cache/ownership primitives | Atlas page key mapping/cache, lease/refcount ledgers, construction rollback ledger, owned-object cleanup adapter | Standalone tests; cleanup tested with Unity stubs | Full skeleton/atlas/material/mesh/animation loading and rendering |
| Spine JSON-to-SKEL converter | iPhoneOS ARM64 static library and C ABI; explicit Spine 4.2 target version | CI build, symbol checks, 4 invalid-input checks and 25 upstream JSON fixtures | AzureArchive-owned skeletons, Unity runtime linkage, iPhone playback |

## Recovered Android 1.0 behavior encoded in compatibility primitives

- Local custom background overrides take precedence over global overrides; native resource resolution remains a separate fallback path.
- Custom image paths are confined beneath the selected resource root.
- Preview cache policy constants currently represented: 32 MiB and 25-entry threshold.
- Retained Spine skeleton cache budget represented: 768 MiB, with active consumers protected from trimming.
- Spine asset reference ownership, final release, atlas-page leases and partial-construction rollback have engine-independent state models.

These are compatibility building blocks, not a complete recreation of Android 1.0's ScenarioResourceManager or RuntimeSpineLoader.

## Analysis completed, but not itself an implemented game feature

- Platform-matched IL2CPP method/declaration comparisons and Android ARM64 disassembly.
- Addressables catalog decode and 0.7.9.2 iOS versus Android 1.0 comparison: 13,085 bundle names; 12,632 same catalog hashes; 453 different hashes.
- Legacy iOS archive cross-check: 12,627 same-hash/same-size reuse candidates; 453 changed-hash files require Android 1.0-equivalent payloads or a verified replacement source.
- URL string and IL analysis has not established the normal mobile runtime value/source of ScenarioResourceManager.AddrLoadUrl. The authoring-CLI fallback URL is not verified as the gameplay CDN and must not be hard-coded.

## Not implemented / not validated

1. A complete Unity project (Assets/, Packages/manifest.json, ProjectSettings/, scenes, packages and player settings).
2. Full Android 1.0 ScenarioResourceManager gameplay implementation and integration.
3. Complete Spine 4.2 C# Runtime import/linkage and AzureArchive skeleton, atlas, texture, mesh, clipping, constraint, event and animation playback.
4. Actual 1.0 resource payload acquisition for the 453 changed bundles.
5. Verified mobile AddrLoadUrl source/configuration and real catalog/bundle network loading.
6. Character rank/display-rank and any other 1.0 gameplay/UI gaps identified by the method diff.
7. Unity Editor player build, Xcode archive, signed IPA, install, launch and iPhone 13/iOS 18.6.2 test.

## Current truthful milestone

The repository has reusable/tested compatibility primitives and a successfully built iPhoneOS ARM64 Spine converter. It does not yet contain a buildable AzureArchive 1.0 Unity game or an installable iOS 1.0 IPA.