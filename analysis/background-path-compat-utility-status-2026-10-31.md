# Background path compatibility utility status (2026-10-31)

## Why this code was added
Android 1.0 and Windows 1.0.1 recovered IL for BackgroundResource.ResolvePath contains rooted-path checks, normalization, canonicalization and a root-containment test, with explicit InvalidDataException cases for missing roots, absolute paths and resource-directory escape attempts. Windows IL is less degraded than Android IL for this method.

## Code delivered
- `src/AzureArchive.BackgroundCompat/SafeResourcePath.cs`
- Target framework: .NET Standard 2.1, suitable as a candidate library target for Unity 2023-era projects.
- It rejects blank inputs, rooted/drive-qualified paths, parent-directory traversal, and canonical paths outside the supplied root.
- It normalizes slash direction and uses platform-appropriate path comparison.

## Tests
A dependency-free .NET 8 console test runner checks normal nested paths, Windows separators, blank values, parent traversal, mixed traversal, absolute paths and drive-qualified paths. GitHub Actions builds the netstandard2.1 library and runs these checks.

## Scope and limitations
- This is a buildable standalone path-safety utility, not original game source and not an exact reconstruction of every Android/Windows ResolvePath branch.
- It is not integrated into Unity or the AzureArchive game.
- `Path.GetFullPath` does not resolve filesystem symlinks; the resource root must be a trusted application-controlled directory and symlink policy needs separate validation.
- It does not implement resource override selection, image loading, Addressables, preview caching or Spine.
- A Unity project/source tree and macOS/Xcode build environment remain unavailable; no IPA is produced.
