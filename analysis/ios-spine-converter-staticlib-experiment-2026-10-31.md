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
