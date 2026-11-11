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

### ReadAsync / Read / AtlasPages

- `RuntimeSpineLoader.ReadAsync` has only two non-metadata helper candidates in its candidate TSV and its pseudocode is a very small initialization/delegate-dispatch stub. It is not evidence of the asynchronous file-read implementation itself; likely state-machine or delegate entrypoints need separate recovery.
- `RuntimeSpineLoader.Read` similarly has no metadata-confirmed managed call target in its candidate list, so its data path needs direct instruction/callee recovery.
- `RuntimeSpineLoader.AtlasPages` metadata-confirms `System.IO.StringReader`, `Spine.Atlas`, `Spine.Atlas.Dispose`, and `Spine.Unity.NoOpTextureLoader` constructor calls. Its pseudocode constructs/uses a StringReader and Spine Atlas, obtains page/region data through collections and virtual calls, and disposes the temporary Atlas object on the shown path.

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
