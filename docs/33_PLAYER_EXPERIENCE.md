# UNDO — Player Movement, Camera, and Input Experience

Last locked: 2026-09-25

## Goal

Movement should disappear from the player's attention.

UNDO is about observing causal systems, not mastering first-person traversal.

The controller/camera must therefore feel:
- immediate,
- comfortable,
- predictable,
- low-friction,
- non-athletic.

## Locomotion verbs

Canonical core locomotion:
- move,
- look.

Not part of the core game:
- jump,
- crouch,
- mantle,
- slide,
- stamina,
- parkour,
- combat dodge.

Do not add a traversal verb because one level is awkward to block out.

Fix the level instead.

## Movement speed

Exact numeric tuning is prototype-dependent.

Design target:
- faster than literal real-world indoor walking,
- slower than action-FPS sprinting,
- comfortable for repeated archive corridors,
- precise enough around doors/desks/observation anchors.

Baseline tuning range for Prototype A:
- approximately 3.2–4.2 meters/second.

This range is a prototype starting point, not immutable release tuning.

## Sprint

No stamina/sprint system is planned.

Do not add a conventional Shift-to-sprint mechanic during early prototypes.

If full-campaign traversal later proves too slow:
1. shorten traversal,
2. improve spatial routing,
3. adjust baseline movement speed,
4. only then consider an optional fast-walk input.

Puzzle timing must never require a sprint mechanic.

## Player collision

Player collision exists for believable navigation.

It must not become a puzzle tool.

Prevent or mitigate exploits where the player:
- body-blocks NPCs,
- wedges doors,
- pushes actors,
- alters deterministic routes through collision abuse.

Possible implementation methods:
- NPC/player collision layering,
- local NPC avoidance,
- short wait/repath behavior,
- authored route clearance.

Choose the simplest solution that preserves predictable puzzles.

## Camera

First-person camera.

Desired feel:
- no weapon-camera language,
- no aggressive camera inertia,
- no cinematic auto-look,
- no forced camera grabs during puzzle solutions.

Player retains camera control during normal gameplay.

## Field of view

Expose FOV in settings.

Prototype baseline may begin around a conventional first-person value, then tune through motion-comfort testing.

Do not hard-lock a narrow cinematic FOV.

Any scripted Review/cut transition must respect accessibility concerns.

## Head bob

Head bob is:
- subtle if enabled,
- never required for feedback,
- independently disableable.

Do not encode footsteps or movement state only through camera bob.

## Motion blur

Motion blur must be disableable.

It should not obscure:
- Residual Exposure,
- NPC action commits,
- event-state comparison.

Default strength should remain restrained if used.

## Camera shake

No general-purpose dramatic camera shake.

Use only where a physical event genuinely requires it, at low intensity, with accessibility reduction/disable support.

Correction itself does not shake the camera.

## Core input actions

Gameplay code binds to named actions, never hardcoded physical keys.

Canonical action names:

```
move_forward
move_back
move_left
move_right
review
history_prev
history_next
correction_commit
correction_release
inspect
pause
```

Optional future action:
`fast_walk` only if playtesting justifies it.

## Keyboard/mouse baseline

Initial prototype mapping recommendation:

- Move — WASD
- Look — Mouse
- Review — Right Mouse Button (hold by default)
- Previous/next recorded event — Mouse Wheel / alternate keyboard bindings
- Correction commit — Left Mouse Button while in Review
- Correction release — explicit contextual input while reviewing a maintained correction
- Inspect document/terminal focus — E
- Pause — Escape

These are baseline defaults, not immutable canon.

Full remapping is a product requirement.

## Why Review uses a mode

Normal view:
- observe the world.

Review held/toggled:
- focus on one eligible object's recorded history.

This reduces accidental corrections and preserves the Notice → Inspect → Commit hierarchy.

## Review activation

Review only becomes meaningful when:
- a valid target is within authored interaction distance/raycast criteria,
- target has inspectable recorded state.

Do not snap the player's aim to targets.

Do not auto-select a distant object through walls.

## Review time behavior

Entering Review:
- short controlled slowdown,
- transition to pause for cognitive selection.

Exact transition target:
- roughly 0.2–0.4 seconds in the current design direction.

Accessibility option:
- instant pause / reduced temporal transition.

