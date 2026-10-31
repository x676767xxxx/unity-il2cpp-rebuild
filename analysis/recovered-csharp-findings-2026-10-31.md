# AzureArchive recovered C# analysis — 2026-10-31

Source artifact: `azurearchive-decompiled-source`, Actions run 41170018509.
Artifact archive size: 5,720,719 bytes; expanded output includes:
- Android `Assembly-CSharp.decompiled.cs`: 34,884,297 bytes
- Windows `Assembly-CSharp.decompiled.cs`: 23,032,378 bytes
- `target-source-extracts.txt`: 118,275 bytes

## Tool outcome

The GitHub Actions job completed successfully, and both ILSpy outputs were generated. This does NOT mean decompilation was clean:
- Android ILSpy reported 5 methods it could not decompile; affected output was replaced with error text. Reported methods include UIFont.set_dynamicFont, UIInput.DoBackspace, Test.AdvanceScenario closure, and StandardBoard.SetText.
- Windows ILSpy reported 1 failed method: Test.AdvanceScenario.
- ILSpy returned exit code 70 for both assemblies despite writing the output files. The workflow was intentionally changed to preserve artifacts and continue.

The Cpp2IL/ILSpy output is not original source and is not directly buildable. The generated code contains large numbers of Cpp2IL reconstruction placeholders and invalid/unknown IL conversions:
- Android Assembly-CSharp output: 98,878 occurrences of Cpp2ILHelpers.NoteDecompilerIssue.
- Windows Assembly-CSharp output: 37,827 occurrences.
Treat reconstructed control flow as tentative unless corroborated by method metadata, native disassembly, or matching platform behavior.

## Concrete Android 1.0 observations

### ScenarioResourceManager.InitializeAuthoringResources

The recovered method sequence indicates it:
1. Invalidates the global AuthoringResourceCatalog cache.
2. Opens the supplied SQLite database in read-only mode.
3. Initializes query objects for BGM, background effects, background names, character emotions, character names, and scenario transitions.
4. Reads the supplied file-name manifest JSON and assigns it through SetFileManifest.
5. Reads the BGM additional-info binary and parses it as a FlatBuffers BGMAdditionalInfoExcelTable.
6. Loads global resource overrides from the resource override root and initializes local override data.
7. Marks the authoring databases as loaded.

The method body contains Cpp2IL reconstruction placeholders, so this is a sequence-level observation, not a clean source reconstruction.

### ScenarioResourceManager.ResolveBackground

The output shows the method first asks `BackgroundResource.ResolveOverride(id, LocalOverrideData, GlobalOverrideData)`. If no override is found, it proceeds into the background-name database lookup path. The latter control flow is heavily damaged by unresolved metadata/native references, so only the initial override lookup should be treated as a strong observation.

### AuthoringResourceCatalog API surface

The recovered Android type exposes APIs for:
- catalog invalidation and project binding;
- search, resolve, and reference reads;
- capability enumeration;
- staging referenced resources;
- importing resources;
- creating aliases and deleting local resources;
- preparing and discarding verification;
- checking scenario resource warnings.

This supports the conclusion that the 1.0 code contains a local resource-authoring/catalog subsystem. It does not yet prove that all of it is needed for ordinary mobile gameplay.

## Why this is not yet an iOS 1.0 port

1. The available iOS input is still 0.7.9.2, not an iOS 1.0 native binary.
2. Android and Windows recovered assemblies are reconstruction outputs; their method bodies are not original C# source.
3. Android ELF machine code cannot be copied into iOS Mach-O UnityFramework.
4. No Unity project, original source project, iOS 1.0 binary, or verified iOS build environment has been obtained.
5. The current artifact does not establish a compilable iOS implementation or an IPA.

## Next technically useful step

Build a targeted, evidence-labelled comparison for the methods and UI call sites that invoke `AuthoringResourceCatalog` and the new background-resource APIs. Classify each as:
- mobile gameplay-critical;
- local manual resource import / authoring workflow;
- editor/desktop-only;
- uncertain.

Then inspect the matching Android ARM64 native functions and call references for only the gameplay-critical items. Avoid trying to transplant the full reconstructed C# file into the iOS project.
