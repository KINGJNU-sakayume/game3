# UNDO — Current State

Last design sync: 2026-09-25

## Phase

**Pre-production design baseline v1 complete — ready for implementation bootstrap**

The canonical design baseline is complete enough to begin the repository/engine bootstrap described in `docs/29_PROJECT_BOOTSTRAP.md` when explicitly requested. Gameplay remains unimplemented, and campaign production must still pass the prototype gates.

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

## Implementation readiness

The blocking design documents required for repository bootstrap and Prototype A are now present.

Next implementation sequence when requested:

1. repository / Godot 4.7.2 .NET bootstrap,
2. pure-domain correction golden test,
3. content validator skeleton,
4. Prototype A,
5. Prototype B greenlight gate,
6. Prototype C,
7. vertical slice.

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

Not yet locked but not blocking repository bootstrap:
- exact input bindings,
- exact correction color values,
- exact font family/licensing,
- final institution name if legal/localization review requires adjustment,
- exact failure/hint tuning,
- exact save-slot policy,
- final vertical-slice map,
- exact accident environmental hazard implementation after realism/safety review.
