# UNDO — Core Mechanic

## Definition

"Undo" is the player-facing term for suppressing the direct state mutation created by an eligible recorded event.

The in-world institutional language uses terms such as:
- Correction,
- State Correction,
- Event Suppression,
- Record Invalidation.

Characters do not casually call it an "Undo power."

## Fundamental rule

Suppression does **not** delete history and does **not** replay the past.

For an event:

```
Before: Door = OPEN
Event: Guard closes Door
After: Door = CLOSED
```

Suppressing that event changes the door's effective state back toward the previous active state for the relevant channel. The guard does not walk backward. Time does not rewind.

## State channels

Interactable objects are modeled through discrete state channels. Initial canonical channels:

- OPEN
- LOCK
- POSITION
- POSSESSION
- POWER
- SIGNAL
- MOTION
- INTEGRITY
- ALERT
- VISIBILITY
- FLOW
- AUTHORIZATION

An event modifies one or more explicitly declared channels. Suppression affects only those declared mutations.

## Event precedence

Multiple events may affect the same state channel.

Example:

```
09:00 A closes Door
09:02 B opens Door
09:05 C closes Door
```

Current state is CLOSED.

If the 09:05 event is suppressed, the effective state becomes the latest remaining active value: OPEN from 09:02.

If only the 09:00 event is suppressed while 09:05 remains active, no visible change occurs.

This masked-event behavior is introduced only after the player understands basic suppression.

## Consequence Persistence

A later event remains valid if it has already been independently recorded.

Example:

```
TakeBadge
→ ScanBadge
→ UnlockDoor
```

Suppressing TakeBadge does not suppress ScanBadge or UnlockDoor.

This allows states such as:
- badge returned to its original location,
- door still unlocked,
- guard already elsewhere.

These contradictions are intentional gameplay states.

## NPC memory

Suppression does not rewrite NPC memory.

An NPC may remember taking an item that is no longer in their possession. The NPC reacts to the current contradiction according to deterministic reaction rules.

## NPC movement

NPC position is not directly suppressible as a normal player tool.

The player changes:
- doors,
- objects,
- signals,
- machines,
- permissions,
- resources,

and thereby changes where NPCs choose to go next.

This prevents the mechanic from collapsing into NPC teleportation.

## Suppression release

A maintained suppression can be released.

When released, the original event's mutation becomes active again at the current moment. The original actor does not replay the action.

Example:

```
DoorClose suppressed → Door OPEN
NPC passes through
Suppression released → Door CLOSED
```

Release timing is a major puzzle verb.

## Suppression capacity

Canonical progression:
- early game: 1 maintained suppression,
- mid game: 2,
- late game: maximum 3.

Capacity is not mana, cooldown, or consumable energy. It limits how many contradictions the player can maintain simultaneously.

## Eligibility

Not every historical action can be suppressed.

The facility's correction system requires a sufficiently recorded state-changing event. Typical eligible sources:
- access control,
- doors,
- sensors,
- CCTV-correlated systems,
- facility machinery,
- electrical controls,
- archive systems.

Normally ineligible:
- thoughts,
- emotions,
- ordinary speech,
- an unrecorded casual movement,
- "not doing something."

The final narrative depends on this distinction.
