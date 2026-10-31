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


## Controlled implementation checkpoint — duplicate-source guard and remaining gates

Latest branch head at checkpoint: `06769338ac4e11982f334877bcd986535ca32b34`.

### Change-control rule
- Keep all work on `progress/azurearchive-ios-port-20261031`; do not modify `main`.
- Before adding a file, inspect the branch tree for the exact path. If it exists, update it using its current blob SHA; never call create-file on an existing path.
- Prefer extending the existing implementation/specification files. Add a new file only when it represents a genuinely new source module, test, or build asset.
- A new CI preflight, `scripts/check-project-source-includes.py`, checks every explicit C# `Compile Include` in the three current project files for missing paths, duplicate resolved source paths, and duplicate `Link` names. It is wired into the existing compatibility workflow; it does not create a second build workflow.
- Existing successful compatibility tests remain limited to .NET and isolated Unity API stubs. They are not Unity/Xcode/device validation.

### Exact remaining work, in dependency order
1. **Acquire a buildable project base (hard blocker).** Obtain the matching Unity project/source export or a complete reproducible Xcode project/UnityFramework integration source for the target app. The current branch has no `Assets/`, `Packages/`, `ProjectSettings/`, `.xcodeproj`, or `.xcworkspace`; without this, a full app build cannot be run.
2. **Validate exact runtime/API versions.** Import the compatibility sources into that project and compile against the actual Unity 2023.2.20f1c1 and Addressables assemblies. Confirm the bootstrap is compiled into the player and runs before the game's first catalog-backed load. Verify it chains/restores the existing transform without changing unrelated IDs.
3. **Complete static-background path first.** Implement the missing Android 1.0 resource resolution/override semantics and static texture load path using verified IL and the existing iOS APIs. Add tests for local/global override precedence, missing files, path traversal, malformed image data, texture ownership, and repeat loads.
4. **Validate real resource fixtures.** Supply representative Android 1.0 background metadata, image files, Addressables catalog/bundles, and game Spine JSON/binary/atlas/texture assets. Run the converter against these exact assets; resolve or explicitly reject the observed Spine enum/features (point attachments, bone inheritance, physics reset, slot alpha) rather than silently dropping them.
5. **Spine runtime decision.** Verify whether the target iOS Unity runtime contains a compatible Spine 4.2 managed/native runtime. The current iOS static converter archive and P/Invoke wrapper are not sufficient by themselves. If runtime compatibility is absent, port/link the matching runtime before enabling Spine backgrounds; otherwise defer Spine and keep the first milestone static-only.
6. **Build and device validation.** Link the native converter archive only if Spine is enabled; build the complete iOS app with the matching Xcode toolchain; verify bundle ID, entitlements, signing, install, startup, catalog update, static backgrounds, and (if enabled) Spine backgrounds on iPhone 13.
7. **Package release artifact.** Produce and inspect the signed IPA only after the preceding checks pass. Record its SHA-256, artifact URL, build commit, and device-test results.

### Completion definition
The port is not complete until a signed installable IPA is produced from the real project and successfully installed/launched on the target iPhone with the required Android 1.0 background behavior. Compatibility-library CI success or an analysis artifact does not satisfy this definition.


## Spine Unity plugin package verification — 2026-10-31

A later branch workflow run completed successfully:
- Workflow run: [41186251935](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41186251935)
- Commit built: `d19ec444a84de0dc7f7dd19fd52c50f9e8341f57`
- iPhoneOS ARM64 static library build: success
- Exported C ABI symbol check: success
- Host-side JSON-to-SKEL smoke test: success
- Unity plugin packaging step: success
- Uploaded plugin artifact: `azurearchive-spine-unity-ios-plugin`, artifact ID `13036649978`, 1,842,580 bytes
- Uploaded standalone archive artifact: `azurearchive-spine-converter-ios-arm64`, artifact ID `13036814492`, 1,938,905 bytes

