# Husk runtime template adaptation plan

Status: integration branch created; this document records the verified template boundary and the first implementation sequence. It does not claim that the runtime has been merged or that AzureArchive launches.

## Product decision

Use **Husk as the application and runtime template**, not as a Swift package to bolt onto the existing file-import prototype.

Keep the old `UniversalLauncher` prototype as a source of reusable APK-inspection code only. Do not continue its file-manager-first UI. Keep the AzureArchive iOS-port branch separate.

## Verified upstream template

Upstream: https://github.com/Leviidev/Husk

Files inspected on `main`:

- `README.md`: Husk is an iOS app for Android apps/games; documents sideloading and JIT requirements.
- `src/app/project.yml`: XcodeGen project definition; iOS deployment target 15.5; arm64; app, JIT helper and download widget targets; links the QEMU runtime, ANGLE and supporting frameworks.
- `src/app/Husk/ContentView.swift`: existing app shell and tabs (Home, Library, Store, Settings), guest-screen routing, incoming-file handling, JIT coordinator and runtime state.
- `scripts/ci_build.sh`: full macOS build pipeline (fetch sources, native dependencies, ANGLE, QEMU, Husk integration, guest kernel/firmware, MoltenVK, pairing library, IPA packaging).
- `scripts/package_ipa.sh`: regenerates Xcode project from project.yml and packages/validates IPA.
- `docs/01-licensing.md`: GPL/QEMU and bundled-component obligations.
- `docs/04-translation-layer.md`: Android Translation Layer is experimental; the document explicitly says the experimental UI can inspect apps but does not yet open them.
- `docs/06-built-in-jit.md`: JIT choices and requirements, including StikDebug, TrollStore and Built-in StikJIT.

Important status distinction: Husk's established fallback is a full Android guest under QEMU. The translation-layer path is experimental and must not be treated as a proven APK launcher until the upstream source and tests demonstrate that an APK actually opens through it. Some high-level project descriptions mention Unity support, but the detailed translation-layer status document is the safer acceptance criterion.

## Target-device and APK profile

Target device from the project requirements: iPhone 14 Pro Max, iOS 26.5.

AzureArchive Android package profile previously inspected:

- package: `com.foxxlight.AzureArchive`
- version: `1.0.0`
- Unity: `2023.2.22f1`
- scripting backend: IL2CPP
- native ABI: `arm64-v8a`
- launcher: `com.unity3d.player.UnityPlayerActivity`
- Android manifest: min API 23, target API 30

This is a suitable first compatibility test for Husk's Android guest path because the package contains ARM64 native libraries. It is not proof of compatibility: Unity native graphics, JNI, Android framework APIs, memory pressure, audio and JIT behavior still need on-device validation.

## Integration architecture

1. **Husk app shell remains the owner of the runtime.** Retain its library, incoming-file routing, guest lifecycle, logging, settings and JIT coordination.
2. **Game catalogue becomes the primary product surface.** Reuse Husk's Library patterns; add per-game metadata and compatibility state only where needed.
3. **APK inspection is an import/diagnostic service.** Port only the useful, tested AXML/ABI/Unity detection logic from the old prototype. It must not become a second file manager.
4. **Runtime selection is explicit and truthful.** First path: Husk's QEMU Android guest. Translation Layer may appear as experimental/unsupported until it has a demonstrated launch path. Never display “Running” merely because an APK was imported or parsed.
5. **AzureArchive is the first real-game acceptance test.** Measure cold start, time to first frame, login-screen reachability, sustained frame rate, memory use, audio and repeat launches.
6. **Windows EXE execution is out of scope for this Android runtime milestone.** Archive extraction is not Windows execution.

## Build and integration constraints

- Build the upstream app using its own `src/app/project.yml` and `scripts/ci_build.sh` pipeline on macOS with Xcode and the documented native toolchain. Do not transplant only ContentView.swift or add Husk as a Swift Package.
- Preserve the upstream build scripts and native dependency versions initially. Avoid changing QEMU, ANGLE, MoltenVK or JIT internals while establishing the baseline.
- Check upstream changes before copying or patching files; prefer a clean fork/upstream relationship so future runtime fixes can be merged.
- Husk is GPL-2.0-or-later and links QEMU (GPLv2). Any distributed combined build must comply with the applicable GPL source and notice requirements. Review every additional dependency's licence before vendoring it.
- JIT is a runtime requirement for many workloads. On iOS 26.5, verify the selected JIT method on the actual phone; do not assume that the presence of StikDebug or a pairing file means JIT is active.
- A simulator build is not evidence of runtime success. The acceptance device must be the physical iPhone.

## First implementation milestones

### M0 — Establish a reproducible Husk baseline
- Pin the exact upstream Husk commit/release used as the template.
- Build an unmodified Husk IPA from a clean macOS checkout.
- Install it on the iPhone 14 Pro Max and verify JIT status.
- Launch one known-compatible ARM64 Android app and record logs.

Exit criteria: clean build, install, JIT confirmation and a known-good guest launch. No custom launcher changes before this gate.

### M1 — AzureArchive compatibility test
- Import the exact AzureArchive Android 1.0 APK into unmodified Husk.
- Capture APK inspection output, guest/runtime logs and crash reports.
- Test launch to Unity splash, login screen and interactive scene.
- Record memory, frame pacing, audio and repeatability.

Exit criteria: reproducible launch to login screen at minimum; otherwise a specific blocking dependency/error is identified.

### M2 — Bring the existing launcher value into Husk
- Port AXML parser and ABI/Unity metadata inspection as a small isolated module.
- Add automatic mode recommendation with evidence and a reason string.
- Keep Husk's runtime lifecycle and JIT coordinator authoritative.
- Add a clear unsupported state for APKs that cannot launch.

Exit criteria: import -> inspect -> choose runtime -> actual runtime launch; no duplicate runtime implementation.

### M3 — Optimize only after the first frame works
- Profile CPU translation, GPU path, memory and loading.
- Reduce background services and duplicate asset copies where safe.
- Compare QEMU guest performance with any future proven translation-layer path.

## Known blockers / do not assume

- The detailed upstream translation-layer document currently says no app opens through that experimental path.
- QEMU full-system emulation may be slower than native execution; do not promise near-native speed for Unity games.
- APK manifest parsing does not establish compatibility.
- The old iOS 0.7.9.2 resource archive and IL2CPP reverse-engineering reports are not substitutes for the Android APK's runtime dependencies.
- No IPA, real-device launch or AzureArchive compatibility result is claimed by this plan.
