# AddrLoadUrl full-assembly IL findings — 2026-10-31

## Inputs and method

- Android 1.0 and Windows 1.0.1 recovered `Assembly-CSharp.dll` assemblies.
- Full IL export produced by ILSpy from the Cpp2IL recovery artifact.
- Workflow: [Run 41191060720](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41191060720)
- Artifact: `azurearchive-raw-il-ios-port-fixed` (18,291,702 bytes compressed; contains 86,524,808-byte Android IL and 64,278,878-byte Windows IL).

## Verified observations

1. Both assemblies declare `ScenarioResourceManager.AddrLoadUrl` as a public static string field.
2. Android full IL contains the field declaration and two interpolated-prefix references rendered as `"{ScenarioResourceManager.AddrLoadUrl}/"`. It does **not** contain a recovered `stsfld ScenarioResourceManager::AddrLoadUrl` instruction.
3. Windows full IL contains one `stsfld string ScenarioResourceManager::AddrLoadUrl` in `AzureArchive.Automation.AuthoringCliHost.Bootstrap()` (method RVA 0x43e450). The same Bootstrap method is a desktop authoring CLI bootstrapper: it reads command-line/environment inputs, initializes authoring resources and sets `AuthoringCliHost.IsActive`. This is not proof of the Android game's runtime URL initialization.
4. Windows and Android both have URL-prefix checks referencing `{ScenarioResourceManager.AddrLoadUrl}/`; the ILSpy/Cpp2IL output around these checks contains unresolved or indirect-call placeholders, so the exact URL source and branch behavior are not yet recoverable from this export alone.

## Conclusions

- The Windows assignment found so far belongs to the desktop authoring CLI path and must not be copied into the iOS game runtime as the answer.
- Android's actual assignment path remains unresolved. The absence of a recovered store in Cpp2IL IL is not proof that runtime initialization never occurs: it may be assigned through native/indirect code, reflection, a different assembly, or a recovery gap.
- Do not hard-code a CDN URL based on this evidence.
- Do not claim the URL has been recovered.

## Next investigation

1. Inspect Android and Windows IL around every caller of the URL-prefix checks and identify the enclosing method names and full signatures.
2. Search all recovered assemblies, not only `Assembly-CSharp.dll`, for the field token/reference and URL configuration providers.
3. Compare native ARM64/PE code and metadata references for the field token; verify Android's assignment path independently of pseudo-IL.
4. Inspect the Android APK's configuration/catalog/bootstrap data for a canonical remote base URL, validating it against actual resource requests or catalog IDs.
5. Only after the runtime source is established, implement a platform-neutral URL provider and add tests for empty, trailing-slash, and absolute-address cases.

## Limitations

Cpp2IL-generated IL includes unresolved native-memory loads, indirect-call notes and missing-method placeholders. It is an analysis artifact, not source code and not a buildable Unity project. No iOS IPA was produced by this workflow.
