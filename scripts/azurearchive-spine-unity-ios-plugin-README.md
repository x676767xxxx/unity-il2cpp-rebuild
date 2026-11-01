# AzureArchive Spine converter — Unity iOS plugin package

This package bundles the custom Spine skeleton-format converter for iOS device ARM64 and the managed C# P/Invoke bridge.

## Import

1. Unzip this package.
2. Copy the included Assets folder into the root of the matching Unity project's Assets folder.
3. Confirm Unity recognizes Assets/Plugins/iOS/libazurearchive_spine_converter_ios.a as an iOS-only ARM64 native plugin.
4. Build an iOS device target (not Simulator) and inspect the generated UnityFramework link command for these exported symbols:
   - _AzureArchiveSpineJsonToBinary
   - _AzureArchiveSpineConvertBuffer
   - _AzureArchiveSpineFreeBuffer
5. Run on a physical device before using the bridge in game code.

## Managed API

AzureArchive.UnityCompat.SpineConverterNative exposes:
- JsonToSpine42Binary(byte[] utf8Json)
- ConvertToSpine42Binary(byte[] skeletonData)

The bridge copies native output into managed memory and releases native buffers using the matching exported free function.

## Important boundaries

- This package supplies a converter plugin only. It does not contain Unity's project, the game's scenes/assets, a Spine 4.2 C# runtime, atlas texture loader, or animation renderer.
- The native converter was built for iPhoneOS ARM64 with an iOS 13.0 deployment target. It is not an iOS Simulator library.
- The host-side C ABI suite validates four invalid-input/output-parameter contracts and 25 upstream Spine 4.2 JSON fixtures through JSON → SKEL → SKEL conversion. This is broader converter coverage, but it is not an AzureArchive game-asset test or an on-device test.
- Spine 4.2-specific handled enum cases (`BONE_INHERIT`, `PHYSICS_RESET`, and `AttachmentType_Point`) are now explicit in the pinned-source patch and the latest iOS CI build passes. Legacy Spine 3.5–4.1 translation units still emit enum-switch warnings; those paths are not the selected 4.2 target and have deliberately not been modified. Validate real AzureArchive .skel samples, especially sequences, physics, alpha timelines, inheritance and point attachments.
- The included third-party LICENSE is preserved. Check its terms and the Spine runtime's separate licensing before distributing a game build.
