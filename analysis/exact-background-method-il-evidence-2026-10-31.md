# Exact background method IL evidence (2026-10-31)

Inputs: Cpp2IL recovered Android 1.0 and Windows 1.0.1 assemblies; ILSpy IL output. This report contains method-level excerpts, not original C# source. Decompiler gaps are preserved; no guessed behavior is filled in.

## BackgroundResource
### android (SHA256 87f103b680a7fabd5d1492c364a07596644d3fb3052889e46d1b52a04e2eb701)
No method declarations matched the requested names; inspect raw artifact.

### windows (SHA256 f05914a3beba821fec2b1b9a5bae6c8d512c5ff6b7c03eece13c7549933fc0ce)
No method declarations matched the requested names; inspect raw artifact.

## ScenarioResourceManager
### android (SHA256 4a3fdf5a82e7759430e211d09d1a4808381ab940b02e0b05dd8dd2978b99679c)
No method declarations matched the requested names; inspect raw artifact.

### windows (SHA256 85fdcf5366041f3e53058a58417d296f0fa42368b8f87a61e112470e3f7e3eb9)
No method declarations matched the requested names; inspect raw artifact.

## Guardrails
- A recovered IL call is evidence of the call in that recovered method body, not proof that every runtime path succeeds.
- If the IL contains NoteDecompilerIssue/invalid instruction/unmanaged memory load, treat affected branches as unresolved.
- Android and Windows are used as cross-checks; disagreement must not be silently merged.
- No C# compatibility code or IPA is built by this workflow.
