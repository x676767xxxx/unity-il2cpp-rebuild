# AzureArchive recovered-code review (2026-10-31)

Source artifact: Actions run 41170018509, artifact azurearchive-decompiled-source (5.72 MB compressed; contains ~58 MB extracted text).

## What completed

- Cpp2IL recovery artifact downloaded successfully.
- ILSpy produced large C#-like output files for Android and Windows:
  - Android: Assembly-CSharp.decompiled.cs (~34.9 MB)
  - Windows: Assembly-CSharp.decompiled.cs (~23.0 MB)
- Target class names are present in both output files.

## Important quality finding

ILSpy returned exit code 70 on both platforms and explicitly reported decompilation errors:
- Android: 5 methods failed (UIFont.set_dynamicFont twice, UIInput.DoBackspace, Test.AdvanceScenario lambda, StandardBoard.SetText).
- Windows: 1 method failed (Test.AdvanceScenario).

More importantly, the recovered C# is not clean, buildable source. Method bodies contain Cpp2IL placeholders and warnings such as Cpp2ILHelpers.NoteDecompilerIssue, invalid native-memory placeholders, unresolved method tokens, and impossible casts. Example: Android ScenarioResourceManager.ResolveBackground contains many unresolved IL stack-type mismatches and Method not found markers. AuthoringResourceCatalog.CreateAlias likewise has extensive unresolved values.

Therefore:
- Treat the generated C# only as a rough inspection aid.
- Do not copy these files into an iOS Unity project or claim they reproduce the original logic.
- The 99.91% / 99.997% Cpp2IL method-recovery counters do not mean those methods were reconstructed as valid C# source.

## Verified clues worth retaining

The Android 1.0 ScenarioResourceManager output references:
- BackgroundResource.ResolveOverride(id, LocalOverrideData, GlobalOverrideData)
- ScenarioBgNameQuery
- ScenarioBGNameDbSchema / ScenarioBGNameExcel
- BackgroundResource construction with ResourceType.Native
- GetBackgroundPreviewAsync and background preview/cache-related code paths
- InitializeAuthoringResources(databasePath, fileNamesPath, bgmInfoPath)

The Android AuthoringResourceCatalog output includes code paths that reference:
- ValidateIdentifier
- Capture / RequireVersion
- Snapshot.Entries
- resource kinds such as character, native, background, and popup
- CopyVerified, ValidateAudio, and ValidateImage

These are useful leads, but due to IL reconstruction errors they are not yet reliable implementation specifications.

## Next step

Export raw IL for selected methods/types from the recovered assemblies (rather than relying on ILSpy's high-level C# reconstruction), then correlate method tokens with Android ARM64 native RVAs and call references. Only after validating control flow and dependencies should a platform-neutral specification or iOS implementation be written.
