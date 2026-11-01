#!/usr/bin/env python3
"""Fail CI if the managed iOS P/Invoke bridge drifts from the exported C ABI."""
from pathlib import Path
import re
import sys

root = Path(__file__).resolve().parents[1]
header = (root / "scripts/azurearchive_spine_converter_abi.h").read_text(encoding="utf-8")
bridge = (root / "src/AzureArchive.UnityCompat/SpineConverterNative.Unity.cs").read_text(encoding="utf-8")
prepare = (root / "scripts/prepare-ios-spine-converter.py").read_text(encoding="utf-8")

symbols = (
    "AzureArchiveSpineJsonToBinary",
    "AzureArchiveSpineConvertBuffer",
    "AzureArchiveSpineFreeBuffer",
)

errors = []
for symbol in symbols:
    if symbol not in header:
        errors.append(f"missing C ABI declaration: {symbol}")
    entry = re.search(
        r'DllImport\("__Internal",\s*EntryPoint\s*=\s*"' + re.escape(symbol) + r'"([\s\S]{0,160}?)\)',
        bridge,
    )
    if not entry:
        errors.append(f"missing __Internal P/Invoke entry point: {symbol}")
    elif "CallingConvention.Cdecl" not in entry.group(1):
        errors.append(f"non-Cdecl P/Invoke declaration: {symbol}")

if "private const int Spine42Version = 6;" not in bridge:
    errors.append("Spine 4.2 destination enum must remain Version42 = 6")

if "json_len, 6, true, out_buf, out_len" not in prepare:
    errors.append("JSON-to-binary native ABI must explicitly target Spine 4.2 (Version42 = 6)")

if "#if UNITY_IOS && !UNITY_EDITOR" not in bridge:
    errors.append("bridge must be restricted to device iOS builds")

if errors:
    print("FAIL: iOS Spine P/Invoke ABI contract")
    for error in errors:
        print(" - " + error)
    sys.exit(1)

print(f"PASS: {len(symbols) * 3 + 2} iOS Spine P/Invoke ABI contract checks")
