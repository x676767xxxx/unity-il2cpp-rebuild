# Spine 4.2 Runtime Integration Map

## Verified upstream pin

- Repository: https://github.com/EsotericSoftware/spine-runtimes
- Branch: `4.2`
- Resolved branch commit checked on 2026-11-01: `5cb78e77d3711ddcd80c7bd043bb11cf70fcc88c`
- Unity package: `com.esotericsoftware.spine.spine-unity`, version `4.2.121`
- C# core package: `com.esotericsoftware.spine.spine-csharp`, version `4.2.37`
- Runtime export compatibility marker: Spine Editor `4.2.xx`
- Package metadata declares Unity `2018.3` as its minimum package version. Official documentation lists Spine 4.2 as compatible with Unity 2017.1 through Unity 6000.3; use the official compatibility table and package metadata when selecting the actual project editor.

The converter workflow currently pins a separate JSON/SKEL converter source at
`wang606/SpineSkeletonDataConverter@5ecb2139b0a1af266974f95abeec6bb8562d1249`.
That is not the Spine Unity runtime pin; keep the two dependencies distinct.

## Required runtime layout from upstream

The official Git installation instructions map these source trees into a Unity project:

- Copy `spine-unity/Assets/` into the game's `Assets/`.
- Copy `spine-csharp/src/` into `Assets/Spine/Runtime/spine-csharp/`.

The upstream package metadata shows the Unity runtime assembly references the `spine-csharp` assembly. The Unity runtime package depends on UGUI, Animation, Physics, and Physics2D modules.

## Current repository integration blockers

This repository is not currently a complete Unity project: no root `Assets/`, `Packages/manifest.json`, or `ProjectSettings/` were present in the checked branch root. Therefore:

1. Do not copy the upstream runtime into a fabricated project layout yet.
2. First identify the actual Unity editor version from the original Android/Windows project metadata or a complete Unity project archive.
3. Compare the game's Spine assets and any modified runtime code against Spine 4.2 before selecting upstream files.
4. Keep the custom iOS converter separate from the runtime. The converter only changes skeleton data format; it does not load atlases/textures or render animations.
5. After project recovery, import the official 4.2 runtime, resolve assembly/package dependencies, then validate representative skeletons, atlases, clipping, meshes, constraints, events, and animation playback on device.

## Licensing gate

The official Spine documentation states that a Spine license is required to integrate Spine Runtimes into an application. Preserve upstream license notices and verify the applicable license before bundling or distributing the runtime. This repository note does not copy or redistribute upstream runtime source.

## Sources

- https://esotericsoftware.com/spine-unity-installation
- https://esotericsoftware.com/spine-runtime-architecture
- https://github.com/EsotericSoftware/spine-runtimes/tree/5cb78e77d3711ddcd80c7bd043bb11cf70fcc88c
