# Husk 3.4 × AzureArchive Android 1.0 compatibility matrix

Date: 2026-11-21  
Target: `com.foxxlight.AzureArchive` 1.0.0  
APK: XR1 release `base.apk`, 364,837,539 bytes  
Husk pin: 3.4 / `acfd7bcb6d1d2f6655a24e43d1d6f87cf142667a`

This matrix separates verified static compatibility from runtime behavior that still needs a physical-device log. “Supported by source” is not the same as “verified for AzureArchive”.

## Compatibility matrix

| Layer | AzureArchive evidence | Husk 3.4 relationship | Status / remaining work |
|---|---|---|---|
| APK container | APK is readable; Husk scanner `ok=true` | Husk scanner parses APK and extracts entries | **CI verified** |
| Engine family | UnityFS header says `2023.2.22f1` | Native Unity driver selected after version-aware overlay | **CI verified**; old class-name-only Unity 6 heuristic was a false positive |
| Scripting backend | `libil2cpp.so` present, 120,315,032 bytes | Unity native driver has IL2CPP hooks and loads the library | **Static candidate**; runtime initialization not tested |
| CPU ABI | `arm64-v8a` present; APK also packages 32-bit/x86 variants | Husk runs ARM64 native libraries on Apple Silicon | **Static match**; actual mapping not tested |
| Launcher activity | DEX contains `UnityPlayerActivity`; manifest profile previously identified `com.unity3d.player.UnityPlayerActivity` | Unity driver looks up UnityPlayer JNI lifecycle/render methods | **Static match**; lifecycle callback sequence needs runtime log |
| Unity 6 marker | `libunity.so` and DEX also contain `UnityPlayerForActivityOrService` | Upstream scanner previously mistook this symbol for Unity 6 and chose Java path | **Corrected and regression-tested** using UnityFS version |
| Native library set | 8 ARM64 libraries: Spine converter, Burst, libc++_shared, IL2CPP, libmain, SQLite, Swappy, Unity | Husk loads the APK's ARM64 libraries itself | **Static scan passed**; dependency/symbol mapping inventory now added to CI |
| 16 KiB page layout | `lib_burst_generated.so` has one 16 KiB page mixing executable and writable content | Husk's next-generation loader has mixed-page writable-copy handling in `husk-tl-ld.c` | **Source support found**; actual Burst write/relocation path must be validated on device |
| Other ELF page layouts | The other seven ARM64 libraries report no code/writable collision on 16 KiB pages | Loader maps the ordinary ELF segments | **Static scan passed** |
| JNI | APK has Unity player classes; `libmain.so` and `libunity.so` present | Husk implements JNI table, RegisterNatives, Unity JNI_OnLoad and nativeRender bridge | **Partial source match**; missing/stubbed JNI methods only become known from runtime log |
| Bionic / linker | Native libraries import Android system libraries and symbols | Husk provides Bionic wrappers, dlopen/dlsym, Android logging/fdsan and ELF linker | **Dependency mapping in progress**; every imported symbol must be reconciled |
| Graphics | Unity native graphics stack plus `libswappywrapper.so` | Husk uses Metal surface and ANGLE/graphics integration | **Unverified**; GLES extensions, swapchain, surface lifecycle and frame pacing need device test |
| Audio | No reliable full audio dependency inventory yet | Husk has host audio/runtime integrations | **Unverified**; enumerate actual calls and test output/latency |
| Storage / assets | Unity asset bundle and APK assets present; exact streaming access not runtime-tested | Husk maps APK/data paths into its guest filesystem | **Unverified**; test StreamingAssets, persistentDataPath, cache, permissions and large-file reads |
| Android services / permissions | Prior manifest analysis: min API 23, target API 30 | Translation Layer implements selected Android framework/JNI behavior, not a full Android OS | **Unverified / likely gap area**; map every requested permission/service and add shims only when observed |
| Network / login | Login requires game-specific network and platform behavior | No complete Android Play Services/Google account compatibility claim | **Unverified**; check TLS, DNS, sockets, device identity and any SDK dependencies |
| Memory | Large IL2CPP binary; previous iPhone 13/Husk test had an OOM, but on a different device/system/runtime version | Husk has native allocation and memory reporting paths | **High-priority runtime gate** on iPhone 14 Pro Max / iOS 26.5; measure peak RSS/VM and jetsam |
| JIT | IL2CPP native code and Unity runtime | Husk 3.4 has StikDebug / StikJIT / TrollStore routes | **Not verified** until Husk itself reports active JIT on the target device |
| Touch / lifecycle | Unity Activity is present | Native Unity driver forwards host lifecycle/input to the engine | **Unverified**; touch, rotation, pause/resume, keyboard and background/foreground tests |
| Spine plugin | `libSpineSkeletonDataConverterLib.so` (8,293,128 bytes ARM64) | Generic ARM64 loader can load the plugin if its imports/relocations are supported | **Static mapping only**; Spine initialization and asset conversion need runtime trace |

