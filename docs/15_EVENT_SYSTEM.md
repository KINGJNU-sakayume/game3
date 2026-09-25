# UNDO — Event and State System

## Status

Canonical design specification. Implementation names may evolve, but behavior must remain equivalent unless this document is revised.

## Core concepts

### EntityId
Stable identity for any gameplay-relevant actor, object, machine, logical system, or semantic location.

### StateChannel
A declared category of semantic state.

Initial canonical channels:
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

New channels require a real reusable design need. Do not add a channel for every one-off level property.

### StateKey

Conceptually:
```
(EntityId, StateChannel)
```

Example:
```
(DOOR_B2_014_A, OPEN)
```

### StateValue

A serializable semantic value.

Expected value categories include:
- boolean,
- integer,
- scalar when truly required,
- stable string/enum-like identifier,
- EntityId,
- semantic LocationId.

Do not use arbitrary Godot objects or scene references as domain values.

### InitialState

The value of a StateKey at puzzle snapshot initialization before event contributions.

### RecordedEvent

An immutable record of a qualifying state-changing occurrence.

Conceptual shape:

```csharp
RecordedEvent
{
    EventId Id;
    long Sequence;
    GameTime Timestamp;

    EntityId ActorId;
    EventVerb Verb;
    EntityId TargetId;

    IReadOnlyList<StateMutation> Mutations;

    bool Reversible;
    RecordSource Source;
}
```

`active/suppressed` is not stored by mutating the event itself in the canonical model. Suppression state belongs to the correction/session layer so the historical record remains immutable.

### StateMutation

Conceptual shape:

```csharp
StateMutation
{
    StateKey Key;
    StateValue PreviousValue;
    StateValue ResultingValue;
}
```

Normally an event should mutate the smallest meaningful set of channels.

## Event immutability

Once recorded, the historical event object is immutable.

Correction does not delete or rewrite the record.

The session tracks which event contributions are suppressed.

This distinction supports:
- Review history,
- save/load,
- debugging,
- Record 0 narrative logic,
- consequence persistence.

## Event ordering

Each puzzle session assigns a strictly increasing sequence number to committed events.

Timestamp is presentation/narrative metadata.
Sequence is authoritative for state resolution.

Never depend on floating-point clock equality to establish causal ordering.

## Effective-state resolution

For a StateKey:

1. start from the puzzle's InitialState,
2. inspect event mutations affecting that key in sequence order,
3. ignore currently suppressed contributions,
4. the latest remaining active resulting value is effective.

Equivalent conceptual model:

```
effective = initial

for event in ledger order:
    if event not suppressed:
        for mutation matching key:
            effective = mutation.resultingValue
```

Prototype implementation may use this direct algorithm.

Optimize only after profiling. Correctness and inspectability matter more than premature indexing.

## PreviousValue semantics

`PreviousValue` records the effective value immediately before the event was originally committed.

It exists for:
- Review display,
- validation/debugging,
- historical interpretation.

Effective-state resolution should not naïvely assign `PreviousValue` when suppressing an event, because another earlier/later active event may determine the correct state.

Always re-resolve the channel.

## Consequence Persistence

Events do not automatically own their downstream consequences.

Example ledger:

```
E1 TAKE badge: badge possession DESK -> GUARD
E2 AUTHORIZE scanner: authorization DENIED -> GRANTED
E3 UNLOCK door: lock LOCKED -> UNLOCKED
```

Suppress E1:

```
E1 contribution ignored
E2 remains
E3 remains
```

Effective result:
- badge possession resolves to DESK,
- authorization remains GRANTED,
- door remains UNLOCKED.

No cascade-delete or dependency rollback is allowed by default.

## Event eligibility

An event is Review/Correction eligible only when explicitly valid under content/system rules.

Possible factors:
- event marked reversible,
- source sufficiently recorded,
- target still belongs to active puzzle scope,
- mutation channel permitted,
- narrative/system exclusions,
- capacity available for a new maintained correction.

Eligibility must be queryable with a reason code for debugging.

Possible reason codes:
- REVERSIBLE,
- NOT_REVERSIBLE,
- NOT_RECORDED_ENOUGH,
- CHANNEL_NOT_CORRECTABLE,
- OUT_OF_SCOPE,
- ALREADY_SUPPRESSED,
- CAPACITY_FULL,
- NARRATIVE_NULL_EVENT.

Player-facing UI need not display these internal labels.

## Correction state

Conceptual session data:

```csharp
CorrectionState
{
    int Capacity;
    OrderedSet<EventId> SuppressedEvents;
}
```

