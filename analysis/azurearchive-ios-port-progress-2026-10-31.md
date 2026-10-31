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


## Latest verified background-method comparison checkpoint

- iOS method-map workflow [41195591074](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41195591074) succeeded against the actual 0.7.9.2 IPA.
- Artifact ID: `13040971233`, 30,340,597 bytes; contains the iOS IL2CPP dump and bounded ARM64 disassembly.
- Verified iOS methods: `TryGetBackgroundOverridePath` at `0x3D69A54`, `GetBackgroundTexture` at `0x3D69D1C`, `GetBackgroundTextureAsync` at `0x3D6A1F4`, and `GetBackgroundType` at `0x3D6A908`.
- Native call edges: both `GetBackgroundTexture` and `GetBackgroundTextureAsync` directly call the same `TryGetBackgroundOverridePath` method. This confirms that the old iOS build already routes both loading paths through an override-path method. It does not, by itself, prove that its Android 1.0 override precedence or file format is identical.
- The detailed method ranges and instruction-level evidence are recorded in `analysis/ios-0792-il2cpp-method-map-2026-10-31.md`.

### Current execution decision

Do not add another background loader or copy any Android addresses into iOS. The next code change is gated on the Android 1.0 vs iOS 0.7.9.2 behavior diff. In particular, verify:
1. whether iOS override resolution uses local-first then global-first-match semantics exactly as Android 1.0;
2. whether iOS texture loading separates custom-file reads from native Addressables loads and uses the same cache key/ownership rules;
3. whether the async path preserves callback behavior and in-flight request coalescing;
4. whether Image vs Spine dispatch is compatible.

Only a demonstrated difference should produce a code change. Existing adapters remain isolated candidates until compiled against actual Unity/Addressables assemblies.

### Remaining hard blockers

- The branch still contains no Unity `Assets/`, `Packages/`, `ProjectSettings/`, Xcode project or workspace.
- No real Android 1.0 background bundle/override fixture has been attached to a Unity integration test.
- The Spine converter plugin is packaged, but the actual Spine 4.2 runtime/presenter integration is not proven.
- No full Unity iOS build, signed IPA, or iPhone 13 device test exists.


## Addressables bootstrap audit and URL-source blocker — follow-up

### Verified integration surface
- The old iOS 0.7.9.2 metadata contains `Addressables.InternalIdTransformFunc` getter/setter, `LoadAssetAsync` overloads, and `Release(AsyncOperationHandle)`. This establishes API-surface availability only.
- iOS, Android and Windows catalogs contain the same ordered set of 32,353 tokenized internal IDs, using the exact prefix `{ScenarioResourceManager.AddrLoadUrl}`.
- Android 1.0's normal mobile update coroutine follows catalog check → catalog update → download-size check → dependency download → post-update TextAsset/FlatBuffer loading → local global-override manifest loading.
- The ordinary mobile value/assignment of `ScenarioResourceManager.AddrLoadUrl` remains unresolved. The visible `https://filev2.foxxlight.top/aares/` fallback belongs to `AuthoringCliHost.Bootstrap()`, not proven mobile gameplay configuration. Do not hard-code it.
- `AddressablesInternalIdTransformHook` and `AddressablesTransformBootstrap` are candidate integration source only. They have not been included in a real Unity player build, and no runtime evidence proves `BeforeSceneLoad` is early enough for this target's actual Addressables initialization order.

### Source audit / change-control decision
- Existing `BackgroundOverrideResolver` and its tests already cover local-first selection, first matching entry, global fallback, null lists and missing matches. This is only the list-selection primitive: Android's `BackgroundResource.Overrides(data, source)` performs source-specific enumeration, and the recovered predicate body still has decompiler gaps. Do not claim full resolver equivalence or duplicate this helper.
- Existing `AddressableInternalIdResolver` tests cover the verified catalog token and unsafe URL/path cases. The runtime base URL provider is still unknown; do not add a guessed default.
- Existing `AddressablesRuntimeAdapter` and `AddressableAssetLease` provide a candidate handle-ownership pattern, but actual target Addressables package version and runtime callback behavior remain untested.
- No new C# implementation was added in this checkpoint because no behavior delta or safe URL source has been proven. This is intentional: adding a second bootstrap or speculative URL provider would create integration risk rather than progress.

