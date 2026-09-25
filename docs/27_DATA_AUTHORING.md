# UNDO — Data Authoring Standard

Last locked: 2026-09-25

## Decision

Gameplay-causal content is authored in **engine-neutral JSON** under the Godot project so it can be loaded at runtime and tested by `Undo.Core`.

Godot scenes/resources remain responsible for placement and presentation.

Player-facing text is referenced through localization keys rather than embedded as final prose in causal JSON.

## Format ownership

### C# code owns universal rules

C# defines:
- stable ID value types,
- StateChannel definitions and allowed value types,
- EventVerb vocabulary,
- state resolution,
- correction rules,
- routine/reaction interpreter for the deliberately small authored vocabulary,
- save DTOs,
- validation.

Do not put campaign-specific puzzle solutions into C#.

### JSON owns semantic gameplay content

JSON defines:
- PuzzleId,
- entity instances,
- semantic locations used by behavior,
- initial gameplay state,
- NPC routine steps,
- ReactionRules,
- minimal initial ActorMemory,
- puzzle completion facts,
- content references and narrative IDs where required.

### `.tscn` owns scene composition

Godot scenes define:
- geometry,
- gameplay-object placement,
- collision,
- navigation,
- lights,
- cameras,
- presentation nodes,
- EntityId binders,
- LocationId anchors,
- animation/audio bindings.

Scene state is not causal truth.

### `.tres` owns reusable Godot presentation configuration

Use text Resources for editor-friendly presentation data such as:
- Residual Exposure tuning,
- reusable audio/presentation profiles,
- material/presentation settings,
- non-causal UI presentation configuration.

Do not move core causal state into `.tres` merely because the Inspector is convenient.

## Canonical directory target

```
game/
├─ content/
│  ├─ puzzles/
│  │  ├─ PT_A_BASIC_CORRECTION/
│  │  │  └─ puzzle.json
│  │  ├─ PT_B_PERSISTENCE/
│  │  │  └─ puzzle.json
│  │  └─ ...
│  └─ manifests/
│     ├─ puzzles.json
│     └─ narrative.json
│
├─ scenes/
│  └─ levels/
│     └─ PT_A_BASIC_CORRECTION.tscn
│
├─ resources/
│  └─ presentation/
│
└─ localization/
```

Keep a puzzle's semantic definition as self-contained as practical.

Do not fragment one puzzle across many tiny data files before repeated authoring proves that reuse is necessary.

## JSON rules

- UTF-8.
- No comments inside JSON.
- Two-space indentation.
- Stable property naming in lower camelCase.
- Stable IDs in upper snake case.
- Every root content file has `schemaVersion`.
- Every puzzle file has `puzzleId`.
- Array ordering is meaningful only where explicitly defined.
- Dictionary/object key ordering must never affect gameplay.
- Unknown required IDs fail validation.
- Unknown properties should be rejected in strict development validation once the schema stabilizes.

## Puzzle document — conceptual shape

```json
{
  "schemaVersion": 1,
  "puzzleId": "PT_B_PERSISTENCE",
  "displayNameKey": "PUZZLE.PT_B_PERSISTENCE.NAME",
  "scenePath": "res://scenes/levels/PT_B_PERSISTENCE.tscn",
  "correctionCapacity": 1,

  "locations": [
    "LOC_SECURITY_DESK",
    "LOC_SCANNER_FRONT",
    "LOC_STAFF_DOOR_INSIDE"
  ],

  "initialState": {
    "BADGE_01": {
      "POSSESSION": "NONE",
      "POSITION": "LOC_SECURITY_DESK"
    },
    "SCANNER_01": {
      "AUTHORIZATION": "DENIED"
    },
    "STAFF_DOOR_A": {
      "LOCK": "LOCKED",
      "OPEN": false
    }
  },

  "actors": [
    {
      "actorId": "GUARD_01",
      "initialLocation": "LOC_SECURITY_DESK",
      "initialMemory": {},
      "routine": []
    }
  ],

  "completion": {
    "all": []
  }
}
```

This is a structural example, not permission to add arbitrary new properties without updating the validator/spec.

## State values

