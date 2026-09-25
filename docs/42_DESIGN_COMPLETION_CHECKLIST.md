# UNDO — Design Completion Checklist

Last locked: 2026-09-25

## Purpose

UNDO distinguishes between two kinds of "design complete."

1. **Design Baseline Complete** — enough design exists to begin repository bootstrap and prototypes safely.
2. **Campaign Design Lock** — enough validated evidence exists to lock the detailed full-game content plan before broad campaign production.

These are intentionally different milestones.

---

# A. Design Baseline Complete

Status: **COMPLETE**

This milestone is reached when the project has locked:

- game vision and scope,
- core player fantasy,
- fundamental Undo/correction rule,
- Consequence Persistence,
- event/state semantics,
- NPC memory/reaction principles,
- correction capacity progression,
- puzzle grammar,
- campaign act structure,
- world/narrative premise,
- main characters,
- Record 0 concept,
- essential narrative reveal ladder,
- UI/UX direction,
- art direction,
- audio direction,
- engine/technical architecture,
- data-authoring format,
- save/reset strategy,
- test strategy,
- prototype acceptance criteria,
- Prototype A/B/C level sheets,
- vertical-slice scope,
- GitHub/ChatGPT/Codex workflow,
- content-production pipeline.

All of these are present in the repository as of 2026-09-25.

Therefore **no additional high-level design work is required before implementation bootstrap.**

---

# B. What should NOT be fully designed yet

Do not fully lock all remaining campaign details before Prototype B / C and the vertical slice.

Specifically defer final detailed design of:

- all sixteen campaign puzzle grayboxes,
- exact Act III/IV multi-slot puzzle timings,
- final Masked Event content,
- final Authorization puzzle implementation,
- final Contradiction mastery layout,
- exact L15 Reconstruction sequence,
- exact L16 ending-state puzzle layout,
- exact hint thresholds,
- exact final input bindings,
- final correction color values,
- final font families,
- final environment asset counts,
- final save-slot UX.

Reason:

The prototypes and vertical slice exist to discover whether the current rules are:
- legible,
- enjoyable,
- predictable,
- practical to author,
- practical to debug,
- practical to produce.

Locking the entire campaign before obtaining that evidence would create avoidable redesign work.

---

# C. Campaign Design Lock

Status: **NOT YET DUE**

Campaign Design Lock occurs after:
- Prototype A passes,
- Prototype B passes the project greenlight gate,
- Prototype C passes,
- vertical-slice graybox passes,
- representative vertical-slice presentation passes.

At that point, before broad full-game production, complete the following:

## 1. Detailed campaign puzzle sheets

Create full authoring sheets for:
- PZ_A1_01 through PZ_A5_16,

using `docs/34_PUZZLE_AUTHORING_TEMPLATE.md`.

Prototype-equivalent campaign levels may reuse validated designs rather than duplicating documentation unnecessarily.

Each final sheet should define:
- objective,
- semantic initial state,
- normal routine,
- events,
- intended insight,
- solution chain,
- alternate obvious actions,
- observation anchors,
- timing,
- correction-slot use,
- reaction boundaries,
- reset,
- completion state,
- tests,
- playtest questions.

## 2. Full facility route / macro map

Lock:
- how all sixteen spaces connect,
- floor transitions,
- breathing spaces,
- backtracking policy,
- checkpoint placement,
- narrative artifact placement,
- physical relationship between B1–B5.

This is more detailed than the existing conceptual floor structure.

## 3. Advanced-mechanic validation

After prototypes establish the base language, explicitly validate detailed design for:
- 2-slot competition,
- 3-slot late-game use,
- Masked Events,
- Causal Forks,
- AUTHORIZATION contradictions,
- Paradox Construction.

Do not assume these are fun merely because they are logically possible.

## 4. Act V mechanical lock

Before Act V production, lock:
- exact accident reconstruction causal chain,
- which historical events are correctable,
- which apparent solutions fail and why,
- exact presentation of the absent 39-second event,
- final Record 0 destruction/preservation mechanics,
- factual state combinations producing each ending.

This deserves its own review because it carries the theme of the entire game.

## 5. Final production identity decisions

By vertical-slice greenlight, lock:
- correction amber/color system,
- final font families and licenses,
- representative signage system,
- final Residual Exposure technique,
- player-facing input defaults,
- hint-system behavior,
- save-slot release policy,
- controller mapping baseline.

These should be based on actual use, not abstract preference.

---

# D. What counts as "design finished" for the project

For practical production purposes, call the design phase finished when:

1. Design Baseline v1 is complete.
2. Prototype A/B/C have validated the core rules.
3. Vertical slice validates full experience and production pipeline.
4. All sixteen campaign puzzles have detailed sheets based on validated rules.
5. Facility macro route is locked.
6. Act V reconstruction/ending mechanics are fully specified.
7. Essential narrative artifact placement is locked.
8. Final UI/art/audio identity decisions needed for production are locked.
9. Remaining open decisions are tuning, polish, or implementation detail rather than game-definition questions.

At that point, change project phase to:

**FULL PRODUCTION — DESIGN LOCKED**

Design will still receive bug/usability revisions, but there should be no major unanswered question about what game is being built.

---

# E. Current recommendation

**Stop expanding high-level design now.**

The next correct action is:

```
Repository / Godot bootstrap
→ Prototype A
→ Prototype B
→ Prototype C
→ Vertical Slice
```

Only after those produce evidence should the project invest in detailed level sheets for all sixteen campaign spaces.

This protects the project from over-designing an unvalidated mechanic.

---

# F. Current status summary

As of 2026-09-25:

```
HIGH-LEVEL GAME DESIGN          COMPLETE
CORE MECHANIC SPEC              COMPLETE
TECHNICAL DESIGN                COMPLETE FOR BOOTSTRAP
PROTOTYPE DESIGN                COMPLETE
VERTICAL-SLICE DESIGN           COMPLETE ENOUGH TO BUILD
FULL CAMPAIGN DETAILED DESIGN   DEFERRED UNTIL VALIDATION
IMPLEMENTATION                  NOT STARTED
```

The project is therefore **ready to leave design-only mode and begin Phase 1 implementation bootstrap.**
