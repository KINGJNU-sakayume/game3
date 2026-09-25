# UNDO — Prototype A Level Sheet

Last locked: 2026-09-25

## ID

`PT_A_BASIC_CORRECTION`

## Purpose

Prototype A proves the smallest complete UNDO loop:

```
observe recorded state change
→ Review the object
→ suppress the event
→ effective world state changes
→ historical actor does not rewind
→ release restores the event contribution
```

It must be small enough that any confusion is about the mechanic, not navigation or content complexity.

## Status

**CANONICAL GRAYBOX SPEC**

Dimensions/timing below are baseline values for implementation. They may be tuned during playtesting without changing the underlying mechanic.

---

## 1. Space

Single office/archive threshold room plus short exit corridor.

Approximate footprint:

```
10 m × 8 m
```

Ceiling:

```
2.7–3.0 m
```

No stairs.
No side rooms.
No alternate route.

ASCII blockout:

```
┌──────────────────────────────┐
│                              │
│  STAFF DESKS                 │
│                              │
│                 NPC START    │
│                    X         │
│                              │
│         OBS                  │
│          X                   │
│                              │
│ PLAYER                       │
│   X              [DOOR_A]────┼── EXIT CORRIDOR
└──────────────────────────────┘
```

The player should be able to see:
- Door A,
- the NPC,
- the NPC's approach to Door A,

from the spawn/observation area with minimal camera movement.

## 2. Key entities

### PLAYER

Spawn:
`LOC_PT_A_PLAYER_START`

Approximate distance to Door A:
5–6 m.

### NPC_WORKER_01

Role:
ordinary staff worker finishing a task and leaving.

Start:
`LOC_PT_A_WORKER_START`

No reaction rules required.

Routine is fully authored.

### DOOR_A

Gameplay channels:
- OPEN
- LOCK if needed for binder completeness, but LOCK remains `UNLOCKED` throughout Prototype A.

Initial:

```
DOOR_A.OPEN = true
DOOR_A.LOCK = UNLOCKED
```

Door cannot be directly operated by the player.

## 3. Semantic locations

Required IDs:

```
LOC_PT_A_PLAYER_START
LOC_PT_A_OBSERVATION
LOC_PT_A_WORKER_START
LOC_PT_A_DOOR_INSIDE
LOC_PT_A_DOOR_OUTSIDE
LOC_PT_A_EXIT
```

## 4. Initial player state

Player begins already facing roughly toward Door A but not perfectly centered.

Target:
- player notices a normal workplace,
- player can move immediately,
- player naturally interprets Door A as exit.

Do not place a glowing objective arrow on Door A.

A small physical exit sign / plausible corridor composition may guide attention.

## 5. NPC routine

Baseline sequence after scene control begins:

```
T+0.0   NPC_WORKER_01 begins walking toward Door A
T+2.5   NPC reaches Door A interior interaction point
T+2.7   NPC crosses threshold
T+3.2   NPC begins CloseDoor interaction
T+3.7   CLOSE commit point
T+4.0   NPC continues into exit corridor
T+6.0   NPC reaches offstage/despawn-safe point
```

Exact animation seconds are tunable.

The causal order is canonical.

## 6. Recorded event

### E1 — Close Door A

Conceptual:

```
Actor:
NPC_WORKER_01

Verb:
CLOSE

Target:
DOOR_A

Mutation:
DOOR_A.OPEN
true → false

Correctable:
yes
```

### Commit point

Event commits when the door reaches the authored closed/latch state.

Do not commit:
- at animation start,
- when NPC touches handle,
- after the NPC has already walked far away.

Visual and causal commit must feel synchronized.

## 7. Presentation sequence

Before E1:
- no Residual Exposure on Door A,
- Review has no meaningful event history for Door A beyond initial state.

After E1 commits:
- door is visibly closed,
- a subtle prior-open Residual Exposure becomes available when player targets/focuses the door,
- first-time minimal Review input prompt may appear.

Prompt example is implementation-localized and temporary in prototype.

Do not display:
- "Press RMB to Undo Door",
- "Event Available",
- a large icon.

## 8. First-time expected player behavior

Likely sequence:

