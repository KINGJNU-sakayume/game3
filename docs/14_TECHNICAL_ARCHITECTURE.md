# UNDO — Technical Architecture

Last locked: 2026-09-25

## Engine decision

**Engine:** Godot 4.7.2 stable, .NET build  
**Primary language:** C#  
**Initial shipping target:** Windows desktop  
**Renderer target:** Forward+ unless prototype profiling justifies a change

Do not begin production on Godot 4.8 development builds. Engine upgrades require an explicit repository decision after prototype/vertical-slice stability is established.

## Why Godot for UNDO

UNDO does not need the maximum rendering or open-world feature ceiling. Its technical risk is causal-state correctness, deterministic NPC behavior, iteration speed, and maintaining a repository that coding agents can reason about.

Godot is chosen because:
- scenes/resources can remain mostly text-based and version-control friendly,
- C#/.NET allows a testable domain layer independent of engine presentation,
- the engine is light enough for rapid graybox iteration,
- first-person 3D, animation, audio, navigation, UI, localization, and desktop export cover the project's needs,
- the MIT-licensed engine avoids engine revenue-tier decisions affecting architecture,
- headless execution supports automated validation workflows.

## Why not Unity as the default

Unity remains technically capable and has mature first-party testing and 3D tooling.

It is not the default because UNDO benefits more from:
- a smaller project/editor surface,
- highly readable repository artifacts,
- less framework/editor ceremony,
- keeping the core simulation in a compact architecture.

Reconsider Unity only if the vertical slice exposes a concrete Godot blocker in rendering, tooling, animation, middleware, or production workflow.

## Why not Unreal as the default

Unreal provides the highest visual-production ceiling of the three candidates, but UNDO does not currently justify:
- heavier project/editor complexity,
- C++/Blueprint split,
- greater binary-asset dependence,
- slower iteration for a small causal-puzzle project.

Reconsider Unreal only if visual fidelity or production needs become impossible to meet with the chosen stack. Do not switch engines for aesthetic ambition alone.

## Architecture principle

> The causal simulation must not depend on Godot scene state as its source of truth.

Godot scenes present and animate the world. A separate deterministic domain layer owns gameplay-relevant causal state.

This is required so:
- correction logic can be unit tested,
- puzzle state can be reproduced,
- NPC decisions can be inspected,
- restore/save can be deterministic,
- Codex can modify logic without reverse-engineering scene files,
- visual/physics changes do not silently change puzzle rules.

## Three-layer architecture

### 1. Domain layer — `Undo.Core`

Pure C# where practical. No dependency on Godot types in core causal logic.

Owns:
- IDs,
- gameplay state values,
- state channels,
- event records,
- event ledger,
- event activation/suppression state,
- effective-state resolution,
- correction capacity,
- causal query helpers,
- deterministic reaction evaluation,
- puzzle snapshot data contracts.

The core layer must be executable in ordinary .NET tests without launching the Godot editor for most logic tests.

### 2. Game/application layer — `Undo.Game`

Godot-facing C# services and nodes.

Owns:
- binding scene objects to domain entity IDs,
- translating engine interactions into domain events,
- applying effective domain state to scene presentation,
- NPC navigation execution,
- Review Mode,
- correction visual/audio feedback,
- level orchestration,
- save/restore integration,
- localization/UI wiring.

### 3. Content/presentation layer

Godot scenes/resources and external assets.

Owns:
- level geometry,
- object placement,
- animation,
- materials,
- lights,
- audio emitters,
- navigation meshes,
- documents/signage presentation,
- NPC rigs,
- cameras.

Content must not invent gameplay rules that are absent from the domain/content definitions.

## Repository target structure

```
/
├─ AGENTS.md
├─ README.md
├─ docs/
│
├─ game/
│  ├─ project.godot
│  ├─ scenes/
│  │  ├─ bootstrap/
│  │  ├─ player/
│  │  ├─ actors/
│  │  ├─ interactables/
│  │  ├─ levels/
│  │  └─ ui/
│  ├─ resources/
│  │  ├─ events/
│  │  ├─ actors/
│  │  ├─ levels/
│  │  └─ localization/
│  ├─ scripts/
│  │  ├─ application/
│  │  ├─ presentation/
│  │  ├─ input/
│  │  └─ debug/
│  ├─ audio/
│  ├─ art/
│  └─ localization/
│
├─ src/
│  └─ Undo.Core/
│
├─ tests/
│  └─ Undo.Core.Tests/
│
└─ tools/
```

Exact generated project files may adjust this layout, but the separation between `Undo.Core` and the Godot presentation/application project is canonical.

## Semantic state, not raw transforms

Gameplay state must be semantic.

Example:

Bad source-of-truth state:
```
Badge.position = Vector3(12.83, 1.04, -7.20)
```

Preferred:
```
Badge.POSSESSION = GUARD_01
```

or:
```
Badge.POSITION = SECURITY_DESK_SLOT_A
```

The presentation layer then maps the semantic state to a Godot transform/attachment.

This prevents tiny physics differences from changing puzzle logic and makes state restoration deterministic.

## Stable entity identity

Every gameplay-relevant entity receives a stable, human-readable ID.

Examples:
- `DOOR_B2_014_A`
- `GUARD_B2_01`
- `BADGE_STAFF_03`
- `SCANNER_B2_TRANSFER_01`

