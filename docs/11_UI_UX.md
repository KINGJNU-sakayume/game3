# UNDO — UI / UX

## UI philosophy

> The player should primarily read the world, not a HUD.

UI is divided into:
1. **World layer** — physical signs, documents, terminals, objects.
2. **Correction layer** — state traces visible through the correction phenomenon.
3. **System UI** — pause/settings/title screens.

These layers must remain visually distinct.

## Default HUD

Almost none.

Permitted:
- very small central reticle if necessary.

Not displayed persistently:
- health,
- minimap,
- objectives,
- inventory,
- Undo meter,
- slot counter,
- quest log.

## Residual Exposure

Internal name for the core correction visualization: **Residual Exposure / 잔류노출**.

An eligible object's previous recorded state appears as a faint double exposure spatially overlapping the current object.

It should resemble photographic or film double exposure, not a digital hacking effect.

Do not use:
- whole-object glowing outlines,
- RGB split,
- scanline overlays,
- holographic grids,
- neon cyan/magenta,
- generic sci-fi circles,
- AI-analysis waveforms.

## Review Mode

Flow:

```
Notice
→ Inspect
→ Commit
```

Holding/focusing Review on an eligible target slows the world over a short transition and then pauses it for event inspection.

Review is a cognitive interface, not an in-world time-stop superpower.

The target remains in the scene. Historical states are shown spatially on the object itself.

Minimal metadata may appear:
- timestamp,
- simple state term,
- later, a source identifier where useful.

Do not show internal developer schema.

## Selecting events

Scroll / left-right input cycles recorded states.

Feedback:
- subtle mechanical tick,
- residual state changes,
- minimal text.

No floating timeline card.

## Correction commit

When an event is suppressed:
- current and prior states briefly overlap,
- the suppressed state's visual contribution settles,
- a restrained mechanical/paper-like sound confirms the change.

No "UNDO SUCCESSFUL" toast.

## Maintained corrections

A currently suppressed event leaves a stable residual double image distinguishable from merely eligible history.

Capacity is primarily communicated through maintained world traces, not a permanent 1/3 HUD.

If capacity is full, attempting another correction briefly emphasizes currently maintained traces and provides restrained audio feedback.

## System menus

System menus may be non-diegetic but should retain the project's graphic restraint.

Avoid:
- glass panels,
- large rounded cards,
- pills,
- gradient surfaces,
- floating dashboard layouts.

Pause layout is simple vertical text, e.g.:
- CONTINUE
- RESTORE CURRENT RECORD
- SETTINGS
- EXIT TO TITLE

## Title screen

Graphic language resembles an institutional record cover rather than a cinematic sci-fi console.

Title remains simply:

**UNDO**

"NEW RECORD" may replace "New Game"; ordinary usability terms such as Settings should remain ordinary.

## Documents

Paper is inspected as paper, close to the camera or on a surface.

Electronic records are viewed on actual in-world terminals.

Readability overrides literal realism:
- larger text than a real tiny government form,
- strong contrast,
- reasonable line spacing.

## In-world software

Contemporary systems:
- neutral office software,
- tables,
- rows,
- checkboxes,
- rectangular buttons,
- restrained hierarchy.

Legacy systems:
- older typography,
- denser tables,
- monospace where appropriate.

Do not beautify the software into a modern SaaS dashboard.

## Accessibility

Correction eligibility cannot rely on color alone.

Communicate through:
- shape/double exposure,
- motion/parallax,
- sound,
- restrained color.

Planned options:
- Correction visibility strength,
- reduced camera motion,
- motion blur off,
- head bob off,
- subtitle sizing,
- minimal/always prompts,
- instant Review pause,
- hold/toggle alternatives,
- FOV control,
- full remapping.

## Final narrative UX

At the 39-second Record 0 gap:
- no residual state,
- no timestamp selection,
- no tick,
- no event to commit.

At the final exterior door:
- no Residual Exposure,
- no Review interaction.

The absence of interface is itself narrative information.
