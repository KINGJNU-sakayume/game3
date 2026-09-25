# UNDO — Design Pillars

## Pillar 1 — Indirect agency

The player does not directly operate the environment.

Allowed player actions:
- walk,
- look,
- observe,
- inspect readable records,
- enter Event Review,
- suppress an eligible recorded event,
- release a maintained suppression.

The player does not directly:
- open doors,
- press world buttons,
- pull levers,
- pick up portable gameplay objects,
- move crates,
- command NPCs,
- attack,
- use an inventory.

The world is manipulated by changing the conditions under which other actors behave.

## Pillar 2 — Causal, not temporal

Undo never rewinds global time.

Suppressing an event affects only the state channel directly changed by that event. NPC positions, elapsed time, unrelated systems, and later independently recorded events do not rewind.

This rule must remain predictable across the entire game.

## Pillar 3 — Consequences persist

If event A causes event B, and B has already become its own recorded event, suppressing A does not automatically delete B.

Example:

```
Guard takes badge
→ Guard scans badge
→ Door unlocks
```

Suppressing "Guard takes badge" returns the badge to its prior state, but the scan and unlocked door persist.

This is the signature mechanic and must appear in major puzzles throughout the game.

## Pillar 4 — Predictable systems

NPC behavior is deterministic enough to learn.

NPC design is based on:
- routine,
- current state,
- explicit reaction rules.

Avoid random route selection or hidden probability. Unexpected behavior may occur only because the player has not yet learned the relevant condition.

## Pillar 5 — Observation before interface

The environment is the primary information surface.

Priority:
1. world,
2. NPC behavior,
3. state change,
4. Residual Exposure,
5. text.

Do not turn the game into a log-analysis dashboard.

## Pillar 6 — Rule combinations, not gimmick accumulation

Later difficulty comes primarily from combining known systems rather than introducing a new sci-fi device every level.

Core object families and verbs remain stable. Novelty should come from causal arrangement.

## Pillar 7 — Reasoning over dexterity

The challenge is understanding what to change, not executing a 500 ms input window.

Timing puzzles may exist, but their windows should be generous enough that recognition and planning remain the challenge.

## Pillar 8 — Institutional realism

The setting should feel like a maintained but aging Korean public-record facility, not a futuristic laboratory.

The visual language derives from:
- administrative documents,
- archive labels,
- industrial equipment,
- old office software,
- fluorescent-lit workspaces,
- accumulated renovations across decades.

## Pillar 9 — Restraint

Avoid:
- exposition dumps,
- melodramatic monologues,
- jump scares,
- generic glitch aesthetics,
- morality meters,
- explicit "acceptance/forgiveness" messaging,
- good/bad/true ending labels.

Plot facts should be understandable. Their meaning should remain open to interpretation.
