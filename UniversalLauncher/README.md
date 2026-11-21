# Universal Launcher MVP

A native iOS launcher prototype that automatically classifies imported APK and archive files.

## Implemented in this branch
- Import files through the iOS Files picker.
- Copy selected files into the app sandbox.
- Classify APK, ZIP, RAR, and 7Z by extension.
- Persist imported-file metadata and restore existing files on relaunch.
- Inspect APK ZIP structure, manifest presence, DEX count, and native ABI folders without executing it.
- Extract ZIP archives into a dedicated app-sandbox folder.
- Reject archive entries that escape the extraction folder and reject symbolic links.

## Not implemented yet
- RAR and 7Z extraction.
- APK package metadata inspection (package name, version, ABI).
- Android runtime or APK execution.
- Windows EXE execution.

## Important platform boundary
An APK is not executable on iOS merely because it can be parsed or imported. Real APK execution requires a compatible Android runtime, CPU translation where needed, graphics translation, Android system services, and a permitted JIT/runtime strategy. This MVP does **not** claim to run APKs.

“WinZip compatibility” in this first release means archive handling (ZIP first, then RAR/7Z), not running Windows EXE files. Windows executable compatibility would be a separate, much larger engine project.

## Build
The Xcode project is generated from `project.yml` using XcodeGen:

```sh
brew install xcodegen
xcodegen generate
xcodebuild -project UniversalLauncher.xcodeproj -scheme UniversalLauncher -sdk iphonesimulator -configuration Debug CODE_SIGNING_ALLOWED=NO build
```

The project uses ZIPFoundation for ZIP extraction. Xcode must resolve Swift Package dependencies before building.

## Next milestones
1. Verify the macOS GitHub Actions build.
2. Add automated ZIP extraction tests, including malformed and path-traversal archives.
3. Parse binary AndroidManifest.xml to extract package name and version.
4. Add RAR/7Z support after licensing and dependency review.
5. Evaluate a real Android runtime separately on the target iOS version/device before promising game execution.
