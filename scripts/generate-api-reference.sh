#!/usr/bin/env bash
# Regenerates the API reference pages under website/docs/api-reference from the library's XML doc comments.
# Run from anywhere; used by both the docs workflow and `npm run docs:api` (see website/package.json).
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
RAW_DIR="$(mktemp -d)"
trap 'rm -rf "$RAW_DIR"' EXIT

echo "Building DockerComposeFluent (Release)..."
dotnet build "$ROOT/src/DockerComposeFluent" --configuration Release

echo "Restoring the defaultdocumentation tool..."
cd "$ROOT"
dotnet tool restore

echo "Generating raw API reference markdown..."
dotnet tool run defaultdocumentation \
  --AssemblyFilePath "$ROOT/src/DockerComposeFluent/bin/Release/net10.0/DockerComposeFluent.dll" \
  --OutputDirectoryPath "$RAW_DIR" \
  --ConfigurationFilePath "$ROOT/website/DefaultDocumentation.json"

echo "Restructuring into website/docs/api-reference..."
node "$ROOT/website/scripts/build-api-reference.mjs" "$RAW_DIR" "$ROOT/website/docs/api-reference"

echo "Done."
