# AzureArchive IL2CPP comparison — 2026-10-31

Inputs: AzureArchive iOS 0.7.9.2 IPA, Android APK, Windows 1.0.1 ZIP from release XR1.
Analysis: Il2CppDumper v6.7.46; metadata version 29 (tool adjusted parser to 29.1).

## Extraction status

- iOS ARM64 Mach-O UnityFramework: dump succeeded.
- Android ARM64 libil2cpp.so: dump succeeded. Dumper printed a protection warning ("This file may be protected") but completed Dumping, struct generation, and dummy DLL generation.
- Windows x64 GameAssembly.dll: dump succeeded.
- Full outputs are in GitHub Actions artifact `azurearchive-ios-il2cpp-comparison` for run 41169040171.

## Verified application-level differences

Counts below are method declaration counts in each selected class from dump.cs, not counts of executable implementations.

| Type | iOS 0.7.9.2 | Android 1.0 | Windows 1.0.1 | Notes |
|---|---:|---:|---:|---|
| ScenarioResourceManager public methods | 57 | 65 | 65 | Android/Windows expose 8 more public methods than iOS |
| Character public methods | 23 | 24 | 27 | Platform-specific additions differ |
| CharacterManager public methods | 26 | 26 | 26 | Same public method count |
| AuthoringResourceCatalog | absent | present (77 methods total) | present (75 methods total) | Present in Android/Windows dump, absent in iOS dump |
| CharacterCurve / CharacterCurvePlayer | absent | absent | present | Appears Windows-only in these inputs |

Examples of public ScenarioResourceManager methods present in Android 1.0 and Windows 1.0.1 but absent in iOS 0.7.9.2:
- ResolveBackground
- EnumerateBackgrounds
- TryGetBackgroundOverridePath
- GetBackgroundTexture
- AcquireBackgroundSpine
- GetCustomBgEffect
- GetBgEffectChoices
- GetBackgroundPreviewAsync
- InitializeAuthoringResources
- GetPersistentFilePath (extension differs: .aap2 on Android vs .aap on iOS)

Character method differences:
- Android has ApplyDisplayRank(int rank), absent from iOS.
- Windows has CharacterCurvePlayer accessors and PlayCurve(CharacterCurve), absent from Android and iOS.

## Interpretation

1. The iOS 0.7.9.2 code is not a drop-in match for the Android/Windows 1.0 code. Some resource-management and authoring-facing methods/types are absent from the iOS metadata.
2. Windows-only CharacterCurve classes should not automatically be treated as required mobile 1.0 features; they are absent from Android too and may be desktop-specific.
3. The Android and Windows 1.0 application logic is closer to each other than either is to the older iOS build in the inspected resource-management area.
4. Matching metadata version 29 does not make native binaries interchangeable.

## Important limitations

- Il2CppDumper output contains metadata-derived declarations, fields, method signatures, and native addresses. DummyDll assemblies are stubs; they do not contain recovered original C# method bodies.
- The automated type-comparison.txt generated in the run has a parser bug and reports zero types; do not use it. The findings above were checked directly against the three dump.cs files.
- No Unity source project, original C# source, or iOS 1.0 native binary has been provided.
- Android ELF and Windows PE native binaries cannot be inserted into an iOS IPA. A functional iOS 1.0 build still requires an iOS-compatible implementation of the missing code, Unity/iOS build integration, testing, and signing.

## Recommended next step

Produce a targeted method/RVA map for the missing or changed methods in ScenarioResourceManager, Character, and related resource override classes. Then inspect whether their dependencies can be reconstructed from metadata and call references. This is analysis work only; it does not itself produce a buildable iOS binary.
