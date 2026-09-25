# UNDO — Vertical Slice Detailed Level Sheet

Last locked: 2026-09-25

## ID

`VS_R4_TRANSFER`

## Purpose

Create one continuous 25–35 minute representative UNDO experience proving that:
- the causal mechanic is enjoyable beyond isolated prototypes,
- multiple puzzle families coexist coherently,
- Repository 4 feels like a real workplace rather than test chambers,
- the no-HUD / object-history UX is sufficient,
- institutional art/audio direction supports puzzle readability,
- the production pipeline can take a graybox to representative final quality.

## Status

**CANONICAL VERTICAL-SLICE GRAYBOX SPEC**

Do not begin polished production until Prototype A/B/C gates pass.

---

# 1. Narrative placement

The slice is set during the present-day migration/closure period.

It represents a **late B1 / early B2 records-transfer and staff-security route**.

Jaemin is trying to move through the building after a local access inconsistency has made the normal staff route unusable.

The slice may establish:
- facility migration is underway,
- many employees have already left,
- the building combines multiple renovation eras,
- Jaemin knows the workplace well,
- some lower areas are restricted.

The slice must not reveal:
- Record 0's meaning,
- the 39-second gap,
- Seojin's accident details,
- final ending logic.

## Narrative tone

The player should feel:

```
ordinary workplace
→ odd system inconsistency
→ mechanical curiosity
→ confidence
→ first hint that records and current state do not fully agree
```

Not:
```
ordinary workplace
→ giant conspiracy reveal
```

---

# 2. Macro spatial plan

Target total navigable footprint:
approximately 45–65 meters of meaningful traversal, folded through one coherent facility section.

Conceptual map:

```
┌──────────────────────────────────────────────────────────┐
│                                                          │
│  [A] STAFF OFFICE / INTAKE                               │
│      │                                                   │
│      ▼                                                   │
│  [B] SECURITY CORRIDOR ────────┐                         │
│      │                         │                         │
│      ▼                         │                         │
│  [C] TRANSFER DESK / BADGE     │                         │
│      │                         │                         │
│      ▼                         │                         │
│  [D] STAFF GATE / CHECK AREA   │                         │
│      │                         │                         │
│      ▼                         │                         │
│  [E] SERVICE ANTECHAMBER       │◄──── MAINT ROUTE ──────┘
│      │
│      ▼
│  [F] SERVICE LIFT / EXIT
│
└──────────────────────────────────────────────────────────┘
```

The loopback/service connection allows the player's corrections to make NPC routes cross naturally without creating a maze.

## Architectural identity

This is not five isolated puzzle chambers.

Use plausible workplace functions:
- intake office,
- access corridor,
- transfer counter,
- controlled records gate,
- maintenance/service access,
- staff/service lift.

Glass panels, half-height service windows, archive shelving breaks, and doorways create sightlines.

---

# 3. Slice checkpoints

Stable restore boundaries:

### VS_CP_01
Start of Staff Office.

### VS_CP_02
After VS01 basic correction.

### VS_CP_03
After Patrol Redirect / before Persistence centerpiece.

### VS_CP_04
After Badge Persistence comprehension.

### VS_CP_05
Before final release-timing sequence.

Autosave at these stable boundaries only.

Do not autosave in the middle of an NPC action.

---

# 4. Segment A — Staff Office Threshold

## Function

Teach/basic refresh of:
- indirect interaction,
- Review,
- suppression.

Target first-play time:
3–5 minutes.

## Space

Small office intake area, approximately:

```
8 m × 10 m
```

Contains:
- desks,
- migration boxes,
- one staff worker,
- staff corridor door.

## Flow

Worker finishes handling an archive cart/box and leaves.

Worker closes Door A.

Player's direct Inspect does not open the door.

Residual Exposure becomes available.

Player suppresses CLOSE.

Door opens.

Player passes.

## Event

`VS_E001_CLOSE_OFFICE_DOOR`

Mutation:

```
DOOR_VS_OFFICE.OPEN
true → false
```

## Teaching restraint

Only here may a very minimal Review prompt appear.

Do not teach:
- history scrolling,
- release,
- capacity,
- Consequence Persistence terminology.

## Environmental story

Visible migration notice:
- boxes labeled for transfer,
- printed internal notice,
- partially cleared desks.

