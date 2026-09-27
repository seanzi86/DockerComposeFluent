#!/usr/bin/env bash
# Regenerates the property tables in website/docs/supported-properties.md from the library's XML doc
# comments. Run from anywhere; used by both the docs workflow and `npm run docs:supported`
# (see website/package.json). Unlike the API reference, this file is committed: review the diff after running.
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

echo "Building DockerComposeFluent (Release)..."
dotnet build "$ROOT/src/DockerComposeFluent" --configuration Release

echo "Updating website/docs/supported-properties.md..."
node "$ROOT/website/scripts/build-supported-properties.mjs" \
  "$ROOT/src/DockerComposeFluent/bin/Release/net10.0/DockerComposeFluent.xml" \
  "$ROOT/website/docs/supported-properties.md"

echo "Done."
