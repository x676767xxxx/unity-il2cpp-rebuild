# AzureArchive Android 1.0 / legacy iOS 0.7.9.2 — run #19 verified result

- GitHub Actions: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41275944303
- Commit: `329a9d94c3ef5737eb60b17486bdac1cab3080f9`
- Artifact: `azurearchive-resource-catalog-comparison`, ID `13070784596`
- Artifact SHA-256: `a2aaef0613b97ddc7615742bcb4aa92ffe63095f0cde00db5fc2b8890337d6e4`

## Pipeline result

All workflow steps passed: download the three release inputs, inspect archives/catalogs, decode packed Addressables metadata, compare iOS and Android catalogs, export manifests, match the legacy iOS resource archive, and upload the artifact.

## Decoded catalog comparison

| Metric | Result |
|---|---:|
| iOS Bundle rows | 13,085 |
| Android Bundle rows | 13,085 |
| Common logical names | 13,085 |
| iOS-only / Android-only | 0 / 0 |
| Same Addressables hash | 12,632 |
| Different Addressables hash | 453 |

The artifact includes `same-hash-bundles.csv`, `changed-hash-bundles.csv`, `all-bundle-comparison.csv`, the complete decoded JSON map, and the legacy archive match report.

Equal catalog hashes are metadata evidence only; they do not prove byte equality or runtime compatibility. The 453 changed entries must not be silently replaced with legacy iOS payloads.

## Legacy iOS archive match

The 5,738,046,419-byte legacy iOS archive matched all 13,085 iOS catalog bundle references by normalized filename. There are 13,086 archive `.bundle` files and one matched-size discrepancy: `defaultlocalgroup_assets_audio/voc_jp/mute.ogg.bundle` is 2,195 bytes in the archive versus 2,192 bytes in the catalog. The other 13,084 matched entries have equal recorded sizes.

This proves legacy iOS archive coverage, not Android 1.0 payload availability.

## Newly confirmed blocker: remote payload base URL

All 13,085 Android catalog `internal_id` values use the unresolved placeholder `{ScenarioResourceManager.AddrLoadUrl}/...`; none contains an absolute HTTP(S) URL. The catalog alone therefore does not reveal the CDN/base URL. The next analysis step is to search Android APK and Windows 1.0.1 binaries/configuration for the runtime value assigned to `ScenarioResourceManager.AddrLoadUrl` and related endpoint constants.

## Build blockers

- No complete Unity project is present in the repository (`Assets/`, `ProjectSettings/`, `Packages/`, scenes and project configuration are absent).
- Android 1.0 remote AssetBundle payloads have not been obtained.
- A standard Unity iOS player build still requires the complete project and macOS/Xcode.
- No Xcode archive, signed IPA, installation, or iPhone runtime test has been produced.
