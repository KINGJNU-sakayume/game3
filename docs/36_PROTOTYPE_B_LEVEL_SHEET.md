# UNDO — Prototype B Level Sheet

Last locked: 2026-09-25

## ID

`PT_B_PERSISTENCE`

## Purpose

Prototype B proves UNDO's signature mechanic:

> A cause can be suppressed after later consequences have become independent recorded events.

This is the principal project greenlight gate.

The level must make the causal chain physically obvious enough that the player can form a general rule rather than memorize a special case.

## Status

**CANONICAL GRAYBOX SPEC — CRITICAL GATE**

Dimensions/timings are implementation baselines and may be tuned. Causal ordering and observation requirements are canonical.

---

## 1. Space

Single connected security-transfer room with adjacent staff corridor.

Approximate footprint:

```
16 m × 12 m
```

ASCII blockout:

```
┌─────────────────────────────────────────┐
│                                         │
│ [DESK + BADGE]                          │
│      D                                  │
│      X GUARD START                      │
│                                         │
│                 [SCANNER] [STAFF DOOR]──┼── STAFF CORRIDOR
│                     S          A         │
│                                         │
│                                         │
│        OBSERVATION RECESS               │
│                O                        │
│                                         │
│ PLAYER START                            │
│     P                                   │
└─────────────────────────────────────────┘
```

From `O`, player can see in one composition or one short pan:
- Desk/Badge,
- Guard,
- Scanner,
- Staff Door.

This is mandatory for first exposure.

## 2. Key entities

### PLAYER

Spawn:
`LOC_PT_B_PLAYER_START`

### GUARD_01

Routine actor.

No random branches.

### BADGE_01

Gameplay channels:
- POSITION
- POSSESSION

Initial:

```
POSITION = LOC_PT_B_BADGE_DESK
POSSESSION = NONE
```

Presentation:
physically visible on desk.

### SCANNER_01

Gameplay channel:
- AUTHORIZATION

Initial:

```
AUTHORIZATION = DENIED
```

### STAFF_DOOR_A

Gameplay channels:
- LOCK
- OPEN

Initial:

```
LOCK = LOCKED
OPEN = false
```

Door remains closed while locked.

After authorization/unlock, guard operates/passes through according to routine.

## 3. Semantic locations

Required:

```
LOC_PT_B_PLAYER_START
LOC_PT_B_OBSERVATION
LOC_PT_B_BADGE_DESK
LOC_PT_B_GUARD_DESK
LOC_PT_B_SCANNER_FRONT
LOC_PT_B_DOOR_INSIDE
LOC_PT_B_DOOR_OUTSIDE
LOC_PT_B_STAFF_CORRIDOR
LOC_PT_B_EXIT
```

## 4. Causal teaching objective

Player must observe this full chain at least once:

```
Guard takes Badge
→ Guard carries Badge
→ Guard scans Badge
→ Scanner grants authorization
→ Door unlocks
→ Guard passes through
```

The visual/audio design must communicate that these are separate moments.

## 5. Baseline routine timeline

Relative to routine start:

```
T+0.0   Guard at Desk
T+1.0   Guard reaches for Badge
T+1.5   E1 TAKE commits
T+2.0   Guard begins moving to Scanner
T+5.0   Guard reaches Scanner
T+5.4   scan interaction begins
T+5.9   E2 AUTHORIZE commits
T+6.1   scanner feedback confirms grant
T+6.3   E3 UNLOCK commits
T+6.8   Guard begins door passage
T+8.0   Guard is through Door A
T+8.5   door may close as a separate E4 if needed for spatial staging
T+10+  Guard settles at staff-corridor checkpoint
```

Whether DoorClose E4 exists in the initial B prototype should be kept simple. It must not distract from E1/E2/E3.

Recommended first implementation:
- door auto-opens for guard after unlock,
- guard passes,
- door remains physically open briefly or closes in a clearly secondary event.

## 6. Event definitions

### E1 — TAKE Badge

```
Actor:
GUARD_01

Verb:
TAKE

Target:
BADGE_01

Mutations:
BADGE_01.POSSESSION:
NONE → GUARD_01

BADGE_01.POSITION:
LOC_PT_B_BADGE_DESK → LOC_CARRIED_BY_GUARD
```

