# REA-assisted AzureArchive IL2CPP reverse engineering — 2026-11-01

## Decision

Use the official REA project at https://github.com/morluto/rea and its published `rea-agents` CLI for a second, independent static decode of the Android 1.0 Unity IL2CPP metadata. Do not use similarly named forks as the installation source.

REA's official `docs/il2cpp-metadata.md` documents:
- `inspect-il2cpp-metadata` for `global-metadata.dat` versions 24, 27, 29, 31 and 39;
- complete assembly-image, type, method, field, property, event, parameter and string-literal inventories;
- optional linking to a matching Android `libil2cpp.so` ELF, resolving native method addresses without symbols;
- optional generic-method instance analysis and Ghidra function seed export;
- static read-only inspection; no Unity install or execution of the game is required.

The existing Android 1.0 analysis identifies metadata version 29 and has an ARM64 `libil2cpp.so` extraction workflow. This is within REA's documented metadata-version support.

## Added workflow

`.github/workflows/rea-il2cpp-metadata-analysis.yml`:
1. Downloads the existing Android 1.0 APK from the project's XR1 release.
2. Extracts `global-metadata.dat` and ARM64 `libil2cpp.so`.
3. Records SHA-256 fingerprints and ELF header facts.
4. Runs `npx rea-agents@latest inspect-il2cpp-metadata` with the matching binary and generic-instance analysis.
5. Stores complete REA Evidence and CLI logs as a GitHub Actions artifact.

Workflow run: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41306973067

## Planned use of the evidence

- Compare REA's assembly/type/method inventory with existing Cpp2IL/Il2CppDumper and custom IL2CPP extraction results.
- Resolve candidate method addresses for game bootstrap, resource catalog loading, Addressables, Spine/background resource loading, scene and animation initialization.
- Use exact method indices, names, owners and native addresses to create a reviewed method map; preserve ambiguous, unresolved or inferred cases as unknown rather than guessing.
- If the native provider is later configured, export selected method seeds for Ghidra and continue native control-flow/call-chain analysis.
- Reconstruct behavior and tests in the iOS compatibility layer; do not copy Android machine code into iOS.

## Environment and limitations

REA's analysis runs on the GitHub Actions Linux host in this workflow, not on the user's iPhone. The workflow does not require installing REA on the phone. Deep decompilation of native functions still requires a configured Ghidra/Hopper/IDA provider; the current first pass only decodes IL2CPP metadata and links method addresses. It does not itself produce original C# source or a complete Unity project.

This workflow uses only the Android 1.0 APK already referenced by the existing repository workflow. It does not inspect Natsu.zip. The 5.73 GB resource archive remains a separate resource-recovery task and is not a prerequisite for this metadata pass.

## Acceptance criteria

- The REA run completes and produces a non-empty Evidence artifact.
- Evidence reports metadata version 29 and includes both artifact hashes.
- The metadata and native binary pair pass REA's structural registration/method mapping checks; any fallback/inferred linkage is explicitly identified.
- Selected recovered methods are cross-checked against the existing analysis before being used to change port code.