## Verified CI run

- Successful scan: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/44913606971
- Artifact: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/44913606971/artifacts/14710014289
- The job built Husk's scanner with warnings-as-errors, ran the page-layout regression test, downloaded the exact APK, tested both Unity 2023 and Unity 6 fixtures, scanned the APK, and retained the scan evidence.
- Confirmed output: `ok=true`, `verdict=native`, `engine=Unity (IL2CPP)`, ABIs `arm64-v8a`, `armeabi-v7a`, `x86_64`, `x86`; 16 library entries, including 8 ARM64 libraries.
- Unity 2023 fixture was classified as `Unity (IL2CPP)`; Unity 6 fixture as `Unity 6`. Both include the ambiguous UnityPlayerForActivityOrService marker, proving the version-aware correction handles the false positive.

## Missing-compatibility work queue

Do not add speculative compatibility stubs. Resolve the gaps in this order, based on real APK imports and then runtime logs.

1. **ELF dependency closure (current CI step):** enumerate all ARM64 `DT_NEEDED` libraries and undefined dynamic symbols. Reconcile each Android platform dependency with Husk's Bionic/graphics/JNI shim exports. Separate true missing symbols from symbols resolved dynamically at runtime.
2. **Burst mixed-page behavior:** verify `lib_burst_generated.so` through the next-generation loader's mixed-page copy/relocation logic. Capture loader logs for copy count, relocation and first write. A static `work` label is not a failure by itself.
3. **Unity JNI contract:** capture `RegisterNatives`, `JNI_OnLoad`, `UnityPlayer.nativeRender`, UnityMain and all `jni: stub` / `NOT FOUND` entries. Add only the missing method signatures that AzureArchive actually calls.
4. **Android framework contract:** inventory manifest permissions, services, receivers, providers and Java SDK classes. Match them to existing Husk host bridges. Implement missing behavior behind explicit compatibility tests.
5. **Graphics and audio:** collect GLES/Vulkan extension requests, EGL surface creation, Swappy frame pacing and audio device calls. Add shims or fallbacks only where the trace shows a missing function.
6. **Game data and network:** test StreamingAssets, persistent storage, cache, TLS, DNS and sockets. Separate Android path semantics from server-side login failures.
7. **Memory / stability:** instrument each stage and compare memory before Unity load, after IL2CPP initialization, after first frame and after entering login. Test cold start, repeated start and background/resume.

## Runtime acceptance gates

1. APK import and static scan.
2. Native loader maps all required ARM64 libraries, including Burst mixed-page library.
3. JNI and UnityMain initialize without unresolved required symbols.
4. First rendered frame.
5. AzureArchive logo.
6. Login screen.
7. Login and interactive scene.
8. Sustained gameplay with stable memory, frame pacing, touch, audio and lifecycle.

Only gate 7 proves the game can reach the user's main functional goal; a successful scan or splash screen is not a runtime pass.

## Scope boundary

This work is for AzureArchive Android 1.0 through Husk 3.4's native Translation Layer. QEMU Android guest remains a fallback only if the native path has a specific blocker. Windows EXE execution is not covered by Husk 3.4's Android runtime; ZIP extraction is not Windows execution.


## Exact ARM64 dependency findings from CI

The ELF dependency inventory job completed successfully on run https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/44913798823. The latest version of the inventory script is being regression-checked in the current run after correcting its symbol-table parser.

The eight ARM64 libraries and their direct `DT_NEEDED` relationships are:

- `libSpineSkeletonDataConverterLib.so`
- `lib_burst_generated.so`
- `libc++_shared.so`
- `libil2cpp.so`
- `libmain.so`
- `libsqlite3.so`
- `libswappywrapper.so`
- `libunity.so`

