#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
EXPECTED_HUSK_COMMIT="acfd7bcb6d1d2f6655a24e43d1d6f87cf142667a"
ACTUAL_HUSK_COMMIT="$(git -C "$ROOT/Husk" rev-parse HEAD)"

if [[ "$ACTUAL_HUSK_COMMIT" != "$EXPECTED_HUSK_COMMIT" ]]; then
  echo "Unexpected Husk submodule revision: $ACTUAL_HUSK_COMMIT" >&2
  echo "Expected pinned Husk 3.4: $EXPECTED_HUSK_COMMIT" >&2
  exit 1
fi

OUT="${1:-$ROOT/Husk/build/Husk-AzureArchive.ipa}"
mkdir -p "$(dirname "$OUT")"

python3 "$ROOT/scripts/apply_husk_3_4_unity_version_fix.py"
cd "$ROOT/Husk"
./scripts/ci_build.sh "$OUT"

echo "Built IPA: $OUT"
echo "TrollStore package: ${OUT%.ipa}.tipa"
