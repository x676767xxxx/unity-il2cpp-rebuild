# Unity runtime version and native ABI comparison (2026-10-31)

## Run and artifact
- Workflow: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41174745490
- Job: compare, completed successfully.
- Artifact: azurearchive-unity-runtime-abi (ID 13031554022; SHA256 487b8203992e4ac77866cfcb3efecd3f41ce2e647b5e20084e7f44be87f46e5c).
- Artifact was downloaded and inspected. It contains runtime-abi-comparison.txt.

## Confirmed Unity versions
- Old iOS 0.7.9.2 UnityFramework contains version string 2023.2.20f1c1.
- Android 1.0 libunity.so contains version string 2023.2.22f1 in all packaged ABI variants.
- Windows 1.0.1 UnityPlayer.dll contains version string 2023.2.22f1.
- Thus Android/Windows are on Unity 2023.2.22f1 while old iOS is 2023.2.20f1c1. This is a patch-level engine mismatch, not evidence that the iOS binary cannot run the game logic. It does mean native engine ABI and generated IL2CPP runtime compatibility must be verified; do not assume the 20f1c1 UnityFramework is interchangeable with 22f1.

## Native architecture evidence
- Old iOS UnityFramework is a Mach-O universal container with one arm64 architecture (64-bit arm64 dynamically linked shared library), 82,150,064 bytes.
- Android package includes ARM64 AArch64, ARMv7, x86 and x86_64 ELF variants. ARM64 libil2cpp.so is 120,315,032 bytes; ARM64 libunity.so is 19,229,160 bytes.
- Windows GameAssembly.dll is PE32+ x86-64, 81,656,320 bytes; UnityPlayer.dll is PE32+ x86-64, 30,741,240 bytes.
- These are distinct native formats/ABIs. Android and Windows machine code is not reusable in iOS.

## Important inspection caveat
- The workflow attempted readelf -h on the iOS Mach-O file. readelf correctly rejected the non-ELF file; the workflow had || true, so the job succeeded. Architecture was still identified by the file utility. Future workflow revision should use llvm-objdump --macho or lipo -info for iOS rather than readelf.
- Unity version strings were found in native engine binaries. Strings are useful evidence of the embedded engine version but are not a full proof of every package/editor setting.

## Porting implications
1. The old iOS runtime is close in Unity version (2023.2.20f1c1 vs 2023.2.22f1), so it remains a plausible baseline for a narrow compatibility-layer approach. This is more encouraging than a major Unity-version gap.
2. It is not safe to replace UnityFramework or patch it with Android native code. Any new IL2CPP code must be compiled for arm64 iOS against matching Unity headers/libraries and a consistent Unity build pipeline.
3. The biggest functional mismatch remains the Android 1.0 game-level additions (ScenarioResourceManager background APIs, RuntimeSpineLoader/RuntimeSpineAsset and newer Spine API surface), not the two-patch Unity version gap by itself.
4. Need compare the iOS/Android Unity package/plugin inventories and determine whether required Addressables/Spine shaders/materials and platform-native plugins exist in the old iOS app. The existing iOS UnityFramework's ImageConversion strings are encouraging but actual PNG/JPEG decode and texture lifecycle still require runtime tests.

## Delivery status
- Analysis/report only.
- No compilable iOS compatibility-layer source has been produced.
- No Unity iOS project or Xcode/macOS build host is present in this workflow.
- No iOS 1.0 IPA has been built, signed or device-verified.