For domain cleanliness, final representation may use POSSESSION as authoritative and derive presentation attachment. If POSITION and POSSESSION are both stored, their invariants must be explicit.

E1 correctable:
yes.

### E2 — AUTHORIZE Scanner

```
Actor:
GUARD_01

Verb:
AUTHORIZE

Target:
SCANNER_01

Mutation:
SCANNER_01.AUTHORIZATION
DENIED → GRANTED
```

Correctability:
may be enabled in debug, but the first player-facing intended solution must center on E1.

### E3 — UNLOCK Door

```
Actor:
SECURITY_SYSTEM or SCANNER_01

Verb:
UNLOCK

Target:
STAFF_DOOR_A

Mutation:
STAFF_DOOR_A.LOCK
LOCKED → UNLOCKED
```

E3 is independent history after commit.

## 7. Commit points

E1:
when badge visually transfers to Guard possession anchor.

E2:
when scanner gives accepted/authenticated response.

E3:
when door lock actuator/latch visibly/audibly releases.

Do not merge E2 and E3 into one event if the implementation intends them to be independently represented.

The prototype's central lesson requires visible separation between:
- badge taken,
- authorization granted,
- door unlocked.

## 8. Observation anchor

`LOC_PT_B_OBSERVATION`

Approximate:
- 6–8 m from Desk,
- 5–7 m from Scanner,
- clear Door sightline.

The player can reposition, but the room composition should naturally invite this vantage point.

Use architecture:
- recessed waiting area,
- records cart parking bay,
- service alcove,

rather than a videogame platform.

## 9. Residual Exposure timing

After E1:
Badge can show prior desk state in Review.

After E2:
Scanner may show prior denied state if inspected.

After E3:
Door lock state becomes part of recorded history.

Do not overwhelm the room with multiple equally loud amber traces.

Priority when player looks naturally:
1. current target,
2. recent relevant state,
3. other traces remain subtle.

## 10. Intended player solution

The core solution is not merely "wait and click."

Expected cognitive path:

1. watch Guard take Badge,
2. watch Guard scan it,
3. watch Door unlock,
4. recognize Badge's TAKE event remains reviewable,
5. suppress E1 after E2/E3 commit,
6. Badge returns to Desk,
7. Scanner remains GRANTED,
8. Door remains UNLOCKED,
9. Guard remains in staff corridor/current location.

The moment after step 6 must remain visually stable long enough for the player to inspect the contradiction.

## 11. Required resulting state

After E1 suppression:

```
BADGE_01.POSSESSION = NONE
BADGE_01.POSITION = LOC_PT_B_BADGE_DESK

SCANNER_01.AUTHORIZATION = GRANTED

STAFF_DOOR_A.LOCK = UNLOCKED

GUARD_01 = current semantic location after passage
GUARD memory/history = unchanged
```

This exact conceptual result is non-negotiable.

## 12. Player goal

Prototype goal:
reach `LOC_PT_B_EXIT` / staff corridor by exploiting the useful persisted state.

If Door A is physically closed after Guard passage:
- the player may need the unlocked state to permit an authored automatic/open interaction by another actor/system,
- or the prototype may keep it open for simplicity.

Do not accidentally add a second unrelated door-opening puzzle.

The signature persistence state is the lesson.

## 13. Early suppression behavior

The player may try to suppress E1 before E2.

This must produce a coherent state.

If E1 is suppressed before scanner use:
- Badge is no longer in Guard possession,
- Guard cannot legitimately perform the badge scan at the same authored point without a current valid possession/action.

Prototype behavior should prove the importance of **when** the later event has committed.

Recommended:
- Guard reaches Scanner,
- detects required badge unavailable at the defined interaction check,
- pauses/returns/restarts simple routine or triggers a simple no-badge fallback.

Do not invent E2 anyway if its preconditions are not met.

This makes the difference between:
- suppress before consequence,
- suppress after consequence,

learnable.

## 14. Routine reset / repetition

Because a tester may miss the chain, the routine must be repeatable without waiting several minutes.

Target full cycle:
approximately 15–25 seconds after a failed/reset attempt.

Simplest prototype approach:
- explicit RESTORE CURRENT RECORD for controlled retest,
- optional automatic guard cycle only if deterministic and easy to read.

