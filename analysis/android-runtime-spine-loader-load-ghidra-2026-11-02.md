# RuntimeSpineLoader / Spine page lifecycle — focused Ghidra evidence (Android 1.0)

**Status:** five native pseudocode outputs were extracted by Ghidra 12.1.4 and hash-verified in workflow run `41375306118`. Metadata-to-address mappings were cross-checked. Function-boundary completeness, out-of-line control-flow blocks, AArch64/IL2CPP argument mapping, and exception/LSDA coverage remain provisional. This is reverse-engineering evidence, not recovered C# source, a restored Unity project, or an IPA.

## Evidence and reproducibility

- Repository branch: `progress/azurearchive-ios-port-20261031`
- Extraction commit: `1f7bc7ab9740baf3e1d18e2ab4b5d9d003f81f21`
- Successful workflow run: [41375306118](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41375306118) — all steps succeeded, including extraction, hash/identity/range verification, and artifact upload.
- Artifact: [azurearchive-rea-ghidra-spine-decompile, artifact 13111006971](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41375306118/artifacts/13111006971)
- Artifact ZIP: 4,339,318 bytes; SHA-256 `66642ed0abe821c3061b363a35cec997c83528527346682d3313cdbf1e32760d`.
- Ghidra: 12.1.4, AARCH64 little-endian, Java 21. The ELF image base is `0x0`; the Ghidra import base used by the seed mapping is `0x100000`.
- Binary SHA-256: `7f15b8e7557eb7948f8bea746c629fb15d442d05c649ab2db3667ae08aa2c9b6` (120,315,032 bytes).
- Metadata SHA-256: `717236ea9c650b4fcd373e0afc7c03c25d5949726bebe251d3e8301e4da7a144` (17,132,296 bytes; metadata version 29).
- The artifact contains each raw `*-ghidra.c` pseudocode file, its JSON sidecar, `linked-evidence.json`, the focused Ghidra log, and `focused-ghidra-verification.json`.

The REA metadata inventory covers 77 code-generation modules and 152,789 methods. It linked 136,430 method-pointer slots; 16,359 were observed as no-body. Method-pointer matching uses module name, method count, and metadata token. This is structural pairing evidence, not cryptographic proof that the selected APK and metadata are from the same build; generic method instances, adjustor thunks, and invokers were not linked.

### Why the workflow changed

Run `41373826470` successfully ran the direct focused Ghidra script for `Load`, but the subsequent REA CLI provider call started a separate Ghidra project and timed out at its 600,000 ms startup deadline. No `OutOfMemoryError` was confirmed. The raw focused Ghidra output was already present in that run's artifact. The workflow was therefore changed to use the successful direct Ghidra `-noanalysis` import and repeated focused `-postScript` invocations, instead of paying for a second full-binary REA provider import. Run `41375306118` then extracted and verified all five targets. No image-base or Ghidra-version change was made.

## Metadata identities and method ranges

All addresses below are cross-checked against the linked metadata artifact. Ghidra address = ELF RVA + `0x100000`. End-exclusive bounds use the next distinct metadata method-pointer address in the binary, not merely the next metadata row.

| Managed method | Type / method index | Managed signature | ELF RVA | Ghidra entry | Bounded interval (Ghidra) |
|---|---:|---|---:|---:|---|
| `RuntimeSpineLoader.Load` | 1231 / 25295 | `RuntimeSpineAsset Load(string basePath, UnityEngine.Material material, TemplateMaterials blends, bool quiet)` | `0x325f2dc` | `0x335f2dc` | `[0x335f2dc, 0x335f32c)` |
| `RuntimeSpineLoader.Create` | 1231 / 25296 | `RuntimeSpineAsset Create(Files files, UnityEngine.Material material, TemplateMaterials blends, bool quiet)` | `0x325f32c` | `0x335f32c` | `[0x335f32c, 0x335fbdc)` |
| `PageTextureLoader.Load` | 1227 / 25300 | `void Load(Spine.AtlasPage page, string path)` | `0x326004c` | `0x336004c` | `[0x336004c, 0x33601c4)` |
| `PageTextureLoader.Unload` | 1227 / 25301 | `void Unload(object texture)` | `0x32601c4` | `0x33601c4` | `[0x33601c4, 0x33601c8)` |
| `RuntimeSpineAsset.Dispose` | 1233 / 25316 | `void Dispose()` | `0x325fd1c` | `0x335fd1c` | `[0x335fd1c, 0x335ffc8)` |

The bounds for `Create`, `Dispose`, and `Unload` were selected using the nearest following code pointer across the full linked method-address table. In particular, `Create` ends before `RuntimeSpineAsset..ctor` (method 25317, RVA `0x325fbdc`), `Dispose` ends before `CanLoadPngAsDxt5` (method 25297, RVA `0x325ffc8`), and `Unload` is followed by a generated `<>c..cctor` entry at RVA `0x32601c8`.

### Extracted output and verification

