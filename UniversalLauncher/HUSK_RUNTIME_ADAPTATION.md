# Husk 3.4 runtime baseline and AzureArchive adaptation

## Pinned upstream template

- Upstream: https://github.com/Leviidev/Husk
- Release/tag: 3.4
- Exact commit: acfd7bcb6d1d2f6655a24e43d1d6f87cf142667a
- Integration uses a Git submodule at repository root: Husk/
- The submodule gitlink pins the exact commit above. Do not silently track upstream main.

The Husk/ directory is the actual upstream source checkout, not a copied UI mock or a Swift Package. Update it only by an intentional submodule commit change after checking release notes, source diffs and compatibility.

## Correct runtime model

Husk 3.4 has two Android execution routes:

1. Translation Layer / native runtime — preferred for games. The app's ARM64 native libraries run on Apple silicon in Husk's process with Android ABI/framework shims and engine drivers. No Android guest boot is required.
2. Full Android under QEMU — fallback. A LineageOS guest is booted for apps that the native Translation Layer cannot handle. The upstream release notes explicitly warn that full Android is slow and recommend the Translation Layer for games.

Do not describe the whole Translation Layer as “not implemented”. The older docs/04-translation-layer.md is a design/milestone document and still contains a historical “nothing opens yet” status. The 3.4 application source has since added the translation-layer-next runtime and Unity driver. Use the tagged source and actual launch path as the implementation evidence, while treating release compatibility claims as upstream-tested claims rather than proof for our specific APK/device.

## Source evidence inspected at the pinned tag

- src/app/project.yml compiles both src/translation-layer and src/translation-layer-next into the Husk app. It also embeds ANGLE, QEMU, MoltenVK, JIT helper and supporting frameworks.
- src/app/Husk/TranslationLayer.swift maps Unity APKs to the .unity native engine (Unity 6 is treated separately).
- src/app/Husk/TLUnityView.swift creates the game's Metal layer, prepares the runtime and calls husk_unity_launch(...) for Unity.
- src/translation-layer-next/husk-tl-unity-app.c dispatches the Unity engine and manages launch state, package metadata, crash logs and the runtime thread.
- src/translation-layer-next/husk-tl-unity.c implements UnityPlayer/JNI setup, loads Unity native libraries, invokes JNI_OnLoad, calls NativeLoader.load, starts UnityMain, and drives nativeRender.
- src/app/Husk/GameStatus.swift persists actual last-run outcomes (plays, crashed, failed), which can be used instead of a fake “launch successful” state.
- src/app/Husk/ApkMetadata.swift contains APK metadata and icon parsing; reuse it rather than maintaining a second parallel parser.

Useful upstream links:
- https://github.com/Leviidev/Husk/tree/3.4
- https://github.com/Leviidev/Husk/blob/3.4/src/app/project.yml
- https://github.com/Leviidev/Husk/blob/3.4/src/app/Husk/TranslationLayer.swift
- https://github.com/Leviidev/Husk/blob/3.4/src/app/Husk/TLUnityView.swift
- https://github.com/Leviidev/Husk/blob/3.4/src/translation-layer-next/husk-tl-unity-app.c
- https://github.com/Leviidev/Husk/blob/3.4/src/translation-layer-next/husk-tl-unity.c
- https://github.com/Leviidev/Husk/releases/tag/3.4

## AzureArchive target profile

- Package: com.foxxlight.AzureArchive
- Version: 1.0.0
- Unity: 2023.2.22f1
- Scripting backend: IL2CPP
- Native ABI: arm64-v8a
- Launcher: com.unity3d.player.UnityPlayerActivity
- Previously inspected manifest: min API 23, target API 30
- Target device: iPhone 14 Pro Max, iOS 26.5

This is a strong candidate for the Husk 3.4 Unity driver: it is an ARM64 Unity IL2CPP package, not an x86 or 32-bit-only APK. The upstream Unity driver directly interacts with UnityPlayer and IL2CPP. Compatibility is still unverified for this specific Unity patch version and game: Unity plugins, JNI methods, graphics extensions, Android services, memory/address-space layout and game-specific protections may block it.

## Existing AzureArchive test asset

The XR1 GitHub Release was checked directly. It contains the exact Android test package at `base.apk` (364,837,539 bytes), alongside the older iOS IPA and Windows ZIP. The existing `.github/workflows/inspect-azurearchive.yml` downloads this asset from the release. The APK is therefore already available to CI; do not ask the user to upload it again or commit the 365 MB binary into Git.

