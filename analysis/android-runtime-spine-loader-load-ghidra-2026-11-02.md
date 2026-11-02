# RuntimeSpineLoader.Load — focused Ghidra decompilation (Android 1.0)

**Status:** preliminary native pseudocode extracted; REA CLI confirmation is still running in workflow run `41373826470`. This document does not claim a complete Unity project or IPA build.

## Evidence and reproducibility

- Repository branch: `progress/azurearchive-ios-port-20261031`
- Ghidra-focused extraction run: `41373269434` (the focused Ghidra script completed and wrote pseudocode; the workflow subsequently stopped at a function-boundary guard before the REA CLI step).
- Artifact: [azurearchive-rea-ghidra-spine-decompile, artifact 13110266709](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41373269434/artifacts/13110266709)
- Ghidra: 12.1.4, AARCH64 little-endian, Java 21.
- Binary SHA-256: `7f15b8e7557eb7948f8bea746c629fb15d442d05c649ab2db3667ae08aa2c9b6` (120,315,032 bytes).
- Metadata SHA-256: `717236ea9c650b4fcd373e0afc7c03c25d5949726bebe251d3e8301e4da7a144` (17,132,296 bytes; metadata version 29).
- Focused pseudocode: 8,974 characters; SHA-256 `4e625ff86d62d954efcaff70b52a2384e43cda1980efa357c006d4463eaaa12e`.
- The full raw Ghidra pseudocode is `runtime-spine-loader-load-ghidra.c` inside the artifact. The accompanying JSON records 20 decoded instructions in the metadata-bounded seed interval.

The metadata inventory covers 77 code-generation modules and 152,789 methods. It linked 136,430 method-pointer slots; 16,359 slots were observed as no-body. Method-pointer matching uses module name, method count, and metadata token. This is structural pairing evidence, not cryptographic proof that the selected APK and metadata are from the same build; generic method instances, adjustor thunks, and invokers were not linked.

## Method identity and address cross-check

Metadata identifies:

- Type index 1231: `RuntimeSpineLoader` (`Assembly-CSharp.dll`).
- Method index 25295: `Load`.
- Managed signature: `RuntimeSpineAsset Load(string basePath, UnityEngine.Material material, Spine.Unity.BlendModeMaterials.TemplateMaterials blends, bool quiet)`.
- ELF RVA: `0x325f2dc`.
- Next metadata method pointer, `Create`, method index 25296: RVA `0x325f32c`.

The ELF reports image base `0x0`. The REA/Ghidra seed mapping used a Ghidra load base of `0x100000`, so:

- `Load`: RVA `0x325f2dc` → Ghidra address `0x335f2dc`.
- `Create`: RVA `0x325f32c` → Ghidra address `0x335f32c`.

The address mapping agrees with the previously generated REA method seeds. The focused script imported the exact `libil2cpp.so` whose SHA-256 is recorded above.

## Decompiled evidence

Ghidra emitted this prototype:

```c
long RuntimeSpineLoader_Load(undefined8 param_1, undefined8 param_2, long param_3)
```

This native prototype is not yet reconciled with the four-parameter managed signature above. The metadata signature is the authoritative managed declaration; do not infer a final AArch64 argument mapping from the current Ghidra prototype.

Key recovered call sites:

```c
uVar3 = func_0x0681bb90(0xc,0);
func_0x0335ee08(param_1,uVar3 & 1);  // metadata maps 0x335ee08 to RuntimeSpineLoader.Read

// ... construction / null-check paths ...

func_0x0335fc74(lVar5,lVar6);        // metadata maps 0x335fc74 to RuntimeSpineAsset.Own
func_0x0335fc74(lVar5,unaff_x23);    // another ownership registration

// cleanup path:
func_0x0335fd1c(lVar5);              // metadata maps 0x335fd1c to RuntimeSpineAsset.Dispose
```

The call-address cross-checks are:

| Ghidra address | ELF RVA | Metadata method |
|---|---:|---|
| `0x335ee08` | `0x325ee08` | `RuntimeSpineLoader.Read` (method 25293) |
| `0x335fc74` | `0x325fc74` | `RuntimeSpineAsset.Own` (method 25313) |
| `0x335fd1c` | `0x325fd1c` | `RuntimeSpineAsset.Dispose` (method 25316) |

### What the pseudocode supports

1. `Load` enters the resource-reading path through `RuntimeSpineLoader.Read(basePath, supportsDxt5)`.
2. The returned data is used in a multi-stage construction path. The pseudocode shows repeated null checks, a loop, and multiple early/normal returns; helper functions are still unnamed.
3. The loader registers created objects through `RuntimeSpineAsset.Own`. A cleanup branch calls `RuntimeSpineAsset.Dispose`.
4. The decompiled function uses static pointer references such as `PTR_DAT_06f65b28`, `PTR_DAT_06f65b30`, and related entries. Their managed field/string identities are not yet resolved.
5. No literal resource path was recovered from this function. Path parsing and file selection are expected to be established from `Read`, `ReadAsync`, and `AtlasPages`; do not claim a concrete path until those methods are decompiled.

## Boundary and analysis limitations

- The successful focused extraction used Ghidra `-noanalysis` and an explicit metadata interval `[0x335f2dc, 0x335f32b]`. It decoded 20 instructions and produced the pseudocode above without running whole-program auto-analysis.
- A subsequent attempt to let Ghidra infer the complete function body followed control flow into the next metadata method entry `0x335f32c` (`RuntimeSpineLoader.Create`) and was rejected by a guard. Therefore the exact native body boundary and any shared/out-of-line blocks are not yet settled.
- The emitted pseudocode contains out-of-interval labels (for example around `0x335f960`) and Ghidra warnings about possible PIC construction and unreachable blocks. These blocks must be checked against their own instruction ranges and exception/LSDA metadata before assigning them to `Load`.
- No whole-program analysis ran in this focused project. GCC exception/LSDA recovery was not performed; the earlier full-analysis attempts disabled the `GCC Exception Handlers` analyzer because of the known Ghidra 12.1.x PIE/image-base issue. Exception landing-pad coverage remains limited.
- Ghidra's current prototype has three native parameters while metadata declares four managed parameters. The ABI, hidden IL2CPP arguments, stack arguments, and register mapping remain unresolved.
- The decompilation is reverse-engineering evidence, not recovered C# source and not proof that a Unity project has been restored.

## Next function work, in order

1. `RuntimeSpineLoader.Create` — method 25296, RVA `0x325f32c`, Ghidra address `0x335f32c`; return type `RuntimeSpineAsset`, parameters `Files, Material, TemplateMaterials, bool`.
2. `PageTextureLoader.Load` — method 25300, RVA `0x326004c`, Ghidra address `0x336004c`.
3. `PageTextureLoader.Unload` — method 25301, RVA `0x32601c4`, Ghidra address `0x33601c4`.
4. `RuntimeSpineAsset.Dispose` — method 25316, RVA `0x325fd1c`, Ghidra address `0x335fd1c`.
5. Then trace `RuntimeSpineLoader.Read` (25293), `ReadAsync` (25294), and `AtlasPages` (25292) to resolve actual resource paths, page enumeration, image decoding, and failure handling.

Each next target must be mapped from the current metadata evidence, bounded/disassembled independently, and checked for overlap before its pseudocode is accepted. Do not reuse the `Load` interval for another method.

## Build status

No complete Unity project, iOS Player, or IPA has been produced by this workflow. REA/Ghidra provides native reverse-engineering evidence only. The repository still needs the required Unity project inputs and a supported Unity/macOS/Xcode build environment before an iOS IPA can be claimed.
