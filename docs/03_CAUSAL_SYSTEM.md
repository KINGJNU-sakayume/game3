# UNDO — Causal System

## Purpose

The game world is modeled as a network of discrete state-changing events rather than a globally rewindable physics history.

The technical and design model must support logical contradictions without attempting to retroactively resimulate the entire world.

## Conceptual event shape

```ts
Event {
  id
  actor
  target
  verb
  timestamp

  stateChannel
  previousValue
  resultingValue

  reversible
  active
}
```

This is conceptual, not yet an implementation API.

## Causal decoupling

A causal relationship may exist between events without making them a single reversible unit.

```
A: Guard takes key
B: Guard unlocks door
C: Guard enters room
D: Guard activates scanner
```

Suppressing A leaves B, C, and D intact if they already occurred as independent events.

The result is a causally inconsistent but mechanically valid present.

## Effective state

For a state channel, the effective value is determined from active events according to event ordering plus any initial state.

Suppression toggles the contribution of an event. It does not erase the event record.

## New events after contradiction

NPCs and machines observe the current effective world state and may produce new events.

Example:

```
Guard remembers placing badge in locker
→ player suppresses PlaceBadge
→ locker is now empty
→ guard inspects locker
→ MissingBadge reaction
→ guard changes route
→ new door-open event occurs
```

These new events are real events and may themselves be eligible for suppression.

## No retrospective AI simulation

NPC decision history is never recomputed because an earlier event was suppressed.

AI evaluates the **current** state and decides only the next action.

This keeps behavior understandable and prevents combinatorial resimulation.

## Causal contradiction as target state

Late puzzles may ask the player to create a world state that normal causality cannot generate, for example:

```
Badge = storage desk
Door = unlocked
Guard = restricted sector
Alarm = off
Authorization = invalid
```

Such states are not bugs. They are the culmination of the game's rules.

## Record 0 relationship

The correction system is designed for state-changing events. Record 0 represents a gap where a transition was expected but no state-changing action occurred.

Conceptual shape:

```
previous state = known
event = NULL
resulting state = unchanged
expected transition = unknown
```

The system can suppress events. It cannot suppress the absence of an event.
