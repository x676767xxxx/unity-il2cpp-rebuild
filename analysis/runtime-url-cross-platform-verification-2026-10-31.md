# Runtime URL cross-platform verification — 2026-10-31

## Source artifacts and reproducibility

- Existing Addressables package comparison: [Run 41192034256](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41192034256), artifact `azurearchive-remote-url-resolution`.
- Targeted binary/metadata string scan: [Run 41192201445](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41192201445), artifact `azurearchive-runtime-url-source-report`.
- Earlier Android full-IL URL analysis: [Run 41191060720](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41191060720), documented in `analysis/android-url-source-evidence-2026-10-31.md`.

## Verified package facts

The iOS 0.7.9.2, Android 1.0, and Windows 1.0.1 packages each contain an Addressables catalog with 32,353 distinct Internal IDs. Each catalog has 13,085 occurrences of the `{ScenarioResourceManager.AddrLoadUrl}` token. The ID ordering and initial ID sequence match across the three platform packages.

The platform settings files are not identical:
- iOS: Addressables 1.21.20; catalog hash filename `catalog_2024.08.25.10.06.35.hash`.
- Android: Addressables 1.21.20; catalog hash filename `catalog_2024.08.25.09.46.46.hash`.
- Windows: Addressables 1.21.20; catalog hash filename `catalog_2024.08.17.15.07.45.hash`.

All three settings files use the same tokenized remote hash URL form, `{ScenarioResourceManager.AddrLoadUrl}/catalog_...`. Thus the platform catalog shape is highly consistent, but the hash filenames and settings hashes differ.

## URL literal evidence

The iOS metadata contains the literal `https://filev2.foxxlight.top/aares/`, and its IL2CPP metadata includes the `ScenarioResourceManager.AddrLoadUrl` field name and related resource-manager methods.

The Android metadata/binary scan also finds:
- `https://filev2.foxxlight.top/aares/`
- `ScenarioResourceManager.AddrLoadUrl`
- resource-manager methods including `InitializeAfterSplash`, `CheckForUpdate`, `UpdateAddressable`, and background preload methods.
- the manual-install UI strings `请输入源地址，留空默认` and `例：https://example.com/aares/`.

Separate full-IL evidence in `analysis/android-url-source-evidence-2026-10-31.md` identifies the literal as the blank-`customSource` fallback inside `AuthoringCliHost.Bootstrap()`. It is therefore a verified authoring-CLI fallback value, **not yet a verified normal mobile gameplay CDN value**.

## Critical unresolved point

The normal Android mobile runtime assignment/initialization of `ScenarioResourceManager.AddrLoadUrl` is still not proven. The current evidence does not justify hard-coding the authoring CLI fallback into the iOS runtime. The iOS package also contains the same literal, but string presence alone does not prove it is the effective runtime value.

## Next implementation gates

1. Cross-reference the Android ARM64 native code for `ScenarioResourceManager.InitializeAfterSplash`, `CheckForUpdate`, and the field write to `AddrLoadUrl`; map exact metadata field tokens to native code and callers.
2. Determine whether mobile gameplay derives the URL from `UserSettings.SettingData.customSource`, a serialized setting, a server response, or a hard-coded default.
3. Compare live catalog/hash and bundle URLs against a permitted, working client/network trace or exact release config.
4. Only then wire the source provider into the iOS Addressables initialization path. Keep the provider injectable and preserve the game's default/custom-source precedence.
5. Continue static-background-only integration first; Spine and complete iOS packaging remain separate gates.

## Build status

The standalone compatibility tests and runtime URL scan CI have passed. No full Unity project, integrated iOS UnityFramework, iOS archive, or IPA has been produced.