### Exact next execution gates
1. Recover the mobile assignment/provider for `AddrLoadUrl` from Android/Windows native field writes, startup coroutine state transitions, configuration files, or observed catalog/network requests. Keep authoring CLI/offline URL paths separate.
2. Compare Android and old-iOS Addressables initialization order, catalog locator setup, and transform ownership. Determine the earliest proven hook point; preserve/chain any existing transform.
3. Only after gates 1–2, connect the existing token transform to the real player bootstrap and test that a representative catalog internal ID resolves to the same origin/path the original app uses.
4. Validate catalog update, dependency-size query, dependency download and post-update TextAsset/FlatBuffer loads in a real Unity iOS player.
5. Then connect static background native/custom branches, preview coalescing and cache ownership. Spine playback remains a separate runtime compatibility gate.

### Completion boundary
The current branch still has only three standalone .NET project files and no Unity `Assets/`, `Packages/`, `ProjectSettings/`, Xcode project or workspace. No production CDN value is established, no real-player Addressables test exists, and no signed/device-tested IPA exists.

## Additional native source-flow audit — 2026-10-31

Direct inspection of Android ARM64 artifact azurearchive-android-arm64-disassembly-corrected (artifact ID 13039996236, produced by successful workflow run https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41192985832) confirms:
- CheckForUpdate.MoveNext calls AuthoringOfflineResources.TryGetStartupUrl(out string) at RVA 0x33c5e00 before the ordinary catalog update state machine.
- The offline helper calls ValidateDirectory and returns a string through its out parameter on a success path. This is a candidate optional offline/manual startup override, not proven as the production CDN.
- UserSettings.ReadUserSettings.MoveNext calls UserSettings.InitializeSettings; no visible write to ScenarioResourceManager.AddrLoadUrl appears in this short body.
- UIPatchDownload.CoChangeSource.MoveNext handles the UI source-change coroutine and invokes a final helper at RVA 0x325a97c; no direct visible static-field store to AddrLoadUrl appears in this body either.
- The authoring CLI fallback remains authoring-specific. Neither settings-read nor source-change coroutine currently proves the mobile gameplay URL assignment.

### Next exact investigation
1. Resolve the target and full declaration for 0x325a97c and inspect its callers/callee effects; it may clarify source-change persistence but must not be assumed to set AddrLoadUrl.
2. Inspect AuthoringOfflineResources.TryGetStartupUrl success/failure branches and IsActive gating together with serialized offline startup metadata.
3. Map the ScenarioResourceManager.AddrLoadUrl static field metadata token/storage location and search all ARM64 writes to that location across the Android binary; cross-check every candidate against managed field metadata.
4. If no managed/native field write is found, inspect Addressables token substitution and runtime configuration/provider initialization before adding any iOS code.
5. Keep the existing Addressables bootstrap adapters unchanged until the source value and initialization order are proven.

No C# source files were added or modified in this checkpoint.

## Additional verified distinction: customSource is not AddrLoadUrl

The Android source-change callback was disassembled in successful run [41196234346](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41196234346), artifact ID `13040812483`. It writes the submitted source string into `UserSettings.SettingData.customSource` (managed field offset `0x90`) and then calls the UI confirmation popup. This is not a write to static `ScenarioResourceManager.AddrLoadUrl`; using `customSource` as the gameplay CDN would be an unsupported assumption.

The callback's native body resolves the previously opaque helper call at RVA `0x325A46C` to `PopupManager.ShowPopupOk(...)`. `catalogUrlOverride` is a separate SettingData field at offset `0x18`; its runtime call sites are not yet established.

The existing native-disassembly workflow has been extended to inspect settings persistence/initialization and the Addressables manager's Awake/Initialize/UpdateAddressable methods. New run: [41196441783](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41196441783). No C# implementation was added; existing URL resolver and bootstrap remain unchanged pending proof of the actual URL provider.

## Additional native checkpoint: settings persistence and Addressables manager initialization

Successful run [41196441783](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41196441783), artifact ID `13040553305`, confirms:
- `UserSettings.WriteUserSettings` serializes the current SettingData and writes settings; it does not itself establish the gameplay CDN URL.
- `SettingData.set_catalogUrlOverride` only stores an instance field at offset `0x18`; no direct static `AddrLoadUrl` write is present.
- `ScenarioResourceManager.Awake` starts `InitializeAfterSplash`.
- `ScenarioResourceManager.Initialize` configures runtime resources and calls Addressables-related methods, but the inspected body does not prove an `AddrLoadUrl` assignment.
- `ScenarioResourceManager.UpdateAddressable` is a single `ret` stub in this Android build and is not a URL provider.

The exact URL writer remains unproven. Next focus: generated settings iterator bodies, `InitializeAfterSplash.MoveNext`, and cross-references to the IL2CPP static-field storage after metadata correlation. Existing URL resolver/bootstrap code remains unchanged; no speculative C# file was added.