Narrative function:
establish closure/migration without stopping the player for exposition.

---

# 5. Quiet transition A→B

Target:
30–60 seconds.

Short corridor with:
- archive carts,
- room numbers,
- staff signage,
- distant footsteps/door sounds.

Purpose:
- breathing space,
- establish directional audio,
- let the player feel they are moving through a real facility.

No puzzle.

---

# 6. Segment B — Patrol Redirect

## Function

Teach:
corrections affect conditions under which NPCs choose their next route.

Target:
4–6 minutes.

## Space

Security corridor junction.

Conceptual:

```
                  [STAFF ROUTE]
                       |
                       |
[PLAYER/OBS] ---- [JUNCTION] ---- [NORMAL PATROL]
                       |
                  [LOCKED DOOR]
```

Guard approaches a branch point.

A staff-route door normally remains locked due to an earlier lock event.

## Normal routine

```
Guard reaches Junction
checks Staff Door state
if LOCKED:
  turns toward Normal Patrol
if UNLOCKED:
  enters Staff Route
```

## Intended correction

Player suppresses the relevant LOCK event before Guard's defined branch evaluation.

Guard then enters Staff Route.

## Important behavior

Guard does not instantly turn from far away when correction happens.

Route decision occurs at:
`LOC_VS_PATROL_BRANCH`

This visually teaches deterministic evaluation points.

## New event

Guard's route through Staff Route may create:
- OPEN/close event on a downstream service door,
- positional opportunity for the next segment.

Do not make that downstream event the solution yet; it mainly demonstrates the world continuing.

## Observation

Provide a glass security panel/recess where:
- branch,
- door,
- approaching Guard

are visible.

## Audio

Guard footsteps should make approach rhythm obvious.

Lock/latch state change must be audible.

---

# 7. Checkpoint VS_CP_03

After player passes the redirected-route obstacle, enter a short transfer-desk area.

This becomes the setup for the signature puzzle.

---

# 8. Segment C — Transfer Desk / Badge Persistence

## Function

Vertical-slice centerpiece.

Teach:
**Consequence Persistence**.

Target:
7–10 minutes including first observation and experimentation.

## Space

Approximate:

```
14 m × 12 m
```

Core composition:

```
[TRANSFER DESK + BADGE]
          |
          | clear sightline
          v
      [SCANNER] ---- [STAFF GATE]
          ^
          |
  [OBSERVATION RECESS]
```

## Entities

- `GUARD_VS_01`
- `BADGE_VS_01`
- `SCANNER_VS_01`
- `DOOR_VS_STAFF_GATE`

## Normal chain

```
E_C1 Guard TAKE Badge
E_C2 Guard AUTHORIZE Scanner
E_C3 System UNLOCK Staff Gate
Guard passes
```

All three moments need distinct:
- animation,
- sound,
- debug event record.

## Intended solution

After E_C3 commits:
- player suppresses E_C1.

Result:

```
Badge returns to Transfer Desk.
Scanner authorization persists.
Staff Gate remains unlocked.
Guard remains on far side/current route.
```

## Teaching tableau

After successful E_C1 suppression:
- no immediate additional NPC reaction for several seconds,
- Badge visibly appears on Desk,
- Staff Gate's unlocked state remains legible,
- player can inspect both.

This pause is mandatory in first slice iteration.

## Early correction

If player suppresses TAKE before AUTHORIZE:
- later scan must not commit if its prerequisites are invalid,
- Guard uses deterministic fallback/retry,
- player learns that downstream persistence only exists after the downstream event actually happened.

Do not show explanatory text.

## Completion

The unlocked Staff Gate creates access to the next circulation area.

Keep completion physically obvious.

---

# 9. Quiet transition C→D

Target:
30–90 seconds.

Use:
- transfer shelving,
- old staff photos/rosters,
- mixed-era equipment.

Optional narrative trace:
a small old work record with the name `윤서진` may appear, but it must not be framed as a major reveal.

This is optional in the first vertical-slice build.

---

# 10. Segment D — Missing Badge Reaction

## Function

Prove contradiction → believable observation → deterministic present-time NPC reaction.

Target:
5–7 minutes.

## Space

Controlled staff gate connects to:
- Badge locker/check station,
- service desk,
- maintenance office.

## Setup

Later in Guard routine:
- Guard PLACEs badge into Locker A.
- E_D1 commits.
- Guard memory records expected location.

