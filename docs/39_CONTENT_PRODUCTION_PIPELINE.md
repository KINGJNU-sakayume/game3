# UNDO — Content Production Pipeline

Last locked: 2026-09-25

## Purpose

This document defines the lifecycle for every major gameplay space, narrative artifact, and production-ready puzzle.

The goal is to prevent:
- coding before the causal design is understood,
- art before the graybox is readable,
- level-specific scripting replacing the core systems,
- polish being used to hide rule confusion,
- content status becoming ambiguous across ChatGPT/Codex/human work.

## Core pipeline

Every major puzzle follows:

```
CONCEPT
→ AUTHORED
→ VALIDATED
→ GRAYBOX
→ PLAYTESTED
→ ART_READY
→ INTEGRATED
→ CONTENT_LOCK
→ POLISH
```

Skipping a gate requires an explicit documented reason.

---

# 1. CONCEPT

## Inputs

- `docs/24_CONTENT_REGISTRY.md`
- progression role,
- intended mechanic,
- narrative beat,
- correction capacity,
- known asset families.

## Required output

Create/update a puzzle design using:
`docs/34_PUZZLE_AUTHORING_TEMPLATE.md`

At minimum define:
- player-facing objective,
- initial state,
- actors,
- normal routine,
- event chain,
- intended causal insight,
- intended solution,
- observation plan,
- reset,
- tests,
- playtest questions.

## Gate

Do not leave CONCEPT if:
- solution requires an undefined mechanic,
- NPC knowledge is unexplained,
- event chain cannot be written explicitly,
- completion state is not semantic,
- the puzzle is only "more objects" without a new/composed insight.

---

# 2. AUTHORED

## Purpose

Convert the design into canonical content definitions before detailed scene logic.

## Deliverables

- causal `puzzle.json`,
- stable EntityIds,
- LocationIds,
- actors/routines,
- reaction rules,
- initial semantic state,
- completion predicates,
- localization keys where necessary,
- registry status update.

## Rule

The JSON describes the intended causal game.

The Godot scene does not get to invent missing behavior.

## Gate

Content must be representable with the current canonical vocabulary.

If not:
1. determine whether a reusable core concept is genuinely missing,
2. revise canonical docs/system deliberately,
3. only then extend the schema/interpreter.

Do not insert a level-specific C# workaround first.

---

# 3. VALIDATED

## Required checks

Content validator passes:
- IDs,
- references,
- channel/value types,
- predicates,
- routines,
- localization keys,
- completion rules.

Required domain fixture/tests exist for any new causal behavior.

## Deliverable

A semantic content definition that can be inspected without opening Godot.

## Gate

No validation errors.

Warnings must be understood and either accepted or fixed.

---

# 4. GRAYBOX

## Purpose

Build the minimum Godot scene necessary to test:
- causal flow,
- spatial readability,
- timing,
- NPC path execution,
- Review targeting,
- sound cue position,
- reset.

## Graybox art rule

Use primitives, simple placeholder materials, and temporary actors.

Allowed:
- boxes,
- simple doors,
- color-neutral placeholder surfaces,
- temporary labels,
- placeholder sounds.

Do not spend time on:
- final materials,
- detailed props,
- decorative clutter,
- cinematic lighting,
- final character art.

## Required bindings

- every gameplay EntityId,
- every semantic LocationId,
- player spawn/restore anchors,
- navigation,
- event presentation binder,
- debug overlay.

## Gate

The entire intended solution must work repeatedly from reset.

If Graybox requires decorative cues to become understandable, first improve geometry/logic.

---

# 5. GRAYBOX REVIEW

Before external playtest, internal review checks:

### Causal
- normal routine is correct,
- event commits happen at understandable moments,
- suppression yields correct state,
- reactions fire only at valid observation points,
- completion facts match design.

### Spatial
- observation anchors function,
- routes are legible,
- important objects are visible,
- player cannot bypass puzzle through geometry/collision.

