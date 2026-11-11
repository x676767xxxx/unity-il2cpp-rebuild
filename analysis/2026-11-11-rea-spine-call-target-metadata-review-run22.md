# Run 22 Spine call-target / metadata cross-reference review

Date: 2026-11-11
Workflow: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/43006747443
Artifact: `13849255925`
Artifact SHA-256: `c6975f7fed5382d08f884e5a4e75cc85517e1360679d53773d612bb5056d3a66`
Input metadata SHA-256: `717236ea9c650b4fcd373e0afc7c03c25d5949726bebe251d3e8301e4da7a144`

## Scope and verification

Run 22 completed all 11 workflow steps. The artifact contains three call-target candidate TSV files for `RuntimeSpineLoader.Load`, `RuntimeSpineAsset.Retain`, and `RuntimeSpineAsset.get_EstimatedTextureBytes`. Each TSV has a 14-column header and every data row has 14 columns.

Candidate addresses were cross-referenced against the 152,789-entry IL2CPP metadata method-address table in `linked-evidence.json`. The Ghidra program addresses in this run correspond to metadata RVAs plus `0x100000`; subtracting that relocation delta maps the candidate to the exact metadata method entry. This is an address-space normalization, not evidence that every non-metadata candidate is a managed method.

All candidate destinations in the three tables were in initialized executable memory and had an instruction decoded. This does not automatically establish that each target is a standalone managed method: some are native/runtime stubs, branch thunks, or other code.

## Confirmed direct-call targets from RuntimeSpineLoader.Load

The recovered pseudocode at entry `0x0335f2dc` contains these statically resolved calls, matched to IL2CPP metadata by normalized address:

| Ghidra target | Metadata method | Call-site address(es) in Load | Interpretation |
|---|---|---|---|
| `0x335ee08` | `RuntimeSpineLoader.Read(string,bool)` | `0x335f30c` | Read Spine files |
| `0x335fc74` | `RuntimeSpineAsset.Own(object)` | `0x335f740, 0x335f7c0, 0x335f89c, 0x335f8a8` | Register owned resources |
| `0x335fd1c` | `RuntimeSpineAsset.Dispose()` | `0x335fb18` | Cleanup path |
| `0x5f36e54` | `Spine.Unity.SkeletonDataAsset.GetSkeletonData(bool)` | `0x335f8bc` | Obtain runtime skeleton data |
| `0x5f3acac` | `Spine.Unity.SkeletonDataAsset.CreateRuntimeInstance(...)` | `0x335f878` | Create runtime skeleton-data asset |
| `0x5f3addc` | `Spine.Unity.SkeletonDataAsset.SetupRuntimeBlendModeMaterials(...)` | `0x335f8d8` | Configure blend-mode materials |
| `0x5f3ba50` | `Spine.Unity.SpineAtlasAsset.CreateRuntimeInstance(...)` | `0x335f7b0` | Create runtime atlas asset |
| `0x681bb90` | `UnityEngine.SystemInfo.SupportsTextureFormat(...)` | `0x335f300` | Texture-format capability branch |
| `0x6813c00` | `UnityEngine.Object.op_Equality(object,object)` | `0x335f4bc` | Unity object null/equality check |
| `0x6813fa0` | `UnityEngine.Object.set_name(string)` | `0x335f730` | Assign Unity object name |
| `0x6813390` | `UnityEngine.TextAsset..ctor(string)` | `0x335f6e0` | Construct TextAsset |
| `0x5f360b0` | `Native.SpineConverter.Converter.CastSkeletonDataToDstVersion(...)` | `0x335f7ec` | Spine data version conversion |
| `0x5d47a40` | `System.IO.Path.GetFileName(string)` | `0x335f708` | File-name handling |
| `0x5d7aaa8` | `System.InvalidOperationException..ctor(string)` | `0x335f938` | Exception construction |
| `0x63a0324` | `System.IO.InvalidDataException..ctor(string)` | `0x335f9b4, 0x335fa24` | Invalid-data exception paths |
| `0x5bd7ad8` | `System.String.Concat(string,string)` | multiple | Path/name string construction |

The call order visible in the bounded pseudocode begins with texture-format support detection, then `Read`, followed by atlas/skeleton runtime instance creation and asset ownership registration. The successful return path obtains skeleton data and optionally configures blend-mode materials. Error/cleanup branches include `Dispose`.

This is a useful, evidence-backed high-level call chain. The bounded CFG still reports unresolved/removed blocks and PIC-construction warnings; therefore it is not a complete source-level control-flow proof.

## Confirmed direct-call targets from RuntimeSpineAsset.Retain

Metadata-matched calls include:

- `System.ObjectDisposedException..ctor(string)` at `0x5d89484` (one recorded xref).
- `System.Object..ctor()` at `0x5dbc718` (seven xrefs).
- `UnityEngine.Object.op_Equality(object,object)` at `0x6813c00` (four xrefs).
- `System.Linq.Error.ArgumentNull(string)` at `0x5f72164` (13 xrefs).
- `System.Linq.Error.ArgumentOutOfRange(string)` at `0x5f721c0` (six xrefs).

The decompiled body starts with a reference/consumer-count fast path: if the integer at the observed object offset is nonzero, it increments the count and returns. Otherwise it proceeds into substantially more complex collection/enumerator and resource-related code. The decompiler output contains unresolved indirect dispatch/jump-table warnings. Do not infer the full retain/ownership algorithm from this pseudocode alone.

## Confirmed direct-call targets from get_EstimatedTextureBytes

Metadata-matched calls include:

- `System.Linq.Error.ArgumentNull(string)` at `0x5f72164` (four xrefs).
- `System.Linq.Error.NoElements()` at `0x5f722f4` (three xrefs).

The body performs collection/enumerator operations and arithmetic accumulation with signed-overflow guards in the recovered pseudocode. This is consistent with a collection-based estimate path, but the exact collection element and texture-byte semantics still need to be recovered from the IL2CPP method body and its helper calls.

## Remaining blockers

1. The P-code address sets remain much larger than the bounded target function bodies:
   - `Load`: 188 distinct P-code addresses, 185 outside the target body.
   - `Retain`: 1,306, 1,295 outside.
   - `get_EstimatedTextureBytes`: 622, 572 outside.
2. The extra addresses are real decoded executable instructions, but their attribution may include inline/shared helper code, native/runtime code, or decompiler address propagation. Do not auto-create a function for every address.
3. The call-target exporter currently runs only for these three selected methods. The next pass should extend candidate export to the rest of the Spine family, especially `RuntimeSpineLoader.Create`, `RuntimeSpineAsset.Own/Dispose`, `PageTextureLoader.Load/Unload`, `RuntimeSpineLoader.Read/ReadAsync`, and `RuntimeSpineLoader.DestroyOwned`.
4. After call-target mapping is extended, review each candidate against the full metadata method table, executable bytes, xrefs, prologue and call-site context. Only then consider targeted function-boundary recovery.

## Conclusion

Run 22 provides a stronger, metadata-grounded direct-call map for the three difficult Spine methods. It confirms the main `Load` pipeline touches the Spine converter, atlas and skeleton runtime instance creation, blend-mode material setup, Unity texture-format capability checks, and `RuntimeSpineAsset` ownership/cleanup.

It does **not** prove a complete Android-to-iOS port, complete CFG recovery, correct lifetime behavior on iOS, or a buildable IPA. The next engineering task is to broaden call-target evidence across the remaining Spine ownership and texture-loader methods.
