# UNDO — Puzzle Authoring Template

Last locked: 2026-09-25

## Purpose

Every major puzzle should be designed and reviewed through the same causal worksheet before detailed Godot implementation.

The template exists to prevent:
- arbitrary solutions,
- hidden event chains,
- bespoke code exceptions,
- unreadable spaces,
- narrative dressing from obscuring mechanics.

## Required puzzle header

```md
# <PuzzleId> — <Working Name>

Status:
Act / placement:
Primary function: Teach / Test / Combine / Twist
Target first-play duration:
Correction capacity:
Primary puzzle family:
Secondary puzzle family:
Narrative beat:
```

## 1. Player-facing objective

Describe what the player believes they are trying to accomplish.

Example:

> Reach the staff archive beyond Door A.

Do not phrase this as the hidden solution.

Bad:

> Undo the guard taking the badge after he scans it.

That is an implementation solution, not the player's objective.

## 2. Initial semantic state

List only gameplay-relevant initial facts.

Example:

```
BADGE_01.POSITION = LOC_SECURITY_DESK
BADGE_01.POSSESSION = NONE

STAFF_DOOR_A.LOCK = LOCKED
STAFF_DOOR_A.OPEN = false

GUARD_01.LOCATION = LOC_SECURITY_DESK
```

If the list is enormous, reconsider puzzle scope.

## 3. Actors

For each actor:

```
ActorId:
Role:
Initial location:
Routine summary:
Relevant memory:
Relevant reactions:
What the player can observe:
```

Do not write personality biography here unless it affects the puzzle.

## 4. Normal routine — no player intervention

Write the full causal chain.

Example:

```
R1 Guard reaches desk
R2 Guard TAKE badge          -> E1
R3 Guard reaches scanner
R4 Guard AUTHORIZE scanner   -> E2
R5 System UNLOCK door        -> E3
R6 Guard passes door
R7 Door closes
```

A puzzle is not ready if the designer cannot explain the normal routine clearly.

## 5. Recorded events

Table:

| Event | Actor | Verb | Target | Mutation | Correctable? |
|---|---|---|---|---|---|
| E1 | Guard | TAKE | Badge | possession/position | yes |
| E2 | Guard | AUTHORIZE | Scanner | authorization | yes/no per design |
| E3 | System | UNLOCK | Door | lock | yes/no |

Use conceptual IDs during design; runtime IDs may be generated.

## 6. Intended causal insight

One sentence.

Example:

> Once the scan and unlock are recorded independently, suppressing the earlier badge take returns the badge without removing the unlocked door.

If this sentence is not interesting, the puzzle may not justify itself.

## 7. Intended solution chain

Write sequentially:

```
Observe E1 → E2 → E3
Wait until E3 has committed
Suppress E1
Badge resolves to desk
E2/E3 persist
Archivist later sees/uses returned badge
New event E4 occurs
Player exploits E4
```

Do not rely on unrecorded designer assumptions.

## 8. Why alternative obvious actions fail

List only important alternatives that players are likely to try.

Example:

- Suppress E1 before E2 → scanner never receives a valid badge action in the authored sequence.
- Release E1 too early → badge state returns before the desired actor check.
- Suppressing DoorClose alone opens route temporarily but does not create the later needed reaction.

Do not deliberately add fake red herrings just to fill this section.

## 9. Reaction boundaries

For every NPC/system reaction state:

```
Trigger:
Observation point:
Priority:
Reaction:
New event(s):
Repeat policy:
```

Example:

```
Trigger:
badge expected in locker but actual state differs

Observation:
Guard reaches LOC_BADGE_CHECK

Reaction:
RX_GUARD_MISSING_BADGE

New event:
Guard OPEN maintenance office door

Repeat:
once per missing-badge trigger instance
```

If the actor knows something without an observation channel, fix it.

## 10. Observation plan

Document how the player can learn the chain.

### Observation anchors
- Anchor A:
- Anchor B:

### Sightlines
- First introduction:
- Advanced/off-screen relationship:

### Audio cues
- door:
- scanner:
- footsteps:
- machine:

### Review history
- which target/history must be inspectable?

Every critical causal fact must have a readable path.

## 11. Spatial map

