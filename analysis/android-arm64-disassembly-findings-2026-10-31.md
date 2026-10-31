# Corrected Android ARM64 disassembly findings — 2026-10-31

Source: corrected workflow run 41170866048, artifact `azurearchive-android-arm64-disassembly-corrected`. The corrected script uses RVAs checked against exact method declarations in the Android ARM64 dump.

## Execution result

All 8 selected functions were located and disassembled successfully from the Android 1.0 `libil2cpp.so`. The output is 189,088 bytes.

## Findings supported by disassembly

### ScenarioResourceManager.InitializeAuthoringResources — RVA 0x3264CA8

- Sets up/initializes a series of static generic/runtime metadata references on first entry.
- Opens/initializes a database-like object using the first string argument and read-only/open flags (arguments visible in register setup before call at 0x3264DA4).
- Stores several returned objects into fields of the ScenarioResourceManager instance.
- Initializes additional resource/data structures using the other supplied string arguments.
- Calls several helper routines and sets an initialized flag near function exit.
- This is consistent with the recovered CIL interpretation that the method initializes local authoring/resource data, but individual helper names are not recoverable from this disassembly alone.

### ScenarioResourceManager.ResolveBackground — RVA 0x3260EE0

- Takes the background ID in w1 and stores it into a temporary/resource object.
- Performs a sequence of lookups against manager-held resource/database objects.
- Contains null/error branches and checks on returned object state.
- Calls further resource-resolution and construction helpers before returning an object pointer.
- This confirms the function is a real native runtime path, not merely an unused declaration. The exact meaning of each helper requires symbol/metadata cross-reference.

### ScenarioResourceManager.GetBackgroundTexture — RVA 0x326172C

- Checks resource state fields at offsets 0x20 and 0x24.
- Uses a manager field at offset 0x100 as a lookup/cache-like object.
- Calls a helper with the BackgroundResource and checks a returned status.
- On success, extracts an object and passes it to another helper; on failure branches to alternate handling.
- Strongly consistent with a resource-state-aware texture retrieval/cache path.

### ScenarioResourceManager.GetBackgroundPreviewAsync — RVA 0x3261EA0

- Receives a resource, a boolean-like option and a callback/delegate argument.
- Checks null arguments and resource/cache state.
- Looks up a cached/in-flight object through a manager field at offset 0x38.
- Updates a counter/state field on a resource-related object and invokes a delegate/callback on a completed or fallback path.
- This supports an asynchronous preview/cache mechanism, but exact callback semantics require cross-reference.

### AuthoringResourceCatalog methods

Search (0x336A1F0), Resolve (0x336B5DC), Import (0x3375FE8), and StageReferencedResources (0x3372058) were successfully disassembled. Their raw instruction sequences are present in the artifact. The current report does not yet establish whether these are called by ordinary gameplay UI or only by local authoring/import screens.

## Porting assessment

- Background resolution and texture retrieval are stronger candidates for ordinary mobile runtime behavior than the catalog's import/staging APIs.
- The native code is Android ARM64 and uses Android build-specific addresses, globals, runtime layout and calling conventions. It cannot be copied into the iOS binary.
- The practical next step is to identify call sites and UI entry points for AuthoringResourceCatalog and the background methods, then classify them as gameplay-critical, optional local authoring, or desktop/tooling-only.
- No iOS 1.0 native code or buildable Unity project has been produced. No IPA has been generated.
