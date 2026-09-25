# UNDO — Save / Restore Architecture

## Goals

Save and restore must preserve the causal puzzle model reliably without requiring serialization of every engine object or physics frame.

The system separates:

1. **Campaign Save** — long-lived player progress.
2. **Puzzle Snapshot** — stable restore point for the active puzzle space.
3. **Session Runtime** — current event ledger and corrections since the snapshot.

## Principle

> Save semantic gameplay state; reconstruct presentation.

Do not serialize the entire Godot scene tree as the authoritative save format.

## Campaign Save

Long-lived fields may include:
- schema version,
- campaign/act progression,
- current level/puzzle identifier,
- current stable checkpoint identifier,
- correction capacity unlocked,
- narrative/progression flags required for routing,
- accessibility/settings references where appropriate,
- ending-state flags after completion.

Do not store redundant world state that can be recreated from a checkpoint plus session data.

## Puzzle Snapshot

A snapshot represents a stable authored starting state.

Contains conceptually:
- PuzzleId,
- snapshot schema/version,
- initial StateKey → StateValue map,
- actor stable semantic locations,
- actor routine step,
- required minimal ActorMemory,
- authored machine/session states not represented by ordinary channels,
- player safe spawn transform/anchor,
- sequence baseline,
- local narrative flags required inside the space.

Snapshots should be created from authored/checkpoint state, not arbitrary milliseconds of animation.

## Session Runtime

While solving a puzzle, track:
- RecordedEvent ledger since snapshot baseline,
- current suppressed EventIds,
- actor behavior state required for current deterministic continuation,
- semantic state derivable from snapshot + ledger + suppression,
- player current transform if needed for regular autosave.

## Restore Current Record

Menu command:
**RESTORE CURRENT RECORD**

Behavior:
1. stop active puzzle execution,
2. clear runtime events after snapshot baseline,
3. clear maintained suppressions created in the session,
4. restore snapshot semantic state,
5. restore actor memory/routine stable state,
6. restore machines,
7. place player at safe restore anchor,
8. apply presentation states without replaying historical animations,
9. resume.

This is a game retry, not a Correction mechanic.

## Autosave

Autosave occurs only at stable boundaries, such as:
- entry to a new major puzzle/checkpoint,
- completion of a puzzle,
- act transition,
- selected narrative-safe traversal checkpoints.

Avoid autosaving arbitrary broken mid-animation states during early production.

Mid-puzzle persistence may be added later only if usability testing establishes a real need.

## Initial save policy

Canonical initial approach:
- one primary campaign slot,
- automatic stable-checkpoint saves,
- `NEW RECORD` replaces the current campaign after confirmation.

Multiple manual campaign slots are not required for prototype or vertical slice.

Revisit before release if platform/user-testing requirements justify them.

## Serialization format

Prefer explicit versioned DTO/data contracts.

Do not serialize:
- Godot Node references,
- transient instance IDs,
- navigation path internals,
- animation playback internals,
- physics contacts.

Use:
- stable EntityIds,
- PuzzleIds,
- LocationIds,
- EventIds where persistent,
- enum/string identifiers,
- primitive state values.

## Schema versioning

Every campaign save has a schema version.

Migration strategy:
- prototypes may invalidate saves between major milestones when explicitly documented,
- once vertical-slice compatibility is promised, migrations must be intentional,
- production releases must not silently reinterpret old fields.

## Event persistence

For stable checkpoint autosaves during production, prefer saving only states needed to reconstruct from the checkpoint.

If mid-puzzle save is later implemented, persist:
- event ledger,
- suppression set,
- actor semantic state/memory not fully derivable from the ledger,
- sequence counter,
- puzzle-specific deterministic timers as semantic remaining values where necessary.

On load:
1. load snapshot,
2. restore ledger,
3. restore suppression state,
4. resolve effective world state,
5. restore actor behavior state,
6. bind presentation,
7. resume.

## Correction consistency

Save validation must reject/correct impossible storage states such as:
- suppressed event ID missing from ledger,
- suppression count over unlocked capacity,
- event referencing unknown entity,
- state value incompatible with channel specification.

During development, fail loudly with diagnostic output.

For shipped builds, recover to the last valid stable checkpoint where feasible rather than corrupting the campaign further.

## Player transform

Player position is not a correction channel.

At stable checkpoints:
- save a safe PlayerAnchorId or authored transform.

If arbitrary-position autosave is later introduced:
- validate position against the loaded navigation/collision environment,
- fall back to safe anchor if invalid.

## Presentation restoration

After load/restore:
- doors snap or quietly transition to effective semantic state,
- carried/placed objects attach to their semantic anchors,
- NPCs appear at restored semantic locations,
- machines enter stable presentation state,
- no historical action animation is replayed merely to reconstruct state.

A short neutral transition may hide presentation reconstruction.

## Narrative records

Collected/read-state is only stored if the final UX needs it.

Do not turn narrative documents into an RPG collectible checklist by default.

Essential narrative progression is stored through campaign progression flags, not "found 12/14 logs."

## Ending state

Final outcome is derived from the actual final puzzle state and relevant event/correction choices.

Do not store a direct "GOOD_ENDING" value.

Store factual result flags/state such as:
- physical Record 0 preserved?,
- institutional index contains Record 0?,
- destruction event completed/suppressed?,

then select the ending presentation from those facts.

## Testing

Required automated tests:
- snapshot → runtime events → serialize → load yields same effective semantic state,
- corrections survive mid-session serialization if/when supported,
- stable checkpoint restore clears later events,
- actor memory restores correctly,
- capacity progression persists,
- invalid suppressed event IDs are detected,
- version mismatch is handled intentionally,
- final factual ending state reconstructs consistently.

## Source-control fixtures

Maintain small human-readable test fixtures for:
- basic door snapshot,
- Prototype B badge/door persistence,
- NPC missing-badge reaction,
- masked-event case,
- two-correction capacity case.

Fixtures should be suitable for regression tests and code review.

## Production warning

Do not solve save problems by serializing the entire live scene.

If a new system cannot be saved semantically, that is a signal that its gameplay authority may be living in the wrong layer.
