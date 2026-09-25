# UNDO development setup

Repository design canon lives under `docs/`. Read `AGENTS.md` before implementation work.

## Required toolchain

- Git
- .NET 8 SDK
- Godot 4.7.2 stable .NET editor

Godot must be the .NET/Mono build, not the standard non-.NET editor.

## Fresh clone

```bash
git clone https://github.com/KINGJNU-sakayume/game3.git
cd game3
dotnet restore UNDO.sln
dotnet build UNDO.sln --configuration Release --no-restore
dotnet test tests/Undo.Core.Tests/Undo.Core.Tests.csproj --configuration Release --no-build
dotnet run --project tools/Undo.ContentValidator/Undo.ContentValidator.csproj --configuration Release --no-build -- game/content
```

Then open `game/project.godot` in Godot 4.7.2 .NET.

## Run Prototype A

The project main scene is `PT_A_BASIC_CORRECTION`.

From the Godot editor, press **Run Project (F6/F5 as configured by the editor)** after opening `game/project.godot`.

From a shell, substitute your local Godot 4.7.2 .NET executable name if it is not `godot`:

```bash
godot --path game
```

Prototype A controls:

- Move — WASD
- Look — Mouse
- Review Door A — hold Right Mouse Button while targeting the door
- Commit correction — Left Mouse Button while in Review
- Release maintained correction — Q while in Review
- Inspect — E (non-causal; Door A does not directly respond)
- Pause / Restore Current Record — Escape
- Development debug overlay — F3

## GitHub Codespaces browser playtest

The repository includes a Codespaces-only desktop environment. It installs:

- .NET 8,
- the official Dev Container `desktop-lite` noVNC desktop,
- Godot 4.7.2 stable .NET,
- a private forwarded desktop port on `6080`.

This environment does not change game rules, scenes, inputs, content, or the canonical desktop target.

After the Codespaces configuration is first added or changed, an existing Codespace must be rebuilt:

1. Open the Command Palette.
2. Run **Codespaces: Rebuild Container** (or **Dev Containers: Rebuild Container** if that is the command shown).
3. Wait for `postCreateCommand` to finish installing Godot and restoring the solution.
4. Open the **PORTS** panel.
5. On port **6080 — UNDO Desktop (noVNC)** choose **Open in Browser**.
6. In the VS Code Command Palette run **Tasks: Run Task**.
7. Choose **UNDO: Play Prototype A (Codespaces)**.
8. Return to the 6080 browser tab; the Prototype A game window should appear there.

The Codespaces play task builds the existing C# project and launches the existing main scene. It uses Godot's `gl_compatibility` renderer as a runtime-only override because Codespaces normally has no hardware GPU. It does not modify `game/project.godot`.

To run all automated Prototype A checks from VS Code without typing commands, choose:

**Tasks: Run Task → UNDO: Validate Prototype A**

If `godot: command not found` appears, the container has not been rebuilt with the repository's `.devcontainer/devcontainer.json`.

If port 6080 produces an error immediately after a rebuild, wait for the Codespace startup to finish and open the port again from the **PORTS** panel rather than reusing an old forwarded-port tab.

## Command-line validation

```bash
dotnet restore UNDO.sln
dotnet build UNDO.sln --configuration Release --no-restore
dotnet test tests/Undo.Core.Tests/Undo.Core.Tests.csproj --configuration Release --no-build
dotnet run --project tools/Undo.ContentValidator/Undo.ContentValidator.csproj --configuration Release --no-build -- game/content
godot --headless --path game --quit-after 3
godot --headless --path game res://scenes/tests/prototype_a_integration.tscn
```

The exact Godot executable name may differ by OS/install method.

## Current implementation boundary

Prototype A is implemented for mechanic validation. Prototype B, NPC ReactionRules, campaign levels, final art/audio/UI, and later mechanics remain intentionally unimplemented.
