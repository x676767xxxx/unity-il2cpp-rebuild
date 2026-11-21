# Universal Launcher — Husk 3.4 based

This branch now uses the actual upstream **Husk 3.4 source tree as a pinned Git submodule**. Husk owns the iOS application, game library, Android runtime, graphics integration, JIT coordination and game lifecycle. The old UniversalLauncher prototype is only a source of optional APK-inspection utilities, not the app foundation.

## Pinned runtime

- Upstream: https://github.com/Leviidev/Husk
- Release: 3.4
- Commit: acfd7bcb6d1d2f6655a24e43d1d6f87cf142667a
- Submodule path: Husk/

Initialize it with:

    git submodule update --init --recursive

## Runtime choice

- **Primary for games:** Husk's native Android Translation Layer. ARM64 game libraries run on Apple silicon with Android compatibility shims; no Android guest boot.
- **Fallback:** full Android under QEMU when the native route cannot run an app.

Husk 3.4 includes a native Unity driver. Its app classifies Unity APKs and routes them through TLUnityView to husk_unity_launch. This is the route to test AzureArchive first.

## First target: AzureArchive Android 1.0

com.foxxlight.AzureArchive, Unity 2023.2.22f1, IL2CPP, arm64-v8a, UnityPlayerActivity. This is a promising match for Husk's native Unity driver, but the exact APK is not yet proven compatible.

Target device: iPhone 14 Pro Max / iOS 26.5.

## Build

Husk requires macOS/Xcode and native dependencies. From the repository root:

    git submodule update --init --recursive
    cd Husk
    ./scripts/ci_build.sh

This full clean build can take hours and produces Husk/build/Husk.ipa. A simulator build is not a substitute for a physical-device runtime test.

## Acceptance

Importing/parsing the APK is not running it. Track each checkpoint: native runtime accepted APK, UnityPlayer initialized, first frame, logo, login screen, interactive scene. Preserve Unity logs and crash/jetsam reports. The first meaningful pass is a reproducible login-screen launch.

See HUSK_RUNTIME_ADAPTATION.md for source evidence, JIT requirements, build procedure, device test protocol and licensing.

## Scope

Windows EXE execution is separate. Husk 3.4's release notes say Winlator containers do not run; archive support does not equal Windows game execution.

## Current verified state

The upstream Husk 3.4 source is pinned as a submodule. This branch has not yet built an IPA, installed it on the iPhone or launched AzureArchive. No such success is claimed.
