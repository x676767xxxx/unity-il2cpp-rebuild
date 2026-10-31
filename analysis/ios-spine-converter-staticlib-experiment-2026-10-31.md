# iOS ARM64 Spine converter static-library experiment

## Scope
This experiment wraps the reviewed noncommercial converter source at commit `5ecb2139b0a1af266974f95abeec6bb8562d1249` in a new project-specific C ABI and adds an iPhoneOS ARM64 CMake static-library target. It is not the original game's native library and does not claim ABI compatibility with the game's IL2CPP methods.

## Exported custom ABI
- `AzureArchiveSpineConvertBuffer`: converts Spine binary data from its detected input version to a selected version (-1 keeps input version; 0..6 select 3.5 through 4.2).
- `AzureArchiveSpineJsonToBinary`: converts UTF-8 JSON skeleton data to binary at its detected version.
- `AzureArchiveSpineFreeBuffer`: releases buffers allocated by this wrapper.

The wrapper uses temporary files because the upstream converter's conversion entry point is path-based. This is an integration prototype; temporary-file concurrency, error reporting and malformed-input handling need further stress tests.

## ABI caveat
Recovered managed declarations expose pointer/length outputs and a free-buffer method, but do not prove the native calling convention, allocator, string encoding or error-code contract. This wrapper deliberately uses new symbol names and malloc/free ownership. Do not substitute it for the game's native symbols until those details are verified.

## Build gate
The workflow configures CMake with the Xcode iPhoneOS SDK and arm64 architecture, builds the static-library target, checks the archive and exported symbols, and uploads it. Success proves only that this converter library compiles for iPhoneOS ARM64. It does not prove Unity linkage, runtime API compatibility or an IPA build.

## License and status
The source is used for the user's stated personal, noncommercial purpose. Preserve its PolyForm Noncommercial License 1.0.0 notice. No IPA or Unity integration is produced by this experiment.


## Successful iPhoneOS ARM64 build (run 41181017359)
- GitHub Actions: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41181017359
- Result: completed successfully on a `macos-14-arm64` runner with Xcode 15.4 / iPhoneOS 17.5 SDK.
- CMake configured for `CMAKE_SYSTEM_NAME=iOS`, `CMAKE_OSX_SYSROOT=iphoneos`, `CMAKE_OSX_ARCHITECTURES=arm64`, deployment target iOS 13.0.
- Built `libazurearchive_spine_converter_ios.a` (8,195,344 bytes).
- The archive was downloaded and inspected: its object members are Mach-O 64-bit arm64 objects.
- The runner's `xcrun nm -gU` confirmed exported symbols `_AzureArchiveSpineConvertBuffer`, `_AzureArchiveSpineJsonToBinary`, and `_AzureArchiveSpineFreeBuffer`.
- Artifact: `azurearchive-spine-converter-ios-arm64`, ID `13034582585`, compressed size 1,938,921 bytes.

This is now a **compiled iPhoneOS ARM64 static library**, not merely pseudocode. It remains a custom converter ABI, not a verified drop-in replacement for the game's native converter. UnityFramework linkage, matching the game's native ABI, Spine managed runtime upgrade, resource-loader integration, and IPA build are still outstanding.

A follow-up workflow now adds a host-side functional C ABI smoke test (JSON to SKEL and SKEL round-trip). That test has not yet completed.


## Rechecked converter artifact and managed iOS bridge (2026-10-31)

- Rechecked iOS ARM64 run [41183828305](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41183828305): all 8 substantive steps completed successfully, including iPhoneOS ARM64 static-library build, exported-symbol check, host C ABI smoke test, and artifact upload.
- Downloaded artifact ID `13035881289` and inspected the archive locally. It contains:
  - `libazurearchive_spine_converter_ios.a` (8,195,192 bytes);
  - `azurearchive_spine_converter_abi.h`;
  - upstream `LICENSE`.
- Extracted archive members are Mach-O 64-bit arm64 object files. This confirms the archive is an iPhoneOS ARM64 static library, not a simulator or desktop library.
- Host-side functional smoke test passed: invalid-input/output-parameter checks and JSON -> SKEL -> Spine 4.2 SKEL round-trip (8,910 -> 8,910 bytes).
- Compiler emitted upstream switch-enum warnings for several feature enum values. Build success does not establish that all Spine 4.2 features are faithfully converted; compare against real AzureArchive skeleton fixtures and investigate warning cases before production use.
- Added `src/AzureArchive.UnityCompat/SpineConverterNative.Unity.cs`: an iOS-only `DllImport("__Internal")` bridge for the custom ABI. It copies native buffers to managed byte arrays and releases them through the matching native free function.
- The wrapper is not yet linked into a Unity iOS project and has not been compiled by Unity/Xcode. The static library still needs to be added to the Unity-generated Xcode target, with symbol visibility and linker flags verified.
- No full Unity project, UnityFramework integration, actual AzureArchive skeleton fixture, app signing, or IPA exists in this stage.