### Timing
- routine cycle is acceptable,
- execution window is fair,
- known solution does not require excessive waiting.

### Reset
- stable,
- deterministic,
- no residual NPC/event state.

If this review fails, remain in GRAYBOX.

---

# 6. PLAYTESTED

## Purpose

Test player mental model, not just technical correctness.

Use:
`docs/32_PLAYTEST_PROTOCOL.md`

## Required evidence

Record:
- comprehension,
- predictions,
- interventions,
- restores,
- timing friction,
- UI confusion,
- unexpected solutions,
- perceived bugs.

## Outcome categories

### PASS
Intended mental model works and remaining issues are normal tuning/polish.

### REVISE
Core concept valid but:
- sightline,
- timing,
- cue,
- route,
- presentation

needs iteration.

Return to GRAYBOX.

### DESIGN_REVIEW
Players consistently form the wrong rule or the puzzle requires arbitrary explanation.

Return to CONCEPT/AUTHORED.

### TECH_REVIEW
Wrong state, non-determinism, reset corruption, reaction bug.

Fix system/content before further UX conclusions.

---

# 7. ART_READY

A puzzle becomes ART_READY only when:

- intended solution is stable,
- event/state behavior is tested,
- observation positions are known,
- required silhouettes/state differences are known,
- lighting constraints are documented,
- audio cue needs are documented,
- no large geometry changes are expected.

## Art brief

Before art pass, create a short asset/layout brief containing:
- environment kit(s),
- unique assets if justified,
- gameplay object state readability,
- clutter exclusion zones,
- signage needs,
- narrative prop placement,
- lighting requirements,
- era identity.

## Rule

Art can refine spacing modestly.

Art cannot silently:
- block a sightline,
- move a semantic anchor,
- obscure a state indicator,
- add a false interactable cue.

Any substantial gameplay-space change returns to Graybox review.

---

# 8. INTEGRATED

## Purpose

Replace placeholders with representative/final assets while preserving the tested causal experience.

Integrate:
- modular environment art,
- character models,
- animations,
- final/representative audio,
- signage,
- documents,
- lighting,
- Residual Exposure,
- Review presentation.

## Required regression

After integration rerun:
- domain tests,
- content validation,
- scene-binding validation,
- reset,
- full intended solution,
- key alternate player actions.

Then playtest readability again.

## Common integration regressions

Watch specifically for:
- darker art hiding NPC action,
- props blocking badges/indicators,
- final animation moving event commit perceptually,
- audio ambience masking latch/scanner,
- decoration looking like a gameplay signal,
- collision/navmesh changes altering actor routes.

---

# 9. CONTENT_LOCK

A puzzle reaches CONTENT_LOCK when:

- causal design is final,
- geometry is final except bug fixes,
- narrative content is final in structure,
- asset list is stable,
- localization keys are stable,
- intended solution and accepted alternatives are documented,
- no new mechanic is planned.

After lock:
- fix bugs,
- tune,
- optimize,
- improve accessibility/readability.

Do not redesign the puzzle casually.

---

# 10. POLISH

Polish includes:
- final animation timing,
- audio mix,
- material/lighting quality,
- subtle NPC idle behavior that cannot change puzzle outcomes,
- document typography,
- accessibility tuning,
- performance optimization,
- small prompt wording.

Polish does not include:
- adding a new system,
- changing the core solution,
- introducing a new random behavior,
- replacing tested spatial flow.

Those require reopening the appropriate earlier stage.

---

# 11. Status tracking

Each major puzzle in `docs/24_CONTENT_REGISTRY.md` should eventually track a status from:

```
CANON_CONCEPT
AUTHORED
VALIDATED
GRAYBOX
PLAYTEST_REVISION
ART_READY
INTEGRATED
CONTENT_LOCK
POLISH
DONE
```

Prototype/vertical-slice-specific labels may coexist where useful.

## Rule