Capacity progression is authored by campaign progression, not calculated from an RPG stat.

## Suppression commit

To suppress event E:

1. validate E,
2. validate capacity if E is not already suppressed,
3. add E to suppressed set,
4. collect StateKeys mutated by E,
5. re-resolve those keys,
6. notify application/presentation of effective changes,
7. evaluate relevant reactions against the new current state.

Do not replay historical time.

## Suppression release

To release E:

1. remove E from suppressed set,
2. collect its StateKeys,
3. re-resolve them using all active historical contributions,
4. present the new effective values at the current moment,
5. evaluate reactions.

Do not replay the actor animation that originally created E unless a separate present-time reaction explicitly does so.

## New events caused by contradictions

If an NPC observes a new contradiction and reacts, the reaction generates new current-time events normally.

Example:

```
historical E10: Guard places badge in locker
player suppresses E10
locker no longer contains badge

current reaction:
Guard checks locker
Guard begins missing-badge search
Guard opens maintenance office
→ new current event E27 OPEN door
```

E27 is independent history and may be correctable if eligible.

## Actor memory

Actor memory is separate from WorldState.

An actor may retain facts such as:
- "I placed badge X in locker Y,"
- "I completed inspection route A,"
- "I responded to alarm Z."

Correction does not automatically modify memory.

Memory entries must be minimal and authored for behavior needs; do not attempt to simulate human cognition generally.

## Machine reactions

Machines can also be deterministic reactors.

Example:
```
AUTHORIZATION becomes INVALID
AND person-presence sensor is OCCUPIED
→ security controller creates ALERT
```

Treat machines and human actors consistently at the rule level where useful, but keep content semantics clear.

## Multi-mutation events

Prefer one state-changing intent per event.

A single real action may legitimately change multiple tightly coupled channels, but multi-mutation events require caution because correction suppresses the event as a unit.

Example acceptable:
```
door safety controller CLOSES door:
OPEN = false
MOTION = stopped
```

If two consequences need to be independently suppressible, they must become separate recorded events.

This content-authoring distinction is crucial.

## Continuous movement

Continuous transforms are not event history.

For puzzle-relevant moving systems:
- define semantic milestones,
- generate events at meaningful state transitions.

Example:
```
CONVEYOR MOTION: STOPPED -> MOVING
BOX POSITION: INPUT_ANCHOR -> SCANNER_ANCHOR
```

The presentation may animate continuously between anchors.

Do not record every physics frame.

## Player movement

Player position is not part of the correction event ledger.

The player may be checkpointed for restore/save separately.

## Record sources

Possible source categories:
- access control,
- machinery,
- security,
- archive system,
- environmental sensor,
- correlated institutional record.

Source categories help content eligibility and presentation but do not change the fundamental resolution algorithm.

## Record 0

Record 0 must **not** be faked as a normal reversible event with a special disabled button.

It represents absence of a qualifying transition.

The UI/application query should genuinely find no RecordedEvent for the expected action window.

Narrative metadata can represent:
- expected response interval,
- surrounding events,
- an analytical gap.

But the correction system receives no event ID to suppress.

This preserves the mechanic/theme alignment.

## Debug invariants

The following should be asserted/tested:

1. Event sequence is strictly increasing.
2. Historical events are immutable after commit.
3. Suppression set contains only existing event IDs.
4. Suppression count never exceeds capacity.
5. Resolving the same ledger/suppression set yields the same semantic state.
6. Suppressing E never removes a distinct later event.
7. Releasing E never replays its historical actor action.
8. A state channel with all contributing events suppressed resolves to initial state.
9. Save/load of ledger + suppression state reconstructs the same effective world state.
10. Record 0 gap exposes no fake correctable event.

## Prototype-B acceptance example

Initial:
```
BADGE_03.POSSESSION = DESK_A
DOOR_A.LOCK = LOCKED
```

Events:
```
E1 Guard TAKE Badge
BADGE_03.POSSESSION: DESK_A -> GUARD_01

E2 Scanner AUTHORIZE
SCANNER_A.AUTHORIZATION: DENIED -> GRANTED

E3 Door UNLOCK
DOOR_A.LOCK: LOCKED -> UNLOCKED
```

After suppress E1:
```
BADGE_03.POSSESSION = DESK_A
SCANNER_A.AUTHORIZATION = GRANTED
DOOR_A.LOCK = UNLOCKED
```

Any implementation that automatically re-locks the door because E1 was suppressed violates the core game design.
