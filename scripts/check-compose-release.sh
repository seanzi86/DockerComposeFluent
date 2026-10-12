#!/usr/bin/env bash
# Opens (or updates) an issue when Docker Compose has released a newer version than the one CI pins, so the
# pinned version does not go stale unnoticed. Dependabot cannot do this: it does not update the version input
# of docker/setup-compose-action.
#
# Needs the GitHub CLI, authenticated (GH_TOKEN in Actions). With --dry-run it only prints what it would do.
#
# Usage: scripts/check-compose-release.sh [--dry-run]
#
# For testing, COMPOSE_PINNED and COMPOSE_LATEST override the two versions that are otherwise read from
# .github/workflows/ci.yml and from the latest stable docker/compose release.
set -uo pipefail

dry_run=false
[ "${1:-}" = "--dry-run" ] && dry_run=true

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
label="compose-release"

pinned="${COMPOSE_PINNED:-$(awk '/docker\/setup-compose-action/{found=1} found && /version:/{print $2; exit}' "$repo_root/.github/workflows/ci.yml")}"
if [ -z "$pinned" ]; then
  echo "ERROR: could not find the pinned Compose version in .github/workflows/ci.yml." >&2
  exit 1
fi

# "latest" is the newest published release that is not a pre-release or draft.
latest="${COMPOSE_LATEST:-$(gh api repos/docker/compose/releases/latest --jq .tag_name)}"
if [ -z "$latest" ]; then
  echo "ERROR: could not read the latest Docker Compose release." >&2
  exit 1
fi

echo "CI pins Docker Compose $pinned; the latest release is $latest."

newest="$(printf '%s\n%s\n' "$pinned" "$latest" | sort -V | tail -n1)"
if [ "$latest" = "$pinned" ] || [ "$newest" != "$latest" ]; then
  echo "CI is on the latest Docker Compose. Nothing to do."
  exit 0
fi

title="Docker Compose $latest is out (CI pins $pinned)"
body="$(cat <<BODY
Docker Compose **$latest** has been released ([release notes](https://github.com/docker/compose/releases/tag/$latest)). CI validates the golden fixtures with **$pinned**, pinned in \`.github/workflows/ci.yml\`.

To catch up:

- [ ] Read the release notes for new compose-file attributes or changed behaviour.
- [ ] Bump \`version:\` under \`docker/setup-compose-action\` in \`.github/workflows/ci.yml\` to \`$latest\`, and check the golden fixtures still validate.
- [ ] Re-run the parity audit (diff the compose-spec JSON schema against the modelled properties) and file issues for anything new.
- [ ] Add "Requires Compose X or later" remarks for new properties, taking the versions from the release notes.

This issue is opened and kept up to date by \`.github/workflows/compose-release-watch.yml\`. Close it once CI is on the new version.
BODY
)"

if [ "$dry_run" = true ]; then
  echo
  echo "Would open or update an issue:"
  echo "  Title: $title"
  echo "$body" | sed 's/^/  | /'
  exit 0
fi

gh label create "$label" --description "A newer Docker Compose has been released" --color 0E8A16 2>/dev/null || true

existing="$(gh issue list --label "$label" --state open --json number,title --jq '.[0] | select(. != null) | "\(.number)\t\(.title)"')"
if [ -z "$existing" ]; then
  gh issue create --title "$title" --body "$body" --label "$label"
  exit $?
fi

number="${existing%%$'\t'*}"
if [ "${existing#*$'\t'}" = "$title" ]; then
  echo "Issue #$number already reports $latest. Nothing to do."
else
  # An older report is still open: point it at the newest version rather than piling up issues.
  gh issue edit "$number" --title "$title" --body "$body"
  echo "Updated issue #$number to $latest."
fi
