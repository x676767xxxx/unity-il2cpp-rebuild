# REA/Ghidra findings: Android Spine resource I/O (2026-11-11)

## Run and input provenance

- Repository: `x676767xxxx/unity-il2cpp-rebuild`
- Branch: `progress/azurearchive-ios-port-20261031`
- Workflow run: [42995578057](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42995578057)
- Workflow result: success; all 11 analysis/verification steps completed.
- Artifact: `azurearchive-rea-spine-resource-io` (4,338,431 bytes; artifact ID 13843473556).
- APK input: Android 1.0 release asset `base.apk`.
- ELF: `libil2cpp.so`, ARM64, 120,315,032 bytes, SHA-256 `7f15b8e7557eb7948f8bea746c629fb15d442d05c649ab2db3667ae08aa2c9b6`.
- Metadata: `global-metadata.dat`, 17,132,296 bytes, metadata version 29, SHA-256 `717236ea9c650b4fcd373e0afc7c03c25d5949726bebe251d3e8301e4da7a144`.
- REA linked 152,789 method pointers; this run did not request generic method instances.

## Extracted methods

All five target ranges were resolved from the metadata-linked method pointer table, explicitly disassembled, decompiled, and their pseudocode SHA-256/length checked against the generated metadata JSON.

| Method | ELF RVA | Ghidra seeded instruction count | Pseudocode chars | Initial interpretation |
|---|---:|---:|---:|---|
| RuntimeAtlasTexture.TryPngSize | 0x325c770 | 136 | 3,406 | PNG signature/IHDR and dimensions validation, plus a size/overflow safety check |
| RuntimeSpineLoader.DestroyOwned | 0x325e994 | 51 | 2,809 | Ownership/state checks followed by different indirect cleanup paths |
| RuntimeSpineLoader.AtlasPages | 0x325ea60 | 225 | 7,137 | Builds/iterates a collection associated with atlas pages; includes enumerator-like and disposal/cleanup paths |
| RuntimeSpineLoader.Read | 0x325ee08 | 179 | 2,833 | Reaches a resource-read-related helper and returns an object/result; full control flow is not yet established |
| RuntimeSpineLoader.ReadAsync | 0x325f1c0 | 69 | 3,630 | Current pseudocode is insufficient: it largely shows initialization/wrapper behaviour while Ghidra reports many removed out-of-range blocks |

## Evidence-based observations

### TryPngSize

The pseudocode checks a minimum buffer length, the 8-byte PNG signature, and the IHDR marker. It reads width and height through helper calls at offsets corresponding to PNG IHDR fields, rejects invalid/zero dimensions and alignment cases, and applies a computed upper-bound check before reporting success. This is relevant to texture allocation safety, but it does not establish the complete texture decode/upload path.

### AtlasPages

The extracted body contains collection construction and iteration, indirect calls through runtime metadata/interface tables, and cleanup paths. It is consistent with an atlas-page enumeration routine. The indirect calls have not yet been symbolically resolved, so their exact managed method identities and exception semantics remain open.

### DestroyOwned

The body first checks an object/state predicate and then selects between two indirect cleanup dispatch paths. The actual target methods behind those dispatches are not resolved in this run. Do not yet translate this as a simple native free/dispose operation.

### Read and ReadAsync limitations

The Ghidra output includes warnings about possible PIC branch handling and numerous blocks removed as unreachable, including blocks at addresses outside the explicitly seeded metadata interval. The preparation script intentionally used `-noanalysis` and seeded only a metadata-bounded interval. Its verifier proves that the seeded entry/range, instruction count, output length and hash match the recorded metadata; it does **not** prove complete CFG, exception-handler or async-state-machine recovery.

In particular, `ReadAsync` must not be treated as recovered just because a non-empty pseudocode file exists. Its decompilation needs a second pass that follows branch targets and resolves generated async state-machine methods.

## Next REA/Ghidra pass

1. Query the full metadata records for the five methods: signatures, return types, parameters, attributes, declaring types and generic relationships.
2. Identify all direct branch targets, cross-references and out-of-line blocks for `Read` and `ReadAsync`; run focused disassembly/decompilation over those targets rather than only the initial method-pointer interval.
3. Resolve indirect calls in `AtlasPages` and `DestroyOwned` against metadata method pointers, vtables/interface maps and call-site evidence.
4. Locate async generated state-machine types and methods associated with `ReadAsync`; inspect MoveNext/awaiter paths and exception/finally cleanup.
5. Compare the verified Android behaviour against the old iOS 0.7.9.2 implementation only after the Android call graph is sufficiently grounded.

## Porting decision

No iOS source changes are justified by this evidence alone. Android ARM64 RVAs are not iOS addresses; Ghidra pseudocode is not recovered C# source; the workflow produced neither a recovered Unity project nor an IPA. The next deliverable should be a call-graph/signature evidence report, not a speculative code port.