Do not use transient scene-tree paths or runtime instance IDs as canonical gameplay identity.

IDs must remain stable across:
- save/load,
- scene refactors,
- test fixtures,
- content debugging.

## Service boundaries

Expected core/application services:

### EventLedger
Append-only chronology of recorded gameplay events for the active puzzle scope.

### WorldStateStore
Stores initial semantic states and exposes current effective values.

### EffectiveStateResolver
Resolves a channel from initial state plus active event contributions.

### CorrectionManager
Owns maintained suppressions, capacity, commit/release rules, and correction validity.

### EventRecorder
Validates and creates new recorded events when actors/machines perform qualifying state changes.

### ActorRoutineController
Chooses deterministic routine goals from authored routine data.

### ReactionEvaluator
Evaluates explicit conditions against current world state and actor memory.

### PresentationBinder
Applies semantic state to Godot nodes/animations.

### PuzzleSession
Owns current level scope, snapshot, reset, completion conditions, and debugging state.

### SaveCoordinator
Serializes campaign state and stable puzzle checkpoint state.

### ReviewController
Queries event history for the targeted entity and controls Review Mode.

## Event-driven flow

Normal event:

```
Actor intent
→ application action
→ EventRecorder validates state mutation
→ EventLedger append
→ EffectiveStateResolver updates
→ PresentationBinder animates/applies world
→ ReactionEvaluator observes new current state
```

Correction:

```
Player selects event
→ CorrectionManager validates eligibility/capacity
→ selected event contribution becomes suppressed
→ affected channel is re-resolved
→ PresentationBinder applies corrected state
→ reactions evaluate current state
→ any resulting actor action creates NEW events
```

Release:

```
Player releases correction
→ event contribution becomes active
→ affected channel re-resolves
→ original actor action is NOT replayed
→ presentation moves to restored effective state
→ reactions evaluate current state
```

## Physics policy

Do not build a generic physics rewind system.

Physics may be used for:
- character collision,
- doors/animations where controlled,
- raycasts,
- navigation,
- cosmetic secondary motion.

Puzzle-critical object state should not depend on uncontrolled Rigidbody/RigidBody simulation.

Where a moving object is puzzle-relevant, its meaningful destinations/states must be deterministic and representable semantically.

## Navigation policy

NPC pathfinding executes an already chosen deterministic goal. Navigation is not the decision-maker.

Decision:
```
Current world state
→ routine/reaction rule chooses destination/action
```

Execution:
```
chosen destination
→ navigation/path following
```

If dynamic avoidance produces unacceptable unpredictability in puzzle spaces, prefer authored paths/checkpoints or constrained navigation over simulation realism.

## Signals/events in Godot

Godot signals may be used for application/presentation communication, but they are not substitutes for the domain EventLedger.

Distinguish:
- **Godot signal:** software notification,
- **Recorded Event:** gameplay object representing a state-changing occurrence.

Never conflate the two terms in code or documentation.

## Global singleton policy

Avoid turning every system into an Autoload.

A small composition/bootstrap layer may own long-lived services such as:
- campaign/save coordinator,
- global settings,
- localization,
- scene transition.

Puzzle-specific services should be scoped to the active level/session where possible.

## Data-driven content

Repeated content definitions should live in authored resources/data rather than hardcoded per-level logic.

Good candidates:
- entity IDs,
- initial state,
- event eligibility,
- NPC routines,
- reaction conditions,
- semantic anchors/locations,
- puzzle completion conditions.

Do not create a universal visual scripting language before prototypes prove the exact content-authoring needs.

## Debug tooling is a first-class requirement

Prototype/production tooling must eventually allow developers to inspect:
- current semantic state by entity/channel,
- event ledger,
- suppressed event IDs,
- remaining correction capacity,
- NPC current routine/reaction,
- reason a reaction fired,
- puzzle snapshot/reset status.

This debug UI is development-only and is explicitly exempt from the consumer UI aesthetic rules.

A causal puzzle game without strong state inspection will be unnecessarily difficult to debug.

## Logging

Structured debug logs should use stable entity/event IDs.

Example:
```
[EVENT] E_00421 actor=GUARD_B2_01 verb=TAKE target=BADGE_03
[STATE] BADGE_03.POSSESSION DESK_A -> GUARD_B2_01
[CORRECTION] suppress E_00421
[STATE] BADGE_03.POSSESSION GUARD_B2_01 -> DESK_A
[REACTION] GUARD_B2_01 MissingBadge triggered
```

Do not ship verbose causal debugging logs by default.

## Determinism boundary

Absolute deterministic rendering/physics is not required.

The following **is** required to be deterministic from the same puzzle snapshot and same player corrections:
- semantic state resolution,
- event eligibility,
- NPC rule selection,
- authored routine progression,
- puzzle success conditions.

## Initial platform scope

First production target:
- Windows desktop,
- keyboard/mouse,
- controller support planned but not required for Prototype A/B/C.

Do not expand to consoles/mobile/web during prototype development.

C# Godot web export is not part of the current target.

## Versioning policy

Lock prototypes and vertical slice to Godot 4.7.2 unless a critical fix requires a maintenance upgrade.

Do not upgrade the engine merely because a newer feature release appears.

Any engine change after content production begins requires:
- branch backup,
- import test,
- core tests,
- prototype regression tests,
- representative level load,
- documented decision.
