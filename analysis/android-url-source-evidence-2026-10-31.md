# Android 1.0 URL-source evidence from full IL — 2026-10-31

## Evidence

Source: Android 1.0 `Assembly-CSharp.dll` full IL exported by ILSpy from the Cpp2IL recovery artifact. Workflow: [Run 41191060720](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41191060720).

### 1. Candidate default source literal

Inside `AuthoringCliHost.Bootstrap()` (Android RVA `0x5d32c8`), the IL checks `UserSettings.SettingData.customSource`. On the branch where the value is blank, it loads the literal:

```
https://filev2.foxxlight.top/aares/
```

On the nonblank branch, it trims trailing '/' from `customSource` and appends one slash.

This is concrete evidence of a default **authoring CLI custom-source value** in the Android 1.0 managed assembly. It is not yet evidence that this is the normal mobile game's production CDN endpoint, nor that it is the value assigned to `ScenarioResourceManager.AddrLoadUrl`.

### 2. AddrLoadUrl assignment status

- Android `Assembly-CSharp.full.il` declares public static string `ScenarioResourceManager.AddrLoadUrl`.
- The full Android IL contains two interpolated-string references to `{ScenarioResourceManager.AddrLoadUrl}/`.
- A literal search of the exported Android full IL found no `stsfld ScenarioResourceManager::AddrLoadUrl` instruction.
- Windows has a `stsfld string ScenarioResourceManager::AddrLoadUrl` in its `AuthoringCliHost.Bootstrap()`, but Cpp2IL output around this code contains unresolved native-memory and indirect-call placeholders. Do not transplant the Windows assignment or infer the Android runtime value from it.

## Safe conclusion

The Android package embeds `https://filev2.foxxlight.top/aares/` as a fallback `customSource` for its authoring CLI bootstrap. The normal mobile runtime initialization path for `AddrLoadUrl` remains unresolved. Do not hard-code this URL into an iOS game runtime until its role is verified against runtime configuration, Addressables catalog data, or observed network requests.

## Next work

1. Complete the all-managed-assemblies scan to see whether another assembly contains the missing field initialization or source-provider implementation.
2. Inspect Android bootstrap and resource-manager native code around the field token, since Cpp2IL IL contains unresolved indirect/native operations.
3. Inspect APK configuration and Addressables catalog metadata to establish which remote paths are actually used by gameplay.
4. Implement an injectable URL provider only after the mobile runtime source and precedence rules are established.


### 3. Cross-reference of all recovered Android Assembly-CSharp references

A direct search of the 86,524,808-byte Android `Assembly-CSharp.full.il` export shows:
- The two recovered `{ScenarioResourceManager.AddrLoadUrl}/` string uses are in generated delegates `AuthoringOfflineResources/<>c::<ValidateDirectory>b__5_2` and `<ValidateDirectory>b__5_3`, not in the normal gameplay resource-manager initialization path.
- The `customSource` backing field is read in `AuthoringCliHost.Bootstrap()`; the property getter/setter are otherwise just accessors in this assembly export.
- The `catalogUrlOverride` backing field has getter/setter accessors, but no other call-site use appears in the full `Assembly-CSharp` IL search.
- The Android mobile `ScenarioResourceManager.AddrLoadUrl` field is declared, but no recovered managed IL assignment was found.

This narrows the interpretation: the visible fallback and the recovered prefix-validation helpers are associated with authoring/offline tooling. They do not establish the normal mobile gameplay source of the URL.
