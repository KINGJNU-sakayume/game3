# UNDO — Asset Registry and Budget

## Purpose

This document controls asset scope and reuse before full production.

Counts below are planning targets, not a promise to create every listed variant. New asset families require a gameplay, narrative, or visual-identity justification.

## Asset strategy

UNDO should look specific through:
- coherent kits,
- era layering,
- signage/documents,
- lighting,
- material variation,
- restrained character/environment detail,

not through hundreds of unique hero assets.

## Environment kits

### KIT_ENV_ADMIN_CURRENT
Use: Ground / present-day administration  
Target components:
- modular wall/ceiling/floor set,
- contemporary office doors,
- desks,
- basic chairs,
- filing/storage,
- printer/copier,
- current signage mounts,
- current light fixtures.

### KIT_ENV_ARCHIVE_2000S
Use: B1  
Target components:
- steel archive shelving,
- archive carts,
- gray storage cabinets,
- fluorescent ceiling kit,
- vinyl/linoleum floor variations,
- 2000s staff doors,
- archive boxes/containers.

### KIT_ENV_RESTORATION_1990S
Use: B2  
Target components:
- restoration benches,
- industrial cabinetry,
- service conduits,
- control panels,
- conveyor modules,
- scanner housing,
- ventilation/equipment modules.

### KIT_ENV_RESTRICTED_1980S
Use: B3  
Target components:
- heavier security doors,
- analog/legacy panels,
- older wall/ceiling finish variants,
- robust access hardware,
- observation/service windows,
- older safety signage.

### KIT_ENV_DEEP_ARCHIVE_EARLY
Use: B4  
Target components:
- raw/painted concrete,
- relay cabinets,
- exposed service infrastructure,
- legacy racks,
- older lighting,
- heavy utility doors.

### KIT_ENV_RECORD_ZERO
Use: B5/Record 0  
Not a wholly separate fantasy kit.

Construct from controlled coexistence of pieces from the above eras plus a small number of transition/legacy-specific assets.

## Door families

Doors are mechanically and thematically central.

Planning families:
- D01 current staff office door,
- D02 archive fire/security door,
- D03 staff access-controlled door,
- D04 service/industrial door,
- D05 heavy B3 isolation door,
- D06 final exterior/entry door.

Variants may share rigs/logic where practical.

Every gameplay door needs clear presentation states for:
- open/closed,
- locked/unlocked where relevant,
- correctable history visualization,
- consistent latch/audio binding.

## Gameplay devices

Reusable families:
- badge/keycard reader,
- archive scanner,
- wall switch/relay control,
- proximity/optical sensor,
- alarm indicator/panel,
- CCTV camera,
- elevator/service lift control,
- conveyor,
- restoration machine,
- cabinet/locker/archive drawer,
- environmental control panel.

Do not invent a unique futuristic gadget for each puzzle.

## Portable semantic objects

Target reusable categories:
- staff badge,
- physical key where needed,
- archive box,
- document folder,
- media container,
- maintenance item if mechanically justified.

The player does not carry an inventory. These are actor/world state objects.

## Office dressing

Reusable set:
- mugs,
- pens,
- trays,
- folders,
- keyboards/mice,
- telephones across eras,
- clocks,
- stationery,
- labels,
- tape,
- binders,
- small personal desk objects.

Dressing should support realism without obscuring gameplay objects.

## Characters

### PLAYER_JAEMIN
First-person.
Initial needs:
- hands,
- sleeves,
- minimal first-person document interaction rig if used.

Do not commit to full-body first-person production unless testing proves necessary.

### NPC modular bases

Target a small modular pool rather than many hero characters.

Functional visual categories:
- office/archive staff,
- security guard,
- maintenance/restoration worker,
- cleaning/facility support.

Use clothing/head/hair/age variations to create believable staffing.

### Yoon Seojin

Historical representation requires:
- staff photo/record appearance,
- limited reconstructed silhouette/figure presentation where needed,
- voice performance.

Do not require a high-complexity cinematic hero-character pipeline unless the final reconstruction direction changes.

## Animation budget

Core reusable animation families:
- walk,
- idle/work idle,
- turn,
- door interact,
- badge take/place,
- scanner use,
- inspect object,
- panel/button/relay use,
- carry/archive work,
- look/check,
- maintenance inspect,
- contextual emergency response.

Puzzle-critical action animations require explicit event commit markers.

Avoid large cinematic mocap scope.

## UI / graphic assets

Institutional system:
- Repository 4 mark/word treatment if used,
- signage templates by era,
- door/room numbering system,
- safety notice templates,
- archive label templates,
- forms/report templates,
- terminal UI components,
- old/current system typography rules.

Correction layer:
- Residual Exposure shader/material/presentation system,
- minimal Review metadata,
- correction focus/maintained states.

Do not use generative images as final signage containing critical text.

## Narrative document templates

Required families:
- current migration notice,
- maintenance/inspection form,
- staff roster,
- access log,
- system fault log,
- incident summary,
- archival-status record,
- handwritten annotation layer.

Templates should support localization.

## Audio asset families

See `docs/13_AUDIO_DIRECTION.md`.

Priority reusable families:
- door/latch,
- badge/scanner,
- footsteps by surface/footwear category,
- relay/control,
- elevator/service lift,
- conveyor/machines,
- ventilation,
- fluorescent/electrical room tone,
- correction/Review interaction,
- paper/document handling.

## VFX / shader scope

Core:
- Residual Exposure,
- subtle state overlap/parallax,
- restrained Review transition.

Optional/secondary:
- dust/air particles where realistic,
- machinery effects,
- minor environmental steam/air only if justified.

No large generic glitch/VFX library is required.

## Lighting assets

Reusable:
- fluorescent fixtures by era,
- current office/LED variant,
- emergency fixture,
- service/work lamp,
- exterior dawn light setup.

Lighting variation should come from fixture/material/configuration reuse rather than bespoke fixtures per room.

## Acquisition policy

Assets may be:
- created in-house,
- purchased/licensed,
- sourced from permissive libraries,
- modified from legitimate licensed sources.

Every third-party asset requires:
- source,
- license,
- attribution requirement,
- modification permission,
- proof/record of acquisition where relevant.

Maintain a later machine-readable license registry before release.

## AI-generated reference policy

Generated imagery may support:
- concept exploration,
- mood/reference boards,
- rough composition ideation.

Do not ship raw generated:
- signage text,
- critical documents,
- logos/marks requiring consistency,
- UI screens with fake text.

Final production assets require intentional layout and consistency with the institutional graphic standard.

## Scope-control rule

Before creating a unique asset, ask:
1. Can an existing kit/variant communicate this function?
2. Does the player need this difference to read gameplay?
3. Does it materially support era/world/narrative?
4. Will it recur enough to justify production cost?

If no, reuse existing assets.

## Registry expansion

When implementation begins, convert major asset entries into a trackable registry with:
- AssetId,
- category,
- owner/source,
- status,
- license,
- levels used,
- dependencies,
- final path.
