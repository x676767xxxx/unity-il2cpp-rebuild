# REA Spine control-flow recovery — failure analysis and corrected workflow (2026-11-11)

## Scope and provenance

- Repository: `x676767xxxx/unity-il2cpp-rebuild`
- Branch: `progress/azurearchive-ios-port-20261031`
- Branch head after workflow corrections: `38cf09f704f086f8f19b7246fa55089bc30e3ad6`
- Baseline resource-I/O run: [42995578057](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42995578057), successful.
- First reachable-flow run: [42996125270](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42996125270), failed at output verification.
- Second reachable-flow run: [42996956698](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42996956698), failed during Ghidra launch after the first workflow correction.
- First flow artifact: [azurearchive-rea-spine-flow-recovery, artifact 13844780081](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42996125270/artifacts/13844780081).
- Latest workflow correction commit: [38cf09f](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/38cf09f704f086f8f19b7246fa55089bc30e3ad6).

## Verified facts

### Input pair and metadata

The failed flow artifact reuses the same Android 1.0 input pair as the successful resource-I/O run:

- ARM64 `libil2cpp.so`: 120,315,032 bytes; SHA-256 `7f15b8e7557eb7948fbea746c629fb15d442d05c649ab2db3667ae08aa2c9b6`.
- `global-metadata.dat`: 17,132,296 bytes; metadata v29; SHA-256 `717236ea9c650b4fcd373e0afc7c03c25d5949726bebe251d3e8301e4da7a144`.
- REA linked 152,789 method pointers; the run did not request generic method instances.

### The one function that reached decompilation

The first flow artifact contains a new `RuntimeAtlasTexture.TryPngSize` export:

- Android ARM64 ELF RVA: `0x325c770`; Ghidra import address: `0x335c770`.
- Reachable instructions: 162; decoded instructions: 162; decode failures: 0.
- Pseudocode length: 3,400 characters.
- Pseudocode SHA-256: `48847146bc41d411a5a9dec5a5eae36d46ff080f4f13067a678eea57d8204adb`.
- The earlier metadata-bounded export reported 136 seeded instructions. The reachable-flow pass therefore decoded 26 additional instructions for this function. This is an observed expansion, not proof of complete CFG recovery.

The pseudocode includes calls to the helper at Ghidra address `0x335e4f4` (ELF RVA `0x325e4f4`) for reading PNG fields. It checks the PNG signature and IHDR marker, reads width/height, rejects dimensions not divisible by four, rejects non-positive dimensions, and applies a calculated upper-bound check. The exact helper semantics, endianness and guard arithmetic still require instruction-level review.

The artifact contains no flow pseudocode/JSON for the other four targets. Its file listing contains only the TryPngSize flow export plus shared metadata/log files.

### Metadata signatures and Android ARM64 RVAs

The paired REA metadata provides the following declaration-level signatures. These are metadata-derived declarations and Android ELF RVAs, not recovered C# bodies or iOS addresses.

| Method | Signature | Android ARM64 RVA |
|---|---|---:|
| RuntimeAtlasTexture.Read | `Source Read(string path, bool supportsDxt5)` | `0x325c600` |
| RuntimeAtlasTexture.Load | `Texture2D Load(Source source, string name)` | `0x325d080` |
| RuntimeAtlasTexture.CreateTexture | `Texture2D CreateTexture(int width, int height, TextureFormat format, string name)` | `0x325d49c` |
| RuntimeAtlasTexture.TryReadCache | `byte[] TryReadCache(Source source)` | `0x325c9f8` |
| RuntimeAtlasTexture.QueueWrite | `void QueueWrite(Source source, int width, int height, byte[] bytes)` | `0x325d5f4` |
| RuntimeAtlasTexture.WriteCache | `void WriteCache(Source source, int width, int height, byte[] bytes)` | `0x325daf0` |
| RuntimeAtlasTexture.FinishPendingWritesAsync | `Task FinishPendingWritesAsync()` | `0x325e2e4` |
| RuntimeAtlasTexture.CanLoadPngAsDxt5 | `bool CanLoadPngAsDxt5(byte[] bytes)` | `0x325e484` |
| RuntimeAtlasTexture.DxtBytes | `long DxtBytes(int width, int height)` | `0x325d94c` |
| RuntimeAtlasTexture.EstimateTextureBytes | `long EstimateTextureBytes(Texture2D texture)` | `0x325e56c` |
| RuntimeAtlasTexture.TryPngSize | `bool TryPngSize(byte[] data, out int width, out int height)` | `0x325c770` |
| RuntimeSpineLoader.Read | `Files Read(string basePath, bool supportsDxt5)` | `0x325ee08` |
| RuntimeSpineLoader.ReadAsync | `Task<Files> ReadAsync(string basePath)` | `0x325f1c0` |
| RuntimeSpineLoader.AtlasPages | `string[] AtlasPages(string text)` | `0x325ea60` |
| RuntimeSpineLoader.Load | `RuntimeSpineAsset Load(string basePath, Material material, TemplateMaterials blends, bool quiet)` | `0x325f2dc` |
| RuntimeSpineLoader.Create | `RuntimeSpineAsset Create(Files files, Material material, TemplateMaterials blends, bool quiet)` | `0x325f32c` |
| PageTextureLoader.Load | `void Load(AtlasPage page, string path)` | `0x326004c` |
| PageTextureLoader.Unload | `void Unload(object texture)` | `0x32601c4` |
| RuntimeSpineAsset.Dispose | `void Dispose()` | `0x325fd1c` |

