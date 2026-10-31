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


## Latest follow-up and next CI gate

Run [41196441783](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41196441783) succeeded (artifact ID `13040553305`). It confirms the settings persistence and initialization findings recorded in `analysis/android-checkforupdate-addressables-callchain-2026-10-31.md`. In particular, `UpdateAddressable()` is a `ret` stub in this Android binary, and the inspected settings setters do not establish `ScenarioResourceManager.AddrLoadUrl`.

The existing Android disassembly workflow was then extended in place to inspect the generated iterator bodies:
- `UserSettings.InitTask.MoveNext` at RVA `0x32894A0`
- `UserSettings.InitializeSettings.MoveNext` at RVA `0x3289750`
- `UserSettings.ReadUserSettings.MoveNext` at RVA `0x3289908`
- `UserSettings.SettingData.Deserialize` at RVA `0x3286854`
- `ScenarioResourceManager.InitializeAfterSplash.MoveNext` at RVA `0x32771DC`

New CI run [41196544855](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41196544855) was triggered by this workflow-only change and was in progress at this checkpoint. No runtime C# implementation or duplicate source file was added.


## Android 1.0 background resource descriptor implementation checkpoint

- Added `src/AzureArchive.BackgroundCompat/BackgroundResourceDescriptor.cs` to the existing compatibility library; no duplicate loader or project was created.
- The new immutable descriptor records the Android 1.0 resource identity fields: ID, name, kind, origin, root, path and whether Spine override metadata is present.
- Enum values were copied from the Android 1.0 IL2CPP metadata: background kinds `None=0, Image=1, BlurRT=2, Spine=3, Hide=4`; origins `Native=0, GlobalCustom=1, LocalCustom=2`.
- `FullPath` follows the recovered IL: Native resources return the stored path unchanged; GlobalCustom/LocalCustom resources resolve the relative path beneath the trusted root using the existing `SafeResourcePath` guard. The native path remains nullable to preserve the original field-return behavior.
- `IsCustomSpine` follows the recovered predicate: it is true when Spine override metadata is present. It is intentionally independent of the kind enum.
- Regression coverage includes enum numeric values, Native/custom path behavior, custom Spine metadata and traversal rejection.
- CI run [41197164541](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41197164541) succeeded: source-include uniqueness check, .NET Standard 2.1 library build, background compatibility tests, Unity decoder stub tests and artifact upload. Artifact: `azurearchive-background-path-compat-library` (ID `13041292935`).
- Validation boundary: this is a portable compatibility model, not yet linked into the actual Unity player. It does not implement Addressables acquisition, preview rendering, Spine playback, or produce an IPA.

### Next implementation slice

1. Recover the exact Android override enumeration iterator and ID predicate; then extend the existing resolver only where the source-specific behavior is proven.
2. Reuse the old iOS 0.7.9.2 texture/Addressables path for native resources where the method-body diff supports it.
3. Add the resource descriptor adapter to the actual iOS integration point only after a buildable Unity/Xcode base is established.


## ID-based background override resolver checkpoint

- Added an overload to the existing `BackgroundOverrideResolver.TryResolve` that accepts the requested `uint32` background ID and `BackgroundResourceDescriptor` sequences.
- Evidence: Android 1.0 `BackgroundResource.ResolveOverride(uint32 id, ResourceOverrideData local, ResourceOverrideData global)` calls `Overrides(local, ResourceType=2)`, applies `FirstOrDefault` with a captured-ID predicate, returns immediately on a local match, and only then calls `Overrides(global, ResourceType=1)` and applies the same predicate. Windows IL corroborates the two-stage local/global sequence.
- The resolver overload intentionally accepts already source-separated override sequences. The recovered `Overrides` iterator body contains unresolved Cpp2IL decompiler markers, so the underlying ResourceOverrideData-to-descriptor enumeration is not claimed as implemented.
- Regression cases cover first matching local ID, local-over-global precedence, global fallback, and missing/null override sequences.
- Validation before the latest predicate-only cleanup: [Actions run 41197384423](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41197384423) succeeded on commit `33c3f777ade7bd9d3b5afbc8e2496f6f345cb29b`. A follow-up run for the current source is tracked separately.
- Remaining gap in this slice: recover/implement the `Overrides` iterator that reads actual local/global override records, then connect `ResolveBackground(uint)` to scenario Excel metadata for the no-override fallback. No Unity runtime integration is claimed.


## Scenario background native fallback implementation checkpoint

