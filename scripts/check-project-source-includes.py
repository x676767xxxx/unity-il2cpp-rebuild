#!/usr/bin/env python3
"""Fail CI on missing or duplicated explicit C# source includes."""
from pathlib import Path
import sys
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
PROJECTS = [
    ROOT / "src/AzureArchive.BackgroundCompat/AzureArchive.BackgroundCompat.csproj",
    ROOT / "tests/AzureArchive.BackgroundCompat.Tests/AzureArchive.BackgroundCompat.Tests.csproj",
    ROOT / "tests/AzureArchive.UnityCompat.Tests/AzureArchive.UnityCompat.Tests.csproj",
]

errors = []
checked = 0
for project in PROJECTS:
    if not project.is_file():
        errors.append(f"missing project file: {project.relative_to(ROOT)}")
        continue
    xml_root = ET.parse(project).getroot()
    seen_paths = {}
    seen_links = {}
    for node in xml_root.iter():
        if node.tag.split("}")[-1] != "Compile":
            continue
        include = node.attrib.get("Include")
        if not include:
            continue
        checked += 1
        resolved = (project.parent / include.replace("\\", "/")).resolve()
        normalized = resolved.relative_to(ROOT).as_posix() if resolved.is_relative_to(ROOT) else str(resolved)
        link = node.attrib.get("Link")
        if normalized in seen_paths:
            errors.append(
                f"{project.relative_to(ROOT)}: duplicate Compile Include '{include}' "
                f"(also '{seen_paths[normalized]}', resolves to {normalized})"
            )
        else:
            seen_paths[normalized] = include
        if link:
            if link in seen_links:
                errors.append(
                    f"{project.relative_to(ROOT)}: duplicate Link '{link}' "
                    f"for '{include}' and '{seen_links[link]}'"
                )
            else:
                seen_links[link] = include
        if not resolved.is_file():
            errors.append(
                f"{project.relative_to(ROOT)}: Compile Include '{include}' does not exist"
            )

if errors:
    print("SOURCE INCLUDE PREFLIGHT FAILED")
    for error in errors:
        print(f" - {error}")
    sys.exit(1)

print(f"PASS: {checked} explicit C# source includes checked; no missing paths or duplicate includes/Link names.")
