# UNDO — NPC Behavior Architecture

## Goal

NPCs must feel like workers following understandable routines, while remaining deterministic enough to function as puzzle machinery.

The player should be able to form and test statements such as:

> "If the guard finds this door unlocked on the next patrol, he will enter the archive instead of turning left."

NPC behavior is not designed to imitate unrestricted human intelligence.

## Core model

NPC behavior consists of:

```
Routine
+
Current semantic world state
+
Minimal actor memory
+
Prioritized reaction rules
=
Next intent
```

Navigation and animation execute that intent. They do not decide it.

## Behavior layers

### RoutinePlan

Authored ordered work pattern.

Example:

```
GUARD_B2_01

1. SECURITY_DESK
2. CORRIDOR_CHECKPOINT_A
3. STAFF_DOOR_A
4. ARCHIVE_CHECKPOINT
5. SECURITY_DESK
repeat
```

Routine steps may include:
- MoveTo semantic location,
- Inspect entity,
- Operate eligible machine,
- Wait for authored duration/window,
- Perform work animation,
- transition to next step.

### ReactionRule

Explicit conditional behavior that can interrupt or alter routine.

Conceptual form:

```csharp
ReactionRule
{
    ReactionId Id;
    int Priority;
    Condition Condition;
    ReactionPolicy Policy;
    IReadOnlyList<IntentStep> Steps;
}
```

Examples:
- missing assigned badge → search desk/locker,
- active alarm → inspect alarm panel,
- expected route blocked → choose authored alternate route,
- restricted door unexpectedly open → inspect/close,
- assigned machine abnormal → maintenance inspection.

### ActorMemory

Minimal facts needed to make reactions coherent.

Examples:
- placed badge in locker,
- completed inspection A,
- already responded to alarm X,
- saw door Y open unexpectedly.

Memory is not a simulation of belief, emotion, or full episodic history.

Correction does not automatically rewrite memory.

### Intent

One semantic next action.

Examples:
- MOVE_TO(LOCATION_B2_DESK)
- INSPECT(SCANNER_A)
- TAKE(BADGE_03)
- OPEN(DOOR_A)
- CLOSE(DOOR_A)
- USE(SCANNER_A)
- WAIT(3s)

Intent is separate from animation/navigation execution.

## Evaluation timing

Do not re-plan full behavior every rendered frame.

Evaluate reactions at meaningful deterministic boundaries:
- a relevant StateKey changes,
- the actor reaches a semantic location,
- the actor completes an intent,
- an authored timer expires,
- a relevant signal/event is recorded.

This reduces jitter, improves debugging, and makes cause/effect easier to inspect.

## Reaction priority

Rules have explicit priority.

If multiple rules are true:
1. highest priority eligible reaction wins,
2. ties must be resolved by stable authored order,
3. no randomness.

Example priority concept:
- immediate facility safety response,
- assigned alarm response,
- contradiction/missing-object investigation,
- route fallback,
- ordinary routine.

Exact priorities are content data and should be visible in debugging tools.

## Reaction policy

Every reaction declares repeat behavior.

Possible policies:
- ONCE_PER_PUZZLE,
- ONCE_PER_TRIGGER_INSTANCE,
- REPEAT_AFTER_CLEAR,
- CONTINUOUS_WHILE_TRUE where genuinely needed.

This prevents NPCs from opening/closing the same door forever due to a correction loop.

## Trigger identity and debouncing

A reaction to an event/state must have enough identity to know whether it already responded.

Example:
- Alarm A activation sequence 42 is not the same trigger as a later activation sequence 57.

Do not use arbitrary cooldowns to hide logic loops. Fix the trigger semantics.

## Deterministic alternate routes

Route choice is authored.

Bad:
```
if blocked:
    50% stairs
    50% service corridor
```

Good:
```
if DOOR_A unavailable:
    use ALT_ROUTE_B
else:
    use PRIMARY_ROUTE
```

If multiple alternates exist, use explicit priority/order.

## Navigation boundary

Behavior layer chooses:
```
MOVE_TO(ARCHIVE_CHECKPOINT_A)
```

Godot navigation/path following computes a physical path.

Navigation may handle local movement, but it must not silently choose a different semantic goal.