Additional mapped methods in the same metadata inventory include `RuntimeSpineLoader.DestroyOwned` at `0x325e994`, `RuntimeAtlasTexture.ReadChecksum` at `0x325d8f0`, and `RuntimeAtlasTexture.WriteCache`/cache helpers listed above. The exact method index and type/parameter records are in the linked evidence artifact.

## Workflow failures and corrections

### Run 42996125270

The job successfully installed Ghidra, downloaded the APK, linked metadata, and ran the reachable-flow script for the first target. The verification step then failed with:

`RuntimeAtlasTexture.TryPngSize: identity/entry mismatch`

Cause confirmed from the artifact: the script serializes the Ghidra address as hexadecimal text `0335c770`, while the target TSV stores the same address as decimal `53856112`. The verifier compared the strings directly instead of comparing parsed numeric values.

The same run also attempted to import `libil2cpp.so` repeatedly into one existing Ghidra project. Logs show `Found conflicting program file in project: /libil2cpp.so` on later targets. Consequently, only the first function export was present. The workflow did not correctly fail fast on those Ghidra import errors.

### Run 42996956698

The first correction introduced separate project directories per target and explicit checks for the Ghidra success marker and output files, but the shell variable expressions were accidentally escaped in the workflow text. Ghidra rejected the resulting project path with:

`Path element contains invalid character: '}'`

This is a workflow-script quoting error, not a failure of the Android binary or metadata.

### Current workflow state

Commit `e59886a` introduced per-target project isolation and numeric entry comparison. Commit `38cf09f` corrected the shell variable expansion. The current workflow file was re-read from the branch and verified to contain:

- per-target project paths using expanded `${stem}`;
- hexadecimal parsing of the metadata entry before comparison with the decimal target address;
- per-target log marker and output-file checks.

No successful run of this corrected revision has yet been observed. The available GitHub integration supports reading runs and logs but does not expose a workflow-dispatch action in this session. A new run therefore remains unverified and requires a manual `Run workflow` from the [flow-recovery Actions page](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/workflows/rea-ghidra-spine-flow-recovery.yml).

## Preliminary interpretation — not yet port-ready

