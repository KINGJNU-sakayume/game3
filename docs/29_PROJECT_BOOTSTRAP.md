# UNDO — Project Bootstrap Specification

Last locked: 2026-09-25

## Purpose

This document defines the first implementation task after pre-production design approval.

Bootstrap creates a clean, testable repository foundation for Prototype A.

It does **not** implement the campaign.

## Toolchain

Locked baseline:

- Godot 4.7.2 .NET editor
- .NET 8 SDK
- C# 12 baseline
- Git
- xUnit for `Undo.Core` tests
- Windows desktop as initial run/export target

A developer may use Rider, Visual Studio, VS Code, or another compatible editor. IDE choice is not canonical.

## Root structure after bootstrap

```
/
├─ AGENTS.md
├─ README.md
├─ UNDO.sln
├─ .editorconfig
├─ .gitignore
├─ docs/
│
├─ game/
│  ├─ project.godot
│  ├─ Undo.Game.csproj
│  ├─ content/
│  │  ├─ manifests/
│  │  └─ puzzles/
│  ├─ localization/
│  ├─ scenes/
│  │  ├─ bootstrap/
│  │  ├─ player/
│  │  ├─ actors/
│  │  ├─ interactables/
│  │  ├─ levels/
│  │  └─ ui/
│  ├─ resources/
│  │  └─ presentation/
│  ├─ scripts/
│  │  ├─ application/
│  │  ├─ presentation/
│  │  ├─ input/
│  │  └─ debug/
│  ├─ art/
│  └─ audio/
│
├─ src/
│  └─ Undo.Core/
│     └─ Undo.Core.csproj
│
├─ tests/
│  └─ Undo.Core.Tests/
│     └─ Undo.Core.Tests.csproj
│
└─ tools/
   └─ Undo.ContentValidator/
      └─ Undo.ContentValidator.csproj
```

Godot folder/file names should generally use lowercase snake_case.

C# source files use normal C# PascalCase file/class conventions.

## Solution projects

### Undo.Core

Path:
`src/Undo.Core/Undo.Core.csproj`

Target:
`net8.0`

Purpose:
- engine-independent causal domain,
- IDs,
- state,
- events,
- corrections,
- actor behavior definitions/evaluation,
- content DTOs/validation primitives,
- save DTOs.

Forbidden dependency:
- Godot assemblies.

### Undo.Game

Path:
`game/Undo.Game.csproj`

Godot .NET project.

Purpose:
- Godot application,
- scene bindings,
- player controller,
- navigation execution,
- presentation,
- Review Mode,
- audio/UI,
- level loading.

Must reference:
`../src/Undo.Core/Undo.Core.csproj`

### Undo.Core.Tests

Path:
`tests/Undo.Core.Tests/Undo.Core.Tests.csproj`

Target:
`net8.0`

Purpose:
- xUnit domain regression suite.

References:
- `Undo.Core`

No Godot dependency for core tests.

### Undo.ContentValidator

Path:
`tools/Undo.ContentValidator/Undo.ContentValidator.csproj`

Target:
`net8.0`

Purpose:
- validate causal JSON,
- validate manifests,
- validate localization-key references,
- emit command-line errors suitable for CI/Codex review.

References:
- `Undo.Core`

It must not require launching Godot for ordinary semantic validation.

## Conceptual creation sequence

Equivalent .NET project creation may vary by environment, but the resulting structure must match the architecture.

Example sequence for non-Godot projects:

```bash
dotnet new sln -n UNDO

dotnet new classlib   -n Undo.Core   -o src/Undo.Core   -f net8.0

dotnet new xunit   -n Undo.Core.Tests   -o tests/Undo.Core.Tests   -f net8.0

dotnet new console   -n Undo.ContentValidator   -o tools/Undo.ContentValidator   -f net8.0

dotnet add tests/Undo.Core.Tests/Undo.Core.Tests.csproj   reference src/Undo.Core/Undo.Core.csproj

dotnet add tools/Undo.ContentValidator/Undo.ContentValidator.csproj   reference src/Undo.Core/Undo.Core.csproj
```

The Godot C# project should be generated/configured using the Godot 4.7.2 .NET toolchain rather than guessing a future Godot SDK version.

After creation:

```bash
dotnet sln UNDO.sln add src/Undo.Core/Undo.Core.csproj
dotnet sln UNDO.sln add tests/Undo.Core.Tests/Undo.Core.Tests.csproj
dotnet sln UNDO.sln add tools/Undo.ContentValidator/Undo.ContentValidator.csproj
dotnet sln UNDO.sln add game/Undo.Game.csproj
```

Exact CLI syntax may differ slightly by installed .NET CLI version; the repository result is authoritative.

## Game project baseline

`game/project.godot` initial configuration should establish:

- 3D project,
- Forward+ renderer,
- Windows desktop development target,
- neutral temporary window title `UNDO`,
- input-map placeholders,
- main bootstrap scene once created.

Do not configure final graphics presets during bootstrap.

## Initial scene tree

Create a minimal bootstrap scene capable of starting the application safely.

Conceptual:

```
Bootstrap
├─ GameRoot
├─ LevelRoot
├─ UiRoot
└─ DebugRoot
```

Names may adjust if implementation proves a cleaner structure.

Do not create a giant global manager node containing all systems.

## Core bootstrap types

Before Prototype A gameplay work, create only enough domain primitives to establish architecture.

