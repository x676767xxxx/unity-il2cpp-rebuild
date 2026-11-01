# Latest authoritative decoded bundle comparison — 2026-11-01

> **This section supersedes the older provisional counts and the earlier “full hash comparison not completed” note below.** Use the decoded result from Actions run [41275488274](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41275488274), artifact `azurearchive-resource-catalog-comparison` (ID `13071287556`, ZIP SHA-256 `1b6137c91ac94d291a798505306e646d01c289c8ecaf2647eb83e21f446163c4`). The artifact contains the full machine-readable `addressables-bundle-map.json` (13,085 comparison rows).

## Verified results

| Metric | iOS 0.7.9.2 | Android 1.0 |
|---|---:|---:|
| Decoded bundle rows | 13,085 | 13,085 |
| Common logical bundle names | 13,085 | 13,085 |
| Platform-only logical names | 0 | 0 |
| Same Addressables hash | 12,632 | 12,632 |
| Different Addressables hash | 453 | 453 |

Additional detail: four rows have the same catalog hash but a different declared bundle size (each differs by one byte). Therefore matching catalog hashes must not be treated as proof that all metadata or payload bytes are identical.

| Logical path | Catalog hash | iOS bytes | Android bytes |
|---|---|---:|---:|
| defaultlocalgroup_assets_audio/bgm/theme_104.ogg.bundle | abf2696797c856bb092f909f8747ace0 | 1,081,002 | 1,081,001 |
| characters_assets_np0035_npcall.bundle | ab916db1fb9fa6fbd09d4455175de19f | 35,633 | 35,632 |
| defaultlocalgroup_assets_audio/voc_jp/jp_maki/maki_cafe_act_5.ogg.bundle | ecbca7fcbc825af2bfcd62cb68a9ceb2 | 13,726 | 13,725 |
| defaultlocalgroup_assets_audio/voc_jp/jp_tutorial/tutorial_101130104_02.ogg.bundle | 9272489dd3ddf26547f10ea9d3d934b1 | 34,550 | 34,549 |

The 453 changed-hash entries are fully enumerated in the JSON artifact. A repeatable Markdown renderer has been added at `scripts/render-addressables-bundle-diff.py`; run it with the decoded JSON and an output path:

```bash
python3 scripts/render-addressables-bundle-diff.py addressables-bundle-map.json ios-android-addressables-catalog-diff.md
```

## Changed-bundle classification

The 453 different-hash rows were grouped by their decoded logical paths:

| Category | Changed bundles | iOS declared bytes | Android declared bytes |
|---|---:|---:|---:|
| Characters | 448 | 221,412,571 | 222,551,769 |
| Avatars | 1 | 45,060,290 | 45,039,642 |
| Built-in / shaders | 2 | 120,075 | 231,270 |
| Effects | 1 | 2,922,907 | 2,585,707 |
| Popups | 1 | 197,919,040 | 197,767,330 |

The character bundles account for 448/453 changed hashes. The five remaining entries are shared/global bundles: one avatar bundle, one effects bundle, one popups bundle, and two built-in/shader bundles. Resource recovery should prioritize the Android 1.0 character payload set first, while retaining all five global bundles. These are catalog-declared sizes, not verified local payload sizes.

## Cross-check against the supplied 5.73 GB legacy iOS archive

The decoded comparison was joined by logical bundle name against the legacy archive's 13,085-file inventory (archive SHA-256: d330920a20262b49bb32f33f517aff43cc9f04544793ad6c6681c377870dadd2).

| Check across the 453 different-hash entries | Count |
|---|---:|
| Entries with a matching file in the legacy archive | 453 |
| Archive file size equals legacy iOS catalog size | 453 |
| Archive file size equals Android 1.0 catalog size | 0 |

This is a file-size cross-check, not an individual payload SHA-256 comparison. However, it confirms that all 453 changed entries in the supplied archive correspond to the **legacy iOS catalog's declared sizes**, and none correspond to Android 1.0's declared sizes. Therefore the 5.73 GB archive is not the missing Android 1.0 resource pack. It cannot be used as a drop-in replacement for those 453 Android 1.0 bundles.

The changed entries total 467,434,883 declared bytes on iOS and 468,175,718 declared bytes on Android. The actual Android 1.0 bundle payloads (or a valid Android 1.0 resource archive/CDN source) are still required.

## Correct interpretation

- Same logical names mean the catalogs refer to the same logical bundle paths.
- Same catalog hash means the Addressables version identifier matches; it is not a file SHA-256 and does not prove byte identity or cross-platform compatibility.
- Different hashes require the matching platform/version payload; do not rename or substitute old iOS bundles based only on logical names.
- The supplied 5.73 GB archive is the legacy iOS 0.7.9.2 resource archive. Its filename/size coverage does not prove it contains Android 1.0 payloads.
- Android 1.0's APK contains the catalog but not the full set of remote Addressables bundle payloads. The Android 1.0 payload archive/CDN resources are still required to complete byte-level comparison.

---

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


## Windows 1.0.1 / APK / legacy iOS file inventory (Actions run 41274012434)

The repeatable cross-package inventory workflow completed successfully:
- Run: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41274012434
- Artifact: `azurearchive-resource-catalog-comparison`, ID `13070348155`
- Artifact ZIP SHA-256: `95e686acebc8496a48ec7974d1cae3e9f669b1d1697c328731c59bc5db745ded`

Observed extracted file counts:
| Package | Files | Catalogs |
|---|---:|---:|
| Legacy iOS 0.7.9.2 IPA | 77 | 1 |
| Android 1.0 APK | 69 | 1 |
| Windows 1.0.1 ZIP | 507 | 1 |

Observed resource candidates:
- Android APK: no standalone `.bundle` files were found by the inventory; the candidate list includes only Unity shared resource files such as `assets/bin/Data/sharedassets0.resource` (1,279,424 bytes), `sharedassets3.resource`, `sharedassets4.resource`, plus Spine converter native libraries.
- Windows 1.0.1: contains Unity player data including `AzureArchive_Data/sharedassets0.assets` (184,889,776 bytes), `sharedassets0.assets.resS` (60,666,832 bytes), `sharedassets1.assets.resS` (39,902,784 bytes), and other sharedassets/resource files. It also includes `BepInEx/interop/Unity.Addressables.dll`.
- Legacy iOS 0.7.9.2: contains `Data/sharedassets0.assets` (184,465,336 bytes), `sharedassets1.assets.resS` (26,841,072 bytes), and related Unity sharedassets/resource files. The inspected app bundle did not contain a local directory of Addressables `.bundle` payloads.

Interpretation and limits:
1. The Android APK's Addressables catalog does not mean all referenced remote bundles are packaged inside the APK. The APK inventory did not reveal standalone bundle payloads.
2. Windows 1.0.1 is a useful 1.0 Unity data/code reference, but Windows sharedassets files are not interchangeable with iOS files without platform validation/rebuild.
3. The 5.73 GB iOS resource archive remains the verified 0.7.9.2 baseline. It was not an input to run 41274012434; its 13,086 bundles have not yet been file-by-file matched to Android 1.0's resolved bundle keys.
4. The comparison script now explicitly marks full per-bundle hash decoding as NOT COMPLETED. Its prior zero-difference output was a parser false negative and must not be used.