Artifact download is available from the run's Actions artifacts section:
https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41186251935

This closes the converter-plugin packaging gate only. The package includes an iOS ARM64 converter archive, ABI header, Unity P/Invoke bridge, metadata and license. It does not include the game's Unity project, UnityFramework, game scenes/assets, Spine 4.2 runtime, Addressables catalog/provider configuration, signed app, or IPA. The converter has not been validated against the actual AzureArchive background skeletons or on an iPhone.

## Current execution queue after plugin packaging

1. **Obtain the real Unity/Xcode project and game resource fixtures** — still the hard blocker; no `Assets/`, `Packages/`, `ProjectSettings/`, `.xcodeproj`, or `.xcworkspace` exists in this repository.
2. Import the existing converter plugin package into that project and compile against its exact Unity/Addressables/Spine assemblies.
3. Test the converter with the game's real Spine JSON/SKEL, atlas and texture resources; resolve the known unsupported enum/features before enabling it.
4. Wire the static background override and Addressables path into the actual game's initialization and loading lifecycle.
5. Build the full iOS app, then install and test it on iPhone 13.
6. Generate and verify a signed IPA only after all runtime checks pass.

No duplicate plugin or source file was added for this checkpoint; the existing progress document was updated in place.


## iOS 0.7.9.2 background API baseline recovered from actual IPA — 2026-10-31

A successful platform-matched Il2CppDumper run against the actual iOS 0.7.9.2 IPA produced `ios-dump/dump.cs` and `DummyDll/Assembly-CSharp.dll` in Actions artifact `azurearchive-ios-il2cpp-method-map` (run [41194368222](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41194368222), artifact ID `13040330066`). Inspection of the recovered top-level `ScenarioResourceManager` declaration confirms the old iOS player already has:
- `TryGetBackgroundOverridePath(uint, out string)`
- `GetBackgroundTexture(uint)`
- `GetBackgroundTextureAsync(uint, bool, Action<Texture>)`
- `GetBackgroundType(uint)`
- `GetTextureFromPersistentDataAsync(string, string, Action<Texture2D>)`
- `GetResourceOverrideRoot()` and `CorrectPath(string)`
- `AddrLoadUrl` as a public static string field
- `GlobalOverrideData` and `LocalOverrideData`
- Existing caches for custom background textures and loaded Addressables objects.

This materially changes the static-image port strategy: do not rebuild the entire old iOS ID-based image loader from scratch. First inspect its exact behavior and reuse/extend the old iOS implementation where it matches Android 1.0. The Android-specific missing surface remains the resource-object overload, full `ResolveBackground` / `EnumerateBackgrounds` model, preview coalescing/cache, and custom Spine acquisition. Metadata declarations and fields do not prove method-body equivalence or runtime correctness.

A follow-up update to the existing iOS method-map workflow adds the exact old-iOS disassembly targets for `TryGetBackgroundOverridePath`, `GetBackgroundTexture`, `GetBackgroundTextureAsync`, and `GetBackgroundType`. Run [41195591074](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41195591074) is validating those ranges against the actual Mach-O `__text` section. Do not claim those method bodies are validated until that run succeeds and its disassembly artifact is inspected.

### Revised next implementation sequence
1. Finish and inspect the old-iOS background method disassembly; compare behavior with Android 1.0 recovered IL.
2. Reuse the old iOS image/addressables path for native images where equivalent; implement only missing Android 1.0 behavior as adapters/extensions.
3. Implement resource-object resolution and override semantics with tests against extracted Android/Windows metadata and real fixtures.
4. Add preview cache/callback coalescing only if confirmed necessary for the target gameplay/UI path.
5. Keep Spine as a separate later milestone; the converter plugin artifact alone does not supply the Spine 4.2 runtime or the actual game's loader/presenter.
6. Integrate into the real Unity/Xcode project, then compile and device-test.

No duplicate source file was added in this checkpoint; only the existing workflow and existing progress report were updated.