Direct dependencies observed:
- Packaged: `libc++_shared.so`, `libmain.so`.
- Android platform: `libEGL.so`, `libandroid.so`, `libc.so`, `libdl.so`, `liblog.so`, `libm.so`, `libmediandk.so`, `libz.so`.
- No additional third-party .so dependency outside the APK was found in the direct DT_NEEDED set. This does not rule out later dlopen() calls.

### Important known compatibility limitation: Media NDK

Husk 3.4's `husk-tl-bionic-ndk.c` explicitly states that `libmediandk` decoding is not provided:
- `AMediaCodec_createDecoderByType` and `AMediaExtractor_new` return NULL.
- Codec/extractor configure, start, buffer, seek and decode operations return an error.
- Several `AMediaFormat` setters are no-ops.

Because `libunity.so` directly depends on `libmediandk.so`, this shim is part of the library-load compatibility contract even if the game never calls a decoder. If AzureArchive uses Unity VideoPlayer, intro movies, cutscenes or SDK-driven video, media playback is a likely missing feature. Do not implement a large media bridge speculatively: first inspect the corrected undefined-symbol inventory and runtime trace to determine whether the game calls these functions. If it does, the compatibility work will need an AVFoundation-backed Android MediaCodec/Extractor adapter or a clearly reported unsupported-media fallback.

### Existing NDK compatibility already present

The same Husk source file already provides wrappers for:
- ALooper and AChoreographer frame callbacks;
- AAssetManager / AAsset read, seek, directory and file-descriptor operations;
- ANativeWindow, input queue and motion/key event accessors;
- AConfiguration and device properties;
- thermal and performance hint APIs;
- AAudio and OpenSL ES-related entry points.

These are not all full Android implementations. For example, `AConfiguration_getSdkVersion` currently returns 34, and configuration values are largely host defaults. The app's manifest target SDK is 30, so SDK-level checks may observe a different runtime API level. This needs a deliberate compatibility policy rather than blindly changing the value to 30.

### Graphics relationship

Husk's EGL shim loads ANGLE and requires `eglGetPlatformDisplayEXT`, then requests ANGLE's Metal platform. It wraps EGL config selection and window surfaces, including Android-specific config attributes. This gives AzureArchive a real GLES-to-Metal path, but it is not proof that every Unity 2023.2.22f1 GL extension or render path works. The first runtime test must retain EGL/GL error logs and actual frame-present counts.

### How to interpret the undefined-symbol inventory

A zero or non-zero undefined-symbol count alone is not a compatibility verdict. The final parser must distinguish symbol-table metadata from actual undefined symbol names, then resolve each import against:
1. another packaged ARM64 library;
2. Husk's Bionic/NDK/EGL wrappers;
3. the native Unity driver;
4. a runtime-loaded library or JNI registration.

Any remaining unresolved symbol is a real gap only after that resolution chain is checked.


### Corrected undefined-symbol inventory

The parser regression was corrected and re-run successfully in CI:
- Run: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/44914063579
- Artifact: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/44914063579/artifacts/14710847883
- The report now records real undefined dynamic-symbol imports instead of ELF symbol-table metadata.

Undefined import counts in the eight packaged ARM64 libraries:
- Spine converter: 164
- Burst: 2
- libc++_shared: 159
- IL2CPP: 293
- libmain: 10
- SQLite: 104
- Swappy wrapper: 103
- Unity: 401

These counts are expected for dynamically linked ELF shared libraries and are not themselves 1,236 missing functions. Each symbol must be resolved through the library's DT_NEEDED closure, Husk's Bionic/NDK/EGL exports, or a runtime-loaded/JNI path. The next step is a symbol-by-symbol provider map; only imports with no provider and a runtime call site become implementation tasks.


## Latest source-risk detector run

Run https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/44914330242 completed successfully. Artifact: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/44914330242/artifacts/14710584937.

The CI source-risk detector now enumerates the exact MediaCodec/MediaExtractor NULL/error stubs in Husk 3.4 and confirms:
- `libmediandk.so` is a direct `DT_NEEDED` dependency of `libunity.so`.
- MediaCodec and MediaExtractor calls are explicitly unsupported by the current NDK shim.
- `AConfiguration_getSdkVersion` returns 34, while this APK targets SDK 30.

These are now tracked compatibility risks, not assumed launch blockers. We must determine whether AzureArchive actually invokes media APIs and SDK-gated branches. If media APIs are called, add an AVFoundation-backed compatibility adapter or a safe unsupported-media fallback; do not silently return a fake successful decoder.
