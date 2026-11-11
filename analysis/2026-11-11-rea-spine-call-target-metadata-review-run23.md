# REA Spine call-target and ownership review — Run 23

Date: 2026-11-11  
Branch: `progress/azurearchive-ios-port-20261031`  
Workflow: [Run 23](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43007568169)  
Artifact: `13849575402` (`rea-spine-flow-run23.zip`)  
Artifact SHA-256: `5238e0ddb365d75e137082d25f2214b2e7303e26f47845144d800b84e2a149dd`

## Scope and verification

Run 23 succeeded. All 11 principal workflow steps completed successfully. The artifact contains 170 entries, including pseudocode, flow JSON, provenance/forensics TSVs, and candidate-call-target TSVs for 14 Spine-family targets. The candidate export now covers `RuntimeSpineLoader_*`, `RuntimeSpineAsset_*`, and `PageTextureLoader_*`, rather than only three selected methods.

The verifier reports 34 targets, 5,301 reachable instructions, 5,301 decoded instructions, and zero decode failures. This is bounded reachable-flow decoding, not proof of complete source-level CFG; indirect jump tables and exception LSDA edges may remain incomplete.

The metadata address table has 152,789 method entries. Ghidra candidate addresses map to IL2CPP method RVAs by subtracting `0x100000`. A candidate is treated as a managed method entry only when this normalized address exactly matches the metadata method-address table. Executable instruction presence alone is not enough.

## Metadata-confirmed call relationships

### RuntimeSpineLoader.Load / Create

The two call-target exports agree on the core creation path. Confirmed targets include:

| Ghidra target | Metadata method | Call site(s) in Load/Create |
|---|---|---|
| `0x335ee08` | `RuntimeSpineLoader.Read(string,bool)` | `0x335f30c` |
| `0x335fc74` | `RuntimeSpineAsset.Own(object)` | `0x335f740`, `0x335f7c0`, `0x335f89c`, `0x335f8a8` |
| `0x335fd1c` | `RuntimeSpineAsset.Dispose()` | `0x335fb18` |
| `0x5f3ba50` | `Spine.Unity.SpineAtlasAsset.CreateRuntimeInstance(...)` | `0x335f7b0` |
| `0x5f3acac` | `Spine.Unity.SkeletonDataAsset.CreateRuntimeInstance(...)` | `0x335f878` |
| `0x5f36e54` | `Spine.Unity.SkeletonDataAsset.GetSkeletonData(bool)` | `0x335f8bc` |
| `0x5f3addc` | `Spine.Unity.SkeletonDataAsset.SetupRuntimeBlendModeMaterials(...)` | `0x335f8d8` |
| `0x5f360b0` | `Native.SpineConverter.Converter.CastSkeletonDataToDstVersion(...)` | `0x335f7ec` |

Other metadata-confirmed calls include `SystemInfo.SupportsTextureFormat`, Unity object equality/name operations, `TextAsset` construction, path filename extraction, and exception constructors.

This supports a high-level path of texture-format capability check → read → runtime atlas/skeleton asset creation → blend-mode material setup / skeleton-data access → owned-resource registration, with a Dispose cleanup branch. It does not establish the exact full ordering of every basic block or every exception path.

### RuntimeSpineLoader.DestroyOwned

The decompiled body checks Unity object equality and `Application.isPlaying`, then dispatches through indirect call sites. The p-code provenance includes two computed tail branches at `0x6814cf8` and `0x6814e4c`. Normalizing their nearby method entries by the same `+0x100000` Ghidra relocation maps the first wrapper region to metadata `UnityEngine.Object.Destroy` (`0x6714c4c` RVA) / `Destroy_Injected` (`0x6714cfc` RVA), and the second to `UnityEngine.Object.DestroyImmediate` (`0x6714da0` RVA) / `DestroyImmediate_Injected` (`0x6714e50` RVA). In the decompiled branch structure, the `Application.isPlaying`-true arm selects the first pointer and the false arm selects the second. This is strong cross-evidence for delayed `Object.Destroy` while playing and `Object.DestroyImmediate` otherwise; the indirect function-pointer initialization is still not fully recovered, so retain that distinction as high-confidence rather than complete source-level proof.

### RuntimeSpineAsset.Own and Dispose

Metadata-confirmed cross-calls:

- `RuntimeSpineAsset.Own` calls `RuntimeSpineLoader.DestroyOwned` at `0x335fef0`.
- `RuntimeSpineAsset.Dispose` calls `RuntimeSpineAsset.get_Materials` at `0x335fdb8`.
- `Dispose` calls `RuntimeSpineAsset.Own` at `0x335fdfc`.
- `Dispose` calls `RuntimeSpineLoader.DestroyOwned` at `0x335fef0`.
- `Dispose` calls `Spine.Atlas.Dispose` at `0x335fea4`.
- `Dispose` also invokes Unity object inequality at `0x335fe3c`.

