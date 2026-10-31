# AzureArchive iOS port progress — ABI hardening checkpoint

Date: 2026-10-31

## Changes committed on this branch
- Updated `scripts/prepare-ios-spine-converter.py` so the C ABI validates output pointers first, clears valid output parameters to `nullptr` / zero, then validates input pointer and length. This prevents callers from retaining stale output pointer/length values after an invalid-input failure.
- Expanded `scripts/ios_abi_smoke_test.cpp` with four negative-contract checks: null JSON input, empty JSON input, invalid destination Spine version, and null output pointer.
- Updated the iOS converter workflow path filter so changes to the smoke-test source trigger CI.

## CI runs triggered by these commits
- [Run 41183597756](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41183597756): started by the ABI guard change.
- [Run 41183603298](https://github.com/x676767xxxx/unity-il2cpp-rebuild/actions/runs/41183603298): started by the workflow path-filter change.
- At the time of this checkpoint both runs were still in progress; no result or artifact is claimed yet.

## Why this change matters
The C ABI is intended to be called from managed/runtime code. On invalid input, output parameters must be deterministic so callers cannot accidentally reuse stale pointers or lengths. The new smoke checks exercise those failure contracts in addition to the existing JSON-to-SKEL round-trip.

## Delivery status
This is ABI hardening for the converter component only. It does not create the missing Unity project, integrate the game runtime, prove compatibility with actual AzureArchive Spine assets, or produce an IPA. Host-side tests are not iPhone device tests.
