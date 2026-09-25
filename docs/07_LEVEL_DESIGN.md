# UNDO — Level Design

## Macro layout

The game takes place in one aging Korean public-record preservation facility. Progress generally moves downward through layers of the building.

```
Ground — Administration
B1 — General Archive
B2 — Restoration / Mechanical Systems
B3 — Restricted Records
B4 — Deep Archive
B5 — Record 0
```

The building is not an open world. It should feel spatially coherent, but progression is curated so puzzle complexity and narrative revelations remain controlled.

## Environmental age gradient

The deeper the player goes, the older the visible infrastructure becomes.

- Ground: recent renovations and contemporary office equipment
- B1: 2000s office/archive infrastructure
- B2: 1990s industrial equipment and control systems
- B3: 1980s archive/security systems
- B4: earliest facility structure, relay cabinets, exposed service infrastructure
- B5 / Record 0: multiple eras coexist inconsistently

This is not literal time travel. It is accumulated renovation history becoming visible.

## Major spaces

Sixteen major puzzle spaces are distributed across five acts. They should not be presented as numbered test chambers. Transitions occur through corridors, work rooms, lifts, stairs, service routes, and records areas.

## Spatial legibility

A player should be able to understand:
- where relevant actors travel,
- which doors and systems connect spaces,
- which event happened to which object,
- what changed after a correction.

Avoid visually dense rooms where causal information is obscured by decoration.

## Puzzle-space boundaries

Each major puzzle area has an internal restore snapshot. The technical term may be Record Boundary, but this need not be shown to players.

Entering a stable puzzle state creates or refreshes the restore point used by **RESTORE CURRENT RECORD**.

## Waiting and routine length

NPC/system routines should surface their important event cycle roughly every 20–60 seconds.

Do not require players to stand idle for several minutes to reobserve a routine.

If testing proves repeated waiting remains frustrating, a carefully limited acceleration/replay aid may be explored later; it is not current canon.

## Breathing spaces

Between demanding puzzles, include spaces with little or no mechanical challenge:
- break room,
- copy room,
- quiet archive aisle,
- elevator,
- stairwell,
- maintenance office,
- document intake area.

These spaces support environmental storytelling and pacing.

## Level-review checklist

For each level:
1. What exact mechanic is being taught, tested, combined, or twisted?
2. Can the relevant causal chain be observed before solving?
3. Is the solution predictable from established rules?
4. Is the important reaction visible or audible?
5. Does failure teach something rather than merely waste time?
6. Is a new bespoke mechanic being added unnecessarily?
7. Is the player reasoning, or merely waiting/executing timing?
8. Can the level be reset to a known deterministic state?


## Environmental information design

### Core rule

A puzzle space is an information system before it is a decorative environment.

The player must be able to perceive:
- who can affect what,
- which route an actor is taking,
- which object changed state,
- what changed after a correction,
- where a useful consequence occurred.

Environmental composition should support causal reading.

### Sightline rule

For the first introduction of a new relationship, prefer a composition where the player can see both the action and at least one important consequence.

Example:
- guard scans badge,
- reader responds,
- door unlocks,

should initially be readable from one observational position or through an immediately understandable camera turn.

Later levels may deliberately split these across spaces after the sound language and event history are learned.

### Observation anchors

Every major puzzle space should provide 1–3 natural positions from which the player can understand a routine.

Examples:
- end of archive aisle,
- glass security booth,
- mezzanine landing,
- doorway recess,
- service-window opening.

These should look architecturally plausible, not like glowing "puzzle viewing platforms."

### Landmarking

Navigation uses real institutional landmarks:
- zone numbers,
- floor markings,
- distinctive doors,
- desk clusters,
- equipment families,
- safety signage,
- architectural transitions.

Do not rely on floating objective markers or colored game trails.

### Functional color

Realistic facility safety/wayfinding colors may be used for institutional meaning, but puzzle semantics cannot depend only on color.

Correction amber remains exclusive to the correction phenomenon.

### Causal proximity

During teaching levels, keep causes and consequences spatially close enough that the player can form the relationship without memorizing an entire floor.

As mastery increases, causal chains may span:
- adjacent rooms,
- two sides of a corridor,
- floor/mezzanine relationships,
- nearby service spaces.

Avoid puzzle chains that require remembering invisible state changes across large unrelated areas.

### Off-screen events

A critical off-screen event is valid only when at least one reliable information channel exists:
- recognizable sound,
- visible downstream reaction,
- deterministic routine,
- inspectable event history.

Never require blind guessing about whether an unseen event happened.

### Occlusion

Architectural realism must not hide the game.

Avoid:
- dense shelving placed exactly across key NPC interactions,
- opaque machinery blocking a newly taught event,
- decorative clutter masking state-change animation,
- identical doors in complex spaces without usable identifiers.

Occlusion can become an intentional advanced challenge only after the underlying rule is established.

### State readability

Objects with gameplay state should have physically readable changes where reasonable.

Examples:
- door visibly open/closed,
- latch/indicator changes for lock state,
- equipment motor/motion difference,
- scanner lamp or mechanical state,
- ventilation or flow clues,
- cabinet/drawer geometry.

Do not communicate all state through abstract UI.

### NPC route readability

NPCs should move with understandable intent:
- look toward destination,
- interact visibly,
- pause at meaningful checkpoints,
- use consistent doors/routes under the same conditions.

Avoid over-naturalistic wandering animations that obscure the deterministic routine.

### Reset readability

After RESTORE CURRENT RECORD, the space must clearly return to its stable initial configuration.

Players should not wonder whether one machine or NPC remained in an altered state.

### Spatial escalation

Early:
- one room or short corridor,
- single visible causal chain.

Middle:
- connected rooms,
- multiple actors,
- audio-supported off-screen events.

Late:
- multi-space causal networks,
- intentional historical/state overlap,
- contradiction target states.

Complexity should increase through relationships, not simply room size.

### Environmental storytelling separation

Story dressing should reinforce place without impersonating puzzle cues.

A handwritten note, old photograph, or archived file should not use the same amber, motion, sound, or placement language used for correctable events.

Players must be able to distinguish:
- "this tells me about the world"
from
- "this is mechanically actionable."

### Level blocking requirement

Before final art production, every puzzle is validated in graybox with:
- actor routes,
- state-changing objects,
- observation anchors,
- sightlines,
- sound cue positions,
- reset behavior.

Decoration is not allowed to solve a legibility problem that the graybox itself fails to solve.
