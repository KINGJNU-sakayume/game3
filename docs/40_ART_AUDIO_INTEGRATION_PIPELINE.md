# UNDO — Art and Audio Integration Pipeline

Last locked: 2026-09-25

## Purpose

Art and audio are essential to UNDO's identity and causal readability, but they must enter production at the right time.

This document defines how presentation work integrates without becoming the gameplay authority.

## Principle

> Presentation makes semantic state legible; it does not define semantic state.

If the semantic model says:

```
DOOR_A.LOCK = UNLOCKED
```

then animation, mesh, indicator, sound, and collision presentation should communicate that fact.

A pretty animation state cannot override the domain value.

---

# 1. Environment art stages

## Stage A — Graybox

Goals:
- scale,
- route,
- observation,
- interaction spacing,
- semantic anchors.

Materials:
neutral placeholders.

No decorative clutter.

## Stage B — Kit blockout

Replace broad surfaces with modular kit proxies:
- wall family,
- floor family,
- door frame family,
- archive shelving,
- desk/service modules.

Still prioritize geometry and sightlines.

## Stage C — Material/era pass

Apply:
- era-appropriate surfaces,
- wear,
- fixtures,
- institutional colors,
- lighting hardware.

Do not add high-frequency clutter yet.

## Stage D — Readability pass

Before dressing:
- verify gameplay object silhouettes,
- lock indicators,
- badge/scanner visibility,
- NPC contrast against background,
- Residual Exposure contrast.

## Stage E — Dressing

Add:
- office props,
- paper,
- tape,
- carts,
- boxes,
- personal traces,
- maintenance detail.

Maintain exclusion zones around:
- event commit interactions,
- key sightlines,
- important floor/door state indicators.

## Stage F — Lighting

Lighting supports:
- era,
- mood,
- spatial hierarchy,
- state readability.

Do not darken an area merely to make it atmospheric if it obscures causal behavior.

---

# 2. Door pipeline

Doors are priority assets.

Every gameplay Door asset needs:

### Semantic support
- OPEN,
- LOCK where relevant.

### Presentation states
- fully open,
- fully closed/latch,
- locked indicator if applicable,
- unlocked indicator if applicable.

### Animation
- open,
- close,
- lock/unlock if visibly separate.

### Commit markers
- CLOSE event at latch/closed semantic point,
- OPEN event at defined open semantic point,
- LOCK/UNLOCK at actuator/indicator point.

### Audio hooks
- handle,
- movement,
- latch,
- lock actuator.

### Correction support
- prior/current transform/state usable by Residual Exposure.

Do not author a door whose visual animation makes commit timing ambiguous.

---

# 3. Portable object pipeline

Examples:
- badge,
- archive box,
- document folder.

Semantic state determines:
- possession,
- semantic location.

Presentation binder handles:
- world anchor,
- NPC hand/body attachment,
- desk/locker slot,
- transition animation.

Avoid free physics for puzzle-critical placement.

A Badge should not bounce under a desk and become inaccessible because physics simulation won.

---

# 4. NPC art/animation integration

Behavior remains semantic.

Animation layer receives intents.

Example:

```
Intent TAKE BADGE
→ navigate/reach interaction point
→ play reach animation
→ event commit marker
→ binder changes Badge possession
→ animation exits
```

Animation cannot independently decide the TAKE succeeded.

## Idle variation

Cosmetic idle variation is allowed only when it does not:
- delay a critical event unpredictably,
- rotate actor away from important readable action,
- move actor across semantic boundaries,
- interfere with route timing materially.

## Look-at behavior

Subtle look/attention can improve intent readability.

Use for:
- target object before interaction,
- route destination,
- anomaly inspection.

Avoid uncanny constant player-tracking.

---

# 5. Residual Exposure integration

Residual Exposure needs access to:
- current presentation state,
- previous/reviewed semantic presentation state.

Implementation may use:
- duplicated/ghosted mesh,
- material/shader pass,
- captured transform/state representation,

but it must produce the design outcome:
- photographic double exposure,
- restrained amber,
- slight spatial/parallax separation,
- no neon outline,
- no glitch aesthetic.

## Per-object authoring

A presentation binder should declare how a state is represented.

Examples:

Door:
- ghost previous door transform.

Badge:
- ghost at previous semantic anchor.

Light:
- state may require luminance/fixture-state representation rather than duplicate geometry.

Authorization:
- institutional display/indicator history may be more appropriate than floating hologram.

Do not force one shader trick onto every state channel.

---

# 6. Audio semantic hooks

Gameplay events emit semantic audio requests through presentation/application mapping.

Examples:

```
Door CLOSE committed
→ door family close/latch event

Scanner AUTHORIZE committed
→ scanner accepted family

Correction commit
→ correction tactile event

Correction release
→ correction release event
```