Release asset: https://github.com/x676767xxxx/unity-il2cpp-rebuild/releases/download/XR1/base.apk

## Required runtime path

APK import -> Husk APK metadata/icon parse -> native scan/report -> Unity engine classification -> TLUnityView -> husk_unity_launch -> UnityPlayer/JNI -> libmain/libunity/libil2cpp -> UnityMain/nativeRender

The first attempt should use Husk 3.4's native Translation Layer. Do not boot QEMU first unless the native path rejects the APK or logs identify an unsupported dependency.

## iOS 26.5 JIT strategy

Husk 3.4 documents:
- Automatic JIT method selection: StikDebug first if installed, otherwise TrollStore where applicable, otherwise built-in StikJIT.
- Built-in StikJIT requires iOS 26+, LocalDevVPN and a valid device pairing file. On iOS 27, Husk can pair directly from the phone; on iOS 26.5, use an existing/imported pairing file or StikDebug.
- “Play without JIT” can be available after importing the certificate used to install Husk; this is a separate route and must be tested per game.

Do not infer that JIT is active from VPN connected, pairing-file presence or a helper being installed. Verify Husk's own JIT status and capture the runtime log before testing the game.

## Build baseline

On macOS with Xcode and the upstream dependencies installed, use the pinned submodule's own full build:

    git submodule update --init --recursive
    cd Husk
    ./scripts/ci_build.sh

Expected output: Husk/build/Husk.ipa.

The upstream full build fetches and builds native dependencies including QEMU, ANGLE/virglrenderer, MoltenVK and the on-device pairing library, then packages the app. It can take hours on a clean checkout. Do not replace it with a simulator-only build or claim success from Xcode project generation alone.

Husk 3.4 requires macOS/Xcode and tools including Meson, Ninja, pkg-config, Python 3, XcodeGen, qemu-img and Rust with the aarch64-apple-ios target. The current environment has not run this macOS build; no IPA has been produced here.

## First device test protocol

1. Build/install the pinned Husk 3.4 baseline unchanged.
2. Verify JIT method and status on the iPhone 14 Pro Max / iOS 26.5.
3. Import the exact AzureArchive Android 1.0 APK.
4. Save Husk's scan report. Confirm it identifies Unity and arm64-v8a and records every native library, dependency and unsupported flag.
5. Start through the native Translation Layer, not QEMU.
6. Preserve unity-run.log, Husk crash report and iOS jetsam report if it exits.
7. Record checkpoints separately: runtime accepted APK; UnityPlayer initialized; first rendered frame; game logo; login screen; interactive scene.
8. Measure cold start, memory/VM region use, frame pacing, audio, touch and repeat launch.

A parser pass, successful import, husk_unity_launch returning true, Unity splash or a few frames is not a pass. The first meaningful acceptance gate is reproducibly reaching the login screen; sustained gameplay is a later gate.

## Integration strategy for this repository

- The Husk/ submodule is the runtime/application base and owns its existing library, UI, JIT and launch lifecycle.
- The old UniversalLauncher prototype is not the application base. Reuse only any parser/tests that are demonstrably more complete than Husk's existing code.
- Do not duplicate ApkMetadata, TranslationLayerStore, TLReport, TLUnityView, GameStatusStore or the C Unity driver.
- Add AzureArchive-specific diagnostics and test fixtures in a separate integration layer, without changing upstream runtime behavior until a baseline reproduces.
- Keep the old progress/azurearchive-ios-port-20261031 branch isolated. Its iOS resource pack and Spine/IL2CPP port work are separate from Android APK runtime execution.
- Windows EXE execution remains a separate engine project. Husk's 3.4 release notes say Winlator containers do not run; archive handling is not Windows execution.

## Licensing and distribution

Husk is GPL-2.0-or-later and links QEMU (GPLv2); follow the upstream licence and corresponding-source requirements for any distributed derivative. Review every added dependency. Do not assume an App Store distribution route; Husk's own documentation says it is for sideloading.

## Verified state of this integration branch

- A dedicated branch exists: integration/husk-runtime-template.
- A real Git submodule Husk/ is pinned to upstream tag 3.4 commit acfd7bcb6d1d2f6655a24e43d1d6f87cf142667a.
- The README and this document describe the actual 3.4 native Unity launch route.
- No upstream runtime files have been modified.
- No macOS build, IPA creation, device install or AzureArchive launch has yet been performed by this workflow.

## First real APK scan with Husk 3.4

