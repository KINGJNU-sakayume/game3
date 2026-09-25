# UNDO — Testing Strategy

## Objective

UNDO's highest technical risk is not rendering. It is causal correctness.

Testing therefore prioritizes:
1. semantic state resolution,
2. event immutability,
3. correction behavior,
4. deterministic NPC decisions,
5. save/restore equivalence,
6. content validity,
7. Godot scene bindings,
8. presentation regressions.

## Test layers

### Layer 1 — Pure domain tests

Project:
`tests/Undo.Core.Tests`

Runner:
standard `dotnet test`.

Framework:
**xUnit** unless a concrete integration reason later justifies a change.

These tests run without the Godot editor.

Primary coverage:
- StateValue/channel validation,
- event sequence/order,
- EffectiveStateResolver,
- suppression/release,
- capacity,
- Consequence Persistence,
- masked events,
- ActorMemory behavior,
- ReactionRule priority,
- trigger debouncing,
- semantic save reconstruction.

This layer should contain the majority of logic tests.

### Layer 2 — Content validation

A deterministic validation tool/test suite loads gameplay content definitions and rejects invalid authored data before the game runs.

Validate at minimum:
- duplicate EntityIds,
- unknown EntityId references,
- unknown LocationIds,
- invalid StateValue for channel,
- non-existent event/reaction references,
- reaction-priority ties without stable authored order,
- impossible correction capacity,
- puzzle completion conditions referencing missing state,
- duplicate stable IDs across a puzzle scope.

Content validation must be runnable from command line/CI.

### Layer 3 — Godot headless integration tests

Use Godot's headless capability to load selected scenes and validate application bindings.

Initial integration checks:
- bootstrap scene loads,
- Prototype A/B/C scenes instantiate,
- each declared gameplay EntityId binds exactly once,
- semantic locations resolve to scene anchors,
- required animation/binder nodes exist,
- restore returns actors/objects to expected stable presentation state,
- no missing resource errors.

Do not require visual rendering for these tests.

### Layer 4 — Play-mode functional tests

Small automated/semiautomated scenarios inside Godot.

Examples:
- drive Prototype A through close → suppress → open,
- drive Prototype B through take → scan → unlock → suppress take,
- trigger MissingBadge reaction in Prototype C,
- restore current record from altered state,
- verify correction capacity 1/2/3 behavior.

These are fewer and slower than domain tests.

### Layer 5 — Visual/audio review

Human review remains necessary for:
- Residual Exposure legibility,
- animation commit timing,
- lighting/sightlines,
- sound cue readability,
- UI hierarchy,
- accessibility modes.

Do not pretend these are fully covered by unit tests.

## Test naming

Use behavior-oriented names.

Examples:

`SuppressingTakeBadge_DoesNotRelockDoor_WhenUnlockAlreadyRecorded`

`ReleaseSuppression_ReappliesLatestActiveContribution_WithoutReplayingActor`

`ReactionEvaluator_ChoosesHigherPriorityRule_Deterministically`

Avoid vague names such as:
- TestEvent1
- TestDoor
- WorksCorrectly

## Required domain invariants

Every build intended for shared playtesting should verify:

1. Event sequence strictly increases.
2. RecordedEvent is immutable after append.
3. Effective state is deterministic for the same initial state + ledger + suppression set.
4. Suppression cannot exceed capacity.
5. Suppressing an event never deletes later events.
6. Releasing suppression never invokes the historical actor intent.
7. Masked-event resolution selects the latest active contribution.
8. Actor memory is unchanged unless a present-time behavior explicitly changes it.
9. Save/load reconstructs equivalent semantic state.
10. Record 0 has no fake correctable event.

## Golden regression scenarios

Maintain compact fixtures for:

### G01 — Door
```
initial OPEN
E1 CLOSE
suppress E1
expected OPEN
```

### G02 — Persistence
```
badge DESK
door LOCKED

E1 TAKE badge
E2 AUTHORIZE
E3 UNLOCK

suppress E1

expected:
badge DESK
authorization GRANTED
door UNLOCKED
```

### G03 — Masked events
```
initial OPEN
E1 CLOSE
E2 OPEN
E3 CLOSE

suppress E1 → CLOSED
suppress E3 also → OPEN
```

### G04 — Release
```
initial OPEN
E1 CLOSE
suppress E1 → OPEN
release E1 → CLOSED
historical actor action count unchanged
```

### G05 — Memory contradiction
```
guard memory: badge placed in locker
world after suppression: badge not in locker
guard reaches check point
MissingBadge reaction fires
```

### G06 — Capacity
Capacity 1 rejects a second maintained suppression without releasing/replacing the first according to application rules.

### G07 — Restore
Alter world with several events/corrections, RESTORE CURRENT RECORD, then compare semantic state and actor decision state to snapshot fixture.

## Content authoring gate

No major puzzle enters art production if:
- graybox lacks deterministic reset,
- entity IDs fail validation,
- important rule cannot be asserted in a test/fixture,
- a solution depends on random NPC behavior,
- a correction produces unexplained state not represented in the semantic model.

## Continuous integration target

Once implementation starts, GitHub CI should eventually run:

```
dotnet restore
dotnet build
dotnet test
content validation
Godot --headless integration/smoke checks
```

Exact commands are added after project bootstrap exists.

## Warnings policy

For `Undo.Core` and tests:
- nullable reference types enabled,
- compiler warnings treated seriously,
- warnings-as-errors preferred once bootstrap stabilizes.

Godot-generated/project warnings may require targeted exceptions; do not globally disable warnings to achieve a green build.

## Test data

Prefer small explicit fixtures over giant level dumps.

A test should reveal the causal rule being checked.

Keep fixture IDs human-readable so failing output can be understood by a developer or coding agent.

## Manual playtest instrumentation

Prototype builds must expose development-only overlays/logs capable of showing:
- event ledger,
- state channels,
- suppressed events,
- NPC routine/reaction,
- correction capacity,
- current puzzle completion facts.

These are diagnostic tools, not player UX.

## Definition of a regression

A regression includes:
- incorrect semantic state,
- changed deterministic NPC choice,
- different correction eligibility without a design change,
- restore mismatch,
- previously readable core feedback becoming ambiguous.

Visual improvements do not justify changing causal behavior silently.
