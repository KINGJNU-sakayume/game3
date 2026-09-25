# UNDO — CI and Repository Quality Workflow

Last locked: 2026-09-25

## Purpose

CI exists to catch causal/domain/content breakage before a branch is merged.

It is not intended to reproduce every visual/editor review.

## Initial GitHub Actions checks

Once bootstrap implementation exists, the default pull-request workflow should conceptually run:

1. repository/text sanity,
2. .NET restore,
3. .NET build,
4. domain tests,
5. content validation,
6. Godot headless smoke/integration check.

Exact action versions and download/install scripts are implementation details that must be pinned and reviewed when the workflow is created.

## Canonical command intent

### Restore

```bash
dotnet restore UNDO.sln
```

### Build

```bash
dotnet build UNDO.sln --configuration Release --no-restore
```

### Domain tests

```bash
dotnet test UNDO.sln --configuration Release --no-build
```

If running tests at solution level causes Godot-specific project issues in CI, commands may target the pure .NET test project explicitly.

Do not weaken the architecture merely to make a convenience command work.

### Content validation

Conceptual:

```bash
dotnet run   --project tools/Undo.ContentValidator/Undo.ContentValidator.csproj   --configuration Release   --no-build   -- game/content
```

Final CLI arguments are defined by the implemented validator.

### Godot headless smoke

Conceptual:

```bash
godot --headless --path game --quit-after <frames>
```

The workflow must use the matching Godot 4.7.2 .NET editor/runtime environment.

Headless mode is for:
- project load,
- C# script load,
- selected scene load,
- integration smoke checks.

It is not a substitute for graphical playtesting.

## Version pinning

CI should pin:
- Godot release,
- .NET major/minor baseline,
- GitHub Action major versions intentionally.

Do not automatically float to Godot latest.

Dependency update PRs are reviewed like code changes.

## Required PR status

Before gameplay/architecture PR merge, require where practical:

- build,
- domain tests,
- content validation,
- headless smoke.

Visual/narrative-only changes may still run the same cheap checks to detect accidental file breakage.

## Branch discipline

Recommended implementation workflow:

```
main
  ↑
short-lived feature/prototype branch
  ↑
Codex/developer commits
```

Do not use a permanent "AI branch."

Every Codex task receives a narrow branch/task scope.

## Commit expectations

Prefer commits that are logically reviewable.

Examples:
- `build: bootstrap Godot and .NET solution`
- `core: add event ledger and state resolver`
- `test: add consequence persistence regression`
- `content: add Prototype B semantic definition`

Avoid giant commits combining:
- design rewrite,
- architecture refactor,
- art import,
- multiple puzzle implementations.

## Pull request body

Implementation PRs should state:

- objective,
- canonical docs referenced,
- files/systems changed,
- tests run,
- content validation result,
- known limitations,
- screenshots/video only where presentation changes require review,
- whether docs need updating.

## Design conflict rule

If implementation reveals that canon cannot be implemented cleanly:

1. stop adding exceptions,
2. describe the conflict,
3. update/approve design docs first,
4. then implement the revised rule.

A green CI build does not override design canon.

## Content validation as merge protection

Invalid causal content is a build failure, not a runtime surprise.

Hard errors eventually include:
- duplicate stable IDs,
- invalid channel values,
- missing referenced entity/location,
- unsupported schema version,
- invalid routine/reaction vocabulary,
- missing required localization key.

Warnings may include:
- unused localization key,
- optional unused asset/content entry,
- non-fatal metadata issue.

## Scene-binding validation

As integration tooling matures, headless checks should validate:
- puzzle scene exists,
- required EntityId binders exist exactly once,
- required LocationId anchors exist,
- duplicate IDs are rejected,
- required binder component types are present.

A causal JSON file that validates semantically but cannot bind to its Godot scene must not reach playtesting unnoticed.

## Test artifacts

CI may upload on failure:
- test result files,
- validator report,
- Godot headless log.

Do not upload large game builds on every ordinary PR unless needed.

## Build artifacts

Automated playable builds are useful at milestones:
- Prototype A candidate,
- Prototype B playtest candidate,
- Prototype C candidate,
- vertical slice playtest/review,
- alpha/beta/RC.

Build generation is separate from basic validation CI.

## Caching

CI may cache:
- NuGet packages,
- downloaded Godot tooling,

provided cache keys include relevant version/lock information.

Do not cache `.godot/` blindly if it creates nondeterministic import behavior across revisions.

## Secrets

Do not commit:
- signing credentials,
- store credentials,
- private tokens,
- export credentials.

Use GitHub encrypted secrets only when release/build workflows later require them.

Prototype CI should not require production secrets.

## Formatting

Once implementation exists, add an automated formatting verification compatible with repository standards.

Preferred:
- `dotnet format` check for C#.

Do not auto-rewrite Godot scene/resource files in CI solely for formatting.

## Documentation drift

CI cannot fully understand design correctness, but lightweight checks may ensure:
- required canonical docs exist,
- content registry IDs resolve,
- AGENTS.md references valid files.

PR review remains responsible for semantic agreement with docs.

## Failure policy

Do not "fix CI" by:
- disabling tests,
- swallowing validator errors,
- allowing random retry until green,
- changing deterministic assertions,
- reducing content validation strictness without a design reason.

Flaky tests are bugs.

## Prototype B special protection

Once Prototype B passes greenlight, keep its Consequence Persistence regression test as a permanent high-value test.

A later refactor that makes badge suppression relock the already-unlocked door must fail CI.

## Future release workflow

Release automation is intentionally deferred.

Before Beta/RC, add:
- Windows export preset/build,
- version stamping,
- clean-package verification,
- license/attribution packaging,
- optional symbol/debug artifact handling,
- checksum generation where useful.

Do not burden prototype CI with release engineering prematurely.
