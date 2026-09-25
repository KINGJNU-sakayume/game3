#!/usr/bin/env bash
set -euo pipefail

GODOT_VERSION="4.7.2"
GODOT_ARCHIVE="Godot_v${GODOT_VERSION}-stable_mono_linux_x86_64.zip"
GODOT_URL="https://github.com/godotengine/godot/releases/download/${GODOT_VERSION}-stable/${GODOT_ARCHIVE}"
INSTALL_DIR="/opt/godot-${GODOT_VERSION}-mono"
SYMLINK="/usr/local/bin/godot"

if command -v godot >/dev/null 2>&1; then
  installed_version="$(godot --version 2>/dev/null || true)"
  if [[ "$installed_version" == ${GODOT_VERSION}* ]] && [[ "$installed_version" == *mono* ]]; then
    echo "[CODESPACES] Godot ${installed_version} already installed."
    exit 0
  fi
fi

echo "[CODESPACES] Installing GUI/runtime dependencies..."
sudo apt-get update -qq
sudo DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends \
  ca-certificates \
  curl \
  unzip \
  libasound2 \
  libfontconfig1 \
  libgl1 \
  libegl1 \
  libpulse0 \
  libx11-6 \
  libxcursor1 \
  libxext6 \
  libxi6 \
  libxinerama1 \
  libxrandr2 \
  libxrender1 \
  libwayland-client0 \
  libvulkan1 \
  mesa-vulkan-drivers
sudo rm -rf /var/lib/apt/lists/*

tmp_dir="$(mktemp -d)"
trap 'rm -rf "$tmp_dir"' EXIT

archive_path="$tmp_dir/$GODOT_ARCHIVE"

echo "[CODESPACES] Downloading Godot ${GODOT_VERSION} .NET..."
curl -fL --retry 3 --retry-delay 2 "$GODOT_URL" -o "$archive_path"

echo "[CODESPACES] Extracting Godot..."
unzip -q "$archive_path" -d "$tmp_dir/extracted"

bundle_dir="$(find "$tmp_dir/extracted" -mindepth 1 -maxdepth 1 -type d -name "Godot_v${GODOT_VERSION}-stable_mono_linux_x86_64" -print -quit)"
if [[ -z "$bundle_dir" ]]; then
  echo "[CODESPACES] ERROR: Godot bundle directory was not found after extraction." >&2
  exit 1
fi

godot_binary="$(find "$bundle_dir" -maxdepth 1 -type f -name "Godot_v${GODOT_VERSION}-stable_mono_linux.x86_64" -print -quit)"
if [[ -z "$godot_binary" ]]; then
  echo "[CODESPACES] ERROR: Godot executable was not found after extraction." >&2
  exit 1
fi

sudo rm -rf "$INSTALL_DIR"
sudo mkdir -p "$INSTALL_DIR"
sudo cp -a "$bundle_dir"/. "$INSTALL_DIR"/

installed_binary="$INSTALL_DIR/$(basename "$godot_binary")"
sudo chmod +x "$installed_binary"
sudo ln -sfn "$installed_binary" "$SYMLINK"

echo "[CODESPACES] Installed: $(godot --version)"
echo "[CODESPACES] .NET SDK: $(dotnet --version)"
echo "[CODESPACES] noVNC desktop is provided by the devcontainer desktop-lite feature on port 6080."
