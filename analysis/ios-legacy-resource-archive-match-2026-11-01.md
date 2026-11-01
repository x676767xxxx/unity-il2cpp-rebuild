# Legacy iOS 0.7.9.2 resource archive filename verification — 2026-11-01

## Provenance
- GitHub Actions run: [41275213442](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41275213442)
- Commit analyzed: `d849bc2a70c76e57d34748cd1918c9620d5a57a8`
- Artifact: `azurearchive-resource-catalog-comparison`, ID `13071227388`
- Artifact SHA-256: `58edae0945e96159f6c7963308cecf933b576dd05b5f082ff1e418f266ce2cc8`
- Input: legacy iOS 0.7.9.2 resource archive, not Android 1.0 resources.

## Verified result

The corrected filename normalizer strips the trailing underscore plus 32-hex Addressables hash from `primary_key` bundle filenames before comparing with archive member basenames.

| Metric | Result |
|---|---:|
| Archive bytes | 5,738,046,419 |
| Archive SHA-256 | `d330920a20262b49bb32f33f517aff43cc9f04544793ad6c6681c377870dadd2` |
| ZIP entries | 13,092 |
| Archive `.bundle` files | 13,086 |
| iOS catalog bundle entries | 13,085 |
| Matched by normalized filename | 13,085 |
| Missing catalog entries | 0 |
| Matched entries with size mismatch | 1 |

The one size mismatch:
- Bundle name: `f35ba78322ea789a4c7994f279671bbe`
- Archive member: `defaultlocalgroup_assets_audio/voc_jp/mute.ogg.bundle`
- Archive size: 2,195 bytes
- Catalog size: 2,192 bytes
- Catalog hash: `14c7a2c2426d565904d87eb83b05a7b2`

The remaining 13,084 matched entries have equal recorded sizes.

## Interpretation and limits

1. This establishes filename-level coverage of the legacy iOS 0.7.9.2 catalog by the 5.73 GB archive. It does not establish that the archive is an Android 1.0 resource pack.
2. Filename and size agreement do not prove byte identity, valid Unity AssetBundle contents, or runtime compatibility.
3. The archive has one more `.bundle` file than the catalog entry count. Do not delete or classify the extra file as obsolete without evidence.
4. The current AddressablesTools-based decoder does not expose populated location/key collections for these serialized catalogs. Run 41275213442 therefore does not independently reproduce the earlier 12,631 same-hash / 453 different-hash comparison. That remains a separate prior analysis.
5. Android APK inventory contains its catalog but no standalone `.bundle` payload files. Windows 1.0.1 is a useful Unity data/code reference, but its sharedassets are not automatically interchangeable with iOS.
6. No full Unity project, Xcode archive, signed IPA, or device test was produced.

## Next engineering gate

Implement and validate a version-correct decoder for the packed Addressables key, bucket, and entry data so each catalog bundle location can be associated with its hash-bearing key. Separately identify the actual Android 1.0 remote bundle payload source. Only then classify reusable legacy iOS bundles versus platform-specific resources requiring rebuild.