Do not create complicated self-resetting world logic before needed.

## 15. Review history

Badge Review must make E1 selectable after Guard has left the desk.

The Badge being currently carried/offstage must not make its historical event impossible to target if the player cannot physically point at it.

Therefore Prototype B should decide one of these implementation-friendly interaction patterns:

### Canonical preferred pattern

After Guard passes through, the player can Review the Badge through its **current semantic/presentation target** while it is still visible/trackable on Guard OR while Guard remains nearby.

Then suppress E1 and Badge returns to Desk.

Do not require reviewing an object through walls.

### Alternative if visibility is poor

Design Guard route so the Badge remains visible on the Guard at the intended moment from the observation area.

The level geometry should solve targetability, not a global event menu.

## 16. Post-suppression pause

After E1 is suppressed:
- Guard should remain in a stable nearby checkpoint for several seconds,
- Badge clearly reappears on desk,
- Door lock indicator remains unlocked,
- scanner retains granted state.

This is a teaching tableau.

Do not immediately trigger another noisy reaction.

Prototype C will test reaction complexity.

## 17. Release

If tester releases E1:

Expected:
- Badge contribution returns to Guard possession according to state resolution/presentation,
- Door remains unlocked because E2/E3 still exist,
- no historical events are replayed.

This is useful technical proof, though not the primary player lesson.

## 18. Completion

Preferred:

```
PLAYER at LOC_PT_B_EXIT
AND STAFF_DOOR_A.LOCK == UNLOCKED
```

If physical OPEN state is required, include that fact explicitly.

## 19. Reset snapshot

Restore:

```
PLAYER = LOC_PT_B_PLAYER_START
GUARD_01 = LOC_PT_B_GUARD_DESK
Guard routine = start
Guard relevant memory = initial

BADGE_01 = desk / no owner
SCANNER_01.AUTHORIZATION = DENIED
STAFF_DOOR_A.LOCK = LOCKED
STAFF_DOOR_A.OPEN = false

EventLedger = initial
SuppressedEvents = empty
```

## 20. Audio

Distinct functional cues:

E1 TAKE:
- physical badge handling.

E2 AUTHORIZE:
- restrained accepted scanner response.

E3 UNLOCK:
- mechanical latch/relay release.

The player should be able to tell E2 and E3 are two moments.

Avoid one giant "access granted" sci-fi sound that collapses them perceptually.

## 21. Visual state

Badge:
- obvious enough on Desk,
- visible on Guard during intended observation.

Scanner:
- DENIED/GRANTED state readable without color only.

Door:
- LOCKED/UNLOCKED difference readable through hardware/indicator/audio.

No UI card saying:
"Door remains unlocked due to Consequence Persistence."

## 22. Debug requirements

Display:
- E1/E2/E3 sequence,
- Badge state,
- Scanner authorization,
- Door lock state,
- E1 suppressed?,
- Guard semantic location,
- Guard memory if any,
- precondition failure if E1 suppressed early.

## 23. Domain regression tests

Permanent tests:

```
SuppressTakeAfterUnlock_BadgeReturns_DoorStaysUnlocked
SuppressTakeAfterAuthorize_AuthorizationPersists
SuppressTakeBeforeAuthorize_PreventsInvalidFutureAuthorizeAction
ReleaseTake_DoesNotDeleteAuthorizeOrUnlock
RestorePrototypeB_ReturnsInitialState
```

The exact early-suppression reaction test depends on implemented actor application logic, but an impossible scan must not be silently recorded.

## 24. Human comprehension questions

After play, without teaching terminology:

1. What happened to the Badge?
2. Why did the Door stay unlocked?
3. What would happen if you undid the Badge action before it was scanned?
4. What else would you try this mechanic on?
5. Did anything look like a bug?

## 25. Greenlight condition

Prototype B passes only if both are true:

### Technical
- state matches canonical result,
- no downstream rollback,
- timing/order deterministic,
- reset reliable,
- debug state explains behavior.

### Human
- testers form a generalizable rule,
- they understand the consequence had already become its own event/state,
- they can predict a nearby hypothetical,
- the contradiction creates curiosity rather than distrust.

If this fails, stop campaign production.
