# UNDO — Current State

Last design sync: 2026-09-25

## Phase

**Pre-production / design lock**

No gameplay implementation should be treated as production-ready yet.

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

## Next design work

Before production coding, complete:

1. technical architecture,
2. formal event/state data specification,
3. NPC behavior architecture,
4. save/restore architecture,
5. content registry,
6. asset registry,
7. testing strategy,
8. coding standards,
9. prototype acceptance criteria,
10. vertical-slice specification,
11. production roadmap.

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

Not yet locked:
- engine and exact technical stack,
- exact input bindings,
- exact correction color values,
- exact font family/licensing,
- final institution name if legal/localization review requires adjustment,
- exact failure/hint tuning,
- exact save-slot policy,
- final vertical-slice map,
- exact accident environmental hazard implementation after realism/safety review.
