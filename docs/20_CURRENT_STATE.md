# UNDO — Current State

Last design sync: 2026-09-25

## Phase

**Phase 1 repository / engine bootstrap complete — ready for Prototype A**

The repository foundation defined by `docs/29_PROJECT_BOOTSTRAP.md` is implemented and validated. Prototype A gameplay remains unimplemented, and campaign production must still pass the prototype gates.

## Canon completed

The following areas have a stable design direction:

- game vision and scope,
- design pillars,
- core event-suppression mechanic,
- state-channel model,
- Consequence Persistence,
- NPC memory/history rules,
- correction release,
- correction capacity progression,
- puzzle grammar,
- five-act / sixteen-space progression,
- facility setting and broad lore,
- protagonist and historical accident structure,
- Record 0 thematic role,
- narrative reveal structure,
- minimal/diegetic UI philosophy,
- Residual Exposure visual direction,
- institutional art direction,
- audio direction and sound-as-gameplay-information rules,
- environmental information/readability rules for level design,
- Godot 4.7.2 .NET + C# engine/stack decision,
- Godot-independent causal domain architecture,
- formal event/state resolution specification,
- deterministic NPC routine/reaction architecture,
- semantic save/snapshot/restore architecture,
- layered testing strategy and xUnit domain-test decision,
- C# coding/architecture standards,
- Prototype A/B/C functional and UX acceptance specifications,
- 25–35 minute vertical-slice scope and greenlight criteria,
- campaign/narrative content registry,
- reusable asset-kit budget and acquisition rules,
- gate-based production roadmap,
- JSON causal-content authoring standard,
- Korean/English localization pipeline and key policy,
- repository/bootstrap specification,
- CI and repository quality workflow,
- ChatGPT/GitHub/Codex handoff workflow,
- prototype and puzzle playtest protocol,
- first-person movement/camera/input experience rules,
- standardized puzzle authoring/graybox gate template,
- detailed Prototype A/B/C graybox level sheets with timing, commit points, observation anchors, reset, and playtest criteria,
- detailed 25–35 minute continuous vertical-slice level sheet and pacing/spoiler boundaries,
- gated content-production lifecycle from concept through polish,
- art/audio integration rules preserving semantic causal authority,
- detailed N01–N14 essential narrative artifact/media/reveal specification,
- prohibition of generic AI/SaaS/neon UI language.

## Current high-level game

**Title:** UNDO  
**Genre:** first-person causal puzzle / narrative mystery  
**Length target:** 4–6 hours  
**Setting:** Repository 4, a Korean public-record preservation facility  
**Protagonist:** Han Jaemin  
**Historical figure:** Yoon Seojin  
**Central mechanic:** suppress a recorded event's direct state mutation without rewinding time  
**Signature behavior:** downstream recorded consequences persist  
**Narrative limit:** the absent action at the center of Record 0 cannot be undone.

## Design milestone

**Design Baseline v1: COMPLETE.**

Do not continue expanding high-level design before evidence from prototypes. Full campaign detailed design is intentionally deferred until Prototype A/B/C and the vertical slice validate the mechanic and production pipeline. See `docs/42_DESIGN_COMPLETION_CHECKLIST.md`.

## Implementation readiness

Repository / Godot 4.7.2 .NET bootstrap is complete. The foundation now includes the Godot project, `Undo.Core`, xUnit tests, content-validator skeleton, localization source structure, CI, and a passing pure-domain correction golden test.

Next implementation sequence when requested:

1. Prototype A,
2. Prototype B greenlight gate,
3. Prototype C,
4. vertical slice.

Further design refinement continues as implementation/playtesting produces evidence; it must update canon before behavior changes.

## Prototype gates

### Prototype A — Basic correction
NPC closes door → player suppresses event → door returns to effective open state.

### Prototype B — Consequence Persistence
NPC takes badge → scans badge → unlocks door → player suppresses TakeBadge → badge returns but door remains unlocked.

**This is the critical project gate.** The mechanic must be legible and enjoyable before full production.

### Prototype C — NPC reaction
Player creates a contradiction → deterministic NPC notices → reaction creates a new useful event.

## Implementation warning

Do not start building all 16 levels before the three mechanic prototypes and vertical slice validate:
- readability,
- predictability,
- player enjoyment,
- correction UX,
- NPC reaction clarity.

## Open decisions

Not yet locked but not blocking Prototype A:
- exact input bindings,
- exact correction color values,
- exact font family/licensing,
- final institution name if legal/localization review requires adjustment,
- exact failure/hint tuning,
- exact save-slot policy,
- final vertical-slice map,
- exact accident environmental hazard implementation after realism/safety review.
