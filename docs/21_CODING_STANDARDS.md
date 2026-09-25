# UNDO — Coding Standards

## Scope

These standards apply once implementation begins.

Primary stack:
- Godot 4.7.2 .NET
- C#
- pure .NET `Undo.Core` domain layer
- xUnit domain tests

## Architectural boundaries

### Undo.Core
Must not reference:
- Godot Node,
- Vector3/Transform3D as gameplay authority,
- scene paths,
- runtime instance IDs,
- animation players,
- audio resources,
- UI nodes.

### Godot application layer
May adapt:
- EntityId ↔ Node,
- LocationId ↔ Transform/Marker,
- semantic state ↔ animations/presentation.

Never move causal authority into a scene script because it is convenient.

## Namespaces

Target structure conceptually:

```
Undo.Core.Events
Undo.Core.State
Undo.Core.Corrections
Undo.Core.Actors
Undo.Core.Puzzles
Undo.Core.Save

Undo.Game.Application
Undo.Game.Presentation
Undo.Game.Input
Undo.Game.Debug
```

## Stable ID types

Prefer strongly typed wrappers over passing arbitrary strings throughout domain APIs.

Conceptual:

```csharp
public readonly record struct EntityId(string Value);
public readonly record struct EventId(string Value);
public readonly record struct LocationId(string Value);
public readonly record struct PuzzleId(string Value);
```

Serialization may use strings, but internal APIs should make accidental ID mixing difficult.

## Nullability

Enable nullable reference types.

Avoid `null` as a hidden gameplay state.

If a state intentionally represents "no owner" or "no location", model that explicitly where practical.

## Immutability

Prefer immutable domain records/value objects for:
- RecordedEvent,
- StateMutation,
- reaction definitions,
- authored content definitions.

Mutable session objects may own:
- event ledger append position,
- suppression set,
- current actor runtime state.

Do not mutate historical events.

## Collections and order

If order affects gameplay, use a collection whose order is explicit.

Do not rely on dictionary/hash iteration order for:
- event precedence,
- reaction priority ties,
- routine step order,
- ending state evaluation.

## Time

Distinguish:
- event sequence,
- narrative/game timestamp,
- real frame time.

Event sequence is authoritative for causal ordering.

Do not compare floating-point frame times to determine which event "came first."

## State changes

Gameplay-relevant state must change through defined domain/application operations that can create/validate RecordedEvents.

Avoid arbitrary scripts directly assigning puzzle state behind the EventRecorder.

Presentation-only variables are exempt.

## Magic strings

Do not scatter:
- entity IDs,
- channel names,
- reaction names,
- puzzle IDs,

as ad-hoc string literals through code.

Use typed IDs, enums, constants, or validated authored data.

## Godot node lookup

Avoid deep hardcoded `GetNode("A/B/C/D")` paths for gameplay authority.

Prefer:
- exported references,
- dedicated binder components,
- stable EntityId binding,
- scene unique names where appropriate for local presentation.

A scene refactor should not change causal identity.

## Signals

Name Godot signals as application notifications, not as gameplay RecordedEvents.

Good distinction:
- `PresentationStateChanged` signal
- `RecordedEvent` domain object

Do not call every software callback an "event" without qualification in architecture code.

## NPC code

Decision logic belongs in testable rule evaluation.

Godot actor scripts execute:
- navigation,
- animation,
- interaction sequencing,
- sensing adapters.

They should not contain hidden puzzle-specific random branches.

## Randomness

No gameplay-critical randomness in causal puzzle logic unless a future design document explicitly authorizes it.

Cosmetic randomness is allowed if it cannot affect:
- route,
- event timing materially,
- correction eligibility,
- puzzle solution.

## Animation commit

Every state-changing animation sequence must define one explicit commit point.

Before commit:
- interruption produces no RecordedEvent.

After commit:
- history exists and state is resolved through the domain layer.

Do not infer commit by checking animation time from unrelated systems.

## Async behavior

Avoid uncontrolled async/task chains whose completion order can alter puzzle outcomes.

When asynchronous loading/presentation is needed, causal commits still occur through explicit deterministic application commands.

## Error handling

During development:
- invalid content/state should fail loudly,
- include stable IDs in exception/log messages.

Do not silently substitute defaults for unknown puzzle IDs or invalid channels.

At release boundaries, graceful recovery may restore a stable checkpoint, but diagnostic information should still be logged.

## Logging

Use structured prefixes/categories.

Examples:
- EVENT
- STATE
- CORRECTION
- ACTOR
- REACTION
- SAVE
- CONTENT

Include IDs, not only display names.

## Comments

Comments explain:
- why a rule exists,
- a non-obvious invariant,
- a design constraint.

Do not comment obvious syntax.

When code behavior follows a specific design rule, referencing the relevant doc section in a short comment is acceptable.

## File size / responsibility

Prefer small focused types over giant manager scripts.

Warning signs:
- one Godot node owns event ledger + NPC AI + save + UI,
- a level script contains the full solution logic,
- dozens of special cases switch on PuzzleId.

Extract reusable domain/application concepts before accumulating exceptions.

## Data-driven does not mean meta-framework

Do not build:
- a custom visual scripting language,
- a general-purpose rule compiler,
- a universal ECS,

before prototypes show the need.

Use straightforward C# and validated authored content.

## Testing requirement

Any change to:
- state resolution,
- event suppression,
- correction release,
- capacity,
- reaction priority,
- save reconstruction,

requires corresponding automated tests.

Bug fixes should add a regression test when the bug is reproducible at the domain/content level.

## Formatting

Use standard `dotnet format`-compatible C# formatting.

Prefer:
- file-scoped namespaces,
- braces for control flow,
- descriptive names,
- small pure methods for rule evaluation.

Do not optimize for terseness.

## Dependencies

Add third-party packages only when they solve a demonstrated need.

Every dependency should document:
- purpose,
- license,
- version,
- why built-in/.NET functionality is insufficient.

Avoid introducing large frameworks for a single helper function.

## Generated files

Do not manually edit generated Godot/import/cache outputs.

Keep generated/import caches out of Git according to the project `.gitignore`.

## Pull request/task discipline

A coding task should:
- name the design documents it implements,
- change only relevant systems,
- include tests,
- state any design ambiguity,
- avoid unrelated refactors.

If implementation reveals a design contradiction, update/resolve the design docs before encoding a silent exception.

## Definition of done

A gameplay change is done when:
- build succeeds,
- relevant tests pass,
- content validation passes,
- deterministic behavior is preserved,
- debug output is understandable,
- documentation matches behavior.