| Target | Decoded instructions in bounded interval | Pseudocode characters | SHA-256 |
|---|---:|---:|---|
| `RuntimeSpineLoader.Load` | 20 | 8,974 | `4e625ff86d62d954efcaff70b52a2384e43cda1980efa357c006d4463eaaa12e` |
| `RuntimeSpineLoader.Create` | 509 | 8,808 | `6f05cd50df0080ab4db8df62389252a1cdfc4555111745bfe672848c24aa30e3` |
| `PageTextureLoader.Load` | 94 | 1,957 | `c154617de0914b37bef90955e379d1aa5bb38565d37ab2a6e3c0c877f5a440a8` |
| `PageTextureLoader.Unload` | 1 | 53 | `2e33e5e967b4c0b4a24c13369fa143d67e9ccc25a056c7adabf7acdc6443f8cf` |
| `RuntimeSpineAsset.Dispose` | 170 | 4,488 | `7bb6a0a11a9b3c1181733855d4a86b6d32820ba83b5ba6a1c2b1d38ca4033f9b` |

The workflow verifier checked target identity, entry address, end-exclusive bound, body range, non-empty output, character count, and SHA-256 for every target. This verifies that Ghidra produced stable pseudocode for the bounded ranges. It does **not** prove that each native function's complete CFG, all out-of-line blocks, or exception paths are included.

## Recovered behavior

### `RuntimeSpineLoader.Load` and `RuntimeSpineLoader.Create`

Ghidra's native prototypes are currently:

```c
long RuntimeSpineLoader_Load(undefined8 param_1, undefined8 param_2, long param_3)
long RuntimeSpineLoader_Create(undefined8 param_1, undefined8 param_2, long param_3)
```

These three-parameter native prototypes do not yet reconcile with the four-parameter managed signatures in metadata. Do not infer the final AArch64 register/stack mapping or hidden IL2CPP arguments from them.

The first recovered call in `Load` is:

```c
uVar3 = func_0x0681bb90(0xc,0);
func_0x0335ee08(param_1,uVar3 & 1);
```

The address `0x335ee08` maps to `RuntimeSpineLoader.Read` (method 25293, RVA `0x325ee08`). This supports that `Load` reads the resource files before entering the construction path.

The `Load` and `Create` pseudocode outputs are heavily overlapping: after the initial `Read` call, the `Load` output contains essentially the same construction body emitted separately for `Create`. This is consistent with a wrapper/branch or tail-transfer into `Create`, but the exact branch instruction and complete function boundary still need instruction-level confirmation. Do not count the overlapping blocks as two independent implementations.

Metadata cross-checks for calls inside the construction path:

| Ghidra address | ELF RVA | Resolved metadata method |
|---|---:|---|
| `0x335fc74` | `0x325fc74` | `RuntimeSpineAsset.Own` (25313) |
| `0x335fd1c` | `0x325fd1c` | `RuntimeSpineAsset.Dispose` (25316) |
| `0x5ad7ad8` | `0x5ad7ad8` | `System.String.Concat` (54262) |
| `0x5c47a40` | `0x5c47a40` | `System.IO.Path.GetFileName` (66857) |
| `0x5c7aaa8` | `0x5c7aaa8` | `System.InvalidOperationException..ctor` (55694) |
| `0x5e360b0` | `0x5e360b0` | `Native.SpineConverter.Converter.CastSkeletonDataToDstVersion` (142792) |
| `0x5e36e54` | `0x5e36e54` | `Spine.Unity.SkeletonDataAsset.GetSkeletonData` (142902) |
| `0x5e3acac` | `0x5e3acac` | `Spine.Unity.SkeletonDataAsset.CreateRuntimeInstance` (142898) |
| `0x5e3addc` | `0x5e3addc` | `Spine.Unity.SkeletonDataAsset.SetupRuntimeBlendModeMaterials` (142899) |
| `0x5e3ba50` | `0x5e3ba50` | `Spine.Unity.SpineAtlasAsset.CreateRuntimeInstance` (142919) |
| `0x6713390` | `0x6713390` | `UnityEngine.TextAsset..ctor` (94390) |
| `0x6713c00` | `0x6713c00` | `UnityEngine.Object.op_Equality` (94463) |
| `0x6713fa0` | `0x6713fa0` | `UnityEngine.Object.set_name` (94418) |
| `0x62a0324` | `0x62a0324` | `System.IO.InvalidDataException..ctor` (87881) |

This establishes a runtime construction path involving Unity `TextAsset` objects, path basename handling, Spine skeleton-data creation, atlas creation, version conversion, blend-mode material setup, and owned-object registration. The code also has null checks and cleanup paths that call `RuntimeSpineAsset.Dispose`. A literal asset path or bundle filename was **not** recovered from these two functions; the actual selection logic remains in `Read`, `ReadAsync`, and `AtlasPages`.

### `PageTextureLoader.Load`

Ghidra emitted:

```c
void PageTextureLoader_Load(long param_1, long param_2)
```

Metadata declares `void Load(Spine.AtlasPage page, string path)`. The current native prototype is not sufficient to settle the managed `this`/parameter ABI.

The pseudocode shows:

