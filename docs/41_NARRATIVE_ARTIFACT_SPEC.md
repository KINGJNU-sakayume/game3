# UNDO — Narrative Artifact Specification

Last locked: 2026-09-25

## Purpose

UNDO's plot should be understandable without reading long optional lore.

The fourteen essential narrative artifacts are therefore designed as:
- short,
- placed on the critical path,
- materially different in medium,
- fact-focused,
- progressively revealing.

The player should reconstruct the story from accumulated evidence rather than one exposition document.

## Core rule

> Essential plot information must be recoverable through required progression even if the player ignores optional reading.

An "artifact" may be:
- paper,
- terminal record,
- wall notice,
- CCTV/audio playback,
- structured system log,
- environmental composite.

It does not have to be a collectible.

## Reading-time budget

Target for essential static text:
- usually 5–20 seconds,
- rarely up to 30 seconds,
- avoid more than ~120 Korean characters / equivalent dense body text in one mandatory item unless the information truly requires it.

Complex technical facts should be split across:
- table,
- headings,
- state rows,
- environmental context,

rather than long prose.

## Presentation rule

No artifact is added simply to explain what the player just experienced mechanically.

Narrative artifacts explain:
- institution,
- people,
- history,
- accident facts.

The gameplay teaches gameplay.

---

# N01 — MIGRATION NOTICE

## ID

`N01_MIGRATION_NOTICE`

## Placement

Opening office / Act I, before or around L01.

## Medium

Current printed internal notice + optional matching terminal banner.

## Required information

Player learns:
- Repository 4 is undergoing transfer/migration/partial closure,
- some sections/services are being decommissioned,
- tonight is part of final migration work,
- old systems may be temporarily unavailable.

Do not mention Correction or Record 0.

## Reading time

5–10 seconds.

## Suggested structure

```
제4보존소 기록 이관 안내

- 최종 이관 작업: <date>
- 일부 구역 순차 폐쇄
- 야간 작업 중 출입/시스템 일시 제한 가능
- 미이관 자료는 담당자 확인 후 인계
```

## Narrative function

Makes the emptying workplace and unusual migration activity ordinary before the real anomaly begins.

---

# N02 — SEOJIN NAME FIRST

## ID

`N02_SEOJIN_NAME_FIRST`

## Placement

L03 range.

## Medium

Old equipment inspection/maintenance label or short form attached to a machine.

## Required information

Only:
- the name 윤서진,
- she once worked on facility equipment/records.

Do not frame the name as important.

## Reading time

2–5 seconds.

## Example fields

```
장비 점검
담당: 윤서진
상태: 정상
```

## Narrative function

Plant the name before the player knows why it matters.

---

# N03 — SEOJIN WORK TRACE

## ID

`N03_SEOJIN_WORK_TRACE`

## Placement

L05 range.

## Medium

Handwritten equipment note.

## Required information

No new plot fact required.

Humanizes Seojin:
- practical,
- mildly informal,
- familiar with unreliable equipment.

## Example tone

```
또 걸림.
세게 닫지 말 것.
필터는 월요일 확인.
- 서진
```

Exact final copy will be editorially reviewed.

## Narrative function

Player recognizes a person's working habits before learning her fate.

---

# N04 — B3 CLOSURE NOTICE

## ID

`N04_B3_CLOSURE_NOTICE`

## Placement

L07 / Act II end.

## Medium

Old access restriction notice or sealed corridor signage.

## Required information

Player learns:
- part of B3 was closed after an incident,
- access was restricted,
- closure is old, not caused by tonight.

Do not name Seojin as casualty yet.

## Reading time

5–10 seconds.

## Desired wording style

Administrative and dry.

Avoid:
- "tragic accident,"
- memorial language,
- ominous redacted conspiracy tone.

---

# N05 — RESTORATION TEAM

## ID

`N05_RESTORATION_TEAM`

## Placement

L08 / B2 Restoration.

## Medium

Historical staff roster, shift board, or technical-team directory.

## Required information

Player learns:
- 윤서진 was a 기록 복원 기술자 / restoration technician,
- her work is directly connected to the equipment/space now being explored.

## Optional secondary information

Other ordinary staff names to make roster feel authentic.

Do not make Seojin the only name on the page.

---

# N06 — NIGHT ROSTER

## ID

`N06_NIGHT_ROSTER`

## Placement

L09.

## Medium

Old night-duty roster.

## Required information

Player learns:
- Han Jaemin and Yoon Seojin worked together,
- they shared night duties in the same era.

This is the first explicit documentary link between them.

## Presentation

A simple table is preferred over prose.

```
날짜 | 구역 | 근무자
...
B3 복원 | 윤서진
기록 제어 | 한재민
```

## Narrative function

