# AzureArchive iOS port — Unity decoder contract-test checkpoint

Date: 2026-10-31

## Changes
Added an isolated .NET 8 test harness for `BackgroundImageDecoder.Unity.cs`:
- Compiles the Unity-gated source with `UNITY_2021_3_OR_NEWER` defined.
- Supplies minimal UnityEngine API stubs only for compile/contract testing; this is not a substitute for the real Unity assemblies.
- Verifies texture dimensions (2x2), RGBA32 format, no mip chain, Clamp wrap mode and assigned name.
- Verifies a successful decode leaves the texture alive.
- Verifies a false LoadImage result and a thrown decoder exception both destroy the allocated texture.
- Verifies empty input is rejected before texture allocation.

Added workflow path triggers and a CI step to run this harness.

## CI
- Run [41184314103](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41184314103) was queued and then entered `in_progress` when this checkpoint was written.
- Job: `build-and-test` (job ID `124487959103`).
- No test pass or artifact is claimed until this run completes.

## Limitations
- The test harness compiles against local stubs, not UnityEngine assemblies.
- It does not validate Unity native object lifetime, main-thread behavior, actual image codecs, IL2CPP, iOS rendering, or integration with AzureArchive.
- No real game Spine assets or complete Unity/Xcode project have been validated.
- No installable IPA has been built.

## Next
1. Inspect CI logs and correct any harness/compiler errors.
2. If the stub harness passes, seek the exact UnityEngine assemblies/project version and compile against the real Unity API.
3. Validate with actual game image and Spine assets before integration.