Use simple JSON primitives/IDs where possible.

Examples:

```json
{
  "DOOR_A": {
    "OPEN": true,
    "LOCK": "UNLOCKED"
  },
  "BADGE_01": {
    "POSITION": "LOC_DESK_A",
    "POSSESSION": "GUARD_01"
  },
  "CONVEYOR_A": {
    "MOTION": "MOVING"
  }
}
```

`Undo.Core` owns the type contract for each StateChannel.

Examples:
- OPEN → boolean,
- LOCK → declared lock-state identifier,
- POSITION → LocationId,
- POSSESSION → EntityId or explicit NONE value,
- POWER → declared power-state identifier/boolean according to final channel contract,
- AUTHORIZATION → declared authorization-state identifier.

Do not encode gameplay state in free-form human prose.

## Stable ID conventions

### EntityId

```
<FUNCTION/TYPE>_<AREA>_<NUMBER/NAME>
```

Examples:
- `DOOR_B2_014_A`
- `GUARD_B2_01`
- `SCANNER_B2_TRANSFER_01`
- `BADGE_STAFF_03`

Prototype IDs may be simpler:
- `DOOR_A`
- `GUARD_01`

### LocationId

Prefix:
`LOC_`

Examples:
- `LOC_B2_SECURITY_DESK`
- `LOC_B2_STAFF_DOOR_OUTSIDE`

### PuzzleId

Prefix by purpose:
- `PT_` prototype,
- `VS_` vertical slice,
- `PZ_` campaign puzzle.

Examples:
- `PT_B_PERSISTENCE`
- `VS_R4_TRANSFER`
- `PZ_A2_05_MISSING`

### ReactionId

Prefix:
`RX_`

Example:
- `RX_GUARD_MISSING_BADGE`

IDs are identifiers, never localized player-facing names.

## Routine authoring

Routine steps use a small typed vocabulary.

Initial planned step types:

- `MOVE_TO`
- `WAIT`
- `INTENT`

Conceptual example:

```json
{
  "actorId": "GUARD_01",
  "initialLocation": "LOC_SECURITY_DESK",
  "routine": [
    {
      "type": "INTENT",
      "verb": "TAKE",
      "target": "BADGE_01"
    },
    {
      "type": "MOVE_TO",
      "location": "LOC_SCANNER_FRONT"
    },
    {
      "type": "INTENT",
      "verb": "AUTHORIZE",
      "target": "SCANNER_01"
    },
    {
      "type": "MOVE_TO",
      "location": "LOC_STAFF_DOOR_INSIDE"
    }
  ]
}
```

Do not add a generic script expression field.

If a new routine verb is repeatedly needed, update the canonical vocabulary and validator.

## Reaction authoring

Reactions use structured predicates, not arbitrary code strings.

Conceptual example:

```json
{
  "reactionId": "RX_GUARD_MISSING_BADGE",
  "priority": 200,
  "policy": "ONCE_PER_TRIGGER_INSTANCE",
  "when": {
    "all": [
      {
        "memoryEquals": {
          "key": "expectedBadgeLocation",
          "value": "LOC_BADGE_LOCKER"
        }
      },
      {
        "stateNotEquals": {
          "entity": "BADGE_01",
          "channel": "POSITION",
          "value": "LOC_BADGE_LOCKER"
        }
      }
    ]
  },
  "steps": [
    {
      "type": "MOVE_TO",
      "location": "LOC_SECURITY_DESK"
    }
  ]
}
```

Initial predicate vocabulary should remain deliberately small.

Expected early predicates:
- `stateEquals`,
- `stateNotEquals`,
- `memoryEquals`,
- `memoryNotEquals`,
- `atLocation`.

Add event-history predicates only when a prototype/real puzzle demonstrates need.

## No arbitrary expression language

Prohibited examples:

```json
{ "condition": "badge.pos != locker && guard.knowsBadge && !alarm" }
```

or:

```json
{ "script": "if (...) { ... }" }
```

Reasons:
- hard to validate,
- hard to refactor,
- encourages per-level programming inside data,
- weakens Codex/code-review safety,
- creates an undocumented scripting language.