- Added `ScenarioBackgroundRecord` as a minimal projection of Android 1.0 `ScenarioBGNameExcel`: `Name` (uint key), `BGFileName`, and `BGType`.
- Added `ScenarioBackgroundResolver.Resolve`: returns null for ID 0; resolves local then global override first; only on no override calls the supplied database lookup; rejects a mismatched returned key; maps a row to a Native descriptor with `Path=BGFileName`, `Name=Path.GetFileNameWithoutExtension(BGFileName)`, `Kind=BGType`, `Root=null`, and no Spine override data.
- Evidence: Android 1.0 `ResolveBackground(uint)` checks ID zero, calls `BackgroundResource.ResolveOverride`, and only on no override queries `ScenarioBgNameQuery`. The query schema's indexed key is `ScenarioBgNameDbSchema.Name` (uint), while the decoded FlatBuffer row exposes `ScenarioBGNameExcel.BGFileName` and `BGType`. The recovered method explicitly calls `GetExcel()`, `get_BGFileName()`, `Path.GetFileNameWithoutExtension()`, and `get_BGType()`, then constructs a Native resource with the original filename path.
- The database query itself remains injected through `findByNameId`; the current portable library does not yet parse the SQLite/FlatBuffers files or instantiate the Unity database service.
- Added regression cases for local/global override short-circuit, native fallback mapping, ID zero, missing/mismatched row and missing lookup callback.
- CI run [41197624016](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41197624016) passed for the implementation/test commit before the latest disassembly workflow update. The latest branch revision is undergoing a new run.


## Image override path-to-ID projection checkpoint

- Added `BackgroundResourceIdHash` and `BackgroundImageOverrideFactory` to the existing portable compatibility library.
- Native Android iterator evidence confirms image override path normalization (`\\` to `/`), filename-stem extraction, ID hash call, and construction as Image/source/path/basePath/no-Spine.
- `BgSpineOverrideData.get_Id` loads `Identifier` and branches to the same `Util.CalculateHash` function; full Spine descriptor construction remains separate and is not yet implemented.
- Hash implementation currently covers the normal short-string path as UTF-8 + xxHash32 seed 0, with known-vector tests. Android's separate >256-character path and exact encoding identity are still verification gates; do not treat all edge cases as byte-for-byte proven.
- Native `ResolveOverride` calls local source 2 before global source 1. Local ID predicate is directly confirmed; the symbol-labeled global predicate disassembly is anomalous, so its exact filtering remains an explicit open item.
- Full evidence and limitations: `analysis/android-background-override-enumeration-native-findings-2026-10-31.md`.
- GitHub Actions run [41197855495](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41197855495) passed the current code build and regression suite.


## Unity static custom-image decoder connection checkpoint

- Extended the existing `BackgroundImageDecoder.Unity.cs` with `DecodeCustomResource(BackgroundResourceDescriptor)`.
- It only accepts custom Image resources, uses the descriptor's `FullPath`, and explicitly refuses Native resources (Addressables path) and Spine resources (separate runtime). This connects the newly reconstructed descriptor to the existing Unity Texture2D/ImageConversion decoder without creating a second loader.
- Extended the Unity compatibility test project to compile the descriptor source and test successful descriptor-driven decoding, Native rejection and Spine rejection.
- GitHub Actions run [41198025689](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41198025689) passed: compatibility library build/tests and Unity API-stub compile/runtime tests. Artifact ID: `13041850514`.
- Validation boundary: tests use isolated Unity API stubs, not the real Unity 2023.2.20f1c1 player. The adapter is not yet wired into a complete game scene or the original ScenarioResourceManager.


## Spine background override metadata checkpoint

- Decompiled Android 1.0 `ResourceOverrideData.BgSpineOverrideData` and its direct IL establish fields: Identifier, SpinePath, Animation, Loop, Skin, PreviewPath, PreviewTime, Scale, OffsetX, OffsetY; Id is `CalculateHash(Identifier)`. Constructor defaults are Loop=true and Scale=1.
- Direct Android `BackgroundResource.Validate` IL establishes required Identifier/SpinePath, nonzero hashed ID, positive finite Scale, finite offsets, and nonnegative finite PreviewTime. The portable factory mirrors those constraints and error cases.
- Added `SpineBackgroundOverrideRecord`, `SpineBackgroundOverrideFactory`, and `BackgroundOverrideSetFactory`. The set factory emits Image entries before Spine entries and preserves local/global source.
- Background descriptors now retain Spine animation/framing/skin/preview metadata and resolve the preview image under the custom root. This is still data modeling; no actual Spine skeleton/atlas playback is integrated.
- Caveat: generated iterator and constructor IL contain unresolved/type-inconsistent Cpp2IL output around the final Spine object parameter. Do not claim the original runtime's exact Spine attachment semantics as proven until native method/callsite verification.
- Detailed evidence: `analysis/android-spine-background-override-record-findings-2026-10-31.md`.
- Pre-preview-path CI run [41198701750](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41198701750) passed. A new run is triggered for the latest preview-path tests and report.


