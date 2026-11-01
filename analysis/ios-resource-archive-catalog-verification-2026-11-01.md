# iOS resource archive verification and catalog delta — 2026-11-01

## Provenance

- GitHub Actions run: [41271272492](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41271272492)
- Artifact: `ios-resource-archive-verification` (ID `13069361713`)
- Uploaded Release parts were reassembled and verified by the workflow; this report summarizes its generated metadata analysis.
- **Version attribution:** the user confirms this 5.73 GB archive is the iOS 0.7.9.2 resource package. Treat it as the legacy iOS 0.7.9.2 resource baseline, not as an iOS 1.0 resource package.
- The latest catalog's SHA-256 (`66c3a0a8...`) and byte size (23,911,858) match the `catalog.json` extracted from the supplied iOS 0.7.9.2 IPA in the earlier catalog comparison. This establishes that the archive's August catalog is the same catalog file used by that legacy IPA.
- Archive: `IOS_202408202213.6.zip`
- Reassembled size: 5,738,046,419 bytes
- Archive SHA-256: `d330920a20262b49bb32f33f517aff43cc9f04544793ad6c6681c377870dadd2`
- ZIP integrity: PASS

## Archive inventory

- 13,248 total entries
- 13,086 `.bundle` files
- Three Addressables catalog JSON files and three matching 32-byte hash files
- Latest catalog: `catalog_2024.08.25.10.06.35.json` (23,911,858 bytes; SHA-256 `66c3a0a82bbbd6b540e4c55b7e9fd4f33c8499fc1a82aba3d59c9b7ca0275c04`)

## Catalog evolution inside this archive

| Catalog | Internal IDs | Unique IDs | Bundle refs |
|---|---:|---:|---:|
| 2024-05-22 | 31,647 | 31,647 | 12,915 |
| 2024-06-02 | 31,647 | 31,647 | 12,915 |
| 2024-08-25 | 32,353 | 32,353 | 13,085 |

- May 22 → June 02: one Internal ID added and one removed. The change is the shader bundle reference changing from a hashed iOS RuntimePath entry to `{ScenarioResourceManager.AddrLoadUrl}/shaders_assets_all.bundle`.
- June 02 → August 25: 720 Internal IDs added and 14 removed.
- The August catalog's 13,085 bundle references resolve to 13,084 unique bundle basenames present in the archive; there are no unmatched bundle-reference basenames.
- The archive contains 13,086 bundle files, so one bundle file is not accounted for by the unique-basename count. This is an inventory observation, not proof that it is obsolete or safe to delete.

## Cross-platform interpretation

A previous independently verified comparison of Android 1.0 against the legacy iOS 0.7.9.2 catalog found:
- 32,353 Internal IDs on each side, with the complete Internal ID arrays identical after JSON parsing.
- Same provider IDs, resource types and Internal ID prefixes.
- Different packed Addressables catalog data and build-result hashes.
- 13,084 logical bundle names on both sides; 12,631 matching logical names had the same hash and 453 had different hashes.

The archive's August catalog is the verified iOS 0.7.9.2 catalog: its file size and SHA-256 match the catalog extracted from the iOS 0.7.9.2 IPA. The earlier Android 1.0 comparison therefore applies to this archive's catalog too: complete Internal ID arrays match, while packed key/bucket/entry data and 453 bundle hashes differ.

## Engineering conclusion

1. The 5.73 GB archive is intact and is the iOS 0.7.9.2 resource package; its latest catalog is byte-identical to the catalog extracted from the iOS 0.7.9.2 IPA.
2. Its latest catalog resolves all 13,085 bundle references to 13,084 unique bundle basenames present in the archive.
3. This archive is a verified iOS 0.7.9.2 resource baseline, not a target-version iOS 1.0 resource set.
4. The shared Internal ID array does not make Android bundles interchangeable with iOS bundles: 453 logical bundle names have differing hashes, and settings/build-target metadata are platform-specific.
5. The next useful comparison is file-level bundle matching: map the iOS 0.7.9.2 archive's 13,084 logical bundle names and hashes against Android 1.0 catalog/payloads, classify same-hash, changed-hash, missing and extra payloads, and avoid overwriting the iOS baseline.
6. The full app build remains blocked by the missing matching Unity project/source or a complete reproducible Xcode/UnityFramework integration base. This archive is resources, not a Unity project, Xcode project, or IPA.

## Change-control note

No application source files were added or duplicated for this verification. This document records the output of the existing reassembly workflow only.