The decompiled Dispose body reads the integer at object offset `+0x18`, decrements it, and enters cleanup only when the resulting value is zero. The cleanup path obtains the Materials collection, registers material-owned objects through `Own`, iterates atlas pages/material-related objects, disposes the Spine atlas, and iterates the owned-object collection to call `DestroyOwned`. Some branches use indirect virtual calls and exception/iterator machinery; do not treat the pseudocode's local variable names or every inferred collection type as source truth.

The `Own` body first checks the supplied Unity object for null/equality, then follows an ownership-collection path; its pseudocode includes recursive registration and a call into `RuntimeSpineLoader.DestroyOwned` during cleanup-related logic. Exact container types and ownership deduplication semantics still need instruction-level validation.

### RuntimeSpineAsset reference count / consumers

- `get_HasConsumers` decompiles to `1 < *(int *)(this + 0x18)`. This directly supports a consumer predicate based on the count field at offset `+0x18).
- `Dispose` decrements that same field and performs cleanup at zero.
- `Retain` has a fast path: if the integer at observed offset `+0x18` is nonzero, increment it and return. The `Retain` parameter is typed as `undefined8 *`, so the decompiler expression `param_1 + 3` means three 8-byte slots, i.e. byte offset `+0x18`. This agrees with the field read by `get_HasConsumers` and decremented by `Dispose`. The remaining path is large and includes LINQ error helpers and indirect collection/iterator control flow; the fast-path evidence establishes the shared counter offset, but not every exceptional or initialization path.
- `get_EstimatedTextureBytes` calls `System.Linq.Error.ArgumentNull` and `System.Linq.Error.NoElements`, and contains collection/enumerator loops plus guarded arithmetic. The exact collection element type and byte-estimation formula remain unresolved.

### PageTextureLoader

- `PageTextureLoader.Load` confirms a collection/dictionary lookup using page/texture identity, stores the retrieved object into a field at `+0x38`, then uses Unity Material mainTexture accessors to populate two integer fields (observed offsets `+0x18` and `+0x1c`) when either is zero. It then conditionally calls `Material.EnableKeyword` or `Material.DisableKeyword` based on a byte flag at `+0x34`.
- On failure, it enters an error path that builds a string and throws `System.IO.InvalidDataException`.
- `PageTextureLoader.Unload` decompiles to a single `return` instruction. This is a no-op at this Android 1.0 method entrypoint only; it does not prove that texture resources are globally unmanaged or that other owners do not release them.
- `PageTextureLoader.Load` has metadata-confirmed calls to `UnityEngine.Material.get_mainTexture`, `EnableKeyword`, and `DisableKeyword`, plus string concatenation and invalid-data exception construction.

### Run 24 instruction-level findings: Read and Task scheduling

Run 24 added ARM64 instruction listings for the Spine-family methods. The archive SHA-256 is `3e9aa1028ab9ab4f5744dc7fae250a7bd94064c004317785113dbc90407d775d`.

- `RuntimeSpineLoader.Read` (entry `0x335ee08`) directly calls metadata-confirmed `System.IO.Path.GetFullPath` (`0x5d4cebc`), `System.String.Concat` (`0x5bd7ad8`), `System.IO.File.ReadAllText` (`0x5d39130`), `System.IO.File.ReadAllBytes` (`0x5d39858`), `System.IO.Path.GetDirectoryName` (`0x5d4cb44`), `RuntimeSpineLoader.AtlasPages` (`0x335ea60`), `BackgroundResource.ResolvePath` (`0x353004c`), and `RuntimeAtlasTexture.Read` (`0x335c600`). These are exact method-entry matches after subtracting the Ghidra relocation `0x100000`.
- Register setup and stores show `Read` receives the base path in `x0` and the DXT5-support flag in `w1). It normalizes the path, constructs two path strings, reads one as text and one as bytes, obtains atlas pages, then loops page entries. For each page, it resolves a resource path, invokes `RuntimeAtlasTexture.Read(path, supportsDxt5)`, and passes the resulting texture/data through a further unresolved helper. This is strong evidence for the actual file/atlas loading path, though the semantic names of the two concatenated path suffixes still require string-literal cross-reference.
- The two calls at `0x335efc8`, `0x335efe0` etc. include shared/native helpers and a metadata-confirmed `BackgroundResource.ResolvePath`; the unresolved `0x5686e00`, `0x5686c0c`, and `0x56868dc` addresses are not exact metadata method entries. Do not assign them guessed names.
- `ReadAsync` declares `Task<RuntimeSpineLoader.Files>`. Its instruction flow branches into task/runtime support code; exact metadata calls include `Task.ScheduleAndStart` at `0x4b8280c` and `0x4b82940`, `Task.get_IsCompleted`, `Task.AtomicStateUpdate`, `Interlocked.Exchange`, `ContingentProperties.SetCompleted`, and `Task.FinishStageThree`. This is concrete evidence that the asynchronous path uses Task scheduling/completion machinery, not merely a synchronous method renamed Async. The precise callback hand-off still needs the 4-instruction closure stub's raw listing.
- The callback method `<>c__DisplayClass4_0.<ReadAsync>b__0` is metadata method 25306 at RVA `0x3260250` (loaded address `0x3360250`). Its flow record says 4 reachable/decoded instructions and 40 P-code addresses, all P-code addresses outside the callback body, resolving to the `Read` entry. Therefore the decompiled callback C file is a misattributed cross-function decompilation, not the callback body. Run 24 did not emit a separate `.instructions.tsv` for this callback because its generated function name did not match the Spine-family listing filter; a follow-up script change must include this closure explicitly before using the exact adapter instructions.