If a condition cannot be represented cleanly, first ask whether the puzzle is demanding a non-reusable special case.

## Completion conditions

Puzzle completion also uses structured semantic predicates.

Example:

```json
{
  "completion": {
    "all": [
      {
        "stateEquals": {
          "entity": "STAFF_DOOR_A",
          "channel": "LOCK",
          "value": "UNLOCKED"
        }
      },
      {
        "playerAtLocation": {
          "location": "LOC_EXIT"
        }
      }
    ]
  }
}
```

`playerAtLocation` is a puzzle/application condition, not a correctable StateChannel.

## Scene binding

Every gameplay scene object that participates in causal state has a binder component with exported stable ID.

Conceptually:

```csharp
[Export]
public string EntityId { get; set; } = string.Empty;
```

Location anchors similarly expose a LocationId.

At puzzle load, binding validation must ensure:
- every required content EntityId has exactly one scene binder where presentation is required,
- no duplicate gameplay EntityId binders exist,
- all required LocationIds resolve.

Do not use NodePath as persistent identity.

## Presentation profiles

Causal JSON may refer to stable presentation-profile IDs only where a gameplay concept requires it.

Do not fill causal data with:
- materials,
- mesh paths,
- audio file paths,
- animation names.

Those belong in Godot presentation resources/binders.

Example:
`DOOR_A` knows it has OPEN/LOCK state. Its binder knows which animations/material/audio implement those states.

## Manifests

`game/content/manifests/puzzles.json` provides discovery/order metadata.

Conceptual:

```json
{
  "schemaVersion": 1,
  "puzzles": [
    {
      "puzzleId": "PT_A_BASIC_CORRECTION",
      "path": "res://content/puzzles/PT_A_BASIC_CORRECTION/puzzle.json",
      "scenePath": "res://scenes/levels/PT_A_BASIC_CORRECTION.tscn"
    }
  ]
}
```

Campaign progression still follows canonical progression documents; the manifest is technical discovery data.

## Narrative metadata

Narrative artifact metadata may be stored in a manifest using:
- ArtifactId,
- essential/optional classification,
- localization keys,
- presentation type,
- reveal-stage tags,
- scene/entity reference where required.

Do not store long final prose directly in the manifest.

## Validation

Before a puzzle can run in development, validator checks include:

- supported `schemaVersion`,
- PuzzleId matches manifest/path expectation,
- unique actor/entity IDs,
- valid channel/value combinations,
- referenced entities exist,
- referenced semantic locations exist,
- allowed EventVerb only,
- allowed routine step only,
- allowed predicate only,
- stable reaction priority/order,
- correction capacity valid,
- completion predicate valid,
- scene path exists where validation environment permits,
- localization keys exist where required.

Validation errors include:
- file path,
- puzzle ID,
- offending stable ID,
- property path.

## JSON versus `.tres`

Use JSON when:
- `Undo.Core` needs to read/test it,
- data expresses causal rules,
- code review should see a clear semantic diff.

Use `.tres` when:
- the data exists only to drive Godot presentation/editor workflow,
- it contains Godot resources/types,
- direct Inspector editing is materially useful.

This boundary is architectural, not stylistic.

## Avoid premature shared templates

At first, puzzle JSON may repeat small routine fragments.

Create shared reusable behavior/content templates only after:
- at least two or three real puzzles require the same semantic structure,
- reuse reduces rather than obscures reviewability.

Do not build inheritance-heavy content architecture before prototypes.

## Migration

Every causal JSON root has `schemaVersion`.

During prototypes:
- breaking schema changes are permitted if the version is bumped and fixtures/content are migrated in the same change.

After vertical-slice content stability:
- schema migrations require explicit tooling or a deliberate coordinated repository migration.

Never silently reinterpret old content.

## Review rule for Codex

When implementing or changing a puzzle:

1. read the puzzle's canonical design entry,
2. modify semantic JSON for causal behavior,
3. modify scene/presentation binding separately,
4. run content validation,
5. run relevant domain tests,
6. do not hide puzzle logic in a level-specific C# script unless the design documents explicitly require a reusable new rule.
