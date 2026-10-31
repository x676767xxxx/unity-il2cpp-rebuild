# AzureArchive iOS port progress — 2026-10-31

## Scope
Continue the Android 1.0 to iOS port investigation. This checkpoint records only verified repository and GitHub Actions evidence. It does not claim that a complete Unity project or installable IPA exists.

## Latest verified CI state
- Workflow: `.github/workflows/ios-spine-converter-staticlib.yml`
- Run: [41181332226](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41181332226)
- Run conclusion: `success`
- Job: `ios-arm64` — `success`
- Successful steps include iPhoneOS ARM64 configure/build, host-side C ABI conversion test, and artifact upload.
- Artifact: `azurearchive-spine-converter-ios-arm64`
- Artifact ID: `13035471104`
- Artifact size: 1,938,917 bytes (ZIP)
- SHA-256 of uploaded ZIP (from Actions log): `059b0dfc9e7bf6ec28d560c214cdf4435f9eeb5cb6337b7fe8f4245e0340eb9f`
- Test log: `C ABI JSON -> SKEL -> SKEL round-trip passed (8910 -> 8910 bytes)`

## Important validation boundary
The workflow's C ABI smoke-test step configures a separate host build directory (`build/host-abi`) and links that host-built static library. It verifies the converter's C ABI and JSON-to-SKEL round-trip on the macOS runner, but it is not an iPhone device test and does not execute the iPhoneOS archive on a device. The iPhoneOS ARM64 archive is separately compiled and its exported symbols are checked with `nm`.

## Remaining converter compatibility risks
The successful build log still reports unhandled enum warnings in the pinned converter source, including:
- Spine 4.2 writer: `AttachmentType_Point`
- Spine 4.2 reader/writer: `BONE_INHERIT`
- Spine 4.2 reader: `PHYSICS_RESET`
- Older Spine writer versions: `SLOT_ALPHA`

The smoke test uses the upstream `data/42/alien/export/alien-ess.json` fixture. It does not prove support for every Spine feature or the game's actual background skeletons/atlases. These warnings must be resolved or shown irrelevant by testing the exact game assets before claiming runtime compatibility.

## Unity project / IPA status
The current repository tree contains analysis documents, small compatibility libraries, tests, and build scripts. It does not contain a complete Unity project layout (`Assets/`, `Packages/manifest.json`, `ProjectSettings/`) or an Xcode application workspace/project capable of producing the full game. The current workflow produces only a converter static library plus ABI header and license; it has no UnityFramework integration, app bundle, signing, or IPA packaging stage.

Therefore:
- Converter static library: built successfully for iPhoneOS ARM64.
- Host-side ABI smoke test: passed.
- iPhone hardware/runtime test: not performed.
- Full AzureArchive Android 1.0 iOS port: not complete.
- Installable IPA: not produced.

## Next engineering actions
1. Obtain or recover a legally usable Unity project/export or a complete iOS Xcode project with compatible UnityFramework/runtime sources. Do not substitute Windows PE code for iOS Mach-O code.
2. Extract representative Android 1.0 Spine JSON/binary, atlas and texture assets from the authorized game package, then run the converter against those exact fixtures.
3. Add fixtures covering point attachments, bone inheritance modes, physics reset timelines, slot alpha and any sequence/path constraints present in the game. Fix converter support or explicitly reject unsupported data rather than silently emitting incorrect SKEL.
4. Add an iOS-target validation step that verifies the archive architecture and links it into a minimal iOS test app; distinguish this from host-side tests.
5. Integrate the converter into the actual Unity/iOS background loading path only after the project and runtime APIs are available.
6. Build, sign as permitted, install, and test on the target iPhone; only then report an IPA as complete.

## Commit policy
This checkpoint is committed on the working branch `progress/azurearchive-ios-port-20261031`. Future progress checkpoints should be committed to this branch, leaving `main` untouched unless explicitly requested.
