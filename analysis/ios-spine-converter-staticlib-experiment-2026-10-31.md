# iOS ARM64 Spine converter static-library experiment

## Scope
This experiment wraps the reviewed noncommercial converter source at commit `5ecb2139b0a1af266974f95abeec6bb8562d1249` in a new project-specific C ABI and adds an iPhoneOS ARM64 CMake static-library target. It is not the original game's native library and does not claim ABI compatibility with the game's IL2CPP methods.

## Exported custom ABI
- `AzureArchiveSpineConvertBuffer`: converts Spine binary data from its detected input version to a selected version (-1 keeps input version; 0..6 select 3.5 through 4.2).
- `AzureArchiveSpineJsonToBinary`: converts UTF-8 JSON skeleton data to binary at its detected version.
- `AzureArchiveSpineFreeBuffer`: releases buffers allocated by this wrapper.

The wrapper uses temporary files because the upstream converter's conversion entry point is path-based. This is an integration prototype; temporary-file concurrency, error reporting and malformed-input handling need further stress tests.

## ABI caveat
Recovered managed declarations expose pointer/length outputs and a free-buffer method, but do not prove the native calling convention, allocator, string encoding or error-code contract. This wrapper deliberately uses new symbol names and malloc/free ownership. Do not substitute it for the game's native symbols until those details are verified.

## Build gate
The workflow configures CMake with the Xcode iPhoneOS SDK and arm64 architecture, builds the static-library target, checks the archive and exported symbols, and uploads it. Success proves only that this converter library compiles for iPhoneOS ARM64. It does not prove Unity linkage, runtime API compatibility or an IPA build.

## License and status
The source is used for the user's stated personal, noncommercial purpose. Preserve its PolyForm Noncommercial License 1.0.0 notice. No IPA or Unity integration is produced by this experiment.
