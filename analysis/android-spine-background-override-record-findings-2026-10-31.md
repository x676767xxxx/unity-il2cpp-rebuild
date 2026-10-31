# Android 1.0 Spine background override record findings (2026-10-31)

## Evidence inputs
- Android 1.0 Cpp2IL recovery artifact: run 41169615291.
- Targeted IL export: [run 41198412412](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41198412412), artifact `azurearchive-targeted-spine-background-il`.
- Decompiled source artifact: [run 41198111242](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41198111242), artifact `azurearchive-decompiled-source`.
- Android ARM64 and Windows 1.0.1 are used as cross-checks. Decompiled C# contains many Cpp2IL warnings, so only direct field/getter/validation IL and independently corroborated structure are treated as strong evidence.

## Recovered BgSpineOverrideData fields
Android `ResourceOverrideData.BgSpineOverrideData` exposes:
- `Identifier` (string)
- `SpinePath` (string)
- `Animation` (string)
- `Loop` (bool; constructor default true)
- `Skin` (string)
- `PreviewPath` (string)
- `PreviewTime` (float)
- `Scale` (float; constructor default 1)
- `OffsetX` (float)
- `OffsetY` (float)
- `Id` getter calls `Util.CalculateHash(Identifier)`.

Android's `ResourceOverrideData.AdaptPathSeparator` callback normalizes both `SpinePath` and `PreviewPath` by replacing backslash with slash. The portable record factory applies the same separator normalization.

## Recovered validation contract
Android `BackgroundResource.Validate(BgSpineOverrideData)` IL directly establishes:
1. Identifier and SpinePath must not be null/empty/whitespace; otherwise throw `InvalidDataException("Spine backgrounds require Identifier and SpinePath.")`.
2. `Id` must not be zero; otherwise throw `InvalidDataException("Background identifier hashes to the reserved no-change ID.")`.
3. Scale must be greater than zero.
4. Scale, OffsetX and OffsetY must be finite.
5. PreviewTime must be nonnegative and finite.
6. Invalid framing/time throws `InvalidDataException("Invalid framing or PreviewTime for background " + Identifier + ".")`.

The portable `SpineBackgroundOverrideFactory.Validate` mirrors these checks. It additionally rejects Native origin because this factory is specifically for custom override records.

## Override enumeration and unresolved IL
- Android `ResourceOverrideData` has `BgOverrides: HashSet<string>`, `BgSpineOverrides: List<BgSpineOverrideData>`, and `basePath`.
- Decompiled Android iterator shows it validates Spine records, computes their IDs, and rejects duplicate Spine IDs with `Duplicate Spine background identifier: <Identifier>`.
- The iterator yields image overrides before Spine overrides. The exact Android generated iterator contains extensive unresolved native/decompiler placeholders; constructor argument stores for Root/Spine are also type-inconsistent in the recovered IL. Therefore the precise native constructor attachment of the Spine settings object is still a verification item, not claimed as proven.
- `ResolveOverride` uses local source before global source and applies FirstOrDefault. Windows decompiled predicates both compare the candidate resource ID with the captured requested ID; Android native call chain confirms the two predicate call sites but the global predicate's exact instruction body still needs direct confirmation.

## Implemented portable code
- `SpineBackgroundOverrideRecord.cs`: immutable projection of the recovered settings.
- `SpineBackgroundOverrideFactory.cs`: ID, path normalization, validation, duplicate-ID rejection and descriptor construction.
- `BackgroundOverrideSetFactory.cs`: combines image HashSet entries then Spine List entries, preserving the recovered enumeration grouping.
- `BackgroundResourceDescriptor.SpineOverride`: retains animation, loop, skin, preview, scale and offsets.
- `BackgroundResourceDescriptor.SpinePreviewFullPath`: resolves preview image beneath the custom override root using SafeResourcePath.

This is still metadata/descriptor code. It does not yet load a Spine skeleton, atlas, animation state, or draw a background in Unity.

## Validation
- GitHub Actions [run 41198701750](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41198701750) passed the compatibility library build, path safety suite, Unity decoder stub compilation, and current tests before the subsequent Spine preview path addition.
- The newest revision containing the preview path addition is being validated by a follow-up CI run.