Review pause does not imply Jaemin literally freezes time in the fiction.

## History navigation

When one object has multiple relevant events:

- `history_prev`
- `history_next`

cycle only that target's reviewable history.

Do not scroll a global facility timeline.

Ordering is chronological/sequence based and visually clear.

## Commit safety

Correction commit should require:
- active Review,
- valid selected event,
- eligibility,
- available capacity or valid release/replacement flow.

A stray click during normal movement cannot alter causal state.

## Release UX

Release is intentionally distinct from Commit.

The player must understand:
- Commit suppresses a recorded mutation.
- Release reactivates that recorded mutation's contribution now.

Avoid one ambiguous button that silently toggles behavior before the player learns the distinction.

Exact physical binding may be adjusted after Prototype A.

## Inspect

`inspect` is reserved for non-causal information interactions such as:
- lifting/reading a paper,
- focusing a terminal,
- reading a mounted notice.

It does not mean:
- open world door,
- take badge,
- press gameplay switch.

This preserves the indirect-agency rule.

## Documents and terminals

Entering document/terminal reading focus may:
- reduce movement,
- capture cursor/controller focus,
- expose back/zoom/navigation input.

Exiting returns immediately to normal first-person control.

Do not build a generic inventory screen for collected documents.

## Interaction distance

Keep actionable Review targets close enough to maintain physical legibility.

Do not allow correcting objects across large rooms simply because they are visible.

Exact meters are prototype-tuned by object size/scene scale.

## Reticle

Default:
- tiny neutral dot or similarly minimal reticle.

Correction eligibility should not turn it into a large videogame icon.

Subtle opacity/state change is acceptable.

Reticle can be hideable only if usability remains sufficient.

## Controller architecture

Controller is not required to block Prototype A implementation, but input abstraction must support it.

Intended conceptual mapping:
- left stick — move,
- right stick — look,
- left trigger — Review,
- shoulder/d-pad — history navigation,
- right trigger / face button — Commit,
- distinct face/shoulder action — Release,
- face button — Inspect,
- Menu — Pause.

Final mapping requires controller playtesting.

## Hold versus toggle

Review defaults to hold in the baseline design.

Accessibility setting must allow toggle behavior.

Do not duplicate rule logic; the setting changes input state management only.

## Input rebinding

By vertical slice, player-facing gameplay inputs should support rebinding.

At minimum:
- movement,
- Review,
- history navigation,
- Commit,
- Release,
- Inspect.

Avoid reserved hardcoded assumptions inside gameplay scripts.

## First five minutes

The opening must establish controls without a conventional tutorial panel sequence.

Desired order:

1. player gains movement/look in office,
2. walks naturally toward exit,
3. direct door interaction does nothing because it is not a player verb,
4. NPC performs a recorded close,
5. Residual Exposure appears,
6. minimal Review input cue appears,
7. player reviews and suppresses,
8. world responds.

Only introduce history cycling when multiple events become relevant.

Only introduce Release when Level 07 / designated teaching puzzle requires it, except minimal Prototype A testing.

## No stealth language

The game may involve guards and restricted areas, but input/presentation must not suggest a stealth-action game.

No:
- crouch meter,
- detection cone HUD,
- takedown,
- lean-peek system,
- hiding bodies,
- noise meter.

NPC restrictions exist as causal systems, not combat/stealth opponents.

## No player health

There is no persistent health HUD/system in the canonical design.

Hazards are level-state constraints, not a combat-survival layer.

If a rare dangerous state requires failure, use clear authored consequence/reset rather than adding health points.

## Accessibility requirements

Player controls must support:
- full key rebinding,
- mouse sensitivity,
- invert Y option,
- FOV,
- head bob off,
- motion blur off,
- camera shake reduction/off,
- Review hold/toggle,
- Correction visibility strength,
- subtitle size.

Additional options may be added after testing.

## Prototype A control test

Prototype A must answer:

- Can a player move/aim comfortably within minutes?
- Does Review feel distinct from direct interaction?
- Can they Commit without accidental activation?
- Can they understand Release when demonstrated?
- Does pausing Review create motion discomfort?
- Does minimal HUD remain usable?

Do not polish animation before these controls are comfortable.

## Rule

If the first-person controller becomes an interesting gameplay system by itself, it is probably too complicated for UNDO.
