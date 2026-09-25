# UNDO — Production Roadmap

## Principle

The roadmap is gate-based, not calendar-promise-based.

A later phase begins when the preceding risk is proven sufficiently, not simply because time elapsed.

## Phase 0 — Design Foundation

Status: **IN PROGRESS / NEAR COMPLETE**

Goals:
- vision,
- core mechanic,
- causal rules,
- puzzle grammar,
- campaign progression,
- narrative/world,
- UI/art/audio direction,
- technical architecture,
- testing,
- prototype specification,
- content/asset scope,
- Codex/GitHub source-of-truth workflow.

Exit:
- no unresolved contradiction in core mechanic,
- docs are sufficient for Prototype A implementation,
- AGENTS.md points coding agents to required canon.

## Phase 1 — Repository / Engine Bootstrap

Implementation allowed only after explicit request.

Deliver:
- Godot 4.7.2 .NET project,
- solution/project structure,
- Undo.Core,
- Undo.Core.Tests,
- basic CI,
- .gitignore,
- content-validation skeleton,
- bootstrap scene,
- development debug shell.

No campaign levels.

Exit:
- clean clone builds,
- `dotnet test` runs,
- Godot project opens/runs,
- headless smoke check works,
- coding-agent workflow can make a small reviewed PR safely.

## Phase 2 — Prototype A

Deliver:
- door state channel,
- one NPC routine,
- event ledger,
- basic EventRecorder,
- EffectiveStateResolver,
- one correction,
- primitive Review/Residual Exposure,
- release,
- restore snapshot,
- debug inspection.

Exit:
- all Prototype A functional criteria pass,
- mechanic is legible in first-time test.

## Phase 3 — Prototype B

Deliver:
- badge semantic placement/possession,
- scanner authorization,
- door unlock event,
- event history,
- Consequence Persistence presentation.

Exit:
- domain regression tests pass,
- persistence state exactly matches spec,
- first-time players understand that downstream event persists,
- result reads as intended rule rather than bug,
- mechanic creates interest.

**Project greenlight gate.**

If this fails after reasonable presentation iteration, redesign before proceeding.

## Phase 4 — Prototype C

Deliver:
- RoutinePlan,
- ActorMemory,
- ReactionRules,
- semantic locations,
- deterministic route execution,
- missing-badge contradiction reaction,
- NPC behavior debug panel.

Exit:
- deterministic replay from snapshot,
- reaction is understandable,
- no hidden/random AI,
- new present-time events behave correctly.

## Phase 5 — Vertical Slice Graybox

Build `VS_R4_TRANSFER` entirely in graybox first.

Deliver:
- complete 25–35 min causal flow,
- reset/checkpoint boundaries,
- signage/navigation blockout,
- audio information blockout,
- initial accessibility control architecture.

Exit:
- full slice completable without debug intervention,
- no level requires bespoke causal exception,
- waiting/routine pacing acceptable,
- graybox sightlines pass environmental information rules.

## Phase 6 — Vertical Slice Art/Audio/UI

Bring slice to representative near-final quality.

Deliver:
- representative environment kits,
- modular NPC presentation,
- final-direction Residual Exposure,
- Review UX,
- functional institutional signage,
- representative audio mix,
- representative in-world terminals/documents,
- settings/pause baseline.

Exit:
- greenlight criteria in `docs/23_VERTICAL_SLICE.md`,
- source-control/code workflow remains manageable,
- performance and load stability acceptable,
- art identity no longer resembles generic AI/SF UI.

## Phase 7 — Full Production: Acts I–II

Build:
- PZ_A1_01 through PZ_A2_07,
- required narrative artifacts for those acts,
- current/B1 environment content,
- relevant NPC/audio/signage families.

Exit:
- Acts I–II playable start-to-finish,
- difficulty/readability playtested,
- no tutorial-text dependence replacing level design.

## Phase 8 — Full Production: Act III

Build:
- PZ_A3_08–11,
- second correction capacity,
- restoration/mechanical kit,
- richer NPC reactions.

Exit:
- 2-slot logic stable,
- State Preparation and actor-reaction puzzles validated,
- performance remains acceptable.

## Phase 9 — Full Production: Act IV

Build:
- PZ_A4_12–14,
- masked events,
- authorization/institutional state,
- deep archive environment,
- major mastery puzzle.

Exit:
- advanced rules remain understandable,
- contradiction target states are inspectable/debuggable,
- story reveal ladder remains paced.

## Phase 10 — Full Production: Act V

Build:
- PZ_A5_15–16,
- accident reconstruction,
- third correction capacity if retained,
- Record 0 gap behavior,
- ending-state derivation,
- final exit.

Exit:
- entire game completable,
- all factual ending states work,
- no fake correctable event in the 39-second gap,
- final door has no correction affordance.

## Phase 11 — Alpha

Definition:
content-complete enough for full-game repeated testing.

Focus:
- causal bugs,
- softlocks,
- reset reliability,
- save migration,
- puzzle clarity,
- narrative comprehension,
- accessibility,
- performance.

No uncontrolled new mechanics.

## Phase 12 — Beta

Definition:
feature/content lock except fixes/polish.

Focus:
- optimization,
- final audio mix,
- localization,
- subtitle quality,
- graphics/settings coverage,
- controller support,
- hardware compatibility,
- credits/licenses,
- save robustness.

## Phase 13 — Release Candidate

Required:
- full regression suite green,
- clean install/upgrade testing,
- licenses/attributions verified,
- save compatibility verified,
- ending paths verified,
- platform build packaging verified,
- no debug tools exposed unintentionally.

## Git / Codex workflow

For substantial coding work:

1. ChatGPT/design conversation resolves intent against repository canon.
2. Update docs first if design changes.
3. Create a narrowly scoped implementation task.
4. Codex works on a branch.
5. Automated tests/validation run.
6. Review diff/PR against docs and acceptance criteria.
7. Merge only after behavior matches canon.
8. Update CURRENT_STATE when milestone status changes.

Do not mix large design changes and large implementation refactors in one opaque task.

## Scope freeze rule

Once vertical slice is greenlit, new feature proposals require one of:
- fixes a proven usability problem,
- solves a proven production blocker,
- materially improves the central causal mechanic without expanding scope disproportionately.

"Cool idea" alone is insufficient.

## Cancellation / redesign signals

Stop and reassess if:
- Prototype B remains unintelligible after iterative presentation changes,
- deterministic NPCs feel like passive waiting rather than manipulation,
- correction bugs cannot be diagnosed from state/event tooling,
- full content requires constant per-level code exceptions,
- visual target requires a production scale incompatible with the project.

Early redesign is cheaper than forcing the concept through production.
