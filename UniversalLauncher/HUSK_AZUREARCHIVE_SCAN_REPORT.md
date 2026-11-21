# AzureArchive Android 1.0 — Husk 3.4 static compatibility scan

Date: 2026-11-21
Target APK: XR1 release asset base.apk
APK size: 364,837,539 bytes
Husk upstream pin: 3.4 / acfd7bcb6d1d2f6655a24e43d1d6f87cf142667a
Scan workflow: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/44913060578
Artifact: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/44913060578/artifacts/14710681927

## Results

- Husk scanner parsed the APK successfully: ok=true.
- Scanner verdict: native.
- Scanner's raw engine label: Unity 6 (false positive; see below).
- APK ABIs: arm64-v8a, armeabi-v7a, x86_64, x86.
- 16 native library entries total; 8 arm64 entries.

Largest relevant arm64 libraries:
- libil2cpp.so — 120,315,032 bytes.
- libunity.so — 19,229,160 bytes.
- libSpineSkeletonDataConverterLib.so — 8,293,128 bytes.
- libc++_shared.so — 6,934,032 bytes.

One legacy scanner warning:
- lib_burst_generated.so — 130,104 bytes.
- One 16 KiB page has executable and writable content sharing the same page.
- The Husk 3.4 next-generation loader contains mixed-page writable-copy handling. Treat this as a runtime log checkpoint, not a proven launch blocker.

## Authoritative Unity version

The APK's assets/bin/Data/data.unity3d starts with a UnityFS header whose engine-version string is **2023.2.22f1**. This independently confirms the earlier project/runtime information.

The DEX contains both:
- UnityPlayerActivity
- UnityPlayerForActivityOrService

The libunity.so binary also contains UnityPlayerForActivityOrService. Husk 3.4's current scanner treats that string alone as proof of Unity 6. This is incorrect for this APK: its UnityFS header says 2023.2.22f1.

## Required integration correction

Husk's TranslationLayer.swift maps the scanner label “Unity 6” to the Java runtime path, while other Unity versions map to the dedicated native Unity driver.

Before the first device launch, implement an isolated, version-aware correction so this APK is classified as Unity (IL2CPP) and routed to the dedicated Unity driver. Use the UnityFS engine-version header rather than the mere presence of UnityPlayerForActivityOrService. Preserve the raw scanner label and corrected label in diagnostics.

## What this proves / does not prove

Proves:
- The exact release APK is readable by Husk's scanner.
- It has native ARM64 libraries and is classified as a native-runtime candidate.
- It is Unity 2023.2.22f1, not Unity 6.
- The primary target architecture and Unity driver family are identified.

Does not prove:
- iOS loader can map every library on iPhone 14 Pro Max.
- JIT is active or sufficient on iOS 26.5.
- Unity's native initialization, JNI, graphics, audio, storage or network dependencies all work.
- The game reaches logo, login screen or gameplay.

Next acceptance gate: correct the Unity-version classification, build/install the pinned Husk 3.4 app on the physical iPhone, then capture unity-run.log and record the first-frame/login checkpoints.