Status reflects evidence, not optimism.

A scene containing final-looking models is not ART_READY if the causal Graybox never passed.

---

# 12. Codex task decomposition by stage

## AUTHORED task

Good:
> Implement PZ_X semantic JSON and validator/tests from its approved puzzle sheet. Do not create the Godot level scene.

## GRAYBOX task

Good:
> Create the minimal Godot graybox and bind existing semantic content. Do not import final environment art.

## SYSTEM task

Good:
> Add one reusable predicate required by two approved puzzle designs, with tests and schema validation.

Bad:
> Build Level 8 with final art, new AI, new event rules, and sound.

---

# 13. Narrative artifact pipeline

Narrative items have a simpler path:

```
INFORMATION_NEED
→ DRAFT
→ EDITORIAL_LOCK
→ LOCALIZED
→ TEMPLATE_INTEGRATION
→ IN_SCENE_QA
```

## INFORMATION_NEED

State exactly what the player must learn.

## DRAFT

Write concise Korean source copy.

## EDITORIAL_LOCK

Check:
- factual consistency,
- natural institutional tone,
- reveal ladder,
- no repeated exposition.

## LOCALIZED

English and any later locale translations.

## TEMPLATE_INTEGRATION

Render through the appropriate:
- paper form,
- terminal,
- sign,
- label.

## IN_SCENE_QA

Verify:
- readable at intended distance,
- correct era/style,
- correct localization,
- not mistaken for a correction cue.

---

# 14. Asset-request pipeline

A unique asset request requires:

```
Need
→ Reuse check
→ Gameplay/narrative justification
→ Registry entry
→ Brief/reference
→ Production
→ Integration
→ License/source record
```

Before commissioning/creating an asset, check `docs/25_ASSET_REGISTRY.md`.

If an existing kit can satisfy the need without harming readability/world identity, reuse it.

---

# 15. Audio production pipeline

For a puzzle-critical sound:

```
Semantic function
→ placeholder cue
→ playtest readability
→ sound-family assignment
→ final production
→ mix test in level
```

Do not final-produce an elaborate sound before the event/commit point is stable.

---

# 16. Level-specific code exception policy

A major warning sign is:

```
if (PuzzleId == "PZ_...")
```

inside shared causal systems.

Before accepting a puzzle-specific code path, ask:

1. Is this presentation only?
2. Is it a reusable application concept?
3. Is the content schema missing a legitimate reusable rule?
4. Is the puzzle violating the game's existing grammar?

A one-off content behavior may sometimes require a local presentation script, but it must not bypass causal authority.

Document any accepted exception.

---

# 17. Parallel work

Parallel production is permitted only after dependencies are stable.

Examples:

Can parallelize:
- environment kit art after ART_READY,
- audio family production after functional cues stable,
- localization after source copy lock,
- unrelated puzzle grayboxes using stable systems.

Do not parallelize:
- three levels all depending on an unproven new ReactionRule,
- final art for geometry still changing,
- localization for narrative copy still being rewritten daily.

---

# 18. Review ownership

At each gate:

### Design review
Does this fit UNDO?

### Technical review
Does this obey causal architecture?

### Level review
Can the player perceive/predict it?

### Presentation review
Does art/audio/UI preserve readability and identity?

### QA
Can it break, softlock, desync, or restore incorrectly?

An AI agent may assist every review, but passing one category does not imply the others passed.

---

# 19. Definition of content done

A campaign puzzle is DONE when:

- canonical design matches implementation,
- semantic content validates,
- automated tests pass,
- scene bindings pass,
- full solution/reset works repeatedly,
- intended mental model passed playtesting,
- art/audio/UI integrated,
- localization complete for required locales,
- accessibility issues addressed,
- performance acceptable,
- registry status updated,
- no known P0/P1 issue remains.

---

# 20. Golden rule

> Never use production polish to postpone discovering whether the puzzle is actually understandable and fun.