If avoidance/pathfinding causes an actor to enter a gameplay-significant area unexpectedly, constrain the space or use authored waypoints.

## Semantic locations

Important positions are stable IDs, not raw vectors.

Examples:
- `LOC_B2_SECURITY_DESK`
- `LOC_B2_STAFF_DOOR_OUTSIDE`
- `LOC_B2_ARCHIVE_INSIDE`

Godot maps each LocationId to a transform/navigation target.

This permits:
- tests without Godot,
- deterministic saves,
- route inspection,
- easier level refactoring.

## Action execution

An intent that changes a correctable gameplay state follows:

```
Intent chosen
→ actor navigates/animates into interaction
→ application validates action
→ EventRecorder commits RecordedEvent
→ semantic state resolves
→ presentation finishes/updates
```

Animation is not the authority for whether the event occurred.

Use a controlled commit point in the action sequence.

## Commit point

Every state-changing NPC action needs a defined commit point.

Examples:
- door close: latch reaches closed state,
- badge take: badge transfers to possession anchor,
- scanner use: scan acceptance occurs,
- switch: relay activation point.

If animation is interrupted before commit, no RecordedEvent is created.

If interrupted after commit, the event remains history unless another event changes the state.

## Actor memory update

Memory updates must also have explicit timing.

Example:
```
Guard PLACE badge in locker
→ event commits
→ actor memory records "badge placed in locker"
```

If the player later suppresses the Place event:
- semantic world says badge is not in locker,
- guard memory still says it was placed there.

That contradiction can trigger a reaction when the guard checks.

## Observation

NPCs do not magically know every world state.

Reaction conditions declare observation requirements where relevant.

Possible observation mechanisms:
- direct interaction,
- semantic proximity/zone,
- assigned system notification,
- facility alarm,
- authored line-of-sight check.

Do not overbuild a universal perception simulation before required.

The important property is that the player can understand why the NPC learned something.

## Example: Missing Badge

Initial routine:
```
Guard takes Badge
Guard scans Door
Guard enters area
Guard later checks locker
```

Player suppresses TakeBadge after scan consequence persists.

State:
```
Badge = DESK
Door = UNLOCKED
Guard memory = "I took badge"
```

At authored badge check:
```
expected possession != current possession
→ MissingBadge reaction
→ Guard searches desk
```

The reaction is triggered by a defined check, not by omniscient AI instantly noticing the correction.

## Example: Route Redirect

Routine goal:
```
MOVE_TO ARCHIVE
```

Decision:
```
if STAFF_DOOR.LOCK == UNLOCKED:
    route through STAFF_DOOR
else:
    route through CORRIDOR_LEFT
```

Player suppresses a lock event before the guard reaches the branch point.

At branch evaluation, guard chooses staff door.

No past path is recomputed.

## Player blocking

Do not make physical body-blocking NPCs a primary puzzle verb.

If the player stands in a route:
- NPC may wait briefly,
- use a small local avoidance behavior,
- or the player collision configuration may prevent exploitative blocking.

Do not allow player collision to replace correction mechanics.

## Failure recovery

NPC systems must support deterministic reset to the puzzle snapshot:
- routine step,
- semantic location,
- relevant memory,
- active reaction,
- timers,
- authored trigger state.

Do not attempt to restore arbitrary animation-frame state.

Reset to stable authored presentation states.

## Debug panel requirements

For selected NPC display:
- ActorId,
- current semantic location,
- routine plan and step,
- active reaction,
- reaction priority,
- reason/condition values,
- relevant memory entries,
- current intent,
- target LocationId,
- last recorded events by actor.

This panel is development-only.

## Test requirements

Core behavior tests must cover:
- same inputs choose same reaction,
- priority order is stable,
- routine resumes after reaction according to policy,
- correction does not rewrite memory,
- route choice responds only at defined evaluation points,
- no random branch selection,
- reaction debouncing prevents duplicate loops,
- restore reproduces actor semantic state and next decision.

## Scope rule

If an NPC needs a new special behavior for one puzzle, first attempt to express it with:
- existing state conditions,
- RoutinePlan,
- ReactionRule,
- semantic locations,
- existing intents.

Do not add general AI complexity because one room is awkward to author.