The pinned Husk scanner was compiled from its own C sources on GitHub Actions and run against the existing XR1 release asset (364,837,539 bytes). Run: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/44912811502

Observed report:
- Scanner parse: ok = true
- Verdict: native
- Reported engine: Unity 6
- ABIs: arm64-v8a, armeabi-v7a, x86_64, x86
- 16 native library entries total; 8 arm64 libraries
- arm64 libil2cpp.so: 120,315,032 bytes
- arm64 libunity.so: 19,229,160 bytes
- arm64 libSpineSkeletonDataConverterLib.so: 8,293,128 bytes
- arm64 lib_burst_generated.so: 130,104 bytes; the old scanner reports one 16 KiB page with executable/writable conflict (status work). Husk's newer loader has mixed-page writable-copy handling, but this must be confirmed in the device runtime log.

### Engine classification discrepancy — must resolve before selecting the launch route

Earlier project evidence recorded Unity 2023.2.22f1 for AzureArchive. The Husk 3.4 scanner currently reports “Unity 6”. In the pinned source, TranslationLayer.swift maps engine == "Unity 6" to the Java runtime path, while other Unity versions map to the dedicated native Unity driver. The scanner's Unity 6 heuristic checks for the string UnityPlayerForActivityOrService inside libunity.so. Therefore this is not a cosmetic label: it changes the launch path.

The follow-up workflow extracts libunity.so and globalgamemanagers to record the exact marker and embedded Unity version. Until that evidence is checked, do not hardcode either .java or .unity for AzureArchive. The native verdict and ARM64 libraries are positive evidence, but the correct Unity driver is still an unresolved gate.

This scan is static compatibility analysis only. It does not prove that the iOS device loader maps every library, that JIT works, or that AzureArchive reaches its login screen.


## Confirmed Unity version and scanner false-positive

The follow-up APK evidence run (GitHub Actions run 44913060578) read the UnityFS header from assets/bin/Data/data.unity3d:

- Bundle header: UnityFS, player version 5.x.x, engine version 2023.2.22f1.
- The APK DEX contains both UnityPlayerActivity and UnityPlayerForActivityOrService class names.
- libunity.so also contains UnityPlayerForActivityOrService.

Therefore the previous Husk scanner label “Unity 6” is a false positive for this APK. The scanner currently marks Unity 6 merely when that class-name string occurs in libunity.so; the string is also present in this Unity 2023.2.22f1 APK. Husk's Swift mapping then routes “Unity 6” to the Java driver, whereas other Unity versions use the dedicated Unity driver.

**Required fix before the first device launch:** make engine classification version-aware using the UnityFS engine-version header (or equivalent authoritative build metadata), not a bare class-name string. For this APK the expected classification is Unity (IL2CPP), with the dedicated native Unity driver. Keep the raw scanner output in the report so the heuristic discrepancy remains auditable. Do not patch the upstream submodule in place; implement and test an isolated integration patch against the pinned commit first.

This is now a concrete integration issue, not an unknown Unity version. The actual Unity version is verified as 2023.2.22f1.


## Version-aware correction implemented and CI-verified

The isolated overlay is now implemented in scripts/apply_husk_3_4_unity_version_fix.py. It adds a bounded ZIP-prefix reader and uses the UnityFS header's engine-version field when available, avoiding decompression of the full 168 MiB data.unity3d file. The old UnityPlayerForActivityOrService string remains a fallback only when no UnityFS version is available.

GitHub Actions run 44913246560 successfully:
- applied the overlay to the pinned Husk 3.4 submodule;
- compiled Husk's own APK scanner with warnings treated as errors;
- scanned the exact AzureArchive release APK;
- asserted the corrected engine result is Unity (IL2CPP);
- retained the raw Unity marker and UnityFS header as diagnostic evidence.

The resulting classification selects Husk's dedicated native Unity driver (.unity), not the Unity 6 Java path. This is a static scanner/driver-selection validation, not a device launch.

## Reproducible full app build entry

The wrapper scripts/build_husk_azurearchive_ipa.sh verifies the exact submodule pin, applies the same overlay, then runs Husk's complete scripts/ci_build.sh pipeline.

Manual command on a prepared Mac:

    git submodule update --init --recursive
    bash scripts/build_husk_azurearchive_ipa.sh

A manual-only macOS Actions workflow is also available at .github/workflows/build-husk-ipa.yml. Its confirmation input defaults to false because the full build can take hours and consume macOS Actions minutes. It has not been triggered. No IPA has yet been produced.
