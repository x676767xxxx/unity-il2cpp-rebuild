# iOS resource archive verification and catalog delta — 2026-11-01

## Provenance

- GitHub Actions run: [41271272492](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41271272492)
- Artifact: `ios-resource-archive-verification` (ID `13069361713`)
- Uploaded Release parts were reassembled and verified by the workflow; this report summarizes its generated metadata analysis.
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

The August 2024 resource archive's latest catalog also has 32,353 Internal IDs. However, this workflow did not directly compare the August catalog's complete Internal ID array or packed key/bucket/entry payload against the Android 1.0 catalog. Do not claim byte-for-byte or full semantic equivalence based only on the equal count.

## Engineering conclusion

1. The 5.73 GB archive is intact and its latest catalog resolves all bundle references to bundle files in the archive.
2. The resource archive is a substantial and internally coherent iOS resource set; this removes archive integrity and missing-bundle-reference checks as immediate blockers.
3. It does not prove that every bundle is compatible with the target Android 1.0 executable, nor that the bundle payloads are interchangeable across platforms.
4. The next useful comparison is a direct semantic decode of the August catalog against the Android 1.0 catalog, followed by file-level bundle hash/size comparison where both payload sets are available.
5. The full app build remains blocked by the missing matching Unity project/source or a complete reproducible Xcode/UnityFramework integration base. This archive is resources, not a Unity project, Xcode project, or IPA.

## Change-control note

No application source files were added or duplicated for this verification. This document records the output of the existing reassembly workflow only.
