#!/usr/bin/env bash
# Re-downloads the OpenAPI definitions embedded in the Riskified API reference pages and stores
# each one as <flow>-<endpoint>.json next to this script. Commit the result so schema updates
# show up as a reviewed diff.
set -euo pipefail

BASE_URL="https://developers.riskified.com/reference"
SCHEMA_DIR="$(cd "$(dirname "$0")" && pwd)"

# <output name> <reference page>
PAGES=(
  "pre-auth-decide            pre-auth-flow-decide"
  "pre-auth-checkout_denied   pre-auth-flow-checkout_denied"
  "pre-auth-decision          pre-auth-flow-decision"
  "post-auth-submit           post-auth-flow-submit"
  "post-auth-decide           post-auth-flow-decide"
  "post-auth-checkout_denied  post-auth-flow-checkout_denied"
)

for entry in "${PAGES[@]}"; do
  read -r name page <<< "$entry"
  url="$BASE_URL/$page.md"
  target="$SCHEMA_DIR/$name.json"

  # The definition is the first ```json block after the "# OpenAPI definition" heading.
  curl -sSfL "$url" \
    | awk '/^# OpenAPI definition/ { found = 1; next }
           found && /^```json/      { inside = 1; next }
           inside && /^```/          { exit }
           inside                    { print }' \
    > "$target.tmp"

  if [ ! -s "$target.tmp" ]; then
    rm -f "$target.tmp"
    echo "No OpenAPI definition found in $url" >&2
    exit 1
  fi

  mv "$target.tmp" "$target"
  echo "$name.json <- $url"
done
