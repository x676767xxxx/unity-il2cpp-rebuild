#!/usr/bin/env python3
"""Make already-handled Spine 4.2 enum cases explicit for warning-clean builds.

This patch is intentionally limited to the pinned converter's 4.2 reader/writer.
It does not suppress warnings or alter the binary encoding: the special timeline
cases are handled immediately before their switches, and Point attachments carry
no optional flags in the preceding flag-discovery switch.
"""
from pathlib import Path

ROOT = Path("external/SpineSkeletonDataConverter/src")

PATCHES = {
    "SkeletonData42BinaryReader.cpp": [
        (
            "            switch (timelineType) {\n                case BONE_ROTATE: {",
            "            switch (timelineType) {\n                // Handled by the BONE_INHERIT fast path immediately above.\n                case BONE_INHERIT:\n                    break;\n                case BONE_ROTATE: {",
        ),
        (
            "            switch (timelineType) {\n                case PHYSICS_INERTIA: {",
            "            switch (timelineType) {\n                // Handled by the PHYSICS_RESET fast path immediately above.\n                case PHYSICS_RESET:\n                    break;\n                case PHYSICS_INERTIA: {",
        ),
    ],
    "SkeletonData42BinaryWriter.cpp": [
        (
            "            switch (attachment.type) {\n                case AttachmentType_Region: {",
            "            switch (attachment.type) {\n                // Point attachments have no optional flags in this flag-discovery pass.\n                case AttachmentType_Point:\n                    break;\n                case AttachmentType_Region: {",
        ),
        (
            "            switch (timelineType) {\n                case BONE_ROTATE:",
            "            switch (timelineType) {\n                // Handled by the BONE_INHERIT fast path immediately above.\n                case BONE_INHERIT:\n                    break;\n                case BONE_ROTATE:",
        ),
    ],
}

def main() -> None:
    for filename, replacements in PATCHES.items():
        path = ROOT / filename
        if not path.is_file():
            raise SystemExit(f"Missing pinned Spine 4.2 source file: {path}")
        source = path.read_text(encoding="utf-8")
        for old, new in replacements:
            count = source.count(old)
            if count == 0:
                # Idempotent reruns are allowed only when the intended case is present.
                marker = new.splitlines()[1].strip()
                if marker in source:
                    continue
                raise SystemExit(f"Expected patch context not found in {path}: {old!r}")
            if count != 1:
                raise SystemExit(f"Ambiguous patch context ({count} matches) in {path}")
            source = source.replace(old, new, 1)
        path.write_text(source, encoding="utf-8")
        print(f"PASS: applied/verified explicit Spine 4.2 switch cases in {path}")

if __name__ == "__main__":
    main()
