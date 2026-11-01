#!/usr/bin/env python3
"""Render a complete Addressables bundle diff from decoded bundle-map JSON."""
import argparse
import json
from pathlib import Path

parser = argparse.ArgumentParser()
parser.add_argument("input", type=Path, help="addressables-bundle-map.json")
parser.add_argument("output", type=Path, help="Markdown report path")
parser.add_argument("--legacy-archive-match", type=Path, help="optional legacy-ios-archive-match.json")
args = parser.parse_args()

data = json.loads(args.input.read_text(encoding="utf-8"))
rows = data["comparison"]
counts = data["counts"]
changed = [row for row in rows if not row["hash_equal"]]
same_hash_size_diff = [
    row for row in rows
    if row["hash_equal"] and row["ios"]["size"] != row["android"]["size"]
]

archive_match_stats = None
if args.legacy_archive_match:
    archive_data = json.loads(args.legacy_archive_match.read_text(encoding="utf-8"))
    archive_by_name = {row["bundle_name"]: row for row in archive_data["matches"]}
    matched = [(row, archive_by_name.get(row["bundle_name"])) for row in changed]
    archive_match_stats = {
        "matched": sum(item is not None for _, item in matched),
        "ios_size": sum(item is not None and item["archive_bytes"] == row["ios"]["size"] for row, item in matched),
        "android_size": sum(item is not None and item["archive_bytes"] == row["android"]["size"] for row, item in matched),
    }

def category(path):
    logical = path.split("}/", 1)[-1].lower()
    if "characters_assets" in logical:
        return "characters"
    if "unitybuiltinshaders" in logical or "shaders_assets" in logical:
        return "built-in/shaders"
    if "avatars_assets" in logical:
        return "avatars"
    if "effects_assets" in logical:
        return "effects"
    if "popups_assets" in logical:
        return "popups"
    if "defaultlocalgroup_assets_uis" in logical:
        return "UI/backgrounds"
    if "defaultlocalgroup_assets_audio" in logical:
        return "audio"
    return "other"

category_totals = {}
for row in changed:
    name = category(row["ios"]["internal_id"])
    totals = category_totals.setdefault(name, [0, 0, 0])
    totals[0] += 1
    totals[1] += row["ios"]["size"]
    totals[2] += row["android"]["size"]

def cell(value):
    return str(value).replace("|", r"\|")

lines = [
    "# AzureArchive Addressables bundle hash comparison",
    "",
    "## Provenance",
    "",
    "- Decoder: " + str(data.get("parser", "unknown")),
    "- iOS catalog: " + str(data.get("sources", {}).get("ios", "unknown")),
    "- Android catalog: " + str(data.get("sources", {}).get("android", "unknown")),
    "- Comparison key: decoded logical bundle name; hash and declared size are retained independently.",
    "",
    "## Counts",
    "",
    "| Metric | Count |",
    "|---|---:|",
    "| iOS bundle rows | " + str(counts["ios_bundle_entries"]) + " |",
    "| Android bundle rows | " + str(counts["android_bundle_entries"]) + " |",
    "| Common logical names | " + str(counts["common_names"]) + " |",
    "| iOS-only names | " + str(counts["ios_only"]) + " |",
    "| Android-only names | " + str(counts["android_only"]) + " |",
    "| Same catalog hash | " + str(counts["same_hash"]) + " |",
    "| Different catalog hash | " + str(counts["different_hash"]) + " |",
    "| Same hash, different declared size | " + str(len(same_hash_size_diff)) + " |",
    "",
    "## Changed-hash categories",
    "",
    "| Category | Changed bundles | iOS declared bytes | Android declared bytes |",
    "|---|---:|---:|---:|",
]
for name, totals in sorted(category_totals.items()):
    lines.append(
        "| " + name + " | " + str(totals[0]) + " | "
        + str(totals[1]) + " | " + str(totals[2]) + " |"
    )
lines += [
    "",
if archive_match_stats is not None:
    lines += [
        "",
        "## Legacy iOS archive cross-check",
        "",
        "| Check across different-hash rows | Count |",
        "|---|---:|",
        "| Changed rows with a matching legacy archive file | " + str(archive_match_stats["matched"]) + " |",
        "| Archive size equals legacy iOS catalog size | " + str(archive_match_stats["ios_size"]) + " |",
        "| Archive size equals Android 1.0 catalog size | " + str(archive_match_stats["android_size"]) + " |",
        "",
        "This is a size comparison, not a byte-level content hash. It establishes which catalog's declared sizes the supplied archive matches; it does not prove payload byte identity.",
    ]

    "## Same-hash rows with declared-size differences",
    "",
    "| Logical path | Hash | iOS bytes | Android bytes |",
    "|---|---|---:|---:|",
]
for row in same_hash_size_diff:
    ios, android = row["ios"], row["android"]
    lines.append(
        "| " + cell(ios["internal_id"]) + " | " + ios["hash"] + " | "
        + str(ios["size"]) + " | " + str(android["size"]) + " |"
    )

lines += [
    "",
    "## Different-hash rows (complete list)",
    "",
    "Catalog hashes are Addressables version identifiers, not independently verified file SHA-256 digests.",
    "",
    "| Logical path | iOS hash | iOS bytes | Android hash | Android bytes |",
    "|---|---|---:|---|---:|",
]
for row in sorted(changed, key=lambda item: item["ios"]["internal_id"].lower()):
    ios, android = row["ios"], row["android"]
    lines.append(
        "| " + cell(ios["internal_id"]) + " | " + ios["hash"] + " | "
        + str(ios["size"]) + " | " + android["hash"] + " | "
        + str(android["size"]) + " |"
    )

lines += [
    "",
    "## Interpretation and limits",
    "",
    "1. Equal logical names do not prove payload byte identity or cross-platform compatibility.",
    "2. Equal Addressables hashes are catalog metadata equality only; verify actual payload SHA-256, Unity build target, serialized types, and runtime behavior.",
    "3. Different hashes must not be substituted blindly or repaired by renaming hash-bearing bundle files.",
    "4. The 5.73 GB legacy iOS archive is matched to the old iOS 0.7.9.2 catalog, not proven to contain Android 1.0 payloads.",
    "5. The Android 1.0 APK catalog does not prove that all remote Addressables payloads are packaged inside the APK.",
    "",
]
args.output.parent.mkdir(parents=True, exist_ok=True)
args.output.write_text("\n".join(lines), encoding="utf-8")
print(
    "Wrote " + str(args.output) + ": " + str(len(rows)) + " rows, "
    + str(len(changed)) + " changed hashes, "
    + str(len(same_hash_size_diff)) + " same-hash size differences"
)