A simple ASCII/block diagram is sufficient before graybox.

Example:

```
[Security Desk]
     |
     | sightline
     v
[Scanner] -- [Door A] -- [Staff Archive]
     |
[Observation Recess]
```

Do not begin high-fidelity environment art without a usable causal map.

## 12. Timing

Record:
- routine cycle length,
- time to first meaningful event,
- solution timing window,
- wait after correct insight,
- reset duration.

If exact solution is understood but requires repeated execution failure, redesign timing.

## 13. Correction capacity use

List maintained corrections over the solution timeline.

Example:

```
T0: none
T1: suppress E1        [1/1]
T2: desired E4 occurs [1/1]
T3: release E1        [0/1]
```

For multi-slot levels:

```
T1: E1 [1/2]
T2: E1 + E7 [2/2]
T3: release E1, keep E7 [1/2]
```

This makes slot logic auditable.

## 14. Completion facts

Define factual semantic completion.

Example:

```
PLAYER at LOC_EXIT
AND STAFF_DOOR_A.LOCK == UNLOCKED
```

Avoid opaque level scripts calling `CompleteLevel()` from arbitrary animation callbacks.

## 15. Reset snapshot

Specify stable reset:
- player anchor,
- actor locations,
- actor memory,
- routine step,
- initial semantic state,
- machine state,
- important local narrative state.

Reset must not depend on reversing live animations.

## 16. Failure / softlock analysis

Ask:
- Can the player consume the only useful event permanently?
- Can an NPC become trapped outside authored paths?
- Can capacity become stuck?
- Can player collision block the routine?
- Can a correction make an entity impossible to bind/restore?
- Can the player miss required information forever?

If yes, define recovery or redesign.

## 17. Tutorial burden

List what the player must already know.

Example:

```
Known:
- basic suppression,
- Review,
- consequence persistence.

New:
- NPC notices contradiction only at check point.
```

One puzzle should not teach three unrelated concepts at once without exceptional justification.

## 18. Narrative integration

Specify:
- required narrative information,
- optional environmental context,
- whether narrative competes with puzzle attention.

Do not place a crucial emotional audio line during the moment the player must parse a new causal mechanic.

## 19. Art readability requirements

List:
- gameplay object silhouettes,
- state differences,
- required sign/label,
- light/sightline requirements,
- clutter exclusions.

Art must serve these constraints.

## 20. Audio readability requirements

List critical functional cues and whether each has a visual alternative.

No puzzle may require directional hearing alone.

## 21. Accessibility risks

Consider:
- color dependency,
- small text,
- motion,
- timing,
- hearing dependency,
- hold input,
- cognitive overload from simultaneous traces.

## 22. Required tests

List domain/content/integration tests.

Example:
- suppressing E1 after E3 preserves E3,
- resetting restores badge/door/guard memory,
- missing-badge reaction fires only at check point,
- duplicate trigger does not fire twice.

## 23. Debug requirements

Specify what needs to be inspectable:
- key StateKeys,
- relevant event IDs,
- actor memory,
- reaction reason,
- current routine step.

## 24. Playtest questions

Write 2–5 questions tied to the puzzle's intended mental model.

Example:
- Why did the door stay unlocked?
- What would happen if you suppressed the badge event earlier?
- When did the guard notice the badge was missing?

## 25. Greenlight checklist

A puzzle may advance from design to graybox when:

- [ ] normal routine is explicit,
- [ ] state/events are representable with canonical systems,
- [ ] intended insight is unique enough to justify the puzzle,
- [ ] no new one-off system is secretly required,
- [ ] observation plan exists,
- [ ] reaction knowledge is plausible,
- [ ] timing is reasoning-first,
- [ ] reset is deterministic,
- [ ] completion condition is semantic,
- [ ] relevant tests can be named.

A graybox may advance to art when:

- [ ] first-time players can observe the causal chain,
- [ ] intended solution works repeatedly,
- [ ] reset works,
- [ ] no random behavior affects solution,
- [ ] waiting is acceptable,
- [ ] debug tools explain every state change,
- [ ] playtest confusion is not caused by geometry/readability.

## Authoring principle

> If a puzzle cannot be described clearly in this template, it is not ready to be implemented.
