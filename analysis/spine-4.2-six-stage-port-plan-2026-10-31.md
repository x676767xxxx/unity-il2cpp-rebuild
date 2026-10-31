# Spine 4.2 implementation plan and upstream compatibility checkpoint

Date: 2026-10-31

## Six implementation gates

1. **Confirm input format and exact runtime generation**
   - Confirm the game converter enum maps value 6 to Spine 4.2.
   - Inventory actual `.skel`, `.json`, `.atlas`, and atlas page images from the supplied Android 1.0 package.
   - Record skeleton header/version, atlas page names, texture dimensions/formats, and required feature usage.
2. **Parse skeleton and atlas data**
   - Read the binary skeleton and atlas metadata without creating Unity objects on worker threads.
   - Enumerate atlas pages and map each page name to a validated resource path.
   - Reject missing/duplicate/ambiguous page mappings.
3. **Load and own atlas page textures**
   - Implement PNG/JPEG fallback and DXT5 cache path as supported by the game.
   - Create page materials on the Unity main thread.
   - Test missing pages, corrupt textures, cleanup and ownership.
4. **Integrate the matching Spine 4.2 C# + Spine-Unity runtime**
   - Resolve licensing and pin a specific upstream revision.
   - Adapt the game's byte[] SkeletonDataAsset factory and custom texture-loader hooks.
   - Build for the exact Unity/iOS target and IL2CPP.
5. **Validate skeleton semantics and animation playback**
   - Exercise bones, slots, skins, weighted/unweighted meshes, clipping, sequences, constraints, physics timelines, events, animation mixing and time scaling where present in actual game assets.
   - Compare output against Android/Windows 1.0 reference behavior.
6. **Integrate with AzureArchive gameplay and release**
   - Implement RuntimeSpineAsset ownership/retain/release, caches and trimming.
   - Integrate the background presenter/transitions and scenario selection.
   - Build and device-test a complete iOS app before producing/signing an IPA.

## Verified upstream runtime candidate

Repository: https://github.com/EsotericSoftware/spine-runtimes
Branch: `4.2`
Inspected commit: `5cb78e77d3711ddcd80c7bd043bb11cf70fcc88c`
- spine-csharp package version: `4.2.37`
- spine-unity package version: `4.2.121`
- spine-csharp README states it supports Spine 4.2.xx data and all Spine features.
- Source API includes `SkeletonBinary : SkeletonLoader`, `Sequence`, and `PhysicsConstraint`.
- Spine-Unity source includes `SpineAtlasAsset.customTextureLoader`, `SpineAtlasAsset.CreateRuntimeInstance(...)`, `SkeletonDataAsset.CreateRuntimeInstance(TextAsset,...)`, and `SetupRuntimeBlendModeMaterials(...)`.

This API family aligns with the broad feature generation found in Android 1.0 metadata, but it is not yet proven to be the exact runtime revision embedded by AzureArchive. The game adds its own byte-array factory/loader methods, which need an adapter.

## License gate — do not bypass

The upstream repository's LICENSE and official Spine Editor License Agreement (last updated 2025-04-05) require a valid Spine Editor license at the time Spine Runtimes are integrated into a product or derivative runtime is created. The Trial license explicitly does not grant runtime integration rights. Personal/noncommercial intent alone does not remove this requirement.
- Official agreement: https://esotericsoftware.com/spine-editor-license
- Upstream source: https://github.com/EsotericSoftware/spine-runtimes/tree/4.2

No upstream Spine runtime source has been copied into this project. Before integrating it, the product owner must confirm an applicable valid Spine Editor license or obtain written permission. A separate clean-room implementation is a different, substantially larger engineering route and must not copy upstream runtime code.

## Existing project evidence and blockers

- Android 1.0 / Windows 1.0.1 use the SpineVersion enum where value 6 is Version42.
- Android/Windows RuntimeSpineLoader reads `.atlas` and `.skel`, enumerates pages, loads page textures, creates runtime atlas/materials, converts skeleton bytes to Version42, then creates SkeletonDataAsset and sets up blend-mode materials.
- Old iOS 0.7.9.2 has an older Spine API: no Sequence/Physics API family, no RuntimeSpineLoader/RuntimeSpineAsset, no game's native converter declaration, and no byte[] SkeletonDataAsset factory/custom texture-loader API.
- Supplied Windows 1.0.1 and iOS 0.7.9.2 archives are player builds, not a rebuildable Unity project. No `Assets/`, `ProjectSettings/`, or `Packages/manifest.json` project was found.
- The current iOS converter static library is a custom ABI experiment, not the original game's native ABI and not a Spine renderer/runtime.
- No real game's `.skel`/atlas/page fixture has yet been validated end-to-end by the iOS runtime.
- No complete Unity/Xcode target or IPA exists.

## Current implementation status

- Spine 4.2 source/version candidate identified and API inspected.
- Six-gate implementation plan recorded.
- No Spine 4.2 runtime has been copied or integrated.
- No actual skeleton is currently being animated by the iOS build.
- No IPA is claimed.
