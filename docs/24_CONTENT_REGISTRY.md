# UNDO — Content Registry

## Purpose

This registry gives every major campaign puzzle and narrative beat a stable identity.

Status terms:
- **CANON_CONCEPT** — design purpose is locked; detailed implementation/content may evolve.
- **PROTOTYPE** — built specifically for mechanic validation.
- **VERTICAL_SLICE** — representative production-quality content.
- **PRODUCTION** — campaign content in full production.
- **LOCKED** — content-complete except fixes/polish.

Current campaign entries are **CANON_CONCEPT**.

## Campaign structure

### Act I — DISCOVERY

#### PZ_A1_01_CLOSING_TIME
Status: CANON_CONCEPT  
Primary function: Teach  
Primary mechanic: single CLOSE suppression  
Actors: office worker  
Core objects: office door  
Narrative: final-shift normality  
Player realization: the player cannot directly open the door; a recorded close can be suppressed.

#### PZ_A1_02_AFTER_YOU
Status: CANON_CONCEPT  
Primary function: Test  
Primary mechanic: suppress closure after NPC passage  
Actors: employee  
Core objects: staff door  
Player realization: actor history/position does not rewind.

#### PZ_A1_03_THE_LIGHT
Status: CANON_CONCEPT  
Primary function: Combine  
Primary mechanic: suppress LightOff → system reacts  
Actors: cleaning/facility worker  
Core objects: light circuit, sensor/system  
Player realization: corrected state can trigger present-time consequences.

### Act II — CONTROL

#### PZ_A2_04_PATROL
Status: CANON_CONCEPT  
Primary function: Teach Redirect  
Actors: guard  
Core objects: lock/door, route branch  
Player realization: correction changes conditions; NPC chooses next route.

#### PZ_A2_05_MISSING
Status: CANON_CONCEPT  
Primary function: Signature mechanic teach  
Actors: guard  
Core objects: badge, scanner, staff door  
Primary mechanic: Consequence Persistence  
Narrative: early traces of Seojin may appear around this range.

#### PZ_A2_06_ROUTINE
Status: CANON_CONCEPT  
Primary function: Test/Combine  
Actors: worker A, worker B  
Core objects: document/archive box, scanner, gate  
Primary mechanic: multiple independent recorded events across a workflow.

#### PZ_A2_07_LET_IT_CLOSE
Status: CANON_CONCEPT  
Primary function: Teach release  
Actors: employee/worker  
Core objects: service/elevator door, emergency response system  
Primary mechanic: suppress → wait → release → induced action.

### Act III — CONSEQUENCE

#### PZ_A3_08_RESTORATION
Status: CANON_CONCEPT  
Primary function: Teach State Preparation  
Actors: restoration worker as needed  
Core objects: conveyor, scanner, restoration machine, archive box  
Primary mechanic: semantic motion/position milestones.

#### PZ_A3_09_INSPECTION
Status: CANON_CONCEPT  
Primary function: Teach Reaction  
Actors: maintenance worker  
Core objects: abnormal machine state, service path  
Primary mechanic: contradiction → deterministic inspection reaction.

#### PZ_A3_10_TWO_ERRORS
Status: CANON_CONCEPT  
Primary function: Capacity escalation  
Actors: TBD within existing families  
Primary mechanic: demonstrates insufficiency of one maintained correction, then unlocks capacity 2.

#### PZ_A3_11_CROSSED_PATHS
Status: CANON_CONCEPT  
Primary function: Combine  
Actors: guard + maintenance worker  
Primary mechanic: two simultaneous causal distortions create a new interaction/event.

### Act IV — ARCHIVE

#### PZ_A4_12_OVERWRITTEN
Status: CANON_CONCEPT  
Primary function: Teach Masked Event  
Core objects: one or more multiply-modified state channels  
Primary mechanic: older suppression may be invisible until newer contribution is removed.

#### PZ_A4_13_AUDIT
Status: CANON_CONCEPT  
Primary function: Institutional-state escalation  
Actors: employee + security response  
Core objects: access control, person-presence state, security system  
Primary mechanic: suppress AUTHORIZATION after entry → system detects present contradiction.

#### PZ_A4_14_CONTRADICTION
Status: CANON_CONCEPT  
Primary function: Mastery  
Scale: large connected multi-space puzzle  
Required techniques:
- persistence,
- reaction,
- release,
- multiple corrections,
- masked events,
- institutional state.
Target: intentionally impossible world-state combination.

### Act V — RECORD 0

#### PZ_A5_15_RECONSTRUCTION
Status: CANON_CONCEPT  
Primary function: Narrative/mechanical convergence  
Core: reconstruction of historical B3 accident  
Capacity: up to 3  
Player repeatedly improves historical conditions but cannot select the decisive absent action.

#### PZ_A5_16_RECORD_ZERO
Status: CANON_CONCEPT  
Primary function: Final present-day decision through mechanics  
Core: present-day archival destruction/preservation processes  
Outcome is derived from factual final world/record state, not a choice menu.

## Prototype content IDs

### PT_A_BASIC_CORRECTION
Status: PROTOTYPE  
Implementation: graybox and automated technical validation complete; human playtest pending.  
See `docs/22_PROTOTYPE_SPEC.md`.

### PT_B_PERSISTENCE
Critical project gate.

### PT_C_NPC_REACTION
Deterministic reaction gate.

## Vertical slice ID

### VS_R4_TRANSFER
Representative B1/B2 archive/security transfer area.

See `docs/23_VERTICAL_SLICE.md`.

## Narrative artifact registry

Essential narrative information targets:

### N01_MIGRATION_NOTICE
Introduces Repository 4 migration/closure and final-shift context.

### N02_SEOJIN_NAME_FIRST
Old maintenance/inspection record with Yoon Seojin's name.

### N03_SEOJIN_WORK_TRACE
Handwritten equipment/work trace establishing ordinary personality.

### N04_B3_CLOSURE_NOTICE
Establishes historical B3 closure/accident without full details.

### N05_RESTORATION_TEAM
Establishes Seojin as restoration technician.

### N06_NIGHT_ROSTER
Connects Jaemin and Seojin as coworkers on old shifts.

### N07_RECORD_ZERO_ERROR
Introduces abnormal Record 0 identifier during migration.

### N08_ACCIDENT_ACCESS_LOG
Shows both were present/on duty on the accident date.

### N09_ISOLATION_LOG
Reconstructs technical start of B3 compound failure.

### N10_RELEASE_AUTHORITY_AUDIT
Shows Jaemin had remote emergency-release capability.

### N11_ACCIDENT_COMMS
Contains Seojin's ordinary request for Jaemin to check the system.

### N12_39_SECOND_COMPOSITE
CCTV/console composite revealing the no-state-change interval.

### N13_INVESTIGATION_SUMMARY
Shows multi-factor responsibility and uncertain survivability.

### N14_RECORD_ZERO_STATUS
Defines current archival status and supports final mechanical decision.

## Optional environmental narrative

Optional items may include:
- handwritten equipment notes,
- old duty notebook,
- meal/shift artifacts,
- repair tickets,
- leave/swap roster,
- staff photo,
- equipment manual annotations,
- post-accident safety notice,
- sealed locker,
- employee labels.

Optional content must humanize people/place, not contain required plot logic.

## Content rule

Every new campaign puzzle must be added to this registry before full production implementation.

Every major new narrative artifact must declare:
- what information it carries,
- whether it is essential or optional,
- where in the reveal ladder it belongs.

Avoid uncontrolled content accumulation.
