# iOS 0.7.9.2 runtime baseline: verified findings (2026-10-31)

## Evidence
Workflow: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41173880844
Artifact: azurearchive-ios-runtime-baseline (artifact ID 13032277536)
The artifact was downloaded and its report/inventory inspected.

## Confirmed facts
- `Payload/AzureArchive.app/Frameworks/UnityFramework.framework/UnityFramework` exists; `file` identifies it as a Mach-O universal container with one arm64 architecture, dynamically linked shared library. Size: 82,150,064 bytes.
- The UnityFramework string scan contains `2023.2.20f1c1` and `2023.2.20f1c1 (881fe566f400)`.
- UnityFramework strings include `Texture2D`, `UnityEngine.ImageConversion::LoadImage_Injected`, and the ImageConversionModule reference. This is evidence that the old iOS Unity runtime contains texture/image-loading implementation references, not proof that new managed code can call a compatible public API or that all formats are accepted.
- The old app contains Spine-related metadata/type strings, including Spine runtime namespaces, and a `spine-unity.dll` string. This suggests managed Spine integration was included in the old build. It does not prove a separately reusable native Spine runtime exists or that the exact Spine version/API matches Android 1.0.
- The app includes Addressables data: `Data/Raw/aa/catalog.json` (23,911,858 bytes), settings/link files, Unity serialized assets, .resS and .resource files. `sharedassets0.assets` is 184,465,336 bytes. The inventory scan found no separately named .skel/.atlas files; Spine assets may be embedded in bundles/serialized files or downloaded through Addressables.
- The UnityFramework SHA256 is `b805eab664f9a0a306cd201aa597db6c619dea76aa1ca64706aede46f14c7ae4`; global-metadata.dat SHA256 is `2f474269ec36ca6cb0eb857cc5225f67e5baf3cda11de3ce1913383f493c87cc`.

## Inspection limitations (do not over-interpret)
- The `plutil` command was unavailable on the Ubuntu runner; Info.plist parsing failed. Re-run using Python plistlib or install a compatible plist parser.
- The native-components scan used `find -type f -name '*.framework'`, which cannot match framework directories. The framework-files list independently confirms UnityFramework.framework exists. Correct the scan to enumerate directories as well as dylibs.
- String scans and metadata strings are not callable-API evidence. A symbol name or managed type string does not establish a callable method, correct signature, runtime registration, or binary compatibility.
- The artifact contains no separate Spine framework/dylib and no obvious loose .skel/.atlas files. This is not proof that Spine code/assets are absent: managed assemblies and Addressables bundles need deeper extraction.
- No iOS 1.0 Unity project, iOS 1.0 IL2CPP build, source project, or successful IPA build was produced by this inspection.

## Porting implications
1. Keep the old iOS UnityFramework as a candidate runtime baseline only; verify its Unity version and required managed/native interfaces against the 1.0 Android/Windows evidence.
2. Implement resource lookup as an app-level compatibility layer only after extracting and comparing Android/Windows Addressables catalogs, bundle paths, and resource naming/override rules with old iOS catalog data.
3. For image loading, first test supported formats and actual LoadImage call paths in a minimal Unity project using the same Unity version. Do not infer format support from the presence of LoadImage_Injected.
4. For Spine, extract the managed Spine assembly/version and real serialized assets from both old iOS and Android 1.0. Compare assembly identity, API signatures, skeleton/atlas formats, and runtime behavior before choosing reuse vs reimplementation.
5. The preview cache/coroutine path depends on Unity main-thread object lifetime and RenderTexture behavior; it cannot be validated by metadata/string inspection alone.

## Current status
Analysis evidence only. No production compatibility layer, compilable iOS port, signed IPA, or verified iOS 1.0 build exists.