The player now knows Jaemin is personally connected to the name that has been recurring.

---

# N07 — RECORD ZERO ERROR

## ID

`N07_RECORD_ZERO_ERROR`

## Placement

L10.

## Medium

Current migration/error terminal.

## Required information

Player learns:
- an abnormal record identifier `0` exists,
- it is failing migration/index validation,
- the error originates from an older causal/event record subsystem.

Do not explain what Record 0 means.

## Example structure

```
MIGRATION ERROR

RECORD ID: 0
INDEX STATUS: INVALID
STATE TRANSITION: UNRESOLVED
SOURCE: LEGACY EVENT MODEL
```

## Reading time

5–15 seconds.

## Narrative function

Turns the recurring anomaly into a concrete investigative object.

---

# N08 — ACCIDENT ACCESS LOG

## ID

`N08_ACCIDENT_ACCESS_LOG`

## Placement

L11.

## Medium

Access-control table / old event log.

## Required information

Player learns:
- exact accident date,
- Seojin entered the B3 restoration area,
- Jaemin was logged in/on duty nearby in the control function.

Do not yet show the critical communication.

## Structure

Table/log is preferable.

Use exact timestamps consistently with canonical accident timeline.

---

# N09 — ISOLATION LOG

## ID

`N09_ISOLATION_LOG`

## Placement

L12.

## Medium

Technical system timeline.

## Required information

Reconstructs the compound failure:

- environmental-control warning,
- Seojin enters,
- power-control anomaly,
- equipment/environment system transitions,
- protective isolation activates,
- door remains closed,
- early condition does not appear immediately catastrophic.

## Reading time

15–25 seconds.

## Format

Chronological state rows.

Example:

```
22:11:16 POWER CONTROL — ABNORMAL TRANSFER
22:11:32 ENVIRONMENT CONTROL — PROTECTIVE MODE
22:12:07 ROOM 3 — ISOLATION ACTIVE
22:12:19 INTERNAL RELEASE — NO RESPONSE
```

## Narrative function

The player can read the accident using the same causal grammar learned in gameplay.

---

# N10 — RELEASE AUTHORITY AUDIT

## ID

`N10_RELEASE_AUTHORITY_AUDIT`

## Placement

L13.

## Medium

Permissions/audit record.

## Required information

Player learns:
- Jaemin had access to remote emergency release,
- using it required a deliberate manual override/authorization step,
- it was available before his eventual attempt.

Do not conclude:
"Jaemin could definitely have saved Seojin."

## Format

Permission table + audit history.

This keeps interpretation factual.

---

# N11 — ACCIDENT COMMS

## ID

`N11_ACCIDENT_COMMS`

## Placement

L14.

## Medium

Required audio communication with minimal transcript/subtitles.

## Required information

Player hears Seojin directly ask Jaemin to check the situation.

Canonical key line:

> "재민 씨, 이거 제어실에서 한번만 봐줄래요?"

Delivery:
- ordinary,
- slightly inconvenienced/concerned,
- not screaming,
- not foreshadowing death.

## Audio context

Include enough room/radio noise to sound captured from work communications, but keep speech intelligible.

## Narrative function

Turns technical records into a human interaction.

---

# N12 — 39 SECOND COMPOSITE

## ID

`N12_39_SECOND_COMPOSITE`

## Placement

L15 / Reconstruction.

## Medium

Composite of:
- control-console event history,
- reconstructed CCTV behavior,
- surrounding audio/system state.

Not one giant written report.

## Required information

Player learns the precise sequence:

```
22:14:03  Seojin requests help/check
22:14:08  manual release path is available
22:14:08–22:14:47
           no qualifying state-changing control action by Jaemin
22:14:47  remote emergency release requested
```

Within the gap, Jaemin can be seen:
- looking at screens,
- lifting/setting down phone,
- checking another warning,
- looking toward the door,
- hesitating/assessing.

Do not show him frozen motionless for 39 seconds.

## Mechanical integration

Surrounding events have Review traces.

The expected response gap has no RecordedEvent to select.

This artifact is partly the level itself.

## Narrative function

This is the major convergence of story and mechanic.

---

# N13 — INVESTIGATION SUMMARY

## ID

`N13_INVESTIGATION_SUMMARY`

## Placement

Late L15 after the player understands the gap.

## Medium

Short official investigation summary.

## Required information

Player learns:
- accident had multiple technical/design/procedure causes,
- delayed initial emergency action was noted,
- Jaemin was not found in clear violation under the policy then in effect,
- investigators cannot establish that immediate intervention would certainly have changed survival outcome.

## Reading time

15–30 seconds.

## Tone

Dry, cautious, legally/administratively restrained.

Avoid:
- absolving Jaemin emotionally,
- condemning him morally,
- claiming precise medical survival certainty.

