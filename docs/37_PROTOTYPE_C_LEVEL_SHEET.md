# UNDO — Prototype C Level Sheet

Last locked: 2026-09-25

## ID

`PT_C_NPC_REACTION`

## Purpose

Prototype C proves that a contradiction can be discovered by an NPC at a believable observation point, producing a deterministic present-time reaction and a new useful event.

It extends Prototype B.

It must not become a general AI sandbox.

## Status

**CANONICAL GRAYBOX SPEC**

---

## 1. Space

Use Prototype B's security-transfer room plus:
- Badge Locker/check station,
- Maintenance Office,
- short service corridor.

Approximate total footprint:

```
20 m × 16 m
```

ASCII blockout:

```
┌──────────────────────────────────────────────────┐
│                                                  │
│ [DESK]                                           │
│   D                                              │
│                     [SCANNER][DOOR A]────────────┼── STAFF CORRIDOR
│                                                  │
│              OBS                                 │
│               O                                  │
│                                                  │
│                                   [LOCKER/CHECK] │
│                                         L        │
│                                         |        │
│                                [MAINT OFFICE]    │
│                                     [DOOR M]     │
│                                                  │
│ PLAYER P                                         │
└──────────────────────────────────────────────────┘
```

The layout must make:
- original Badge chain observable,
- later Guard check point understandable,
- Guard reaction route visible/audible.

## 2. Core entities

Carry forward:
- GUARD_01
- BADGE_01
- SCANNER_01
- STAFF_DOOR_A

Add:
- BADGE_LOCKER_A or semantic CheckLocation,
- MAINT_DOOR_M,
- MAINTENANCE_OFFICE,
- optional simple search marker at Security Desk.

## 3. Core contradiction

The intended world/memory conflict:

```
Guard memory/expectation:
"I placed / expect Badge at LOCKER_A"

Current world after player correction:
Badge is NOT at LOCKER_A
```

The exact preceding event chain must create this naturally.

Preferred Prototype C chain:

```
1. Guard takes Badge from Desk.
2. Guard uses Badge / passes security.
3. Guard later places Badge in Locker A.      -> E4 PLACE
4. Guard continues routine.
5. Player suppresses E4.
6. Effective Badge state resolves to its previous active state/location.
7. Guard memory still says Badge was placed in Locker A.
8. At next authored locker check, Guard observes mismatch.
9. RX_GUARD_MISSING_BADGE fires.
10. Guard searches Security Desk / Maintenance Office.
11. Guard opens MAINT_DOOR_M.                 -> E_NEW
12. Player exploits E_NEW.
```

This is cleaner for memory contradiction than suppressing the original TAKE event, because Guard can explicitly remember completing PLACE at Locker A.

Prototype C may reuse E1/E2/E3 from Prototype B, but the new lesson is E4 → observed contradiction → reaction.

## 4. New event E4 — PLACE Badge

```
Actor:
GUARD_01

Verb:
PLACE

Target:
BADGE_01

Mutation:
BADGE_01.POSSESSION:
GUARD_01 → NONE

BADGE_01.POSITION:
LOC_CARRIED_BY_GUARD → LOC_BADGE_LOCKER_A

Correctable:
yes
```

Commit point:
badge visibly attaches/settles into Locker A semantic anchor.

At commit:
Guard memory updates:

```
expectedBadgeLocation = LOC_BADGE_LOCKER_A
```

Correction later does not rewrite that memory.

## 5. Suppress E4

After E4 is suppressed:

World state re-resolves to previous active contribution.

Preferred effective result:
- Badge returns to Guard's prior semantic possession/location OR to a deliberately authored earlier location according to the event ledger.

For the reaction to be visually clear, the prototype should choose a previous state that results in:
- Locker A empty,
- Guard no longer actively holding/using Badge by the time of check,
- a coherent visible Badge location.

The final exact previous-state chain must be represented honestly in the ledger.

Do not teleport the Badge to an arbitrary "missing" location that is not produced by prior active state.

## 6. Actor memory

Minimal:

```
expectedBadgeLocation = LOC_BADGE_LOCKER_A
placedBadgeEventId = E4
```

No general belief system.

Memory update occurs on E4 commit.

Suppressing E4:
- world changes,
- memory unchanged.

## 7. Observation boundary

Guard must not instantly react when E4 is suppressed from across the room.

Reaction evaluation is allowed when Guard reaches:

`LOC_BADGE_CHECK_A`

and performs an authored CheckBadge intent/observation.

At that moment:

```
memory expectedBadgeLocation = LOC_BADGE_LOCKER_A

actual BADGE_01.POSITION != LOC_BADGE_LOCKER_A

→ mismatch observed
```

This explicit boundary is central to Prototype C.

## 8. Reaction definition

### RX_GUARD_MISSING_BADGE

Priority:
high enough to interrupt ordinary patrol, below immediate safety alarm if such a system existed.

Policy:
`ONCE_PER_TRIGGER_INSTANCE`

Trigger:
observed mismatch at CheckBadge.

Reaction steps, baseline:

