# AzureArchive resource catalog and Spine dependency findings (2026-10-31)

## Sources checked
- Old iOS 0.7.9.2 IPA, Android 1.0 APK, Windows 1.0.1 ZIP from release XR1.
- Deep archive inspection: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41174212438
- Catalog/file inventory: https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41174257250
- Spine native dependency inspection (in progress at report creation): https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41174332705

## Confirmed cross-platform structure
- All three archives contain an Addressables catalog at the expected platform path.
- Catalog sizes are close: iOS 23,911,858 bytes; Android 23,911,850 bytes; Windows 23,911,826 bytes.
- Each catalog has 32,353 entries in m_InternalIds and the same two provider IDs: AssetBundleProvider and BundledAssetProvider.
- Catalog internal IDs use the runtime substitution token {ScenarioResourceManager.AddrLoadUrl}, with bundle names such as avatars_assets_all.bundle and characters_assets_*all.bundle.
- The catalog JSON stores key, bucket, entry and extra data in encoded string fields. A first-pass generic JSON parser that expected direct m_Locations/m_Keys arrays incorrectly reported zero locations/keys; those zero counts must not be interpreted as an empty catalog.
- The platform catalog hash workflow is checking exact hashes of the catalog, settings, internal-ID sequence and encoded data fields. Results will be appended after the run finishes.

## Spine-related binary evidence
- Android APK contains lib/arm64-v8a/libSpineSkeletonDataConverterLib.so (8,293,128 bytes), plus 32-bit variants.
- Windows 1.0.1 contains AzureArchive_Data/Plugins/x86_64/SpineSkeletonDataConverterLib.dll (4,025,856 bytes).
- Windows BepInEx interop output contains spine-csharp.dll (563,712 bytes) and spine-unity.dll (520,192 bytes).
- The old iOS 0.7.9.2 IPA contains IL2CPP global-metadata.dat and UnityFramework, but the archive has no loose .dll file and no separately named Spine converter framework/dylib. Spine managed code may be compiled into its IL2CPP binary; metadata strings indicate Spine types are present.
- Android/Windows matching converter library names indicate a platform-specific native converter dependency exists in the 1.0 distributions. They do not yet prove the converter is required for in-game Spine playback rather than import/conversion tooling. Export and import-call analysis is pending.

## Resource/runtime implications
1. The catalog shape and internal-ID naming are consistent across all three platforms at the high level. This is a strong lead for preserving the existing Addressables lookup model, but the actual bundle payloads and platform-specific bundle hashes must still be compared.
2. Since the iOS 0.7.9.2 catalog contains the same number of internal IDs and matching sample naming, old iOS already has an Addressables catalog model similar to 1.0. This does not prove that its catalog's referenced remote bundles are binary-compatible with 1.0.
3. The Android/Windows converter library is a distinct potential blocker for Spine resource conversion. Determine its exported symbols, dependencies and actual callers before deciding whether it needs a native iOS counterpart or whether the old iOS IL2CPP runtime already provides equivalent functionality.
4. Static image loading through Unity Texture2D/ImageConversion is a more promising first compatibility layer than Spine until the converter dependency is understood.

## Current implementation status
- Evidence/reporting only. No production compatibility-layer code has been compiled.
- No iOS 1.0 Unity project/source tree or iOS 1.0 native build is available.
- No iOS 1.0 IPA has been built or device-verified.