### ReadAsync / Read / AtlasPages

- Metadata identifies `RuntimeSpineLoader.ReadAsync(string)` (method index 25294, RVA `0x325f1c0`) as returning `Task<RuntimeSpineLoader.Files>`; `Read(string,bool)` (25293, RVA `0x325ee08`) returns `RuntimeSpineLoader.Files`.
- The nested closure type `<>c__DisplayClass4_0` has captured fields `basePath` (string) and `supportsDxt5` (bool). Its `<ReadAsync>b__0` method (25306, RVA `0x3260250`) is a 4-instruction entry stub. Flow recovery records one skipped other-method entry, `0x335ee08`, and the decompiler's P-code provenance resolves entirely to `RuntimeSpineLoader.Read` rather than to the 4-instruction stub body. This is consistent with a tiny closure adapter that forwards the captured `basePath` and `supportsDxt5` into `Read`; the exact register moves should be verified from raw ARM64 listing before claiming the adapter implementation instruction-for-instruction.
- `ReadAsync` itself has 329 reachable decoded instructions, but only 11 P-code source addresses; the decompiler emits a short static-initialization/delegate-dispatch stub and does not recover the Task return expression. Treat the metadata return type as authoritative for the declared API, but do not infer which Task scheduler or worker implementation is used from this pseudocode alone. The current evidence points to the captured synchronous `Read` method being the callback body, not a separately recovered async state machine.
- `RuntimeSpineLoader.Read` has 179 reachable decoded instructions and 40 distinct P-code source addresses. Its body performs several static initializations, obtains a delegate-like object through an indirect invoke, retrieves a value from a singleton/static object chain, and calls an unresolved shared/native target at `0x05686294`; this call target is not an exact metadata method entry. It then stores the resulting object into the returned `Files` object at offset `+0x28`. The exact source of the files and the meaning of the unresolved call remain unknown.
- `RuntimeSpineLoader.AtlasPages` metadata-confirms `System.IO.StringReader`, `Spine.Atlas`, `Spine.Atlas.Dispose`, and `Spine.Unity.NoOpTextureLoader` constructor calls. Its pseudocode constructs/uses a StringReader and Spine Atlas, obtains page/region data through collections and virtual calls, and disposes the temporary Atlas object on the shown path.

### Metadata field-layout cross-check from Run 24

The linked metadata record provides concrete declared field types for the Spine loader data containers:

- `RuntimeSpineLoader.Files`: `BasePath : string`, `AtlasText : string`, `Skeleton : byte[]`, `Pages : Dictionary<string, RuntimeAtlasTexture.Source>`.
- `RuntimeSpineAsset`: `owned : HashSet<UnityEngine.Object>`, `references : int`, `<Asset>k__BackingField : Spine.Unity.SkeletonDataAsset`.
- `PageTextureLoader`: `materials : Dictionary<string, UnityEngine.Material>`.
- `<>c__DisplayClass4_0`: `basePath : string`, `supportsDxt5 : bool`.

This metadata strongly supports interpreting the two file-read results in `RuntimeSpineLoader.Read` as the Atlas text and skeleton byte array, with the page dictionary populated from Atlas page paths. The `EstimatedTextureBytes` method begins from the `owned` field at `this+0x10` and contains collection/enumerator plus virtual-dispatch loops. It is therefore traversing the owned Unity-object set; the exact texture subtype filter and per-texture size formula are still not resolved. Do not assume the total is simply width × height × 4 or DXT5 block size without recovering the virtual targets and arithmetic.

### Run 25: exact ReadAsync closure adapter and texture-byte estimator boundaries

