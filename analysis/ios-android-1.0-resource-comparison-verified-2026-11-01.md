# AzureArchive Android 1.0 / legacy iOS resource comparison — verified run #16

Date: 2026-11-01  
Branch: `progress/azurearchive-ios-port-20261031`  
Workflow: [run #16](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41275488274)  
Commit: `d630a0be45ef0cb221525976cd5348866f2b9bad`  
Artifact: `azurearchive-resource-catalog-comparison` (ID `13071287556`, SHA-256 `1b6137c91ac94d291a798505306e646d01c289c8ecaf2647eb83e21f446163c4`)

## Verified Addressables decode

The native Python decoder successfully decoded both packed catalogs:

| Metric | iOS 0.7.9.2 | Android 1.0 |
|---|---:|---:|
| Keys / buckets | 52,639 | 52,639 |
| Entry / location rows | 32,357 | 32,357 |
| AssetBundle provider locations | 15,661 | 15,661 |
| Unique Bundle rows | 13,085 | 13,085 |

Comparison:
- Common logical bundle names: 13,085
- iOS-only / Android-only names: 0 / 0
- Same Addressables hash: 12,632
- Different Addressables hash: 453

These are catalog metadata comparisons. Matching hashes are strong evidence that the catalog considers those bundle versions identical, but do not by themselves prove byte-for-byte equality or runtime compatibility on iOS. The 453 changed entries must not be substituted blindly.

## Legacy iOS resource archive coverage

The supplied legacy iOS resource archive was 5,738,046,419 bytes, SHA-256 `d330920a20262b49bb32f33f517aff43cc9f04544793ad6c6681c377870dadd2`.

- ZIP entries: 13,092
- Bundle files: 13,086
- iOS catalog Bundle entries: 13,085
- Catalog entries matched by normalized Addressables filename: 13,085
- Missing entries: 0
- Size mismatches: 1

The single size mismatch is `defaultlocalgroup_assets_audio/voc_jp/mute.ogg.bundle`: archive 2,195 bytes versus catalog 2,192 bytes (3-byte difference).

This archive match is specifically against the **legacy iOS 0.7.9.2 catalog**, not proof that it contains Android 1.0's changed payloads. The match is by normalized filename (the catalog's primary key without the hash suffix), not by downloading and comparing Android-side bundle bytes.

## Important artifact consistency note

The artifact's `addressables-bundle-map.json` contains the full decoded 13,085-row comparison above. Its older `ios-0792-vs-android-1.0-catalog-diff.md` text still contains the earlier pre-decoder note saying full hash comparison was incomplete. Treat the decoded JSON and its counts as the result of run #16; the stale prose file is not the final hash-comparison authority and should be regenerated in a follow-up cleanup.

## Remaining blockers to a real iOS build

1. The repository still does not contain a complete Unity project (`Assets/`, `ProjectSettings/`, `Packages/`, scenes and project configuration).
2. The Android APK contains the catalog but not the full set of remote Addressables bundle payloads. The legacy iOS archive is not a substitute for the Android 1.0 resource set, especially for the 453 changed entries.
3. A normal Unity iOS player build still requires the complete project and a macOS/Xcode build environment.
4. No Xcode archive, signed IPA, installation, or device runtime test has been produced by this workflow.

## Next engineering work

- Regenerate the human-readable catalog diff from the decoded JSON so it cannot contradict the machine-readable result.
- Produce explicit same-hash and changed-hash manifests (including logical path, iOS/Android hashes and sizes).
- Locate and obtain the Android 1.0 remote Addressables payload archive/CDN files, then validate actual payload bytes and platform compatibility.
- Obtain the complete Unity project and establish the macOS/Xcode build path before attempting IPA generation.