Player suppresses E_D1 after Guard moves on.

Locker becomes empty according to previous active Badge state.

Guard memory remains.

## Observation boundary

Guard does not react until reaching:
`LOC_VS_BADGE_CHECK`

At check:
```
expected locker location != actual badge state
→ RX_VS_MISSING_BADGE
```

## Reaction

Guard:
1. checks locker,
2. checks transfer/security desk,
3. moves to maintenance office,
4. opens Maintenance Door.

New event:
`E_D_NEW_OPEN_MAINT_DOOR`

## Player learning

The player should be able to state:

> I changed the record. The guard still remembered what he had done. He only noticed when he checked, then his search opened the route I needed.

No NPC exposition required.

## Route exploitation

Player uses opened Maintenance Door to enter Service Antechamber.

---

# 11. Segment E — Service Antechamber / Let It Close

## Function

Teach:
suppression release is an active puzzle verb.

Target:
4–6 minutes.

## Space

Small service-lift antechamber with:
- service worker or same Guard depending on final pacing,
- automatic/heavy service door,
- emergency/service call logic,
- lift access beyond.

Preferred actor:
a separate maintenance worker if visual workload permits; otherwise reuse a worker actor with clear functional role.

Avoid making the Guard perform every job in the building.

## Normal flow

Actor passes Service Door.

Door closes.

Player needs actor to enter the inner service zone under altered conditions.

## Intended chain

```
E_E1 Service Door CLOSE commits
Player suppresses E_E1
Door remains open
Actor crosses into inner area
Player waits until actor reaches required checkpoint
Player RELEASES E_E1
Door closes now
Actor is separated/placed into service condition
Actor triggers service/emergency response
E_E2 new service access/lift event occurs
```

The exact response must be plausible and not dangerous melodrama.

Example:
actor uses internal service-call control because the expected exit route is closed, causing the service lift vestibule to open/activate.

## Timing

Once the player understands the solution, release window should be generous:
approximately 4–8 seconds.

Do not require frame-perfect timing.

## Release introduction

This is the first polished slice teaching of Release.

Use:
- state contrast,
- appropriate Review presentation,
- one minimal input cue if needed.

Do not add a permanent "Release Mode" HUD.

---

# 12. Segment F — Service Lift / Slice End

Player enters lift/service exit.

No major puzzle.

Allow 20–40 seconds of decompression.

Possible environmental beat:
- lift panel shows lower restricted levels,
- one label is faded/covered,
- migration error light/terminal entry hints that the building's record state is not normal.

Do not name Record 0.

Fade/cut to slice completion.

Development/demo build may show:
`VERTICAL SLICE COMPLETE`

Final game obviously would continue.

---

# 13. Estimated first-play timing

| Segment | Target |
|---|---:|
| A Basic correction | 3–5 min |
| Transition | 0.5–1 min |
| B Redirect | 4–6 min |
| C Persistence | 7–10 min |
| Transition | 0.5–1.5 min |
| D NPC Reaction | 5–7 min |
| E Release | 4–6 min |
| End traversal | 0.5–1 min |
| **Total** | **24–37.5 min** |

Desired median target:
approximately 30 minutes.

If first-time sessions consistently exceed 40 minutes due to waiting/confusion, revise.

---

# 14. Correction capacity

Entire vertical slice:

```
Capacity = 1
```

This is intentional.

The slice demonstrates depth through:
- when to suppress,
- when to release,
- when downstream events become independent,
- how actors react,

not through multi-slot complexity.

---

# 15. Puzzle teaching order

The slice's mental-model progression:

```
A:
I can suppress a recorded state change.

B:
Changing state changes what an NPC does next.

C:
Later recorded consequences survive suppression of an earlier cause.

D:
NPC memory can disagree with current reality, and present observation creates a new reaction.

E:
I can deliberately release a maintained contradiction at the right time.
```

Every segment adds exactly one major idea.

---

# 16. NPC roster

Target visible actors:
- Staff Worker 01,
- Guard 01,
- optional Maintenance Worker 01.

Do not populate the slice with crowds.

Background staffing may be implied through:
- voices,
- distant doors,
- desks,
- movement outside inaccessible glass,

but must not create false puzzle actors.

---

# 17. Narrative artifact budget

Maximum required slice artifacts:
approximately 3–5 short items.

