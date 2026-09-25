# UNDO

A first-person causal puzzle game about undoing recorded actions without rewinding time.

## Project status

**Phase:** Phase 1 repository/engine bootstrap complete; ready for Prototype A  
**Implementation:** Bootstrap foundation implemented; gameplay prototypes not started  
**Current objective:** Validate Prototype A → B → C before any campaign production.

## Core premise

> Time does not rewind. A recorded event's direct state change can be suppressed, while later consequences remain.

The player does not directly manipulate the world. They observe people and machines, identify recorded state-changing events, suppress selected events, and exploit the resulting causal contradictions.

## Canon rule

The Markdown files under `docs/` are the source of truth for game design. Chat discussions are exploratory until a decision is written into the repository.

When implementation begins, `AGENTS.md` will define how coding agents must read and obey these documents.

## High-level identity

- Genre: first-person causal puzzle / narrative mystery
- Target length: approximately 4–6 hours
- Structure: 5 acts, 16 major puzzle spaces
- Setting: a Korean public-record preservation facility
- Core mechanic: event suppression ("Undo")
- Signature rule: consequences that became separate events persist after their cause is suppressed
- Interaction philosophy: indirect manipulation only
- Visual identity: institutional, analog, restrained, photographic double-exposure
- UI direction: minimal HUD; no generic AI/SaaS/neon sci-fi visual language
- Narrative theme: actions can be undone; inaction cannot

## Documentation

Start here:

1. `AGENTS.md` — mandatory rules for ChatGPT/Codex/implementation agents
2. `docs/20_CURRENT_STATE.md` — current project phase and next action
3. `docs/19_DECISION_LOG.md` — locked canonical decisions
4. `docs/29_PROJECT_BOOTSTRAP.md` — first implementation task
5. `docs/22_PROTOTYPE_SPEC.md` — Prototype A/B/C gates

Core design:
- `docs/00_GAME_VISION.md`
- `docs/01_DESIGN_PILLARS.md`
- `docs/02_CORE_MECHANIC.md`
- `docs/03_CAUSAL_SYSTEM.md`
- `docs/05_PUZZLE_GRAMMAR.md`
- `docs/06_PROGRESSION.md`
- `docs/09_NARRATIVE.md`
- `docs/11_UI_UX.md`
- `docs/12_ART_DIRECTION.md`
- `docs/13_AUDIO_DIRECTION.md`

Engineering:
- `docs/14_TECHNICAL_ARCHITECTURE.md`
- `docs/15_EVENT_SYSTEM.md`
- `docs/16_NPC_BEHAVIOR.md`
- `docs/17_SAVE_SYSTEM.md`
- `docs/18_TESTING_STRATEGY.md`
- `docs/21_CODING_STANDARDS.md`
- `docs/27_DATA_AUTHORING.md`
- `docs/28_LOCALIZATION.md`
- `docs/30_CI_WORKFLOW.md`
- `docs/31_AI_DEVELOPMENT_WORKFLOW.md`
- `docs/32_PLAYTEST_PROTOCOL.md`
- `docs/33_PLAYER_EXPERIENCE.md`
- `docs/34_PUZZLE_AUTHORING_TEMPLATE.md`
- `docs/35_PROTOTYPE_A_LEVEL_SHEET.md`
- `docs/36_PROTOTYPE_B_LEVEL_SHEET.md`
- `docs/37_PROTOTYPE_C_LEVEL_SHEET.md`
- `docs/38_VERTICAL_SLICE_LEVEL_SHEET.md`
- `docs/39_CONTENT_PRODUCTION_PIPELINE.md`
- `docs/40_ART_AUDIO_INTEGRATION_PIPELINE.md`
- `docs/41_NARRATIVE_ARTIFACT_SPEC.md`
- `docs/42_DESIGN_COMPLETION_CHECKLIST.md`

Production:
- `docs/23_VERTICAL_SLICE.md`
- `docs/24_CONTENT_REGISTRY.md`
- `docs/25_ASSET_REGISTRY.md`
- `docs/26_PRODUCTION_ROADMAP.md`
