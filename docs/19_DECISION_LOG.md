# UNDO — Decision Log

This file records design decisions considered canonical unless explicitly revised.

## D-001 — Core premise
**Locked.** Undo suppresses the direct state mutation of a recorded event. It does not globally rewind time.

## D-002 — Consequence Persistence
**Locked.** Later independently recorded consequences remain when an earlier cause is suppressed.

## D-003 — Player agency
**Locked.** Core gameplay uses indirect manipulation. The player does not normally operate world doors, switches, levers, puzzle items, or NPC commands directly.

## D-004 — NPC memory and history
**Locked.** NPC memory is not rewritten by correction. NPCs do not retrospectively recompute history; they react to the current world state.

## D-005 — NPC movement
**Locked.** NPC location is not a normal directly suppressible gameplay state. The player redirects actors by changing conditions.

## D-006 — Correction capacity
**Locked.** Maintained corrections progress from 1 to 2 to a maximum of 3. No mana/cooldown/consumable-resource model.

## D-007 — Puzzle philosophy
**Locked.** Difficulty grows through combinations of a stable event vocabulary and causal rules, not constant introduction of bespoke sci-fi mechanics.

## D-008 — Determinism
**Locked.** NPC puzzle behavior is rule-driven and predictable. Avoid random route probabilities.

## D-009 — Reflex requirement
**Locked.** Reasoning is primary. Timing windows must be generous enough not to turn the game into a dexterity challenge.

## D-010 — Genre scope
**Locked.** First-person causal puzzle / narrative mystery. No combat, crafting, inventory system, skill tree, open world, multiplayer, dialogue-choice tree, or platforming focus.

## D-011 — Campaign scope
**Locked as target.** 4–6 hours, five acts, sixteen major puzzle spaces.

## D-012 — Setting
**Locked.** Aging Korean public-record preservation facility. Institutional realism; not a secret supernatural organization or futuristic laboratory.

## D-013 — Correction explanation
**Locked.** The institution discovered a record/reality correction phenomenon through causal accident-reconstruction research. Its fundamental physical mechanism remains unresolved.

## D-014 — No evil AI
**Locked.** Facility failure is caused by incompatibility/causal inconsistency around Record 0, not sentient malicious AI.

## D-015 — Protagonist
**Locked.** Han Jaemin (43 present, 27 at accident), long-serving records-integrity employee, cautious and procedural.

## D-016 — Yoon Seojin
**Locked.** Yoon Seojin (31 at accident), restoration technician and Jaemin's senior colleague. They were not romantic partners.

## D-017 — Historical accident
**Locked in principle.** Sixteen years earlier, a compound B3 restoration-room failure trapped Seojin. Jaemin received a request, delayed a qualifying system action for approximately 39 seconds, later attempted emergency release, and was not found clearly legally/procedurally responsible.

## D-018 — Record 0
**Locked.** Record 0 represents the expected-but-absent state transition during the critical gap. It is not a normal event and cannot be suppressed.

## D-019 — Narrative theme
**Locked.** "Actions can be undone. Inaction cannot." This must be communicated mechanically, not stated as a moral lesson.

## D-020 — Endings
**Locked in structure.** No explicit moral-choice UI. Final state is produced through the correction mechanics. Endings are not labelled Good/Bad/True.

## D-021 — Final door
**Locked.** Jaemin exits near dawn; the exterior door closes with no correction trace and no Undo interaction.

## D-022 — HUD
**Locked.** Minimal persistent HUD. No permanent correction-slot counter unless testing proves essential.

## D-023 — Correction visualization
**Locked in direction.** Residual Exposure: photographic/film-like double exposure with restrained amber cue; not neon outline or glitch.

## D-024 — Global timeline
**Locked.** No global scrolling event log as the primary puzzle interface. Event history is inspected through world objects.

## D-025 — UI style
**Locked.** Avoid AI/SaaS visual language: glassmorphism, rounded floating cards, purple-blue gradients, pill buttons, generic futuristic HUDs, neon data dashboards.

## D-026 — Environmental art
**Locked.** Maintained-but-aged institution, not ruined horror. Deeper levels reveal older infrastructure rather than becoming a different fantasy world.

## D-027 — Narrative delivery
**Locked.** Essential plot cannot depend on optional long documents. Optional records add human context, not required facts.

## D-028 — Production order
**Locked.** Design → technical design → prototypes → vertical slice → full content. Full production does not begin merely because the story bible exists.


## D-029 — Engine
**Locked.** Godot 4.7.2 stable .NET build with C# is the initial production engine/language stack. Prototype/vertical-slice work stays on this stable line unless a critical maintenance upgrade is explicitly approved.

## D-030 — Domain separation
**Locked.** Gameplay causal truth lives in a Godot-independent C# domain layer (`Undo.Core`) where practical. Godot scenes present/apply semantic state; they are not the authoritative causal model.

## D-031 — Semantic state
**Locked.** Puzzle state uses stable EntityIds, StateChannels, and semantic values/locations rather than raw scene paths, runtime instance IDs, or unconstrained physics transforms.

## D-032 — Event immutability
**Locked.** Recorded events are immutable historical records. Correction stores suppression separately and re-resolves effective state.

## D-033 — Event sequence
**Locked.** Strict monotonic event sequence is authoritative for state ordering. Timestamps are presentation/narrative metadata.

## D-034 — NPC behavior
**Locked.** NPC decision-making is RoutinePlan + current semantic world state + minimal ActorMemory + prioritized deterministic ReactionRules. Navigation executes a chosen goal and does not choose gameplay intent.

## D-035 — NPC evaluation
**Locked.** Reactions are re-evaluated at meaningful boundaries/state changes, not through unrestricted every-frame replanning. Random puzzle branching is prohibited.

## D-036 — Save model
**Locked.** Save/restore serializes semantic gameplay data and stable checkpoint snapshots, not the live Godot scene tree.

## D-037 — Initial save policy
**Locked for prototype/vertical slice.** One primary campaign record with autosave at stable checkpoints. Multiple manual slots are deferred until release usability/platform requirements justify them.

## D-038 — Debug tooling
**Locked.** Causal-state/event/NPC inspection tools are first-class development requirements and are exempt from player-facing visual style restrictions.