```
1. Inspect Locker A briefly.
2. Move to Security Desk.
3. Inspect Desk.
4. If Badge still not found at expected search condition,
   move to Maintenance Office.
5. OPEN MAINT_DOOR_M.
6. Inspect/search inside.
7. Reaction ends or transitions to stable wait.
```

No randomness.

## 9. New useful event

When Guard opens Maintenance Door M:

### E_NEW — OPEN Maintenance Door

```
Actor:
GUARD_01

Verb:
OPEN

Target:
MAINT_DOOR_M

Mutation:
MAINT_DOOR_M.OPEN
false → true
```

This event is present-time history produced because of the player's contradiction.

Player can:
- pass through while open,
- potentially later Review it if eligible.

The key lesson:
**Correction can cause the world to create new events.**

## 10. NPC reaction visibility

The player must understand:
- Guard checked Locker,
- found something wrong,
- began searching,
- opened Maintenance Office because of that search.

Use:
- body orientation,
- brief inspect animation,
- footsteps,
- locker interaction,
- predictable route,
- door sound.

Do not rely on spoken exposition like:
"Where did my badge go? I should check maintenance."

A small natural mutter may be tested later but is not required.

## 11. Baseline timing

After E4 commit:

```
T+0   Guard leaves Locker A
T+4   Guard completes one short routine waypoint
T+8   Guard returns toward Locker check
T+10  CheckBadge observation
T+11  MissingBadge reaction begins
T+14  Security Desk search
T+19  Move to Maintenance Office
T+22  E_NEW OPEN commits
```

Target from player suppression to useful reaction:
roughly 10–20 seconds, not a minute-long wait.

Exact timing is tunable.

## 12. Early/late correction cases

### Suppress E4 before Guard leaves Locker

Guard should still not magically know suppression occurred unless the authored observation immediately exposes it.

If Guard is visually still looking at Locker at the moment state changes, an immediate observation may be plausible.

For first implementation, define a clean evaluation point after Guard moves away, reducing ambiguity.

### Release E4 before CheckBadge

Then Badge resolves back to Locker A.

At CheckBadge:
- expected == observed,
- MissingBadge reaction does not fire.

This is an important deterministic test.

### Release during reaction

The reaction already exists as a present-time behavioral response.

Releasing E4 does not retroactively erase the fact that Guard noticed the discrepancy.

Depending on authored reaction design:
- Guard may continue current search to completion,
- or a later explicit recheck can cancel/redirect.

For Prototype C, canonical simple behavior:
**once RX_GUARD_MISSING_BADGE has fired for the trigger instance, Guard completes the authored reaction sequence.**

This demonstrates consequence persistence at the behavioral level without recursive complexity.

## 13. Completion

Preferred:

```
PLAYER at LOC_MAINT_OFFICE_EXIT
```

The Maintenance Door event is the enabling route.

## 14. Reset snapshot

Restore all:

```
player start
Guard routine start
Guard initial memory
Badge desk/initial state
Scanner denied
Staff Door locked
Locker empty or authored initial state
Maintenance Door closed
EventLedger baseline
Suppressions empty
Reaction trigger history clear
```

## 15. Debug requirements

Selected Guard panel:

```
ActorId
Routine step
Current intent
Semantic location

Memory:
  expectedBadgeLocation

Current observed facts:
  BADGE_01.POSITION

Active Reaction:
  RX_GUARD_MISSING_BADGE / none

Reason:
  expected != actual at LOC_BADGE_CHECK_A

Trigger instance:
  <id>

Reaction step:
  ...
```

World panel:
- E4 active/suppressed,
- Badge current effective location,
- E_NEW present?,
- Maintenance Door state.

## 16. Automated tests

Domain:

```
SuppressPlace_DoesNotRewriteExpectedBadgeMemory
AtCheckPoint_Mismatch_FiresMissingBadgeReaction
BeforeCheckPoint_Mismatch_DoesNotFireOmniscientReaction
ReleaseBeforeCheckPoint_PreventsMissingBadgeReaction
SameSnapshotAndCorrection_ProducesSameReaction
ReactionTrigger_DoesNotDuplicate
ReactionOnceFired_CompletesWhenOriginalSuppressionReleased
```

Integration:
- Guard navigation reaches semantic search locations.
- Maintenance Door event commits at the correct animation point.
- Reset restores reaction history/memory.
- No duplicate EntityId/LocationId bindings.

## 17. Player comprehension questions

After play:

1. Why did the Guard change what he was doing?
2. When did he notice something was wrong?
3. Why didn't he react immediately when you changed the Badge?
4. What would happen if you restored the Badge before he checked?
5. Was his search route predictable?

## 18. Failure conditions

Prototype C fails if:
- Guard reacts omnisciently,
- route feels random,
- correction timing creates different outcomes from the same defined state,
- reaction reason cannot be explained in debug,
- player cannot see/hear the observation boundary,
- bespoke level script bypasses ReactionRule architecture,
- reset cannot reproduce the initial actor state.

## 19. Pass condition

Prototype C passes when:
- contradiction and memory can coexist,
- reaction fires only through understandable observation,
- the same conditions produce the same next behavior,
- reaction generates a new independent event,
- players predict the NPC rather than merely follow him,
- implementation remains within canonical Routine/Memory/Reaction architecture.
