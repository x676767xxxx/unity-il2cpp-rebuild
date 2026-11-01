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

## Verified iOS converter build milestone — 2026-11-01

- Latest GitHub Actions run: [41304851374](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41304851374), completed successfully. The earlier expanded-fixture run [41304639881](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41304639881) also passed.
- iPhoneOS ARM64 static library compiled; CI checked the ARM64 archive and all three exported C ABI symbols.
- C ABI invalid-input/output-parameter checks: 4 passed.
- Pinned upstream Spine 4.2 sample fixtures: 25 JSON files passed JSON → SKEL → SKEL conversion through the custom C ABI. The test log reports 25/25 passed.
- Latest Unity plugin artifact: `azurearchive-spine-unity-ios-plugin`, ID `13082656726`, ZIP digest `1ee90d86e267eaf38198f8f63380fe5f739105383518ea4a4a9cf1235c5f422c`.
- This verifies converter compilation and generic upstream fixtures only. It does not validate real AzureArchive skeleton/atlas/page assets, Spine C# Runtime integration, Unity linking, or iPhone playback.



## SKEL converter stability milestone — 2026-11-01 (latest)

- Commit: [2c23520564da2c8061263c1687c13f3b1830b69a](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/2c23520564da2c8061263c1687c13f3b1830b69a), which extends the host ABI smoke test with a second SKEL-to-SKEL conversion and byte-for-byte equality check.
- Workflow: [41305648281](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41305648281), completed successfully on macOS 14; iPhoneOS ARM64 static library build, 11 managed P/Invoke/C ABI contract checks, 4 invalid-input/output-parameter checks, and packaging all passed.
- All 25 pinned upstream Spine 4.2 JSON fixtures passed JSON -> SKEL -> SKEL -> SKEL. For every fixture, the second SKEL conversion produced the same byte length and identical bytes as the preceding SKEL output.
- Artifacts: Unity iOS plugin ID 13082434942 (1,843,690 bytes; SHA-256 `ac51f9c8017a8219cdc504a0cac0a4c2d458b6c16735c4178761d7b500aaf4d4`); iOS ARM64 archive/ABI package ID 13082514570 (1,938,912 bytes; SHA-256 `af1a39911960d53c9a8078c9bd94f72f1b6f1b9d230dafc1d38ed4379a2a3088`).
- Build emits switch-enum warnings in the pinned converter's multi-version reader/writer sources (including older 3.5-4.1 paths and some 4.2 cases). They did not fail this build. Do not suppress or patch them blindly: inspect the exact converter switch semantics and determine whether each warning is relevant to the Spine 4.2 path before changing upstream code.
- Scope boundary: this remains a converter/ABI milestone using upstream sample JSON fixtures. It does not prove that AzureArchive's own `.skel` assets parse, that atlas pages/texture formats resolve, or that Spine 4.2 C# runtime and Unity rendering are integrated. The uploaded `Natsu.zip` has not yet been inspected; keep that file-analysis task queued until the analysis allowance is available.
