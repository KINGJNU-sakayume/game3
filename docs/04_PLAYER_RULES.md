# UNDO — Player Rules

## Direct capabilities

The player may:
- walk,
- rotate view,
- observe,
- inspect readable environmental information,
- enter Event Review on an eligible target,
- choose a recorded event,
- suppress it,
- release a maintained suppression,
- use system menus.

## Direct capabilities intentionally excluded

The player does not directly:
- open or close gameplay doors,
- press environmental buttons,
- operate levers,
- pick up puzzle objects,
- carry inventory,
- push physics crates,
- command NPCs,
- fight,
- jump through traversal challenges.

Exceptions for non-gameplay readability interactions, such as viewing a document, do not create general world-manipulation verbs.

## Design consequence

Every progression puzzle must therefore be solvable through:
- waiting for or inducing another actor's action,
- suppressing recorded state changes,
- releasing suppressions,
- positioning the player to observe or pass through a temporarily available route.

## Failure

The default failure state is not death.

A puzzle can become unfavorable because:
- an NPC took an unwanted route,
- a door returned to an inaccessible state,
- the player released a correction too early,
- a needed event occurred under the wrong conditions.

The player may restore the current puzzle-space snapshot.

## Restart terminology

System-menu action:

**RESTORE CURRENT RECORD**

This is a retry mechanism, not an in-world Undo ability.

## Timing

Avoid sub-second execution tests.

If a solution requires timing, the meaningful window should normally be several seconds and the cognitive challenge should remain the principal difficulty.

## Tutorial policy

Instruction priority:

1. level design,
2. animation and sound,
3. context,
4. minimal input prompt,
5. explanatory tutorial text.

Text is the fallback, not the default.

## Hint policy

Hints should be opt-in rather than forced.

Preferred hint escalation:
1. make a relevant residual trace easier to notice,
2. emphasize a useful recurring action/sound,
3. highlight a relevant event/time within Review Mode,
4. only then provide more explicit wording.

Never have an NPC simply state the puzzle solution.
