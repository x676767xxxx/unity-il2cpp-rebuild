# AzureArchive iOS port progress — background path hardening

Date: 2026-10-31

## Changes made on the progress branch
1. `SafeResourcePath.ResolveUnderRoot` now rejects:
   - null and control characters;
   - explicit current-directory segments (`.`) and parent traversal (`..`);
   - colon-bearing path segments (including Windows alternate data stream syntax);
   - segments ending in a dot or space (ambiguous on Windows filesystems).
2. Expanded the standalone path-safety test program from 8 to 13 cases to cover those inputs.
3. Made `BackgroundImageDecoder.Decode` exception-safe after allocating its Texture2D: setup, wrap-mode assignment and image decode are now inside one cleanup guard, and a failed setup/decode attempts to destroy the allocated Unity object before rethrowing.

## CI findings and correction
- First runs after the path-hardening edit failed at compile time because the generated C# null-character literal had an extra backslash (CS1012). This was detected from the GitHub Actions log.
- Corrected the source to use the C# null character literal `'\\0'`.
- A fresh run completed successfully: [41184151823](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41184151823).
- Build result: 0 warnings, 0 errors.
- Test output: `PASS: 13 SafeResourcePath checks`.
- Artifact: `azurearchive-background-path-compat-library`, ID `13036136760`, 8,878 bytes. SHA-256: `ccf00173363e525a109f7d2416cb05cc3ae6df278f4113cd808d7b508371a78c`.
- Earlier failed runs: [41184126992](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41184126992) and [41184130094](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41184130094); both were due to the null-character literal issue fixed in the final run.

## Validation boundaries
- The path-safety utility is standalone .NET code and does not yet prove integration into AzureArchive's Unity runtime.
- `BackgroundImageDecoder.Unity.cs` is Unity-gated source and is not compiled by the standalone path utility workflow because the actual UnityEngine assemblies/project are not present.
- This work does not test symlink/reparse-point escape behavior; the current helper performs lexical full-path containment only. Do not treat it as a complete defense against an attacker who can create symlinks inside the resource root.
- No actual game Spine atlas/skeleton/texture bundle has yet been tested end-to-end.
- No Unity 1.0 project/Xcode export has been recovered; the full IPA remains unbuilt.

## Next actions
1. Verify run 41184151823 and fix any remaining compile/test failures.
2. Add a dedicated Unity stub compile/test target or build against the actual matching Unity/Spine assemblies when available.
3. Recover actual game Spine assets and validate all atlas page mappings and unsupported timeline/attachment features.
4. Recover the complete Unity project or a legitimate 1.0 iOS Xcode export before attempting a full app build.