Initial expected types include conceptually:

```
EntityId
EventId
LocationId
PuzzleId

StateChannel
StateKey
StateValue
StateMutation

RecordedEvent
EventLedger
WorldStateStore
EffectiveStateResolver

CorrectionState
CorrectionManager
```

Do not implement Actor AI or the entire campaign during bootstrap unless required by the explicit task.

## First golden test

The first meaningful test should prove domain architecture without Godot:

```
initial DOOR_A.OPEN = true
E1 CLOSE => false
effective = false

suppress E1
effective = true

release E1
effective = false
```

This test validates the architectural boundary before scene work.

## Content bootstrap

Create:

```
game/content/manifests/puzzles.json
game/content/puzzles/PT_A_BASIC_CORRECTION/puzzle.json
```

The initial Prototype A puzzle file may contain only the data needed to validate:
- schema version,
- PuzzleId,
- correction capacity,
- Door A initial state,
- worker actor identity/routine shell,
- semantic locations,
- completion shell.

Do not populate later campaign content just to make directories look complete.

## Content validator bootstrap

Initial validator only needs to prove the pipeline.

Minimum checks:
- JSON parses,
- supported schemaVersion,
- PuzzleId non-empty/valid form,
- unique IDs in a puzzle,
- valid known StateChannel,
- valid type for the channel,
- referenced semantic locations exist.

Then expand according to `docs/27_DATA_AUTHORING.md`.

## Localization bootstrap

Create the localization folder and source CSVs:

```
game/localization/ui.csv
game/localization/subtitles.csv
game/localization/documents.csv
game/localization/signage.csv
game/localization/system.csv
```

Bootstrap files may contain headers and a small number of real keys.

At minimum:
- pause continue,
- restore current record,
- settings,
- exit/return,
- Prototype A temporary name if displayed.

Do not populate hundreds of placeholder strings.

## `.gitignore`

Must exclude at minimum:
- Godot import/cache directory `.godot/`,
- .NET build output `bin/`,
- `obj/`,
- IDE user/cache files,
- export credentials/private signing material,
- generated local logs/build output.

Must **not** ignore:
- `project.godot`,
- `.tscn`,
- `.tres`,
- causal JSON,
- localization source CSV,
- `export_presets.cfg` when intentionally created,
- source code,
- docs.

## `.editorconfig`

Add repository-wide text rules.

Minimum intent:
- UTF-8,
- LF preferred in repository text,
- final newline,
- C# standard formatting compatible with `dotnet format`,
- JSON two-space indentation,
- Markdown consistent whitespace policy.

Do not force rules that fight Godot-generated text formats.

## Dependencies

Bootstrap third-party dependencies should be minimal.

Expected:
- xUnit packages created by the standard test template.

Do not add:
- DI framework,
- ECS framework,
- behavior-tree package,
- reactive framework,
- JSON framework other than `System.Text.Json` unless a demonstrated need emerges,
- save framework,
- tween/UI framework.

Godot already supplies engine-level facilities; causal architecture should remain small.

## Input placeholders

Input architecture should allow rebinding.

Prototype baseline conceptual actions:
- `move_forward`
- `move_back`
- `move_left`
- `move_right`
- `review`
- `correction_commit`
- `correction_release`
- `pause`

Exact physical key bindings are not yet canonical.

Do not hardwire gameplay code directly to specific keyboard key codes.

## Debug mode

Bootstrap should reserve an explicit development/debug path.

Eventually debug tooling exposes:
- event ledger,
- state values,
- suppressions,
- actor behavior.

Prototype A bootstrap may begin with console/structured logging and a very simple overlay shell.

Debug UI does not need final player-facing art.

## Build expectations

At the end of bootstrap:

```
dotnet restore
dotnet build
dotnet test
```

must succeed for the solution's non-editor requirements.

The Godot project must:
- open in Godot 4.7.2 .NET,
- compile C#,
- run the bootstrap scene,
- load without missing-script/resource errors.

A headless smoke command should be established once the bootstrap scene exists.

## Headless expectation

Use Godot's command-line/headless mode for CI-friendly scene startup checks.

The precise command is recorded in repository scripts/CI after the project exists, because executable naming differs by development environment.

Conceptual:

```
godot --headless --path game --quit-after <small_frame_count>
```

The CI/editor binary must be the Godot .NET editor compatible with the project.

## Bootstrap non-goals

Do not implement:
- campaign acts,
- final first-person art,
- Seojin narrative,
- multi-slot correction,
- final UI,
- full NPC reaction system,
- save migration system,
- final audio,
- Steam/platform integration.

## Bootstrap acceptance criteria

Repository bootstrap is complete only when:

1. Godot 4.7.2 .NET project exists under `game/`.
2. `Undo.Core` builds without Godot dependency.
3. xUnit project runs a real correction-state golden test.
4. Godot project references `Undo.Core`.
5. content JSON location exists.
6. validator executable can validate Prototype A's minimal data.
7. localization source structure exists.
8. .gitignore excludes generated caches/output.
9. fresh-clone setup instructions are present in README or a developer doc.
10. CI can be added without restructuring the repository.
11. no campaign implementation has accidentally started.

## Codex task boundary

A bootstrap Codex task must not silently continue into Prototype A after satisfying this document.

Bootstrap and Prototype A should be separate reviewable tasks/PRs unless the user explicitly requests them together.
