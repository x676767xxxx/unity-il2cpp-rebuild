# Android 1.0 vs legacy iOS 0.7.9.2 — decoded Addressables bundle map

Date: 2026-11-01

## Run and artifact
- GitHub Actions run: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41275437699
- Commit: edf7353c9808a73a92f154e54bab3c016ee2ba89
- Artifact ID: 13071242628
- Artifact SHA-256: fb17472d1b2b90ea5b6c89cc12328a31a2e20b5f94c11bf2e30cb33a7b611801
- Decoder: scripts/decode-addressables-bundle-map.py (native Python parser for packed Key/Bucket/Entry/ExtraData tables)

## Decode integrity counters
| Counter | iOS 0.7.9.2 | Android 1.0 |
|---|---:|---:|
| Key count | 52,639 | 52,639 |
| Bucket count | 52,639 | 52,639 |
| Entry/location count | 32,357 | 32,357 |
| Buckets with locations | 52,639 | 52,639 |
| AssetBundle-provider locations | 15,661 | 15,661 |
| Unique bundle names | 13,085 | 13,085 |

All bucket entry indices passed bounds checks. The decoder produced a non-empty hash and size for every unique bundle row in both catalogs.

## Bundle comparison
| Result | Count |
|---|---:|
| Common logical bundle names | 13,085 |
| iOS-only names | 0 |
| Android-only names | 0 |
| Same Addressables hash | 12,632 |
| Different Addressables hash | 453 |

The earlier independent note recorded 12,631 same-hash and 453 different-hash items. The new direct decode gives 12,632 + 453 = 13,085, matching the complete common-name total. This direct decode supersedes the earlier same-hash count.

## Meaning
- The logical bundle-name set is identical between Android 1.0 and legacy iOS 0.7.9.2.
- 12,632 bundles have equal Addressables content-hash metadata. This is evidence of reusable content identity, but is not yet binary-byte equality or target-device runtime compatibility.
- 453 bundles have different hashes. Sampled changed entries include character bundles; their catalog sizes also differ, so treat them as changed platform/version payloads until proven otherwise.
- The m_BundleName field is a hashed bundle identifier; m_Hash and the hash suffix in primary_key identify the content version. Do not confuse these hashes.
- The 5.73 GB legacy iOS archive matches all 13,085 iOS catalog bundle references by normalized filename, with one 3-byte size discrepancy. This validates only the legacy archive mapping.

## Limitations and next work
1. The Android APK contains the catalog but no standalone remote .bundle payloads. The 453 Android 1.0 changed payloads have not yet been obtained.
2. Matching Addressables hashes do not alone prove that legacy iOS archive payloads are byte-identical or load correctly in the target iOS runtime. Validate representative bundles with a Unity AssetBundle parser/runtime and compare archive members against catalog sizes.
3. Continue with a classified export: same-hash candidates, changed-hash candidates, and actual archive payload availability. Then prioritize static backgrounds/UI resources for a first playable build.
4. The full Unity project and reproducible iOS Unity/Xcode integration base remain absent. No signed or installable IPA was built or device-tested by this run.