#!/usr/bin/env python3
"""Compare legacy iOS 0.7.9.2 and Android 1.0 Addressables catalogs.

Important: m_InternalIds lists logical bundle URLs but does not carry the
Addressables bundle hash for most entries. Do not infer hash equality from it.
"""
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

def logical_bundle_names(obj):
    ids = obj.get("m_InternalIds", [])
    return {s.rsplit("/", 1)[-1].lower() for s in ids
            if isinstance(s, str) and s.lower().endswith(".bundle")}

def diagnostic_hash_tokens(obj):
    """Return only directly visible hash-prefixed tokens; not a full catalog decoder."""
    encoded = obj.get("m_KeyDataString", "")
    if not isinstance(encoded, str):
        return set()
    try:
        data = base64.b64decode(encoded, validate=False)
    except Exception:
        return set()
    return {x.decode("ascii", "ignore").lower()
            for x in re.findall(rb"[0-9a-fA-F]{32}_[A-Za-z0-9_.-]+\\.bundle", data)}

ios_name, ios_raw, ios = read_catalog(ios_ipa, "ios")
and_name, and_raw, android = read_catalog(android_apk, "android")
report = [
    "# iOS 0.7.9.2 baseline vs Android 1.0 Addressables catalog",
    "",
    f"- iOS source: `{ios_name}`",
    f"- Android source: `{and_name}`",
    f"- iOS catalog bytes/SHA-256: {len(ios_raw)} / `{hashlib.sha256(ios_raw).hexdigest()}`",
    f"- Android catalog bytes/SHA-256: {len(and_raw)} / `{hashlib.sha256(and_raw).hexdigest()}`",
    "",
    "## Addressables metadata",
    "",
]
ids_i, ids_a = ios.get("m_InternalIds", []), android.get("m_InternalIds", [])
set_i, set_a = set(ids_i), set(ids_a)
report += [
    f"- Internal IDs: iOS {len(ids_i)} ({len(set_i)} unique); Android {len(ids_a)} ({len(set_a)} unique)",
    f"- Complete Internal ID arrays equal: **{ids_i == ids_a}**",
    f"- Internal IDs only in iOS: {len(set_i-set_a)}",
    f"- Internal IDs only in Android: {len(set_a-set_i)}",
    f"- Provider IDs equal: **{ios.get('m_ProviderIds') == android.get('m_ProviderIds')}**",
    f"- Resource types equal: **{ios.get('m_resourceTypes') == android.get('m_resourceTypes')}**",
    f"- Internal ID prefixes equal: **{ios.get('m_InternalIdPrefixes') == android.get('m_InternalIdPrefixes')}**",
    "",
]
for key in ("m_KeyDataString", "m_BucketDataString", "m_EntryDataString", "m_ExtraDataString"):
    report.append(f"- {key} equal: **{ios.get(key) == android.get(key)}**")
report += [
    f"- Build result hashes: iOS `{ios.get('m_BuildResultHash')}`; Android `{android.get('m_BuildResultHash')}`",
    "",
    "## Logical bundle-name inventory (from Internal IDs)",
    "",
]
bi, ba = logical_bundle_names(ios), logical_bundle_names(android)
common = bi & ba
report += [
    f"- Logical bundle names: iOS {len(bi)}; Android {len(ba)}",
    f"- Logical names in both: {len(common)}",
    f"- iOS-only logical names: {len(bi-ba)}",
    f"- Android-only logical names: {len(ba-bi)}",
    "",
    "## Hash comparison status",
    "",
]
hi, ha = diagnostic_hash_tokens(ios), diagnostic_hash_tokens(android)
report += [
    f"- Directly detectable hash-prefixed tokens in decoded m_KeyDataString: iOS {len(hi)}; Android {len(ha)}",
    "- **Full per-bundle hash comparison: NOT COMPLETED.** The decoded key-data scan does not expose a complete hash-to-logical-name map.",
    "- A previous independent analysis recorded 453 changed bundle hashes; this run does not reproduce that result and must not overwrite or contradict it.",
    "- The earlier automated result of 0 changed hashes was a parser false negative, not evidence of equality.",
    "",
    "## Next required decoding",
    "",
    "Decode the Addressables packed key, bucket, and entry data structures together (using the correct Unity Addressables serialization layout/version), then resolve each bundle location to its hash-bearing internal ID. Only after that can a complete per-bundle hash comparison be reported.",
    "",
    "Do not rename, overwrite, or substitute Android bundles for legacy iOS bundles based on matching logical names alone.",
    "",
]
out_path.parent.mkdir(parents=True, exist_ok=True)
out_path.write_text("\n".join(report), encoding="utf-8")
print(f"Wrote {out_path}; logical names iOS={len(bi)} Android={len(ba)}; full hash comparison=NOT COMPLETED")