## Static Image runtime adapter checkpoint

- Recovered Android `ScenarioResourceManager.GetBackgroundTexture(BackgroundResource)`: custom Image uses `FullPath` + a path-keyed Texture2D cache + `File.ReadAllBytes`; Native Image uses `CorrectPath(Path)` + Addressables and retains the resulting Unity object.
- Recovered decode arguments: Texture2D 2x2 RGBA32/no mipmaps, name, Clamp wrap mode, ImageConversion.LoadImage(markNonReadable:true).
- Added `BackgroundImageRuntimeCache.Unity.cs`: custom texture cache with explicit destroy ownership; native Addressables cache with caller-supplied CorrectPath function, in-flight request deduplication, and idempotent handle leases. It rejects non-Image resources.
- Unity adapter stub tests cover custom cache reuse/disposal, native key correction, concurrent native request sharing, retained handle release, and failed-load release.
- CI [41199007745](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41199007745) passed the full compatibility and Unity adapter stub tests before the latest callback error-handling cleanup; the newest revision is in a follow-up run.
- Still not integrated into the game's actual `ScenarioResourceManager` or presenter. The correct `CorrectPath` delegate and scene lifecycle hookup must be provided by the recovered game runtime; the repo has no full Unity project.
- Detailed evidence: `analysis/android-static-background-image-runtime-path-2026-10-31.md`.


### Native image runtime binding refinement