Run 25 succeeded (workflow run `43009553595`, artifact `13850270986`, SHA-256 `fa83a88f3a67fb0338dec17eefe964c02f5e3db7f19b04a08f5610c8742c3bdc`). The artifact now includes an independent instruction listing for the nested callback.

**Exact callback machine code.** At loaded address `0x3360250`, the four instructions are:
- `ldr x8,[x0,#0x10]`
- `ldrb w1,[x0,#0x18]`
- `mov x0,x8`
- `b 0x0335ee08`

This proves that the closure's `basePath` field at +0x10 and `supportsDxt5` flag at +0x18 are passed as the two arguments to `RuntimeSpineLoader.Read(string,bool)`. It is a tail branch, not a separately implemented file loader. It does not alone identify the exact scheduling API used by the outer ReadAsync method.

**EstimatedTextureBytes: what the machine code does establish.** The function reads the `owned` field from `RuntimeSpineAsset` at +0x10, obtains/initializes cached collection/enumerator support objects, and performs multiple collection iterations with virtual getter calls. The loops include signed 64-bit overflow guards and an accumulator; other branches handle empty/null collections and enumeration cleanup. The linked metadata confirms `owned` is `HashSet<UnityEngine.Object>`, `references` is `int`, and the Asset backing field is `Spine.Unity.SkeletonDataAsset`. However, the virtual getter slot targets are not yet mapped to named methods. Consequently, the exact texture filtering predicate, size property (for example dimensions or format), and per-texture formula remain unresolved. The current evidence does not justify asserting a particular bytes-per-pixel or mipmap formula.

**ReadAsync scheduler caveat.** The four-instruction closure is now correctly isolated. The larger `ReadAsync` reachable flow includes Task runtime and completion paths, but the current decompiler output still does not provide a trustworthy high-level Task expression. The exact task creation/scheduling and callback registration relationship requires mapping the indirect calls and generic method instances; do not reduce this to an unqualified claim that it uses `Task.Run`.

### Run 25: exact per-texture byte estimator recovered

The same artifact contains `RuntimeAtlasTexture.EstimateTextureBytes` with 123 reachable ARM64 instructions. Unlike the higher-level `RuntimeSpineAsset.EstimatedTextureBytes` aggregation, this per-texture routine exposes a concrete size formula in the decompilation and instruction stream.

After a Unity-object null/equality guard, it obtains a format code and dimensions through virtual calls. The arithmetic is:

- Format code `12`: block-compressed path, `ceil(width/4) × ceil(height/4) × 16 bytes`.
- Format code `10`: block-compressed path, `ceil(width/4) × ceil(height/4) × 8 bytes`; the code includes a signed-overflow-aware half-size operation.
- Other formats: `width × height × (3 if format code is 3, otherwise 4)`.
- The resulting estimate is then left-shifted by the low bit of another virtual getter result. This is a conditional ×2 adjustment; the exact property represented by vtable slot `+0x1e8` is not yet mapped, so do not label it mipmaps or readability without further metadata/vtable evidence.

The dimension rounding is implemented as `(dimension + 3) >> 2` for the block paths. The code does not currently show a mip-level summation loop in this method. This is a per-texture estimate, not yet proof that the outer `RuntimeSpineAsset.EstimatedTextureBytes` returns the sum of precisely these values: its virtual dispatch chain still needs to be mapped to establish that relationship.

## Important caveats

1. The `metadata_method_entry` flag is the strongest call-target identity evidence in this report; other executable candidates can be native helpers, shared runtime helpers, inlined code, or unresolved managed targets.
2. Ghidra output contains PIC-construction warnings, removed-unreachable-block warnings, and unresolved indirect jump-table warnings. Treat the pseudocode as an analysis aid, not compilable source.
3. A one-instruction `PageTextureLoader.Unload` does not imply that `RuntimeAtlasTexture.DestroyTexture`, `RuntimeSpineAsset.Dispose`, or other ownership paths are no-ops.
4. These results concern the Android 1.0 ARM64 IL2CPP binary. They do not prove iOS 0.7.9.2 behavioral equivalence, nor do they constitute a completed iOS port or IPA.

## Next recovery priorities

1. Recover the concrete indirect/virtual callees used by `RuntimeSpineLoader.Read` and the async state-machine path behind `ReadAsync`.
2. Recover full instruction-level ownership field layout for `RuntimeSpineAsset`, reconciling the `+0x18` count used by `HasConsumers/Dispose` with the fast-path count observed in `Retain`.
3. Recover the exact collection/element semantics and arithmetic in `get_EstimatedTextureBytes`.
4. Cross-check `PageTextureLoader.Load` and Atlas page/material ownership against `RuntimeAtlasTexture.Load/CreateTexture/DestroyTexture`.
5. Only after those are resolved, compare with iOS 0.7.9.2 resource loading and determine the minimum compatibility shim surface.