Suggested:
1. migration notice,
2. room/transfer signage,
3. one mundane work record,
4. optional old staff trace,
5. one system/migration-status terminal.

No long lore documents.

---

# 18. UI scope

Required representative quality:
- reticle,
- Review Mode,
- Residual Exposure,
- history selection where applicable,
- Commit,
- Release teaching,
- pause menu,
- Restore Current Record,
- settings baseline,
- subtitles infrastructure.

Not required:
- final credits,
- ending UI,
- full campaign chapter selection.

---

# 19. Art kit proof

The slice must exercise:
- current/2000s office/archive kit,
- early industrial/service transition kit,
- at least three door families,
- badge/scanner,
- archive shelving/desk props,
- institutional signage,
- one in-world terminal.

Do not import B3/B4/Record 0 final assets merely to show scope.

---

# 20. Audio proof

Required:
- at least two floor/footstep surfaces,
- door families distinguishable,
- scanner authorization,
- mechanical unlock,
- distant actor approach,
- service lift,
- Review/Correction/Release language,
- office/HVAC/fluorescent ambience.

Critical causal events must remain audible over ambience.

---

# 21. Lighting proof

Use representative:
- fluorescent office light,
- security corridor practical light,
- service/industrial transition.

No horror blackout sequence.

Residual Exposure must remain legible under all three lighting conditions.

---

# 22. Performance / state scope

Keep actor count and dynamic objects representative but controlled.

The slice should prove:
- state model works across several connected rooms,
- scene transitions/checkpoints do not corrupt event history,
- semantic IDs remain unique,
- restore remains reliable.

Do not optimize by removing the actual causal complexity being tested.

---

# 23. Save / restore

Stable checkpoints as above.

Each segment must be restartable from its nearest checkpoint without replaying the entire slice.

Restored NPCs appear in authored stable states.

No live-animation reversal.

---

# 24. Required automated regression scenarios

In addition to Prototype tests:

### VS_G01 Redirect
Suppressed lock at branch point causes authored staff route.

### VS_G02 Persistence
Badge TAKE suppression after UNLOCK preserves unlock.

### VS_G03 Reaction
PLACE suppression + later check fires MissingBadge once.

### VS_G04 Release
Service-door suppression maintains OPEN; release restores CLOSED without replaying original actor action.

### VS_G05 Checkpoint restore
Each checkpoint reconstructs expected semantic state and next routine state.

---

# 25. Slice playtest questions

After full slice:

1. In your own words, what does Undo change?
2. Why did the unlocked gate stay unlocked when the badge came back?
3. Why did the guard react later rather than immediately?
4. What did releasing a correction do?
5. Could you usually predict what an NPC would do next?
6. Was there a point where you were waiting without knowing why?
7. Did anything look like a bug rather than a rule?
8. Did the interface feel like a futuristic computer overlay?
9. What kind of place did Repository 4 feel like?
10. What would you want to try with the mechanic next?

---

# 26. Slice failure signs

Do not greenlight full production if:

- players interpret correction as global rewind,
- Persistence repeatedly reads as a bug,
- Guard reactions seem omniscient/random,
- release timing becomes dexterity,
- players depend on a HUD/log instead of observing the world,
- one correction slot already feels mentally overloaded,
- level iteration requires per-room C# hacks,
- restore/state corruption remains common,
- the art reads primarily as sci-fi lab/horror facility,
- causal audio is masked by atmosphere/music.

---

# 27. Slice greenlight

The vertical slice advances to full campaign production when:

### Mechanics
- all required causal rules function,
- players form the intended mental model,
- the same state produces deterministic outcomes.

### UX
- Review is usable with minimal HUD,
- Residual Exposure is noticed but not intrusive,
- Commit/Release are distinguishable,
- correction capacity 1 does not require a permanent counter.

### Level design
- observation anchors work,
- waiting is controlled,
- off-screen events remain legible,
- no solution requires untelegraphed special cases.

### Production
- content JSON is practical to author/review,
- Godot scene binding remains manageable,
- debug tools make failures diagnosable,
- asset-kit reuse feels specific rather than repetitive,
- performance is acceptable.

### Interest
The strongest qualitative signal:

> After understanding the system, players propose or attempt their own causal experiments.

That curiosity is the desired proof that UNDO can sustain a full game.