- Native background loading now uses Unity `Texture` (the game's recovered `GetBackgroundTexture(BackgroundResource)` returns Texture), while custom file decoding remains Texture2D.
- Added reflection binding to the live `ScenarioResourceManager.CorrectPath(string)` instance method and a singleton-based factory that resolves inherited `Singleton<ScenarioResourceManager>.Instance`.
- Tests verify real method binding, singleton resolution, native Addressables in-flight release after cache disposal, and the custom/native cache ownership split.
- Final current CI [41199317839](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41199317839) passed all compatibility and Unity adapter stub tests.
- This removes the need to reimplement the game's manifest path correction, but still requires the runtime cache to be called by the actual game background presenter; that hookup and a full Unity project remain missing.


## Combined background override set projection checkpoint — 2026-10-31

- Decompilation workflow [41198111242](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41198111242) completed successfully and produced artifact `azurearchive-decompiled-source` (ID `13041442362`). The extracted Android 1.0 `ResourceOverrideData` declaration confirms `BgOverrides` is `HashSet<string>`, `BgSpineOverrides` is `List<BgSpineOverrideData>`, and each manifest has `basePath`.
- The recovered `BackgroundResource.Overrides` state machine enumerates image path entries before Spine records. Spine records are validated and duplicate Spine IDs are rejected before their descriptors are yielded. The Spine descriptor mapping is `Id=hash(Identifier)`, `Name=Identifier`, `Type=Spine`, `Source=source argument`, `Path=SpinePath`, `Root=basePath`, `Spine=record`. Image descriptors are `Id=hash(filename stem)`, `Name=filename stem`, `Type=Image`, `Source=source argument`, `Path=slash-normalized relative path`, `Root=basePath`, with no Spine record.
- Updated the existing `BackgroundOverrideSetFactory` rather than creating a duplicate. It now validates the override source, preserves exact-string uniqueness for image paths (matching the source HashSet), appends Spine descriptors after image descriptors, and relies on the existing Spine factory to validate records and reject duplicate Spine IDs.
- Added regression checks for exact-path deduplication, null collection handling, null image-path rejection, invalid Native source rejection, and image-before-Spine ordering. The corresponding CI run is [41199460222](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41199460222) and must be checked before claiming this checkpoint passed.
- This is a projection of already-deserialized collections. The project still does not parse the actual `manifest.json` file or bind the records to the game's persistent override loader. The global `ResolveOverride` predicate body is still an anomalous switch-based native method and remains a separate verification item.


## Preview request coalescing primitive — 2026-10-31

### Evidence
The recovered Android 1.0 IL for `ScenarioResourceManager.QueueBackgroundPreview` shows:
- `pendingBackgroundPreviews` is keyed by the preview cache key and stores `List<Action<Texture>>`.
- A callback for an existing key is appended to the existing list; no second coroutine is started.
- A new key creates the callback list and starts `CoLoadBackgroundPreview(resource, preferThumbnail, key)`.
- `BackgroundPreviewKey` begins with `BackgroundResource.CacheKey` plus the `preferThumbnail` flag. Custom Spine keys additionally incorporate sidecar stamps; the full key logic remains partly decompiler-noisy.

### Implemented
Added `src/AzureArchive.BackgroundCompat/BackgroundPreviewRequestCoalescer.cs`:
- One pending callback list per key.
- `TryEnqueue` returns true only for the first request, so the Unity owner knows when to start a coroutine.
- `TryComplete` removes the pending entry and returns callbacks in registration order.
- `Cancel` drops pending callbacks without claiming to cancel an external coroutine or release Unity assets.

The existing test file was extended in place; no duplicate test project or workflow was added. Tests cover first-request behavior, same-key coalescing, callback order, completion/removal, independent keys, cancellation and invalid arguments.

### CI
- Run [41200035732](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41200035732): success.
- Build: .NET Standard 2.1, 0 warnings and 0 errors.
- Source include preflight: 6 explicit C# includes checked; no missing paths or duplicate includes/Link names.
- Background compatibility tests: all existing checks plus 11 preview request coalescer checks passed.
- Unity compatibility stub tests also passed.
- Artifact: `azurearchive-background-path-compat-library`, ID `13042476136`, 20,706 bytes.

### Validation boundary
This is a tested coordination primitive, not a completed preview pipeline. It is not yet connected to a Unity coroutine, `backgroundPreviews` cache, RenderTexture validation/eviction, callback failure behavior, or the real game manager. Those require the actual Unity project/runtime and further exact coroutine IL recovery. It does not produce or advance an IPA build.

### Next
1. Recover the remaining `CoLoadBackgroundPreview` iterator behavior and cache completion/eviction semantics without guessing from unresolved IL.
2. Implement a Unity-side adapter only after the actual `ScenarioResourceManager` and Unity assemblies are available.
3. Keep the project/Xcode source acquisition as the hard blocker for full app integration and IPA packaging.


## Preview coroutine recovery and native-disassembly checkpoint — 2026-10-31

### Newly recovered nested iterator metadata
The Android 1.0 and Windows 1.0.1 compiler-generated nested type `ScenarioResourceManager.<CoLoadBackgroundPreview>d__28` was exported directly from both recovered assemblies. Android ILSpy output identifies these state fields:
- `resource`, `ScenarioResourceManager` owner, `preferThumbnail`, and cache `key`;
- `Texture texture`, `bool addressable`, and `string full`;
- `AsyncOperationHandle<Texture> handle`;
- `Task<byte[]> read`;
- `Task<RuntimeSpineLoader.Files> read` (a second task field).

This confirms the coroutine's state machine retains both an Addressables handle and asynchronous byte/Spine file reads. It does not, by itself, establish their ordering or success/failure cleanup.

### Decompiler limitation confirmed
The nested `MoveNext` method is present in both IL exports, but its recovered IL body is not usable as a behavioral reconstruction:
- Android body is only 93 IL bytes and contains unresolved unmanaged-memory loads plus an unresolved indirect jump.
- Windows body is only 101 IL bytes and contains the same unresolved indirect-jump pattern.
- The generated `Dispose` method is empty; `Reset` also contains unresolved method references.
Therefore no guessed C# coroutine was added. The exact remote/local fallback sequence, task await states, RenderTexture allocation, callback fan-out and handle/texture release remain unverified from this IL export.

### Native follow-up
The Android ARM64 dump identifies the compiler-generated iterator's actual native method RVA as `0x3270D7C` for `MoveNext` (distinct from the ILSpy IL RVA). The Android native disassembly workflow was extended to inspect this method and the adjacent preview-cache lifecycle methods:
- `BackgroundPreviewCachePath` — `0x32627E4`
- `SaveBackgroundPreview` — `0x3262840`
- `ReleaseBackgroundPreview` — `0x3262C54`
- `TrimBackgroundPreviews` — `0x3262DC4`
- `DestroyBackgroundPreview` — `0x32631E8`
- `LimitBackgroundPreviewSize` — `0x3263310`

Workflow run [41200328434](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41200328434) is running to generate this Android-only disassembly evidence. These RVAs are not iOS addresses and will not be copied into the iOS binary.

### Validation boundary
- Nested iterator type export completed for Android and Windows in run [41200220219](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41200220219).
- The first export attempt (run 41200200402) failed in the report regex; the extraction escaping was corrected and the subsequent run completed successfully.
- The preview coalescer remains a standalone tested primitive, not a coroutine or RenderTexture implementation.
