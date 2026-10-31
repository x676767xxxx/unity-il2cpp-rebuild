# AzureArchive iOS port progress — successful ABI hardening CI

Date: 2026-10-31

## Latest CI
- Run: [41183828305](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41183828305)
- Result: success
- Job: ios-arm64 — success
- iPhoneOS ARM64 CMake configure/build: success
- Host-side C ABI smoke test: success
- Artifact upload: success
- Artifact ID: 13035881289
- Artifact name: azurearchive-spine-converter-ios-arm64
- ZIP size: 1,938,915 bytes
- SHA-256: `8b89fb16450d7ba9b28839ac15f7f4495855a38ee6b187b301c117496b319488`

## Test evidence
The job log confirms:
- `PASS: 4 invalid-input and output-parameter checks`
- `C ABI JSON -> SKEL -> SKEL round-trip passed (8910 -> 8910 bytes)`

The uploaded ZIP was downloaded and inspected. It contains:
- `build/ios-arm64/libazurearchive_spine_converter_ios.a`
- `external/SpineSkeletonDataConverter/include/azurearchive_spine_converter_abi.h`
- `external/SpineSkeletonDataConverter/LICENSE`

## Limitations
- The ABI functional tests execute on the macOS GitHub Actions host; this is not an iPhone device test.
- The artifact is an iOS ARM64 static library, not an IPA.
- Build log still has enum warnings for Spine features including `SLOT_ALPHA`, `BONE_INHERIT`, `PHYSICS_RESET`, and `AttachmentType_Point`.
- The repository still lacks a complete Unity project / Xcode app target for the AzureArchive Android 1.0 port.
- Actual game Spine resources have not yet been validated end-to-end.

## Next steps
1. Obtain/recover complete Unity project or full iOS Xcode project and compatible runtime sources.
2. Test converter with the game's actual Spine JSON/binary, atlas, and texture fixtures.
3. Implement or reject unsupported Spine features explicitly; avoid silent data loss.
4. Integrate the static library into the real Unity iOS runtime.
5. Build and test an app on iPhone, then package/sign IPA.
