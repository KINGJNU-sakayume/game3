# UNDO — Prototype Specification

## Purpose

The prototypes exist to prove the game's core mechanic before production content is built.

They are not mini versions of the final game and do not require final art, final narrative, or polished menus.

Required sequence:
1. Prototype A — Basic Correction
2. Prototype B — Consequence Persistence
3. Prototype C — NPC Reaction
4. Vertical Slice

Do not build the 16-level campaign before these gates are passed.

---

# Prototype A — Basic Correction

## Question

Does the fundamental act of reading and suppressing a recorded state change feel clear and satisfying?

## Space

One small office/archive threshold.

Required entities:
- PLAYER
- NPC_WORKER_01
- DOOR_A

## Initial state

```
DOOR_A.OPEN = OPEN
```

## Scripted routine

1. NPC walks through Door A.
2. NPC closes Door A.
3. CLOSE becomes RecordedEvent E1.
4. Player approaches closed door.
5. Door cannot be directly operated.
6. Residual Exposure exposes prior OPEN state.
7. Player enters Review and suppresses E1.
8. Effective state resolves to OPEN.
9. Player passes.

## Required technical proof

- stable EntityId binding,
- event recording,
- state resolution,
- one correction slot,
- Review query,
- suppression,
- door presentation binder,
- restore current record,
- debug event/state display.

## Acceptance criteria

Functional:
- suppressing E1 resolves DOOR_A.OPEN to OPEN,
- NPC position/history does not rewind,
- release restores CLOSED without replaying NPC close action,
- restore returns all semantic states/routine to initial snapshot.

UX:
- first-time user can identify that the door's previous state is actionable,
- interaction does not resemble a normal "press E to open door" system,
- correction feedback is visible/audible without a success toast.

Not required:
- final art,
- final audio,
- narrative,
- second correction slot.

---

# Prototype B — Consequence Persistence

## Status

**Critical greenlight gate for the project.**

## Question

Is the signature contradiction mechanic understandable and interesting enough to sustain a full puzzle game?

## Space

Security desk + badge reader + staff door.

Required entities:
- PLAYER
- GUARD_01
- BADGE_01
- SCANNER_01
- STAFF_DOOR_A
- DESK_BADGE_ANCHOR

## Initial semantic state

```
BADGE_01.POSSESSION = NONE
BADGE_01.POSITION = DESK_BADGE_ANCHOR
SCANNER_01.AUTHORIZATION = DENIED
STAFF_DOOR_A.LOCK = LOCKED
```

## Routine

1. Guard reaches desk.
2. Guard takes badge.
3. Record E1 TAKE.
4. Guard reaches scanner.
5. Guard scans badge.
6. Record E2 AUTHORIZE.
7. System unlocks door.
8. Record E3 UNLOCK.
9. Guard passes door.

## Player action

After E3 exists, player suppresses E1.

## Required resulting state

```
BADGE_01.POSITION = DESK_BADGE_ANCHOR
SCANNER_01.AUTHORIZATION = GRANTED
STAFF_DOOR_A.LOCK = UNLOCKED
GUARD_01 remains after the doorway/current routine position
```

Door must not re-lock automatically.

## Required technical proof

Everything from Prototype A plus:
- multiple recorded events,
- independent downstream persistence,
- POSSESSION/POSITION semantic binding,
- event history query,
- actor memory retained,
- state changes on multiple entities.

## Player-comprehension test

After solving/observing the prototype, ask the tester to explain in their own words why:
- the badge returned,
- the door stayed unlocked.

Do not teach the phrase "Consequence Persistence" before asking.

Target before full production:
- clear majority of first-time testers can explain the rule after limited exposure,
- testers recognize the result as rule-based rather than a bug,
- testers express curiosity about other ways to exploit the rule.

Exact percentages are a playtest target, not a contractual release metric.

## Failure gate

If players consistently expect all downstream consequences to rewind, or the result reads as arbitrary/buggy even after presentation iteration:
- stop full production,
- revise visual/temporal feedback and/or core rule presentation,
- retest.

Do not compensate by adding long tutorial text first.

---

# Prototype C — NPC Reaction

## Question

Can a player create a contradiction and predictably exploit an NPC's present-time reaction?

## Space

Expand Prototype B with:
- BADGE_LOCKER or check point,
- MAINTENANCE_OFFICE door,
- guard search route.

## Required actor memory

After Guard takes/places/uses the badge, memory preserves a relevant expectation such as:
```
ExpectedBadgeLocation = LOCKER_A
```

## Player contradiction

Suppress an event that causes the actual semantic badge state to conflict with the guard's remembered/expected state.

## Reaction

At an authored check point:
```
expected badge state != observed state
→ MissingBadge reaction
→ guard follows authored search route
→ guard opens maintenance office
→ OPEN becomes new RecordedEvent E_NEW
```

Player uses the newly created event/state.

## Required technical proof

- RoutinePlan,
- ActorMemory,
- ReactionRule,
- reaction priority,
- semantic LocationIds,
- navigation execution,
- new event generation after contradiction,
- debug reason display,
- reset restores routine/memory deterministically.

## Acceptance criteria

- same snapshot + same correction timing at the defined evaluation boundary yields same reaction,
- guard does not instantly know the badge changed from across the map unless an authored information source exists,
- no random branch selection,
- reaction reason can be shown in developer debug panel,
- newly created door event is independent history.

---

# Shared prototype presentation rules

Graybox is acceptable.

Use:
- clear primitive geometry,
- readable doors/anchors,
- placeholder but distinct audio,
- temporary Residual Exposure implementation,
- development debug overlay.

Do not spend significant time on:
- final lighting,
- final documents,
- full narrative voice,
- high-fidelity character models,
- final menus.

## Instrumentation

Every prototype must make it possible for the developer to inspect:
- event ledger,
- effective state,
- suppressed event list,
- correction capacity,
- actor routine/reaction,
- snapshot restore.

If a bug cannot be explained from this instrumentation, improve instrumentation before adding more content.

## Prototype completion order

Prototype A must work before B.

Prototype B must pass its design/readability gate before C is treated as production validation.

Prototype C must pass before vertical-slice content locks NPC-heavy puzzle architecture.

## What success means

Prototype success does not mean "the code runs."

It means:
- core rule is technically correct,
- first-time humans can form a correct mental model,
- the interaction creates interest rather than confusion,
- debugging/reset workflow is reliable enough to build on.
