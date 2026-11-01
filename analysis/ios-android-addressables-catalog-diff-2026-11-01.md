# Addressables catalog hash comparison — 2026-11-01

## Scope and provenance

Compared the serialized Addressables catalogs extracted by GitHub Actions run [41259726479](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41259726479), artifact `azurearchive-ios-0.7.9.2-addressables-baseline` (artifact ID `13064313185`, SHA-256 `0b517a2d8bc7783587cc0952f3e6c440c779214798e499c097eca76c9d6ed139`).

Inputs:
- Legacy iOS IPA: AzureArchive 0.7.9.2, `Payload/AzureArchive.app/Data/Raw/aa/catalog.json`
- Android 1.0 APK: `assets/aa/catalog.json`
- The comparison is between the legacy iOS 0.7.9.2 catalog and Android 1.0; it is not a same-version iOS 1.0 catalog comparison.

## Verified catalog facts

- Both catalogs parse as JSON and expose the same 13 top-level Addressables fields.
- Both have 32,353 `m_InternalIds`; the complete arrays are byte-for-byte equal after JSON parsing.
- `m_ProviderIds`, `m_resourceTypes`, and `m_InternalIdPrefixes` are equal.
- `m_KeyDataString`, `m_BucketDataString`, `m_EntryDataString`, and `m_ExtraDataString` differ.
- `m_BuildResultHash`: iOS `4f0727e4206d81080d2c6a17b4611641`; Android `d7c2fec3f089496ff5696042a8d6c382`.
- Catalog byte sizes: iOS 23,911,858; Android 23,911,850.
- Settings differ in the remote and cached catalog hash filenames, `m_SettingsHash`, and build target (`iOS` vs `Android`). Do not copy one platform's settings file unchanged into the other platform.

## Bundle-name/hash comparison

Decoded the base64-encoded `m_KeyDataString` payload and extracted bundle filename tokens. Matching the logical bundle filename after removing the trailing 32-character hash yielded:

| Result | Count |
|---|---:|
| Logical bundle names in both catalogs | 13,084 |
| Logical names only in Android | 0 |
| Logical names only in legacy iOS | 0 |
| Same logical name and same hash | 12,631 |
| Same logical name but different hash | 453 |

Thus the two catalogs enumerate the same logical bundle-name set, but 453 bundle hashes differ. This is evidence of platform/version-specific bundle outputs, not proof that the underlying payloads are interchangeable. In particular, the hash-bearing names in the packed key data must be resolved against the actual bundle files present in the Android 1.0 resource package.

Representative changed logical names include:
- `40c584651112a1b2d46de1ea3a688afb_unitybuiltinshaders.bundle`
- `avatars_assets_all.bundle`
- `characters_assets_all.bundle`
- `characters_assets_airiall.bundle`
- `characters_assets_akaneall.bundle`

## Interpretation

1. Addressables metadata is present in both packages; the next blocker is not simply a missing `catalog.json`.
2. The identical `m_InternalIds` array indicates the logical resource URL template set is shared, while packed catalog data and platform-specific hashes still differ.
3. Do not rewrite the Android 1.0 catalog to point at legacy iOS bundle hashes, and do not rename Android bundles to the old iOS hashes. That would make catalog references inconsistent with the payloads.
4. The next useful check is a full inventory of the actual Android 1.0 bundle payloads (including the user's separately held 5.73 GB resource pack), matching each catalog hash-bearing bundle key to a file and recording size/SHA-256. Then classify which bundles have a matching iOS payload, which require platform-specific rebuild, and which are absent.
5. The legacy iOS IPA inventory found `Data/Raw/aa/catalog.json`, `settings.json`, and `link.xml`; it did not show a packaged local Addressables bundle directory in the inspected app path. Runtime remote bundle delivery may therefore be part of the old app's resource model and must be verified before assuming the IPA should contain all content.

## Validation limits

- This comparison did not decode every Addressables packed key/bucket/entry into a complete semantic location table.
- Hash differences do not identify which serialized asset changed or prove a particular bundle is incompatible.
- The separately held 5.73 GB resource package was not available to this Actions job, so its contents have not been verified here.
- This analysis does not produce a Unity project, Xcode archive, or IPA.