- `TryPngSize`'s alignment and size arithmetic are consistent with checking dimensions for block-compressed texture allocation, possibly DXT5. The helper at RVA `0x325e4f4` and the caller `CanLoadPngAsDxt5` must be inspected before concluding this is the actual conversion policy.
- The declaration graph establishes a path from `Read/ReadAsync` to a `Files` value, then `Load/Create` to `RuntimeSpineAsset`; the texture pipeline exposes cache reads/writes, DXT sizing, and texture byte estimation. These are signature-level relationships, not proof of exact call order or ownership semantics.
- The old iOS 0.7.9.2 metadata comparison reports that `RuntimeSpineLoader`, `RuntimeSpineAsset`, `RuntimeAtlasTexture`, and the new page texture-loader API surface are absent from the old iOS build. The older iOS Spine API also lacks the newer byte-array SkeletonDataAsset creation and custom texture-loader hooks. See [verified iOS Spine API gap](https://github.com/x676767xxxx/unity-il2cpp-rebuild/blob/progress/azurearchive-ios-port-20261031/analysis/ios-spine-api-gap-verified-2026-10-31.md) and [iOS/Android/Windows method diff](https://github.com/x676767xxxx/unity-il2cpp-rebuild/blob/progress/azurearchive-ios-port-20261031/analysis/ios-android-windows-exact-method-diff-2026-10-31.md).
- No old-iOS runtime body-level comparison has been completed for this texture/cache pipeline. The older API gap makes a thin wrapper-only port unlikely, but matching Spine 4.2 behavior still needs source/runtime and device-level validation.

## Unresolved issues

1. Run the corrected flow workflow and confirm all five target exports are present.
2. Record direct branch targets, out-of-range blocks, decode failures, skipped method-entry edges, and any instruction/span caps per target.
3. Resolve helper RVA `0x325e4f4` and the `CanLoadPngAsDxt5` caller; verify PNG endianness, DXT5 block-byte arithmetic, alignment and overflow checks from ARM64 instructions.
4. Locate generated async state-machine types and `MoveNext` methods for `ReadAsync` and any iterator state machine behind `AtlasPages`.
5. Resolve indirect dispatch in `DestroyOwned`, and trace `PageTextureLoader.Load/Unload` plus `RuntimeSpineAsset.Dispose` for ownership and release semantics.
6. Decompile the texture cache path and compare with the old iOS 0.7.9.2 implementation using the platform-matched iOS binary/metadata, not Android addresses.

## Delivery boundary

This report documents metadata, a single bounded reachable-flow export, workflow failures and a workflow-only correction. It does not claim complete CFG recovery, original C# recovery, a reconstructed Unity project, an iOS build, or an IPA.


## Follow-up check — workflow run 42997115705 and static script review

Checked again on 2026-11-11:

- Branch head is now `8d170bef80efef2002c1a3e79bf7f54289d3800f`.
- Run [42997115705](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42997115705) still reports `in_progress`; its job remains at step 9, “Recover reachable control-flow blocks and decompile”. Steps 1–8 completed successfully. Steps 10–11 have not started.
- No artifact is attached to this run yet.
- No open pull requests are present.

### Static review of the flow script

The current workflow target list is still limited to five methods: `TryPngSize`, `DestroyOwned`, `AtlasPages`, `Read`, and `ReadAsync`. It does not yet produce reachable-flow exports for the remaining texture/cache methods (`RuntimeAtlasTexture.Read/Load/TryReadCache/CreateTexture/WriteCache/FinishPendingWritesAsync`), nor `PageTextureLoader.Load/Unload` or `RuntimeSpineAsset.Dispose`.

The Java script follows decoded instruction flow edges and fall-throughs, deliberately does not follow call targets, and excludes branch destinations that match another metadata method entry. It records decode failures and skipped method entries. This is useful bounded reachable-block evidence, but the implementation itself explicitly leaves indirect jump tables and exception/LSDA edges unresolved. Therefore even a successful run cannot, by itself, prove a complete CFG.

Additional review point: if Ghidra has already created a function at the target entry during import, the script reuses that function rather than explicitly replacing its body with the newly collected `decoded` address set. The resulting decompiler body should be checked against the exported reachable instruction set before treating it as a full recovered function. This is a validation risk, not a confirmed defect in the current artifact.

### Next verification gate

Do not infer that the current run is merely slow or successful. Wait for its terminal status and logs. If it times out or fails, inspect the exact target log and per-target project state, then adjust the script. If it succeeds, verify all five JSON metadata files, pseudocode hashes, entry addresses, decode failures, and skipped method-entry lists before widening the target list to the remaining texture/cache and ownership functions.


## Artifact review — run 42997115705 (completed successfully)

Run [42997115705](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42997115705) completed with conclusion `success`. All 11 steps passed. Artifact [13844003725 — azurearchive-rea-spine-flow-recovery](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42997115705/artifacts/13844003725) is 4,348,107 bytes, SHA-256 digest `9288c20bdd87b11221adde89038248d003956d308722b3a8d64080a2163fb955`.

The archive was downloaded and inspected locally. It contains five pseudocode files and five per-function JSON records. The input pair matches the prior run:

- `libil2cpp.so`: 120,315,032 bytes; SHA-256 `7f15b8e7557eb7948fbea746c629fb15d442d05c649ab2db3667ae08aa2c9b6`.
- `global-metadata.dat`: 17,132,296 bytes; SHA-256 `717236ea9c650b4fcd373e0afc7c03c25d5949726bebe251d3e8301e4da7a144`.

### Actual per-target flow counts

The JSON reports Ghidra import addresses; subtract `0x100000` to obtain the Android ELF RVA.

| Method | Android ELF RVA | Reachable/decoded instructions | Decode failures | Skipped other method entries | Pseudocode chars |
|---|---:|---:|---:|---:|---:|
| RuntimeAtlasTexture.TryPngSize | `0x325c770` | 162 / 162 | 0 | 0 | 3,400 |
| RuntimeSpineLoader.DestroyOwned | `0x325e994` | 51 / 51 | 0 | 2 | 2,809 |
| RuntimeSpineLoader.AtlasPages | `0x325ea60` | 463 / 463 | 0 | 1 | 7,137 |
| RuntimeSpineLoader.Read | `0x325ee08` | 238 / 238 | 0 | 1 | 2,833 |
| RuntimeSpineLoader.ReadAsync | `0x325f1c0` | 331 / 331 | 0 | 1 | 3,630 |

The verification JSON reports all five as `flow_decompiled_and_hash_verified`. That status only means the generated pseudocode files match their own recorded hash/length and the expected entry/name checks passed. It does not establish that Ghidra decompiled the exact newly traversed address set.

### Confirmed decompiler/body mismatch

Inspection of the actual pseudocode exposes two material correctness problems:

1. `RuntimeSpineLoader.ReadAsync` has 331 reachable/decoded instructions and metadata signature `Task<Files> ReadAsync(string basePath)`, but the exported C-like pseudocode is only a short parameterless `void RuntimeSpineLoader_ReadAsync(void)` stub (roughly 20 lines). The body contains runtime initialization and one helper call, not the full 331-instruction region. The script looked up an existing Ghidra function and reused it without setting its body to the `decoded` address set. This is a confirmed mismatch between traversal output and decompiler input.
2. `RuntimeSpineLoader.AtlasPages`'s 7,137-character pseudocode continues into initialization logic beginning at/around the adjacent `RuntimeSpineLoader.Read` entry (`0x335ee08`, ELF RVA `0x325ee08`). The traversal excludes other method entries when following explicit flow targets, but the fall-through path was not subject to the same method-entry exclusion. The pseudocode therefore appears to include adjacent method content. This is a confirmed cross-method contamination risk in the generated export.

These findings mean the successful workflow status must not be treated as proof that all five decompilations are valid. The instruction traversal counters are still useful bounded reachability evidence, but the decompiler outputs for ReadAsync and AtlasPages are not accepted as clean method bodies.

### Additional confirmed limitations from the pseudocode

- `DestroyOwned` contains Ghidra warnings that it could not recover jump tables at import addresses `0x06814cf8` and `0x06814e4c`, then treated indirect jumps as calls. Ownership/dispatch semantics are unresolved.
- `Read` and `ReadAsync` contain multiple Ghidra “Removing unreachable block” warnings and PIC construction warnings.
- `TryPngSize` checks the PNG signature, IHDR marker, dimensions aligned to multiples of four, positive dimensions, and an upper-bound arithmetic expression. The helper at ELF RVA `0x325e4f4` is called to read the relevant PNG fields. This still requires ARM64 instruction-level validation before asserting exact endianness or safety semantics.
- `AtlasPages` shows collection/enumerator and disposal-like helper operations, but the pseudocode is contaminated by the adjacent function and cannot yet support a reliable ownership conclusion.

### Corrective code change committed

Commit [b4d545a](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/b4d545a071f3e9b8799b17c7dda0d7d47bbd5c7b) updates `scripts/ghidra/PrepareFlowFunction.java`:

- Apply the same “do not cross another metadata method entry” check to fall-through edges, including call fall-throughs.
- If Ghidra already has a function at the entry, set its body to the newly traversed `decoded` address set before decompiling.
- Verify that the resulting function body's address count matches the decoded set; fail instead of silently accepting a stale body.

This is a source-level corrective change only. A subsequent GitHub Actions run using this exact commit has not yet been observed, so the correction is not yet runtime-validated. The available GitHub integration in this session exposes run/log/artifact retrieval but no workflow-dispatch action; do not claim a rerun has been triggered.

## Updated conclusion

- The successful run supplies bounded reachable-instruction evidence for five target entries, with zero decoder failures in the reported traversal.
- It does not yet supply five valid decompilations of the intended method bodies.
- `AtlasPages` and `ReadAsync` exports are explicitly rejected pending rerun with the corrective script.
- `DestroyOwned` remains incomplete due indirect jump-table recovery warnings.
- Full CFG recovery, exception/LSDA edges, async state-machine recovery, and exact branch-target audit remain outstanding.
- No texture/cache target beyond `TryPngSize` has yet received this reachable-flow treatment.
- No original C# recovery, restored Unity project, iOS build, or IPA is established by this work.


## Corrected rerun started

The script correction commit [b4d545a](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/b4d545a071f3e9b8799b17c7dda0d7d47bbd5c7b) triggered a new push workflow:

- [Run 42997601270](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42997601270)
- Head SHA: `b4d545a071f3e9b8799b17c7dda0d7d47bbd5c7b`
- Current observed state at report update: `in_progress`
- Steps 1–5 completed; step 6 (download Android 1.0 APK and extract IL2CPP inputs) was in progress; steps 7–11 pending.
- No Artifact was available yet.

This is the first run that includes both the fall-through method-entry guard and explicit Ghidra function-body replacement/size verification. Its eventual success or failure remains to be observed.


## Corrected rerun artifact review — run 42997601270

The corrected run [42997601270](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42997601270) completed with conclusion `success` (all 11 workflow steps passed). Artifact [13845391583 — azurearchive-rea-spine-flow-recovery](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42997601270/artifacts/13845391583) is 4,348,125 bytes; GitHub reports SHA-256 `3dcee536a0c575e697c41f196c4bd465187292afbec1e3c36a93ffcf15602af7`.

### Corrected bounded traversal counts

| Method | ELF RVA | Reachable/decoded instructions | Decode failures | Skipped method entries | Pseudocode chars |
|---|---:|---:|---:|---:|---:|
| RuntimeAtlasTexture.TryPngSize | `0x325c770` | 136 / 136 | 0 | 1 | 3,406 |
| RuntimeSpineLoader.DestroyOwned | `0x325e994` | 51 / 51 | 0 | 2 | 2,809 |
| RuntimeSpineLoader.AtlasPages | `0x325ea60` | 225 / 225 | 0 | 1 | 7,137 |
| RuntimeSpineLoader.Read | `0x325ee08` | 179 / 179 | 0 | 1 | 2,833 |
| RuntimeSpineLoader.ReadAsync | `0x325f1c0` | 329 / 329 | 0 | 1 | 3,630 |

The corrected traversal now reports fewer instructions for TryPngSize, AtlasPages, and Read than the prior flawed run, consistent with the new method-entry fall-through fence. AtlasPages' skipped-entry file explicitly records `0x335ee08` (the Read method entry); ReadAsync records `0x335f2d4` (the following Load method entry). All five per-file JSON records report zero decode failures.

### Important: do not yet accept all five decompilations

The corrected run has fixed the traversal-side method-entry fence and passed the new function-body address-count check. However, a direct review of the generated C files still shows unresolved Ghidra decompiler issues:

- `AtlasPages` still has numerous “Removing unreachable block” warnings at addresses beginning around `0x0335eebc`, and the exported function body ends with runtime initialization/helper logic that resembles the adjacent Read implementation. The JSON confirms the Read entry itself was skipped, but that alone does not prove every later address belongs to AtlasPages. This remains a decompiler-scope/ownership-of-cold-blocks issue requiring instruction-address-to-body-set comparison.
- `ReadAsync` still decompiles as `void RuntimeSpineLoader_ReadAsync(void)` with a very small body (initialization and one helper call), despite the bounded traversal counting 329 instructions. The metadata signature is `Task<Files> ReadAsync(string basePath)`. This could reflect an IL2CPP wrapper/state-machine split or a decompiler function-body/signature limitation; it is not enough evidence to label the whole method body recovered. The pseudocode SHA-256 is unchanged from the earlier export, so this rerun has not yet demonstrated richer ReadAsync semantics.
- `DestroyOwned` still contains explicit Ghidra warnings for unresolved jump tables at `0x06814cf8` and `0x06814e4c`, with indirect jumps treated as calls.
- `Read` and `AtlasPages` still contain PIC-construction and unreachable-block warnings. Hash/length verification validates artifact integrity, not semantic completeness.

### Current evidence grade

- **Traversal evidence:** improved and useful as bounded reachability evidence for five method entries.
- **Decompiler evidence:** TryPngSize and DestroyOwned are inspectable but have unresolved helper/jump-table questions; AtlasPages and ReadAsync remain unaccepted as clean method-body exports; Read remains provisional due decompiler warnings.
- **Full CFG:** not established. The script still does not recover indirect jump tables or all exception/LSDA edges.
- **Original C# / Unity project / iOS IPA:** not recovered or built.

Next engineering step is to compare the exact decoded address set against Ghidra's decompiler output/control-flow graph, and resolve cold blocks and async state-machine helper targets before expanding this workflow to the remaining texture/cache and page-texture methods.


## New validation instrumentation — run 42999506194

Commit [73fc490](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/73fc490e8c9b929e5013588fed0a7255f22cfd5f) adds a second, independent coverage metric to `PrepareFlowFunction.java`:

- collect the set of ARM64 instruction-start addresses traversed by the bounded flow walk;
- collect unique instruction addresses represented in Ghidra's decompiler `HighFunction` P-code operations;
- report decoded instruction starts missing from the decompiler output and P-code addresses outside the traversed set.

This is intended to distinguish “the traversal found N instructions” from “the decompiler actually represented those instructions.” It is a diagnostic, not a proof of complete CFG; decompiler P-code can omit unreachable or unsupported instructions.

Push-triggered validation run [42999506194](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42999506194), head SHA `73fc490e8c9b929e5013588fed0a7255f22cfd5f`, was observed `in_progress` immediately after creation. No result or Artifact was available at that observation time.


## Metadata cross-check for ReadAsync — 2026-11-11

The linked IL2CPP metadata artifact was queried independently for the `RuntimeSpineLoader` type and its nested types:

- `RuntimeSpineLoader.ReadAsync` is metadata method index 25294, return type `System.Threading.Tasks.Task<RuntimeSpineLoader+Files>`, entry RVA `0x325f1c0`.
- `RuntimeSpineLoader` has five nested types in the metadata inventory: `Files`, `PageTextureLoader`, `<>c`, `<>c__DisplayClass4_0`, and `<>c__DisplayClass6_0`.
- None of those five nested types is a compiler-generated `<ReadAsync>d__...` state-machine type. The metadata search did not identify a state machine directly nested under `RuntimeSpineLoader`.

This narrows but does not settle the short `ReadAsync` pseudocode question: it may be a non-async Task-returning wrapper or delegate work to a helper/state machine declared elsewhere. Do not associate unrelated `<ReadAsync>d__...` types elsewhere in the large metadata image without a proven call/reference path. Next step is to identify the actual callees and cross-reference the wrapper's ARM64 call targets with metadata method entries.


## P-code coverage results — run 42999506194

Run [42999506194](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42999506194) completed successfully. Artifact [13844643875](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42999506194/artifacts/13844643875) is 4,348,713 bytes; GitHub digest `sha256:dc9181a8851b4ddc3b587473d03d04b51f84d949ececc7622e943970aac553bc`.

The new metrics compare the traversed instruction-start addresses with unique instruction addresses represented in Ghidra's decompiler P-code:

| Method | Traversed instruction starts | P-code addresses | Traversed starts missing from P-code | P-code addresses outside traversed set |
|---|---:|---:|---:|---:|
| TryPngSize | 136 | 106 | 40 | 10 |
| DestroyOwned | 51 | 83 | 25 | 57 |
| AtlasPages | 225 | 176 | 89 | 40 |
| Read | 179 | 40 | 166 | 27 |
| ReadAsync | 329 | 11 | 319 | 1 |

These counts show that the decompiler representation and the bounded traversal sets differ materially. In particular, ReadAsync's P-code covers only 10 of its 329 traversed starts (about 3.0%); Read covers 13 of 179 (about 7.3%). AtlasPages overlaps 136 of 225 (about 60.4%), TryPngSize 96 of 136 (about 70.6%), and DestroyOwned 26 of 51 (about 51.0%). “Outside traversed set” values mean the decompiler emitted P-code at addresses not found in this bounded walk; they are not automatically proof of another method because the walk can miss indirect and exception edges.

### Interpretation

- The run is technically green but the P-code evidence rejects the assumption that all five reachable-flow sets were faithfully represented by the decompiler.
- ReadAsync and Read are especially poor matches. Their output must remain classified as wrappers/provisional exports, not recovered complete method bodies.
- The P-code metric is diagnostic. It does not prove complete CFG, because the traversal omits indirect jump-table and exception/LSDA edges and the decompiler may legitimately omit unreachable instructions.
- The metric also revealed that checking only `function.getBody().getNumAddresses()` is insufficient: equal byte counts do not prove equal instruction-start sets.

## Exact function-body verification added

Commit [9f830a7](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/9f830a77429d4844079ea4e3bed400e071446942) strengthens the script:

- enumerate the actual instruction starts in Ghidra's assigned function body;
- compare the exact start-address set against the reachable traversal set, not merely byte counts;
- fail if either set contains addresses absent from the other;
- record P-code addresses outside the function body and bounded diagnostic samples for both missing and extra addresses.

This new check is deliberately stricter. It may fail if Ghidra's function manager cannot represent the discontiguous set exactly; that failure would be useful evidence and must not be bypassed by weakening the check. A push-triggered validation run for this commit has not yet been observed at the time of this report update.


### Exact-set validation run status

The push to script commit [9f830a7](https://github.com/x676767xxxx/unity-il2cpp-rebuild/commit/9f830a77429d4844079ea4e3bed400e071446942) has now created [run 42999979795](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42999979795). At the latest check it was `in_progress`; setup, checkout, Java, and runtime dependency steps had passed, while Ghidra download/install was running. No Artifact was available yet. This run will determine whether Ghidra's actual function instruction-start set exactly matches the bounded reachable set under the stricter check.


## Additional async-state-machine metadata scan — 2026-11-11

A full-type-name scan of the linked Android metadata artifact found no compiler-generated `<ReadAsync>d__...` type nested under `RuntimeSpineLoader`. Its five nested types remain `Files`, `PageTextureLoader`, `<>c`, `<>c__DisplayClass4_0`, and `<>c__DisplayClass6_0`.

The scan did find `<ReadAsyncCore>d__40`, but its declaring type is `System.IO.Compression.DeflateManagedStream`, not `RuntimeSpineLoader`. Its methods are:

- `DeflateManagedStream.ReadAsyncCore`: RVA `0x5ffd1a0`.
- `<ReadAsyncCore>d__40.MoveNext`: RVA `0x5ffdf90`.
- `<ReadAsyncCore>d__40.SetStateMachine`: RVA `0x5ffe518`.

This is a separate compression-stream state machine. It must not be mistaken for AzureArchive's Spine loader state machine. Current metadata evidence therefore supports treating `RuntimeSpineLoader.ReadAsync` as a short wrapper/delegator until its actual call target and downstream task implementation are mapped. No direct state-machine association has been proven.

## Latest strict body-set run status

Run [42999979795](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42999979795), head commit `9f830a77429d4844079ea4e3bed400e071446942`, remains `in_progress` at the latest check. Ghidra install and APK extraction passed; metadata linking was running; no Artifact exists yet. Do not infer a timeout or success from the apparent pause.


## ReadAsync/Read decompiler warning review — 2026-11-11

Inspection of the archived pseudocode and the per-target Ghidra log adds a concrete warning:

- `RuntimeSpineLoader.ReadAsync` pseudocode consists of static-class initialization calls followed by a single indirect call through a class/method pointer and return. It does not show path reading, file construction, or task completion logic.
- The same log lists many “Removing unreachable block” warnings at addresses outside the small visible pseudocode body, including ranges near `0x04051054` and `0x04b82764`. This is consistent with a mismatch between the bounded traversal's collected blocks and the decompiler's own control-flow model, but does not by itself identify which blocks are valid or invalid.
- `RuntimeSpineLoader.Read` pseudocode similarly contains initialization and object setup, while Ghidra warns about removed blocks around `0x0335eebc` onward. The overlap in warnings between Read and AtlasPages indicates that the current traversal/function-body construction needs strict boundary validation before semantic interpretation.

Important metric caveat: the P-code metric counts unique instruction addresses that have at least one P-code operation. It is not a literal percentage of all machine instructions translated; branches, returns, and other instructions may not yield ordinary P-code operations. The very low Read/ReadAsync counts remain a strong diagnostic because their pseudocode is visibly only a wrapper, but the count alone is not a formal completeness percentage.

## Run 42999979795 — latest observed state

The run has progressed past metadata linking and target-list generation. At the latest status query, step 9 `Recover reachable control-flow blocks and decompile` was still `in_progress`; steps 10 and 11 had not started and no artifact was published. The workflow has a 60-minute job timeout. Do not report success or a failure until GitHub marks the run complete.


## TryPngSize instruction-level interpretation — provisional

The archived Ghidra pseudocode for `RuntimeAtlasTexture.TryPngSize` (`0x325c770`, loaded at `0x0335c770`) is sufficiently readable to extract several concrete checks, while keeping the helper's exact semantics open:

1. It initializes both output dimensions to zero.
2. It rejects a null byte-array reference and arrays of length `<= 32`.
3. It checks the eight-byte PNG signature at managed-array data offsets `+0x20..+0x27`.
4. It calls helper `0x0335e4f4` with offset `8` and requires the result to equal `13`, consistent with the PNG IHDR chunk length.
5. It checks bytes at offsets `0x0c..0x0f` for `IHDR`.
6. It calls the same helper at offsets `0x10` and `0x14` for width and height.
7. It rejects dimensions if either has low two bits set, or if either signed dimension is non-positive. Thus accepted width and height must both be positive multiples of four.
8. It computes a guarded size estimate using height and a rounded-up width expression, then accepts only when the result is below `0x7fffffa4`. This is a bounds/overflow-style guard; its exact equivalence to the later DXT5 allocation calculation still needs confirmation.

The helper is consistent with reading a 32-bit PNG field, but its endianness and bounds behavior must be confirmed from its own disassembly before calling this a verified big-endian parser. The code is also not proof that the entire atlas texture pipeline only accepts PNGs or that all images use DXT5. This function only appears to pre-validate PNG dimensions and a size bound.


## Exact function-body validation result — run 42999979795

Run [42999979795](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42999979795) completed successfully. Artifact [13846101130](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/42999979795/artifacts/13846101130) is 4,349,984 bytes; digest `sha256:c50d4de69f286a2b2d4e50f7ff1ac8f2208a69f7da69c9aa25e41ca5d1289a94`.

The new exact instruction-start comparison passed for all five targets:

| Method | Reachable starts | Ghidra body starts | Missing from body | Extra in body |
|---|---:|---:|---:|---:|
| TryPngSize | 136 | 136 | 0 | 0 |
| DestroyOwned | 51 | 51 | 0 | 0 |
| AtlasPages | 225 | 225 | 0 | 0 |
| Read | 179 | 179 | 0 | 0 |
| ReadAsync | 329 | 329 | 0 | 0 |

This validates that the function manager body now matches the script's bounded traversal exactly. It does **not** validate that the traversal found every true CFG edge or that the decompiler represented every instruction. The P-code mismatch counts remain unchanged from run 42999506194. In particular, P-code operations appear at 10 addresses outside the TryPngSize body, 40 outside AtlasPages, 57 outside DestroyOwned, 1 outside ReadAsync, and 27 outside Read. These require disassembly/edge attribution; do not automatically label them as erroneous function overlap.

## Newly identified local helper for TryPngSize

The linked IL2CPP metadata identifies the helper called at loaded address `0x0335e4f4` (ELF RVA `0x325e4f4`) as method index 25286:

`RuntimeAtlasTexture.<TryPngSize>g__Read|19_0`

This resolves the helper's ownership: it is a compiler-generated local function belonging to `RuntimeAtlasTexture.TryPngSize`. Its exact byte-reading semantics and bounds behavior are still to be confirmed by decompiling this helper itself. This is the next focused target, rather than guessing its endianness from the caller.
