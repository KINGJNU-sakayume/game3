# AGENTS.md — UNDO

## Source of truth

This repository, especially the Markdown files under `docs/`, is the source of truth for UNDO.

Chat messages, brainstorming transcripts, generated suggestions, and implementation convenience do not override repository canon unless the relevant docs are updated.

Before planning or implementing gameplay work, read at minimum:

1. `docs/00_GAME_VISION.md`
2. `docs/01_DESIGN_PILLARS.md`
3. `docs/02_CORE_MECHANIC.md`
4. `docs/03_CAUSAL_SYSTEM.md`
5. `docs/04_PLAYER_RULES.md`
6. `docs/05_PUZZLE_GRAMMAR.md`
7. `docs/19_DECISION_LOG.md`
8. `docs/14_TECHNICAL_ARCHITECTURE.md`
9. `docs/15_EVENT_SYSTEM.md`
10. `docs/16_NPC_BEHAVIOR.md`
11. `docs/17_SAVE_SYSTEM.md`
12. `docs/18_TESTING_STRATEGY.md`
13. `docs/21_CODING_STANDARDS.md`
14. `docs/22_PROTOTYPE_SPEC.md`
15. `docs/23_VERTICAL_SLICE.md`
16. `docs/24_CONTENT_REGISTRY.md`
17. `docs/25_ASSET_REGISTRY.md`
18. `docs/26_PRODUCTION_ROADMAP.md`
19. `docs/27_DATA_AUTHORING.md`
20. `docs/28_LOCALIZATION.md`
21. `docs/29_PROJECT_BOOTSTRAP.md`
22. `docs/30_CI_WORKFLOW.md`
23. `docs/31_AI_DEVELOPMENT_WORKFLOW.md`
24. `docs/32_PLAYTEST_PROTOCOL.md`
25. `docs/20_CURRENT_STATE.md`

For narrative, level, or presentation work also read:
- `docs/06_PROGRESSION.md`
- `docs/07_LEVEL_DESIGN.md`
- `docs/08_WORLD.md`
- `docs/09_NARRATIVE.md`
- `docs/10_CHARACTERS.md`
- `docs/11_UI_UX.md`
- `docs/12_ART_DIRECTION.md`

## Current instruction

**The design baseline is implementation-ready for bootstrap/prototypes, but do not assume permission to implement the full game.**

Technical architecture, data authoring, bootstrap, CI, and Prototype A/B/C specifications are locked. Implementation may proceed only when explicitly requested. Start with `docs/29_PROJECT_BOOTSTRAP.md`, then advance through Prototype A → B → C before vertical-slice/full-production work. Do not skip Prototype B's readability/interest gate.

## Conflict rule

If a requested implementation conflicts with repository canon:
1. do not silently choose one,
2. identify the conflict,
3. preserve the existing canon unless the task explicitly requests a design revision,
4. update the design document first when a revision is approved.

## Core invariants

Never casually violate these:

- Undo does not rewind global time.
- Suppression affects declared state mutation(s), not all downstream history.
- Independent downstream events persist.
- NPC memory is not rewritten.
- NPC history is not retrospectively resimulated.
- NPC position is not a normal directly suppressible tool.
- Player core agency is indirect.
- NPC puzzle behavior should be deterministic.
- Correction capacity is 1 → 2 → maximum 3.
- No generic mana/cooldown model.
- Do not add combat, inventory, crafting, skill trees, or unrelated mechanics.
- Do not turn the correction interface into a global log dashboard.
- Do not introduce generic neon/glass/AI/SaaS visual language.

## Engineering behavior

When implementation begins:

- Use Godot 4.7.2 stable .NET + C# unless the repository explicitly revises the engine decision.
- Keep causal gameplay authority in `Undo.Core`; Godot scenes are presentation/application bindings.
- Prefer data-driven event and puzzle definitions.
- Keep causal state logic separate from presentation.
- Keep NPC routine/reaction rules inspectable and deterministic.
- Avoid global physics rewind/history recording.
- Do not refactor unrelated systems as part of a feature task.
- Add tests for state/event logic.
- Preserve localization readiness for player-facing text.
- Treat accessibility requirements in `docs/11_UI_UX.md` as product requirements, not polish.

## Definition of done for a gameplay task

A task is complete only when:
- behavior matches documented rules,
- relevant automated tests pass,
- deterministic puzzle behavior is preserved,
- no unrelated mechanic or visual language is introduced,
- affected documentation is updated if implementation changes a documented assumption.

## Documentation updates

When a design decision changes:
- update the primary relevant document,
- add or revise an entry in `docs/19_DECISION_LOG.md`,
- update `docs/20_CURRENT_STATE.md` when project phase/status changes.

Do not use chat history as a substitute for these updates.
