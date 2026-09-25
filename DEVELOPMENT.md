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