1. A null/invalid-object guard before loading.
2. A call to `func_0x05688728(...)` with object data, a path-like value, an output pointer, and a static argument. This address did not match a method pointer in the linked metadata table, so it remains an unresolved image/texture-loading helper; it must not be labelled as `ImageConversion.LoadImage` or another API without additional evidence.
3. The returned texture-like object is stored in an object field, then `UnityEngine.Material.get_mainTexture` is called (method 92008).
4. The loader calls `UnityEngine.Material.EnableKeyword` (92045) or `DisableKeyword` (92046) depending on a flag.
5. A failure path constructs an `InvalidDataException` (method 87881) using `System.String.Concat` (54262).

This confirms a texture-loading/material-configuration path and an explicit invalid-data failure path, but not the exact image decoder, file format, or keyword name.

### `PageTextureLoader.Unload`

Ghidra emitted only:

```c
void PageTextureLoader_Unload(void)
{
  return;
}
```

The bounded interval contains one instruction. Metadata declares one managed `object texture` parameter, so the native prototype/parameter mismatch remains unresolved. The current interval shows no explicit release operation; this may be an empty stub or a thunk whose cleanup is elsewhere. Do not conclude that texture memory is never released until the instruction bytes and callers are checked.

### `RuntimeSpineAsset.Dispose`

Ghidra emitted:

```c
ulong RuntimeSpineAsset_Dispose(ulong param_1)
```

Metadata declares `void Dispose()`. The native return/prototype mismatch is not resolved.

The pseudocode supports the following lifecycle:

- Reads and decrements an integer at object offset `+0x18`; expensive cleanup is entered only when the value reaches zero. This is consistent with the metadata field `references: int`, but the field offset has not yet been independently proven by a field-offset dump.
- Calls `RuntimeSpineAsset.get_Materials` (method 25315, Ghidra address `0x33604f4`) and registers objects with `RuntimeSpineAsset.Own` (25313).
- Calls `Spine.Atlas.Dispose` (method 141847, Ghidra address `0x5f09fc8`) in the cleanup path.
- Calls `RuntimeSpineLoader.DestroyOwned` (method 25291, Ghidra address `0x335e994`) while releasing tracked objects.
- Also references `RuntimeAtlasTexture.TryPngSize` (method 25283, Ghidra address `0x335c770`) and `UnityEngine.Object.op_Inequality` (method 94464, Ghidra address `0x670d258`).

This is consistent with reference-counted ownership and a final-release path that disposes Spine atlas data and owned Unity objects. The exact managed field layout and exception cleanup still need confirmation.

## Boundary and analysis limitations

- The extraction imports the ELF with Ghidra `-noanalysis` and explicitly bounds each function interval using linked metadata method pointers. It does not run whole-program auto-analysis.
- The `Load` output contains out-of-interval branch labels (for example around `0x335f960`) and is heavily overlapping with `Create`. Those blocks must be checked against disassembly before assigning them conclusively.
- Ghidra reports possible PIC construction and unreachable blocks. These warnings are retained in the raw artifact; they have not been manually resolved.
- GCC exception/LSDA analysis was not run. The earlier full-analysis path hit the known Ghidra 12.1.x PIE/LSDA issue; the current focused `-noanalysis` path avoids that analyzer rather than recovering exception landing pads. Exception-path coverage remains limited.
- The separate REA CLI provider cross-check is not part of the successful batch because it repeatedly re-imported the 120 MB binary and timed out after 600,000 ms. The direct Ghidra script output is real decompiler pseudocode with per-file hashes, but there is no independent REA-provider result.
- Metadata-to-native pairing remains structural: module name, method count, and metadata token. Generic method instances, adjustor thunks, invokers, and exact field offsets were not fully resolved.
- These are native decompiler outputs, not restored C# source and not proof of a complete Unity project.

## Next analysis tasks

1. **Resolve `Load` → `Create` control flow.** Disassemble the final instructions in the `Load` interval and the first instructions at `Create`; verify the branch/tail-transfer and determine which out-of-line blocks belong to each function.
2. **Resolve `PageTextureLoader.Unload`.** Record its single instruction bytes/mnemonic and follow callers/indirect cleanup before treating it as a no-op.
3. **Decompile `RuntimeSpineLoader.Read` (method 25293, RVA `0x325ee08`, Ghidra `0x335ee08`), `ReadAsync` (25294, RVA `0x325f1c0`, Ghidra `0x335f1c0`), and `AtlasPages` (25292, RVA `0x325ea60`, Ghidra `0x335ea60`).** These are the next priority for actual resource filenames, atlas page enumeration, byte loading, PNG/DXT5 decisions, and error handling.
4. After those, inspect `RuntimeSpineLoader.DestroyOwned` (25291) and `RuntimeAtlasTexture.TryPngSize` (25283) to close the resource-ownership and image-metadata paths.

Each new method must be mapped from the current metadata evidence and bounded by the nearest next code entry. Do not reuse another method's interval or infer a managed API from a raw function address alone.

## Build status

No complete Unity project, iOS Player, or IPA has been produced by this workflow. The repository still lacks the full `Assets`, `Packages/manifest.json`, and `ProjectSettings` project inputs identified earlier. REA/Ghidra evidence does not replace a Unity project, and the final iOS Player/IPA still requires a supported Unity, macOS, and Xcode build environment.
