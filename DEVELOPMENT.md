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

For a command-line smoke check, use the Godot 4.7.2 .NET executable available on your machine:

```bash
godot --headless --path game --quit-after 3
```

The exact executable name may differ by OS/install method.

## Bootstrap boundary

The current repository foundation intentionally does not implement Prototype A gameplay. Bootstrap establishes only the domain, project, content-validation, localization, debug/logging, test, and CI foundations required by `docs/29_PROJECT_BOOTSTRAP.md`.
