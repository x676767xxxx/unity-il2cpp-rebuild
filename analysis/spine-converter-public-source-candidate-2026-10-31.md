# Public Spine converter source candidate review (2026-10-31)

## Candidate
- Repository: https://github.com/wang606/SpineSkeletonDataConverter
- Default branch: main
- Public C++20/CMake project; README advertises Spine 3.5, 3.6, 3.7, 3.8, 4.0, 4.1 and 4.2 input/output support.
- The source's `SpineVersion` enum maps Version35..Version42 to 0..6, matching the Android/Windows IL2CPP metadata enum.
- Source tree includes version-specific JSON/binary readers and writers, conversion passes for curves/order/rotation/spacing, and atlas downgrade code.

## Important compatibility finding
The repository is a command-line project, not an iOS framework:
- Its current CMakeLists builds `SpineSkeletonDataConverter` and `SpineAtlasDowngrade` executables.
- It does not expose the game's three native C exports as a documented iOS C ABI.
- The game's managed declarations expect native functions equivalent to `CastSkeletonDataToDstVersionNative`, `CastJsonSkeletonToBinary`, and `FreeBufferNative`.
- An iOS integration would require a new C ABI adapter, a static-library target, explicit buffer ownership/error-code semantics, and tests against the game's real 3.5-4.2 test cases.
- The matching enum and feature set make this a promising source candidate, but do not prove that the shipped game converter binary was built from this exact repository revision or that outputs are byte-identical.

## License gate
The repository is licensed under PolyForm Noncommercial License 1.0.0. It permits personal research, experimentation, testing, private entertainment and hobby use without anticipated commercial application; it is not a general commercial-use license. Do not copy, modify, compile into, or distribute this code as part of the AzureArchive project until the intended use is confirmed to be noncommercial or separate permission is obtained.

## Buildability and integration gates
1. Confirm the intended AzureArchive use is noncommercial before using this source.
2. Build the unmodified CLI and run its included cross-version test suite on macOS first.
3. Add a separate iOS static-library target and C ABI wrapper without changing the converter's data model.
4. Verify 3.8-to-4.2, 4.2-to-3.8, 4.2-to-4.2 and JSON-to-binary test cases against the game's converter outputs.
5. Obtain the Unity project/source and Xcode integration path before claiming this can be linked into UnityFramework.

## Current status
- Source and license inspected only; no source files have been copied into this repository.
- No iOS static library has been built.
- No Unity integration, signed IPA, or device test has been performed.
- This is a potential way to replace the missing native converter source, not a complete replacement for the old iOS Spine 4.2 managed runtime API gap.
