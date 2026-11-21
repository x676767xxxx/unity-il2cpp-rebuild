# Universal Launcher — Husk-based runtime integration

This branch pivots the prototype toward a real iPhone game launcher by using [Husk](https://github.com/Leviidev/Husk) as the application/runtime template.

## Product direction

- Husk owns the iOS app shell, game library, Android guest lifecycle, graphics/runtime integration and JIT coordination.
- The existing `UniversalLauncher` prototype contributes only reusable APK inspection and import code.
- The first real execution path to validate is Husk's QEMU-backed Android guest.
- Husk's Android Translation Layer is treated as experimental until an APK is demonstrably launched through it.
- AzureArchive Android 1.0 is the first target game for real-device compatibility testing.
- Windows EXE execution is a separate future engine project. ZIP/RAR/7Z archive support is not Windows execution.

## Upstream template

- [Husk source repository](https://github.com/Leviidev/Husk)
- [Husk XcodeGen project definition](https://github.com/Leviidev/Husk/blob/main/src/app/project.yml)
- [Husk build pipeline](https://github.com/Leviidev/Husk/blob/main/scripts/ci_build.sh)
- [Husk runtime/translation-layer status](https://github.com/Leviidev/Husk/blob/main/docs/04-translation-layer.md)
- [Husk JIT setup and methods](https://github.com/Leviidev/Husk/blob/main/docs/06-built-in-jit.md)
- [Husk licensing notes](https://github.com/Leviidev/Husk/blob/main/docs/01-licensing.md)

Read [HUSK_RUNTIME_ADAPTATION.md](HUSK_RUNTIME_ADAPTATION.md) before making integration changes. It records the inspected upstream files, runtime boundaries, build constraints and acceptance gates.

## AzureArchive target profile

- Package: `com.foxxlight.AzureArchive`
- Version: `1.0.0`
- Unity: `2023.2.22f1`
- Scripting backend: IL2CPP
- Native ABI: `arm64-v8a`
- Launcher activity: `com.unity3d.player.UnityPlayerActivity`
- Previously inspected manifest: min API 23, target API 30

These facts make the APK a useful ARM64 Unity test target; they do not prove it is compatible with Husk.

## Existing prototype code

The earlier prototype has a SwiftUI importer, APK ZIP/AXML inspection, ABI detection, metadata persistence and basic ZIP extraction. It does not execute APKs. Keep these modules isolated and port only what the Husk library needs; do not preserve the file-manager-first UI as the product architecture.

## Acceptance standard

A successful build, APK import, manifest parse, guest boot or splash screen is not a successful game launch. The first runtime milestone requires a reproducible physical-device launch to AzureArchive's login screen, with logs and memory/runtime measurements. Sustained gameplay and performance optimization come afterward.

## Build boundary

Husk's documented build pipeline requires macOS, Xcode and its native toolchain. A simulator build is not a substitute for a physical iPhone test. JIT must be verified on the actual target device and selected installation method.

Husk is GPL-2.0-or-later and links QEMU (GPLv2). Any derivative distribution must preserve applicable source, notices and licence obligations. Review every added dependency before vendoring it.

## Current status

This branch currently contains the integration plan and product-direction correction. It does **not** yet contain a merged Husk runtime, a new IPA, or a successful AzureArchive launch.
