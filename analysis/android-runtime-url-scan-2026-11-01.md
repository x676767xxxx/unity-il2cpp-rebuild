# Android 1.0 runtime URL string scan — 2026-11-01

## Provenance

- Workflow: [41260033886](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41260033886)
- Result: success
- Artifact: `azurearchive-ios-0.7.9.2-addressables-baseline`, ID `13064304021`
- Artifact SHA-256: `312aa744af92442dba52d5e91c9821c1df08579796ca93288c7cd1ab3b97d1a2`
- Scan targets: Android APK IL2CPP metadata and native Unity/IL2CPP libraries, ASCII and little-endian string extraction.

## Findings

The broad string scan did not reveal a confirmed production Addressables host. It found:
- `https://example.com/aares/` in the Android 1.0 metadata strings; this is a placeholder-style host and must not be treated as the live content endpoint without runtime confirmation.
- Unity, Spine, Newtonsoft.Json, FlatBuffers, Android NDK and other library/documentation URLs. These are dependency/documentation strings, not game content servers.
- Generic `http://` and `https://` strings from Unity's bundled networking stack.

No credentials or query parameters were retained in the report artifact. The scanner intentionally redacted query parameter values.

## Next verification

The content URL should be recovered from the actual Android 1.0 runtime call path and configuration, not guessed from a placeholder string. Correlate:
1. `ScenarioResourceManager.AddrLoadUrl` assignment and its callers;
2. Addressables `InternalIdTransformFunc` registration and token replacement;
3. app configuration / remote config / API response values used at startup;
4. a captured runtime request or the separately held resource package's manifest.

The string scan is only candidate evidence. It does not prove that `example.com` is contacted at runtime or identify the current production content host.
