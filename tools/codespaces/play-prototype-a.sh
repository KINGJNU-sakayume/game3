#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cd "$repo_root"

if ! command -v godot >/dev/null 2>&1; then
  echo "[UNDO] Godot is not installed in this Codespace." >&2
  echo "[UNDO] Rebuild the Codespace container, then try again." >&2
  exit 1
fi

if [[ -z "${DISPLAY:-}" ]]; then
  echo "[UNDO] DISPLAY is not configured." >&2
  echo "[UNDO] Rebuild the Codespace so the desktop-lite feature is applied." >&2
  exit 1
fi

echo "[UNDO] Godot: $(godot --version)"
echo "[UNDO] Display: $DISPLAY"

if [[ -n "${CODESPACE_NAME:-}" ]] && [[ -n "${GITHUB_CODESPACES_PORT_FORWARDING_DOMAIN:-}" ]]; then
  echo "[UNDO] Desktop URL: https://${CODESPACE_NAME}-6080.${GITHUB_CODESPACES_PORT_FORWARDING_DOMAIN}"
fi

echo "[UNDO] Building C# game project..."
dotnet build game/Undo.Game.csproj --configuration Debug

echo "[UNDO] Launching Prototype A in the Codespaces desktop."
echo "[UNDO] Open PORTS -> 6080 -> Open in Browser if the desktop is not already visible."

# Codespaces generally has no hardware GPU. The rendering-method override is
# development-environment-only and does not modify project.godot or game content.
export LIBGL_ALWAYS_SOFTWARE=1
exec godot --path game --rendering-method gl_compatibility
