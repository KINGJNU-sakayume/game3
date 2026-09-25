# UNDO — Localization Standard

Last locked: 2026-09-25

## Goals

UNDO is set in Korea and should preserve that sense of place while remaining fully localizable.

Localization must be designed into:
- UI,
- subtitles,
- in-world terminals,
- documents,
- signs,
- narrative artifacts,

rather than added after assets are finalized.

## Initial locale scope

Canonical development locales:

- `ko` — Korean
- `en` — English

Korean is the world-setting/source-writing language for Korean institutional material and dialogue.

English is maintained during production as the first complete localization target so layouts and pipelines are tested continuously.

Additional languages are release-scope decisions and are not required during prototypes.

## Runtime language behavior

Default:
- use the user's OS-preferred language if supported,
- otherwise fall back to the project's configured fallback language.

The Settings menu always allows manual language selection.

Do not force Korean merely because the game is set in Korea.

## Localization keys

Player-facing content uses stable semantic keys.

Do not use final English/Korean prose as the identifier.

Good:

```
UI.PAUSE.CONTINUE
UI.PAUSE.RESTORE_RECORD
DOC.N01_MIGRATION_NOTICE.TITLE
DOC.N01_MIGRATION_NOTICE.BODY
SIGN.B2.RESTORATION_ROOM
SUB.ACCIDENT.N11.LINE_003
PUZZLE.PT_B_PERSISTENCE.NAME
```

Bad:

```
CONTINUE_THE_GAME
"계속하기"
"Continue"
```

Keys remain stable when wording changes.

## File format

Use UTF-8 CSV translation sources compatible with Godot's localization importer.

Target layout:

```
game/localization/
├─ ui.csv
├─ subtitles.csv
├─ documents.csv
├─ signage.csv
└─ system.csv
```

Conceptual CSV:

```csv
keys,ko,en
UI.PAUSE.CONTINUE,계속,Continue
UI.PAUSE.RESTORE_RECORD,현재 기록 복원,Restore Current Record
SIGN.B2.RESTORATION_ROOM,기록복원실,Restoration Room
```

CSV files must remain valid UTF-8 and must not be edited in a way that silently changes encoding.

## Separation by domain

### `ui.csv`
- pause,
- settings,
- accessibility,
- confirmation dialogs,
- Review helper prompts.

### `subtitles.csv`
- dialogue,
- radio/communication,
- meaningful voiced lines,
- accessibility captions where authored as text.

### `documents.csv`
- document titles,
- form labels,
- essential/optional narrative prose.

### `signage.csv`
- rooms,
- wayfinding,
- institutional notices represented as dynamic/localizable text.

### `system.csv`
- in-world software,
- event-status wording,
- archive database fields,
- migration status messages.

Do not create a separate CSV for every single level.

## Text ownership

Causal JSON stores localization keys.

Example:

```json
{
  "artifactId": "N01_MIGRATION_NOTICE",
  "titleKey": "DOC.N01_MIGRATION_NOTICE.TITLE",
  "bodyKey": "DOC.N01_MIGRATION_NOTICE.BODY"
}
```

It does not store the final Korean/English paragraph inline.

## Signage strategy

Whenever practical, gameplay/environment signage uses:
- 3D/text nodes,
- dynamic text surfaces,
- template-based document/sign rendering,

rather than rasterizing language directly into a texture.

Benefits:
- localization,
- consistent typography,
- easier correction,
- less duplicate texture production.

If a sign truly requires a baked graphic:
- use localized resource variants/remaps,
- preserve identical layout semantics,
- keep the language asset source editable.

## Korean place identity

Localization does not erase the setting.

Examples:
- names remain Korean personal names,
- location hierarchy remains a Korean institution,
- environmental conventions remain Korean,
- voice language can remain Korean depending on final audio policy.

English UI/sign translations communicate meaning without pretending the building is located elsewhere.

Where appropriate, Korean and English may coexist in-world because bilingual institutional signage is plausible.

Do not force every tiny label to be bilingual if it makes the facility look artificially designed for tourists.

## Personal names

Canonical Korean:
- 한재민
- 윤서진

Canonical Romanization for internal English content:
- Han Jaemin
- Yoon Seojin

Do not vary spelling across files without a deliberate naming decision.

## Institutional naming

Current canonical working name:

```
중앙기록보존원 제4보존소
Central Records Preservation Service — Repository 4
```

This remains a working fictional institution name subject to final editorial/legal review.

Do not automatically translate every internal department literally if a natural English institutional term is clearer.

## Documents

Documents are authored from structured templates.

Separate:
- layout,
- static graphic elements,
- localization keys,
- variable data such as dates/IDs/names.

Do not create a flattened image for a critical document early in production.

Long text should be avoided by narrative design, not "solved" by shrinking fonts.

## Text expansion

English/Korean lengths differ.

Layouts must:
- allow wrapping,
- avoid fixed-width buttons based on one language,
- tolerate approximately 30–50% text expansion where practical,
- support font fallback and glyph coverage,
- avoid text embedded into decorative UI shapes.

This reinforces the project's non-card-based UI direction.

## In-world terminals

Terminal data tables should localize:
- field labels,
- status text,
- explanatory messages.

Stable IDs and technical record identifiers generally remain unchanged.

Example:

```
EVENT ID          E-0421
시간 / TIME       22:14:47
상태 / STATUS     ...
```

Exact bilingual presentation depends on terminal era and scene context; do not make every screen bilingual by default.

## Dates and times

Narrative evidence must use a consistent canonical timestamp internally.

Presentation may localize date formatting where doing so does not obscure narrative comparison.

For critical accident/event comparisons, preserve a format that makes chronological matching unambiguous.

Use 24-hour time in institutional logs.

## Numbers / IDs

Record IDs, EntityIds, Event IDs, room codes, and PuzzleIds are not localized.

Player-facing explanatory labels around them are localized.

## Subtitles

Subtitle system requirements:
- speaker label where needed for clarity,
- configurable text size,
- strong contrast,
- multiple-line wrapping,
- no critical information delivered only through color,
- support for environmental/radio voice.

Subtitle timing follows spoken meaning rather than exposing internal audio file chunks.

## Environmental captions

Puzzle-critical sounds must have a non-audio path to understanding.

Where captions are useful, describe function rather than cinematic prose.

Prefer:

```
[보안문 잠금 해제]
[엘리베이터 도착]
```

over:

```
[멀리서 금속이 불길하게 울린다]
```

Exact caption verbosity can be an accessibility option if needed.

## Font policy

Exact font families are not yet locked.

Requirements:
- complete Korean glyph coverage,
- readable Latin/numerals,
- clear small-size institutional UI,
- licensing suitable for commercial redistribution,
- separate role consideration for modern UI, legacy terminal, printed forms.

Do not choose a font solely because it appears "futuristic."

## Voice localization

Prototype/vertical slice does not require dubbed English voice.

Canonical production priority:
1. Korean voice performance fits the setting.
2. All required spoken information has localized subtitles.
3. Additional dubs are a later budget decision.

Do not design gameplay that requires understanding spoken Korean without subtitles.

## Translation QA

At minimum test:
- Korean,
- English,
- longest-string stress cases,
- missing-key fallback,
- line wrapping,
- subtitle overlap,
- document readability,
- terminal tables,
- signs viewed at intended gameplay distance.

Godot's language preview/runtime override should be used during production.

## Missing keys

Development builds:
- missing keys must be obvious in logs/debug presentation,
- do not silently substitute unrelated prose.

Release:
- configured fallback may display where a translation is genuinely absent,
- localization completeness validation should prevent known required missing keys before release.

## Source control

Translation source CSVs are committed.

Generated/imported translation cache artifacts follow Godot's normal import/cache rules and are not manually edited.

A wording change should produce a clean text diff.

## AI-generated text policy

Do not bulk-generate final institutional Korean/English text and ship it without editorial review.

Particularly review:
- government/administrative terminology,
- safety notices,
- accident reports,
- human dialogue.

Consistency and natural language matter more than volume.

## Content validation

Narrative/content validation should check that every required localization key referenced by:
- causal JSON,
- narrative manifest,
- UI config,

exists in the source localization tables.

Unused-key reporting is desirable but is a warning rather than an initial hard failure.

## Prototype scope

Prototype A/B/C:
- localization architecture/key usage exists,
- full bilingual content is not required for every placeholder.

Vertical slice:
- Korean and English must both be complete for all player-facing slice text,
- layout/Review/document/signage must be tested in both languages.
