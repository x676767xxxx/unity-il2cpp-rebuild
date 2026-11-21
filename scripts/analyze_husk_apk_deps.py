#!/usr/bin/env python3
"""Inventory Android ARM64 ELF dependencies in an APK for Husk integration.

This is static evidence only. Android platform imports are not marked missing:
they must be matched against Husk's Bionic/JNI/graphics shims and runtime logs.
"""
from __future__ import annotations
import re
import subprocess
import sys
import zipfile
from collections import defaultdict
from pathlib import Path

apk = Path(sys.argv[1])
out = Path(sys.argv[2])
out.mkdir(parents=True, exist_ok=True)
libdir = out / "arm64"
libdir.mkdir(exist_ok=True)

with zipfile.ZipFile(apk) as z:
    names = sorted(n for n in z.namelist()
                   if n.startswith("lib/arm64-v8a/") and n.endswith(".so"))
    for name in names:
        target = libdir / Path(name).name
        target.write_bytes(z.read(name))

provided = {Path(n).name for n in names}
needed_by = defaultdict(list)
undefined_by = defaultdict(list)
report = [
    "# AzureArchive ARM64 ELF dependency inventory",
    "",
    "Static ELF inventory only; an Android import is not proof of a missing implementation.",
    "DT_NEEDED names are grouped as packaged, Android platform, or unresolved/needs mapping review.",
    "",
]
platform = {
    "libc.so", "libm.so", "libdl.so", "liblog.so", "libandroid.so",
    "libEGL.so", "libGLESv1_CM.so", "libGLESv2.so", "libGLESv3.so",
    "libvulkan.so", "libOpenSLES.so", "libz.so", "libjnigraphics.so",
    "libatomic.so", "libmediandk.so", "libcamera2ndk.so", "libaaudio.so",
    "libnativewindow.so", "libsync.so", "libc++_shared.so",
}
for name in sorted(provided):
    path = libdir / name
    dyn = subprocess.run(["readelf", "-d", str(path)], text=True,
                         stdout=subprocess.PIPE, stderr=subprocess.PIPE)
    syms = subprocess.run(["readelf", "--dyn-syms", "--wide", str(path)], text=True,
                          stdout=subprocess.PIPE, stderr=subprocess.PIPE)
    if dyn.returncode:
        report.append(f"## {name}\n- readelf dynamic table failed: {dyn.stderr.strip()}")
        continue
    needed = sorted(set(re.findall(r"Shared library: \[([^]]+)\]", dyn.stdout)))
    for dep in needed:
        needed_by[dep].append(name)
    und = set()
    for line in syms.stdout.splitlines():
        match = re.match(r"^\s*\d+:\s+\S+\s+\d+\s+\S+\s+\S+\s+\S+\s+UND\s+(\S+)", line)
        if match:
            sym = match.group(1).split("@", 1)[0]
            if sym and sym not in ("_ITM_deregisterTMCloneTable", "_ITM_registerTMCloneTable",
                                   "__gmon_start__", "__cxa_finalize"):
                und.add(sym)
    undefined_by[name] = sorted(und)

report += ["## Packaged ARM64 libraries", ""]
report += [f"- {n}" for n in sorted(provided)]
report += ["", "## DT_NEEDED dependencies", ""]
if not needed_by:
    report.append("- No DT_NEEDED entries found.")
for dep, owners in sorted(needed_by.items()):
    if dep in provided:
        status = "packaged in APK"
    elif dep in platform:
        status = "Android platform dependency; map against Husk shims"
    else:
        status = "not packaged / needs mapping review"
    report.append(f"- **{dep}** — {status}; required by: {', '.join(sorted(owners))}")
report += ["", "## Undefined dynamic symbol counts", ""]
for name, symbols in sorted(undefined_by.items()):
    report.append(f"- {name}: {len(symbols)} undefined dynamic symbols")
report += ["", "## Full undefined symbol inventory", ""]
for name, symbols in sorted(undefined_by.items()):
    report += [f"### {name}", ""]
    report += [f"- {s}" for s in symbols]
    report.append("")
(out / "elf-dependency-map.md").write_text("\n".join(report) + "\n")
print("\n".join(report[:80]))
