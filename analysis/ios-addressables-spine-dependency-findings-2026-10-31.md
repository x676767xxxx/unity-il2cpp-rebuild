# AzureArchive resource catalog and Spine dependency findings (2026-10-31)

## Sources checked
- Old iOS 0.7.9.2 IPA, Android 1.0 APK, Windows 1.0.1 ZIP from release XR1.
- Deep archive inspection: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41174212438
- Catalog/file inventory: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41174257250
- Spine native dependency inspection: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41174332705

## Confirmed cross-platform structure
- All three archives contain an Addressables catalog at the expected platform path.
- Catalog sizes: iOS 23,911,858 bytes; Android 23,911,850 bytes; Windows 23,911,826 bytes.
- Each catalog has 32,353 internal IDs and the same two provider IDs: AssetBundleProvider and BundledAssetProvider.
- The SHA256 of the ordered internal-ID list is identical on all three platforms: a99ecaca2e23ae9208e228cdab867a72ee07aacc1171978d96eadf341bbd6326.
- Full catalog hashes and build hashes differ by platform. Encoded KeyData and ExtraData lengths differ slightly; EntryData and BucketData lengths match.
- Catalog internal IDs use the runtime substitution token {ScenarioResourceManager.AddrLoadUrl}, with bundle names such as avatars_assets_all.bundle and characters_assets_*all.bundle.
- The catalog stores keys, buckets, entries and extra data in encoded fields. The generic parser's zero m_Locations/m_Keys count was a parser assumption error, not an empty catalog.

## Spine converter: export and dependency evidence
- Android ARM64 library: libSpineSkeletonDataConverterLib.so, ELF64 AArch64, built with Android NDK r23b, SHA256 0775f4442c0ee7d63453319255694c9ee9c3bd7f6dd264559b47c6b417714cf8.
- Android library SONAME is libSpineSkeletonDataConverterLib.so. Its dynamic dependencies are only libm.so, libdl.so and libc.so in the inspected dynamic section.
- Its exported C functions are CastJsonSkeletonToBinary, CastSkeletonDataToDstVersion and FreeBuffer.
- Windows library: SpineSkeletonDataConverterLib.dll, PE32+ x86-64, SHA256 8196e6aa6c04377585e3029df7791f3de1560c8b8bfe623c63a81e70bbd51653.
- Windows DLL exports the same three functions: CastJsonSkeletonToBinary, CastSkeletonDataToDstVersion and FreeBuffer.
- Both binaries contain command-line/help strings describing .json <-> .skel conversion, Spine version detection/conversion, and supported versions 3.5.x through 4.2.x.
- The Windows binary contains CLI examples and conversion messages. This strongly indicates the converter is a file-format migration/import utility, not the runtime Spine renderer itself. Do not classify it as a mandatory gameplay dependency without finding its call sites.
- The old iOS UnityFramework scan has no matching SpineSkeletonDataConverterLib or SkeletonDataConverter strings. This absence is consistent with the converter being an optional tool rather than a missing gameplay renderer, but is not by itself proof.
- Android and Windows also include Spine managed interop/runtime assemblies in the BepInEx interop directory on Windows; old iOS is IL2CPP and therefore does not expose loose managed DLLs in the IPA.

## Resource/runtime implications
1. The identical ordered internal-ID list is strong evidence that the same logical Addressables bundle-name set is used across platforms. This supports preserving the existing catalog lookup and AddrLoadUrl substitution model in a future iOS compatibility layer.
2. Platform-specific catalog and build hashes differ, so do not reuse the Android or Windows catalog binary as an iOS catalog without checking platform-specific serialized locations, hashes, providers and bundle payload compatibility.
3. Spine converter native library is now evidenced as a JSON/binary skeleton conversion tool. It is lower priority than runtime Spine loading; first inspect the Android 1.0 Spine runtime assembly/API and compare it with the old iOS IL2CPP metadata.
4. For a first playable port, prioritize background ID/override resolution and static Texture2D/ImageConversion loading; implement preview cache after those pass. Defer the skeleton converter unless actual gameplay call sites prove it is required.
5. Spine background playback still needs runtime compatibility checks: Spine assembly version, skeleton binary/JSON version, atlas/material assets, animation playback and cache lifetime.

## Current implementation status
- Analysis and binary export inspection only. No production compatibility-layer code has been compiled.
- No iOS 1.0 Unity project/source tree or iOS 1.0 native build is available.
- No iOS 1.0 IPA has been built or device-verified.


## Addressables internal-ID substitution adapter (2026-10-31 follow-up)

- Re-opened the catalog comparison artifact from run 41174257250 and the deep runtime archive report from run 41174212438.
- Confirmed the catalog's internal IDs begin with the exact token `{ScenarioResourceManager.AddrLoadUrl}`, followed by a slash and a bundle filename. The iOS catalog has 32,353 internal IDs; the ordered tokenized internal-ID list matches Android 1.0 and Windows 1.0.1.
- Added `AddressableInternalIdResolver.Resolve(internalId, addrLoadUrl)`. It replaces only this verified leading token with a runtime-supplied base URL. It does not guess or hardcode a CDN endpoint.
- Added URL validation for HTTP(S), origin preservation, credentials/query/fragment rejection, absolute/authority overrides, empty path segments, literal and percent-encoded traversal, encoded separators and control characters.
- Regression coverage: 13 internal-ID resolver checks. The standalone compatibility CI passed on [run 41187379970](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41187379970) with 0 warnings and 0 errors. Test output: `PASS: 13 SafeResourcePath checks; 5 BackgroundOverrideResolver checks; 13 Addressables internal-ID checks; 5 Spine atlas metadata checks; 4 page resource mapping checks; 12 texture cache lifecycle checks; 8 Addressables handle ownership checks`, plus Unity adapter checks.
- Artifact: `azurearchive-background-path-compat-library`, ID `13037047891`, 14,808 bytes.
- Important limit: the exact runtime value/selection logic that populates `ScenarioResourceManager.AddrLoadUrl` has not yet been reconstructed. This resolver is an independently tested token-substitution primitive; it is not yet wired into the actual catalog provider or game ScenarioResourceManager.
