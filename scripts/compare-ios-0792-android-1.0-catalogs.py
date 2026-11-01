#!/usr/bin/env python3
"""Compare the legacy iOS 0.7.9.2 Addressables catalog with Android 1.0."""
import base64
import hashlib
import json
import pathlib
import re
import sys
import zipfile

ios_ipa, android_apk, out_path = map(pathlib.Path, sys.argv[1:4])

def read_catalog(archive: pathlib.Path, platform: str):
    with zipfile.ZipFile(archive) as z:
        names = z.namelist()
        candidates = [n for n in names if n.lower().endswith("/catalog.json") or n.lower() == "catalog.json"]
        if platform == "android":
            candidates = [n for n in candidates if "/assets/aa/" in "/" + n.lower()]
        else:
            candidates = [n for n in candidates if "/data/raw/aa/" in "/" + n.lower()]
        if not candidates:
            raise RuntimeError(f"No expected {platform} catalog.json found")
        name = candidates[0]
        raw = z.read(name)
    return name, raw, json.loads(raw.decode("utf-8-sig"))

def logical_bundles(obj):
    encoded = obj.get("m_KeyDataString", "")
    if not isinstance(encoded, str):
        return {}
    try:
        data = base64.b64decode(encoded)
    except Exception:
        return {}
    tokens = set(x.decode("ascii", "ignore") for x in re.findall(rb"[0-9a-fA-F]{32}_[A-Za-z0-9_.-]+\.bundle", data))
    result = {}
    for token in tokens:
        prefix, logical = token.split("_", 1)
        result[logical.lower()] = prefix.lower()
    return result

ios_name, ios_raw, ios = read_catalog(ios_ipa, "ios")
and_name, and_raw, android = read_catalog(android_apk, "android")
report = []
report += ["# iOS 0.7.9.2 resource baseline vs Android 1.0 catalog", ""]
report += [f"- iOS source: `{ios_name}`", f"- Android source: `{and_name}`"]
report += [f"- iOS catalog bytes/SHA-256: {len(ios_raw)} / `{hashlib.sha256(ios_raw).hexdigest()}`",
           f"- Android catalog bytes/SHA-256: {len(and_raw)} / `{hashlib.sha256(and_raw).hexdigest()}`", ""]
ids_i = ios.get("m_InternalIds", [])
ids_a = android.get("m_InternalIds", [])
set_i, set_a = set(ids_i), set(ids_a)
report += ["## Addressables metadata", ""]
report += [f"- Internal IDs: iOS {len(ids_i)} ({len(set_i)} unique); Android {len(ids_a)} ({len(set_a)} unique)",
           f"- Complete Internal ID arrays equal: **{ids_i == ids_a}**",
           f"- Internal IDs only in iOS: {len(set_i-set_a)}",
           f"- Internal IDs only in Android: {len(set_a-set_i)}",
           f"- Provider IDs equal: **{ios.get('m_ProviderIds') == android.get('m_ProviderIds')}**",
           f"- Resource types equal: **{ios.get('m_resourceTypes') == android.get('m_resourceTypes')}**",
           f"- Internal ID prefixes equal: **{ios.get('m_InternalIdPrefixes') == android.get('m_InternalIdPrefixes')}**", ""]
for key in ("m_KeyDataString", "m_BucketDataString", "m_EntryDataString", "m_ExtraDataString"):
    vi, va = ios.get(key), android.get(key)
    report.append(f"- {key} equal: **{vi == va}**")
report += [f"- Build result hashes: iOS `{ios.get('m_BuildResultHash')}`; Android `{android.get('m_BuildResultHash')}`", ""]
bi, ba = logical_bundles(ios), logical_bundles(android)
common = set(bi) & set(ba)
same = sorted(x for x in common if bi[x] == ba[x])
changed = sorted(x for x in common if bi[x] != ba[x])
report += ["## Bundle logical-name/hash comparison", ""]
report += [f"- Decoded bundle tokens: iOS {len(bi)}; Android {len(ba)}",
           f"- Logical names in both: {len(common)}",
           f"- iOS-only logical names: {len(set(bi)-set(ba))}",
           f"- Android-only logical names: {len(set(ba)-set(bi))}",
           f"- Same logical name and hash: {len(same)}",
           f"- Same logical name, different hash: {len(changed)}", ""]
if changed:
    report += ["### Changed hash examples", ""] + [f"- `{x}`: iOS `{bi[x]}`, Android `{ba[x]}`" for x in changed[:100]] + [""]
report += ["## Interpretation", "",
           "This compares Android 1.0 against the iOS 0.7.9.2 catalog. Identical Internal IDs do not make platform bundles interchangeable. Different packed data and bundle hashes must be preserved as platform-specific outputs; do not rename or overwrite the legacy iOS bundles with Android files.",
           ""]
out_path.parent.mkdir(parents=True, exist_ok=True)
out_path.write_text("\n".join(report), encoding="utf-8")
print(f"Wrote {out_path}; changed bundle hashes: {len(changed)}")
