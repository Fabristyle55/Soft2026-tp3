#!/usr/bin/env bash
# Configura las Branch Protection Rules de main y development con la GitHub CLI.
# Uso: gh auth login  &&  ./scripts/configurar-proteccion-ramas.sh <owner>/<repo> [aprobaciones]
set -euo pipefail

REPO="${1:?Uso: $0 <owner>/<repo> [aprobaciones]}"
APROBACIONES="${2:-2}"   # La DoD del equipo pide 2 revisores (la consigna exige al menos 1)

# Crea development a partir de main si todavía no existe.
if ! gh api "repos/$REPO/branches/development" >/dev/null 2>&1; then
  SHA=$(gh api "repos/$REPO/git/ref/heads/main" --jq .object.sha)
  gh api "repos/$REPO/git/refs" -f ref=refs/heads/development -f sha="$SHA" >/dev/null
  echo "Rama development creada desde main"
fi

for RAMA in main development; do
  gh api -X PUT "repos/$REPO/branches/$RAMA/protection" --input - <<JSON
{
  "required_status_checks": { "strict": true, "contexts": ["Build y tests", "Imagen Docker"] },
  "enforce_admins": true,
  "required_pull_request_reviews": {
    "required_approving_review_count": $APROBACIONES,
    "dismiss_stale_reviews": true
  },
  "restrictions": null,
  "allow_force_pushes": false,
  "allow_deletions": false,
  "required_conversation_resolution": true
}
JSON
  echo "Protección aplicada a $RAMA ($APROBACIONES aprobaciones, CI obligatorio, sin push directo)"
done