Do not have domain code reference WAV/OGG paths.

Audio binder/profile maps semantic cue to assets.

---

# 7. Sound-family variation

A semantic sound family may have controlled random variation in sample/pitch where:
- it does not obscure event identity,
- it cannot affect puzzle logic.

Example:
footstep variation is fine.

Authorization grant versus denial must remain distinctly recognizable.

Random variation is presentation-only.

---

# 8. Commit synchronization

For state-changing interactions, audio and visual feedback must cluster around the semantic event commit.

Bad:
- unlock sound occurs 1 second before the domain UNLOCK event,
- player sees door open before event exists,
- scanner green light appears while authorization still denied.

Good:
- domain commit and perceptual confirmation align closely.

Small animation lead-in is fine.

The causal moment must feel coherent.

---

# 9. Correction sound/visual synchronization

Commit flow:

```
Player confirms correction
→ domain suppression applied
→ effective state changes
→ presentation transition begins
→ correction tactile sound confirms
```

Do not play the sound first and then discover domain validation failed.

If suppression is rejected:
- use restrained invalid/capacity feedback,
- do not show the full correction state transition.

---

# 10. Era-specific presentation

The same semantic function can have era-specific visual/audio implementation.

Example:

AUTHORIZATION:

B1:
- electronic reader,
- compact beep,
- modern latch.

B3:
- older reader/control,
- relay clack,
- heavier actuator.

Gameplay vocabulary stays consistent.

Presentation makes era distinct.

---

# 11. Signage/document integration

Use source templates.

Pipeline:

```
localization key/data
→ layout template
→ rendered/in-world text
→ distance/readability QA
```

Critical text remains editable.

Do not create final signs by baking AI-generated fake text into textures.

## Era differences

Modern:
- cleaner laser-printed signage.

Older:
- laminated labels,
- older print layouts,
- dot-matrix/typed forms,
- stamps/handwriting.

Differences should remain coherent with one institution.

---

# 12. Handwriting

Yoon Seojin's handwriting is a recurring identity asset.

Create one controlled handwriting style/source set.

Use sparingly on:
- equipment note,
- maintenance annotation,
- small work reminder.

Do not turn it into a collectible scavenger hunt.

Localization strategy for handwriting requires an explicit layout/source solution rather than generative fake writing.

---

# 13. Lighting and correction

Residual Exposure must pass readability tests in:
- bright office fluorescent,
- neutral archive aisle,
- greenish/older industrial light,
- lower-light service space.

If correction visibility only works in one lighting condition, fix the presentation system.

Accessibility visibility-strength option must remain functional across lighting profiles.

---

# 14. Audio mix priorities

At each puzzle moment:

```
critical event transient
> nearby actor cue
> relevant machine state
> ambience
> music
```

This is a mix hierarchy, not a requirement that every cue be loud.

Use spectral/temporal separation, not just volume.

---

# 15. Music integration

Music is attached to:
- traversal,
- act transition,
- narrative beat,
- late reconstruction,

not individual puzzle correctness.

Do not trigger:
"puzzle solved music"
after routine-level success.

Music should duck/yield where a causal cue matters.

---

# 16. Asset naming

Production asset paths should be predictable.

Conceptual examples:

```
game/art/environment/doors/
game/art/environment/archive/
game/art/characters/
game/audio/sfx/doors/
game/audio/sfx/correction/
game/audio/ambience/
game/resources/presentation/
```

Use stable functional names rather than:
- final_final_door2,
- newbadge,
- goodscan.

Exact naming convention may be refined during bootstrap.

---

# 17. Asset source/license metadata

Any third-party asset must record:
- source,
- creator/vendor,
- license,
- purchase/date/reference if relevant,
- attribution requirement,
- modification notes.

Do not postpone license tracking until release.

A machine-readable registry can be added when assets first enter the repository.

---

# 18. Generated reference usage

AI-generated images may help:
- mood,
- architectural reference,
- composition exploration.

Before production use, translate references into:
- explicit materials,
- dimensions,
- kit rules,
- lighting,
- graphic standards.

Do not hand a generated image directly to implementation as the sole source of truth.

The canonical art direction remains written/systematic.

---

# 19. Presentation regression checklist

After final art/audio integration into a puzzle:

- Can player still see each event commit?
- Are state differences readable?
- Is Residual Exposure visible but restrained?
- Are actor routes understandable?
- Is any decorative object mistaken for actionable state?
- Are critical sounds audible?
- Did final animation alter timing?
- Did collision/navmesh alter route?
- Are localized signs/documents readable?
- Does scene still fit institutional/era identity?
- Does it accidentally read as horror/SF lab?

If causal readability decreases, presentation is not finished.

---

# 20. Rule

> Final-quality presentation is successful when the player stops noticing the interface and starts noticing the cause-and-effect relationships.
