# Universal Launcher MVP

A native iOS file launcher prototype that automatically classifies imported APK and archive files.

## MVP scope
- Import files through the iOS Files picker.
- Copy selected files into the app sandbox.
- Classify APK, ZIP, RAR, and 7Z by extension.
- Display imported items and clearly distinguish analysis/import from execution.
- Keep the Android runtime behind a future engine adapter.

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

## Next milestones
1. Add ZIP listing and safe extraction with path traversal protection.
2. Persist library metadata and restore it on relaunch.
3. Add APK metadata inspection (package name, version, ABI) without executing it.
4. Evaluate a real Android runtime separately on the target iOS version/device before promising game execution.