## Narrative function

Preserves uncertainty.

The game refuses both:
- "It was entirely his fault,"
- "He did nothing wrong."

---

# N14 — RECORD ZERO STATUS

## ID

`N14_RECORD_ZERO_STATUS`

## Placement

L16.

## Medium

Current archival/correction system status.

## Required information

Player learns factual present state:
- Record 0 exists in the active migration/archive process,
- current system intends to destroy/invalidate it to restore consistency,
- destruction is a state-changing current event/process,
- the record's institutional and physical status can diverge depending on final causal manipulation.

## Mechanical role

This is not merely a document.

It explains enough present system state for the player to understand the final mechanical problem.

## No choice text

Do not show:

```
PRESERVE RECORD
DESTROY RECORD
```

as a moral-choice menu.

The player acts through world systems/corrections.

---

# Artifact media balance

Do not make all fourteen items papers.

Target balance:

- printed/wall administrative: N01, N04
- maintenance/work traces: N02, N03
- roster/table: N05, N06
- current terminal/system: N07, N14
- historical logs/audit: N08, N09, N10
- voice/comms: N11
- environmental composite/reconstruction: N12
- official summary: N13

This gives varied information rhythm.

---

# Required versus optional interaction

Essential artifacts should be:
- directly encountered on critical path,
- automatically foregrounded by necessary progression,
- or incorporated into required system interaction.

Do not require the player to search every desk drawer for N10.

The player may choose not to stare at every word, but the game should provide the information through the route.

---

# Duplicate information policy

Important facts may be lightly reinforced, but do not repeat whole explanations.

Example:

The accident date may appear in:
- N08,
- N09,
- later reconstruction.

That is useful matching evidence.

But do not have three documents all explain:
"Jaemin waited 39 seconds."

The 39-second fact should arrive decisively in N12.

---

# Terminology staging

Early:
- records,
- migration,
- archive,
- system error.

Middle:
- event model,
- correction,
- Record 0.

Late:
- expected transition,
- absent state change,
- archival status.

Do not dump internal system vocabulary in Act I.

---

# Character information staging

Seojin progression:

```
name
→ handwriting/habit
→ job
→ coworker relationship
→ presence at accident
→ voice
→ request
→ death/outcome context
```

Jaemin progression:

```
current worker
→ long tenure
→ worked with Seojin
→ accident duty
→ had authority
→ received request
→ delayed state-changing response
→ later acted
→ uncertain responsibility
```

---

# Optional artifact policy

Optional artifacts may deepen:
- workplace culture,
- Seojin's ordinariness,
- Jaemin's habits,
- post-accident policy changes,
- institutional age.

They may not contain the sole source of:
- who died,
- what Jaemin did,
- Record 0's nature,
- why correction fails at the gap,
- final system stakes.

---

# Editorial style

Institutional text:
- concise,
- procedural,
- specific,
- not sinister for its own sake.

Human notes:
- natural,
- short,
- context-specific.

Dialogue:
- ordinary Korean,
- avoid polished literary speeches.

Technical logs:
- stable vocabulary,
- clear timestamps,
- no fake hacker jargon.

---

# Localization

Every player-facing artifact uses keys from `docs/28_LOCALIZATION.md`.

Source copy is drafted/editorially locked in Korean first for Korean institutional/dialogue authenticity.

English translation is maintained during production.

Do not flatten Korean setting identity to make translation easier.

---

# Implementation metadata

Narrative manifest entries should eventually contain conceptually:

```json
{
  "artifactId": "N09_ISOLATION_LOG",
  "essential": true,
  "revealStage": "L12",
  "presentationType": "SYSTEM_LOG",
  "titleKey": "DOC.N09.TITLE",
  "bodyKeys": [
    "DOC.N09.ROW_001",
    "DOC.N09.ROW_002"
  ]
}
```

Exact schema follows implementation needs, but essential/optional and reveal-stage classification must remain explicit.

---

# Narrative artifact QA

For every essential artifact ask:

1. What exact new fact does this deliver?
2. Is that fact appropriate at this reveal stage?
3. Could the environment/gameplay deliver it more naturally?
4. Is any earlier artifact already saying the same thing?
5. Can it be understood in under ~30 seconds?
6. Does its medium feel plausible in Repository 4?
7. Does it preserve uncertainty where required?
8. Is the player forced to read a wall of prose?
9. Does it accidentally tell the player how to solve a puzzle?
10. Is it localizable/template-driven?

---

# Locked outcome

The fourteen-item structure is the maximum **essential narrative spine** target.

Additional essential artifacts require a demonstrated information gap, not a desire to add lore.

The player should finish UNDO remembering:
- a place,
- a person,
- a request,
- a 39-second absence of action,

not a folder full of exposition.
