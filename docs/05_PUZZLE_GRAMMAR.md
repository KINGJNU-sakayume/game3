# UNDO — Puzzle Grammar

## Base grammar

Core causal structure:

```
ACTOR
↓
ACTION
↓
STATE CHANGE
↓
REACTION
```

The player intervenes by suppressing an eligible state-changing event or later releasing that suppression.

## Object families

### Barrier
Doors, shutters, gates  
Channels: OPEN, LOCK

### Trigger
Buttons, proximity sensors, optical sensors  
Channels: SIGNAL

### Machine
Elevators, conveyors, restoration equipment  
Channels: POWER, MOTION, POSITION

### Resource
Electricity, water, air/pressure where appropriate  
Channels: POWER, FLOW

### Portable
Badges, keys, documents, archive boxes  
Channels: POSITION, POSSESSION

### Security
CCTV, scanners, alarm systems  
Channels: SIGNAL, ALERT, AUTHORIZATION

### Agent
Employees, guards, maintenance staff  
Uses position, intent, routine, and reaction rules. Position is not a normal directly suppressible channel.

### Container
Lockers, cabinets, archive drawers  
Channels: OPEN, LOCK

### Environment
Lights, ventilation, building systems  
Channels: POWER, FLOW

### Hazard
Electrical, mechanical, environmental hazards  
Channels: ACTIVE/related declared channels

## Core event verbs

Canonical initial vocabulary:
- OPEN / CLOSE
- LOCK / UNLOCK
- TAKE / PLACE
- ENTER / EXIT (recorded for logic, not normally directly suppressible as teleportation)
- START / STOP
- POWER / UNPOWER
- DETECT
- AUTHORIZE / DENY
- ACTIVATE / DEACTIVATE
- RAISE / LOWER
- BREAK / REPAIR
- SEND / RECEIVE
- CONNECT / DISCONNECT

Prefer reuse and combination over adding bespoke verbs.

## Puzzle families

### 1. Negation
Suppress one direct state change.

### 2. Exploitation
Suppress a state change so another actor uses the altered state.

### 3. Redirect
Alter a route condition so an NPC chooses a different deterministic path.

### 4. Induced Action
Redirect an actor to create a new event needed by the player.

### 5. Persistence Exploit
Suppress a cause after its useful consequences have already become independent events.

### 6. Release Timing
Maintain a correction until an actor crosses or a system reaches a state, then release it.

### 7. Slot Competition
Use one suppression to create a persistent opportunity, release it, then reuse capacity elsewhere.

### 8. State Preparation
Use suppression duration to move or position a machine/object into a useful state.

### 9. Reaction Chain
Create a contradiction that causes an NPC or system to detect a problem and perform a new routine.

### 10. Causal Fork
The same correction produces a different reaction depending on the current world state or nearby actor.

### 11. Masked Event
Suppress an older state mutation whose effect is currently covered by a newer event, then expose it through later suppression.

### 12. Paradox Construction
Create a multi-channel world state impossible through normal causal flow.

## NPC rules

NPC logic is:
- routine,
- observable conditions,
- explicit reactions.

Examples:
- IF alarm → inspect alarm,
- IF missing badge → search,
- IF route blocked → use defined alternate route,
- IF restricted door unexpectedly open → investigate/close,
- IF equipment abnormal → maintenance inspection.

Avoid random decision percentages.

## Puzzle quality test

A valid solution should feel inevitable in hindsight.

Reject puzzles based on:
- arbitrary exceptions,
- hidden randomness,
- pixel hunting,
- off-screen untelegraphed failure,
- unexplained new rules,
- developer-specific lateral guesses,
- reflex windows replacing reasoning.