1. player walks toward Door A,
2. NPC closes it before/near player arrival,
3. player tries normal movement through it,
4. may press Inspect/E out of habit,
5. nothing directly opens the door,
6. notices Residual Exposure,
7. enters Review,
8. sees E1/prior OPEN state,
9. commits suppression,
10. door resolves OPEN,
11. player crosses.

Attempting Inspect on the door should not create a misleading "locked" message unless usability testing proves one is required.

## 9. Review Mode

When targeting Door A after E1:

Visible history needs only one meaningful event.

Review display:

```
<timestamp or relative time>
CLOSED
```

Historical visual:
- OPEN geometry/state appears as Residual Exposure.

The player does not need history scrolling in this prototype unless the shared input architecture already supports it.

## 10. Suppression behavior

Suppress E1.

Expected domain result:

```
SuppressedEvents = [E1]

DOOR_A.OPEN = true
```

Expected world:
- Door A opens/settles into OPEN presentation.
- NPC remains outside/current location.
- NPC does not walk backward.
- time does not rewind.
- no other world object changes.

## 11. Release behavior

Prototype A should allow release for developer/testing proof even if campaign teaching of release occurs later.

Player reviews maintained E1 and chooses Release.

Expected:

```
SuppressedEvents = []

DOOR_A.OPEN = false
```

World:
- Door closes at current time through presentation transition,
- NPC does not return to close it,
- no duplicate historical E1 is created merely because its contribution became active again.

Release itself may be logged by application/debug systems, but should not create a fake new historical CLOSE event unless later architecture explicitly defines a present-time system event for that presentation.

## 12. Completion

Prototype completion condition:

```
PLAYER at LOC_PT_A_EXIT
```

Completion may display a development-only result screen.

No campaign transition required.

## 13. Reset snapshot

`RESTORE CURRENT RECORD` returns:

```
PLAYER = LOC_PT_A_PLAYER_START
NPC_WORKER_01 = LOC_PT_A_WORKER_START
NPC routine = step 0
DOOR_A.OPEN = true
DOOR_A.LOCK = UNLOCKED
EventLedger = baseline/empty for prototype events
SuppressedEvents = empty
```

Reset may briefly fade/transition.

Do not reverse animation to achieve reset.

## 14. Debug requirements

Development overlay/log must show:

```
PuzzleId
CorrectionCapacity = 1

DOOR_A.OPEN
EventLedger:
  E1 CLOSE ...
Suppressed:
  [E1] or []

NPC_WORKER_01:
  routine step
  semantic location
```

## 15. Audio

Required placeholders:
- worker footsteps,
- door movement,
- latch close,
- Review enter,
- event selection tick if relevant,
- Correction commit,
- Correction release.

Door latch must make E1 commit perceptible.

No success jingle.

## 16. Visual requirements

Graybox is acceptable.

Door must clearly distinguish:
- OPEN,
- CLOSED,
- Residual OPEN state.

Residual Exposure should already test:
- double exposure,
- low-saturation amber contribution,
- no whole-object outline.

Do not wait until vertical slice to validate basic readability.

## 17. Deliberate non-features

Prototype A contains no:
- key/badge,
- scanner,
- reaction AI,
- alternate route,
- second event on Door A,
- second correction slot,
- story artifact,
- puzzle timer.

## 18. Automated tests

Domain:

```
InitialOpen_CloseEvent_EffectiveClosed
SuppressClose_EffectiveOpen
ReleaseClose_EffectiveClosed
SuppressClose_DoesNotChangeNpcHistory
Restore_ReturnsInitialState
```

Integration:

- Door binder receives OPEN/CLOSED semantic changes.
- Scene has one binder for DOOR_A.
- Required LocationIds exist.
- Restore places actor/player at valid stable anchors.

## 19. Playtest observations

Record:
- time until player first targets Door A,
- direct-interaction attempts,
- time from Residual Exposure notice to Review,
- accidental correction attempts,
- explanation of what happened to NPC/time,
- comprehension of Release.

## 20. Pass condition

Prototype A passes when:
- technical invariants hold,
- door correction is legible,
- player understands NPC did not rewind,
- Review/Commit feels intentional,
- restore is reliable,
- controller does not distract from the puzzle.