## Integration readiness check

- Scanned the current port branch tree: no Unity `Assets/`, `ProjectSettings/`, `.xcodeproj` or `.xcworkspace` exists. Therefore this repository currently has no Unity project into which the archive can be integrated and no project settings/build target to produce an IPA.
- Added the guarded C# iOS P/Invoke bridge, but it is only source scaffolding until the native archive is linked into a Unity-generated Xcode target. It has not been compiled by Unity/Xcode.
- Next required input is a matching Unity project/source tree (or a reproducible project export), including the existing iOS plugin layout and app entry scene. Without it, attempting an IPA build here would be fabricated.


## Managed bridge compile/test integration

- Added `SpineConverterNative.Unity.cs` to the Unity compatibility test project with `UNITY_IOS` defined, so the iOS-only P/Invoke declarations are compiled by CI rather than silently excluded.
- Added four pre-native argument-validation tests: null/empty JSON and null/empty skeleton input. These tests deliberately do not call the unavailable `__Internal` native symbols on Linux.
- The resulting workflow run [41185474591](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41185474591) was in progress at the time of this note; no pass is claimed yet.
- This checks C# compilation and managed argument validation only. It does not prove the iOS static archive is linked into UnityFramework, that symbol resolution works in Xcode, or that the native calls work on device.


- Final result: managed bridge CI [41185474591](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41185474591) completed successfully. The project compiled with `UNITY_IOS` defined, all existing Unity decoder checks passed, and the four Spine bridge argument checks passed. Artifact ID `13036883377`, size 13,136 bytes.
- The workflow's successful result is limited to compilation against .NET and isolated Unity API stubs. Native iOS link/runtime validation remains outstanding.


## Managed/native ABI drift gate added (2026-10-31)

- Added `scripts/check-spine-ios-pinvoke-abi.py` to check that all three exported C ABI symbol names are declared in the header and represented by matching `__Internal` Cdecl P/Invoke entries; it also guards the iOS-only compilation condition and Spine 4.2 enum value (6).
- Wired the checker into `ios-spine-converter-staticlib.yml`, including path filters for the managed bridge and checker. A bridge/header change now triggers the iPhoneOS ARM64 workflow and checks managed/native symbol naming before the native build.
- Checker step passed in run [41185690740](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41185690740). At the latest poll the ARM64 library build and artifact upload were still running; no final workflow success or new artifact is claimed yet.
- This is a static ABI drift check, not proof of iOS runtime linking or UnityFramework integration.


## Importable Unity iOS plugin packaging (2026-10-31)

- Added Unity PluginImporter metadata for an iPhoneOS ARM64-only static archive and metadata for the C ABI header.
- Added `scripts/package-spine-unity-ios-plugin.sh`. It checks the archive exists, verifies the ARM64 architecture with `lipo`, verifies all three exported C symbols with `nm`, and packages the archive, header, managed bridge, third-party license and import instructions into a Unity Assets folder layout. It also emits a SHA-256 checksum manifest.
- Added `azurearchive-spine-unity-ios-plugin` as a separate Actions artifact in the iOS ARM64 workflow. The packaging step is part of the same workflow after the host ABI smoke test.
- Packaging workflow [41185979412](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41185979412) was still running at this checkpoint; no plugin ZIP or artifact ID is claimed until upload completes.
- The package is a reusable converter plugin only. It is not a full AzureArchive Unity project, does not supply the Spine 4.2 C# runtime or game assets, and cannot produce an IPA by itself.


- First complete package workflow [41185950423](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41185950423) succeeded, including iOS ARM64 build, host C ABI smoke test, Unity plugin packaging and both artifact uploads.
- Unity plugin artifact ID: `13035873997` (1,842,358 bytes); static-library artifact ID: `13035859101` (1,938,910 bytes).
- Downloaded and inspected the plugin artifact ZIP. The nested Unity package contains the 8,195,192-byte static archive, iOS PluginImporter metadata, ABI header, managed bridge, README and upstream LICENSE. This confirms the packaging layout is present; it is not yet tested by importing into a Unity project.
- A follow-up hardening check was added to verify the archive's ARM64 slice and three exported symbols before packaging. The first attempt exposed incorrect `lipo -verify_arch` argument order; corrected in commit `23fa33f`. The corrected run [41186109443](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41186109443) was still in progress at this note.
