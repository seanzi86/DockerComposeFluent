#!/usr/bin/env bash
# Compares this checkout's public API with the last stable release on nuget.org, using .NET package
# validation, and reports anything that was removed or changed (adding members is always fine).
#
# Advisory by default: a break is reported as a warning and the script still exits 0. Pass --enforce to
# fail instead (the intended setting from 1.0). If nuget.org cannot be reached it warns and exits 0.
#
# Usage: scripts/check-api-compatibility.sh [--enforce] [version-being-released]
#   version-being-released  The version about to be released, e.g. 0.4.0. The baseline is then the newest
#                           stable version below it, so a release is never compared with itself. Without it,
#                           the baseline is the newest stable version and a "-dev" version is used.
set -uo pipefail

enforce=false
release_version=""
for arg in "$@"; do
  case "$arg" in
    --enforce) enforce=true ;;
    *) release_version="$arg" ;;
  esac
done

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
project="$repo_root/src/DockerComposeFluent/DockerComposeFluent.csproj"
index_url="https://api.nuget.org/v3-flatcontainer/dockercomposefluent/index.json"

report() {
  # $1 = title, $2 = message. A GitHub annotation when running in Actions, plain text otherwise.
  if [ -n "${GITHUB_ACTIONS:-}" ]; then
    echo "::warning title=$1::$2"
  else
    echo "WARNING: $1: $2"
  fi
}

versions="$(curl -fsS --max-time 20 "$index_url" 2>/dev/null)" || {
  report "API compatibility" "Could not read the published versions from nuget.org, so the API was not compared."
  exit 0
}

# Stable versions only (no pre-release suffix), oldest first.
stable="$(echo "$versions" | grep -oE '"[0-9]+\.[0-9]+\.[0-9]+"' | tr -d '"' | sort -V)"
if [ -n "$release_version" ]; then
  baseline="$(echo "$stable" | while read -r v; do
    [ "$(printf '%s\n%s\n' "$v" "$release_version" | sort -V | head -n1)" = "$v" ] && [ "$v" != "$release_version" ] && echo "$v"
  done | tail -n1)"
  version="$release_version"
else
  baseline="$(echo "$stable" | tail -n1)"
  version="${baseline}-dev"
fi

if [ -z "$baseline" ]; then
  echo "No earlier stable release to compare with; skipping."
  exit 0
fi

echo "Comparing the public API with DockerComposeFluent $baseline (building as $version)."
output_dir="$(mktemp -d)"
trap 'rm -rf "$output_dir"' EXIT

if log="$(dotnet pack "$project" --configuration Release --output "$output_dir" \
    -p:PackageValidationBaselineVersion="$baseline" -p:Version="$version" 2>&1)"; then
  echo "No breaking API changes since $baseline."
  exit 0
fi

echo "$log"
problems="$(echo "$log" | grep -E "error (CP|APICOMPAT)" | sed -E 's/^.*error //; s/ \[[^]]*\.csproj\]$//' | sort -u | head -n 20)"
[ -z "$problems" ] && problems="$(echo "$log" | grep -E "error " | sed -E 's/^.*error //' | sort -u | head -n 5)"

if [ -n "${GITHUB_STEP_SUMMARY:-}" ]; then
  {
    echo "### Public API changes since $baseline"
    echo
    echo '```'
    echo "$problems"
    echo '```'
    echo
    echo "If the break is intentional, see \"Intentional breaking changes\" in CONTRIBUTING.md."
  } >> "$GITHUB_STEP_SUMMARY"
fi

if [ "$enforce" = true ]; then
  echo "ERROR: the public API changed since $baseline (see above)."
  exit 1
fi

report "API compatibility" "The public API changed since $baseline; this is advisory for now. See the step log and CONTRIBUTING.md."
exit 0
