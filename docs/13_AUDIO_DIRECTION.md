# UNDO — Audio Direction

## Audio thesis

Sound is a gameplay information channel before it is atmosphere.

The player should be able to learn routines from:
- footsteps,
- doors,
- relays,
- elevators,
- scanners,
- badge readers,
- ventilation,
- conveyors,
- alarm states,
- distant equipment.

The mix must make causal changes legible without turning every event into a gamey notification.

## Overall tone

The facility sounds maintained, occupied, and mechanically ordinary.

Primary sonic materials:
- fluorescent ballast hum,
- HVAC,
- relay clicks,
- steel door latches,
- archive carts,
- office equipment,
- paper,
- hard-soled and soft-soled footsteps,
- elevator motors,
- older fans,
- distant mechanical resonance,
- room-specific electrical noise.

Avoid constant horror drones, jump-scare stingers, aggressive sub-bass, and synthetic sci-fi ambience.

## Audio hierarchy

During normal play:

1. immediately relevant causal event,
2. nearby actor movement,
3. interactive machine state,
4. room tone,
5. music.

Music must not mask puzzle information.

## Event signatures

Repeated functional events require stable audible signatures.

Examples:
- a specific latch family should sound consistently related,
- badge authorization and denial must be distinguishable,
- elevator arrival must be recognizable before line of sight,
- alarm levels must have clearly different cadence,
- a conveyor entering or leaving motion must be audible.

Do not make every object unique. Use recognizable sound families with room/material variations.

## NPC footsteps

Footsteps are mechanical information.

Requirements:
- different surface materials are readable,
- distance and occlusion remain useful,
- key NPC categories may have modestly distinct footwear/rhythm,
- avoid exaggerated character-specific "hero" footsteps.

The player should sometimes infer an actor's route without seeing them, but critical first-time teaching moments should remain visually verifiable.

## Off-screen events

No puzzle-critical event should occur only off-screen without sufficient telegraphing.

If an important event can occur outside view, it needs one or more of:
- stable audible signature,
- visible downstream state,
- Review history,
- predictable routine.

## Correction / Undo sound language

Correction is not magic and not a digital hack.

Preferred material palette:
- short relay action,
- film transport stop/start,
- paper friction,
- mechanical registration click,
- subdued tonal resonance.

A correction commit should be brief and tactile.

Avoid:
- laser sweeps,
- sparkling chimes,
- bass drops,
- reverse-cymbal clichés,
- obvious tape-rewind effects,
- synthetic "AI" confirmation sounds.

## Residual Exposure audio

Eligible event history may have a very subtle local acoustic artifact when focused, but this should not become constant sonar.

Possible qualities:
- faint pre-echo,
- duplicated transient,
- slight temporal smear,
- low-level mechanical tick.

The cue must support visual Residual Exposure rather than replace observation.

## Review Mode

Entering Review:
- world sounds decelerate with the visual slowdown,
- transient events settle into a near-paused sound field,
- target selection remains audible through restrained ticks.

While paused:
- no dramatic mute,
- retain a thin room-tone floor so the interface does not feel like a separate cyberspace menu.

Cycling events:
- consistent dry mechanical tick,
- optional subtle paper/film texture,
- no melodic UI scale.

Leaving Review:
- ambience resumes naturally without a cinematic whoosh.

## Maintained corrections

A maintained correction should not produce a permanent loud loop.

Possible feedback:
- extremely subtle local double/transient effect near the corrected object,
- small release sound when the original event contribution returns.

Players must not be punished sonically for keeping multiple corrections active.

## Failure / restore

RESTORE CURRENT RECORD should sound administrative/mechanical rather than supernatural.

No death sting.

The restore transition may use:
- room tone cut,
- mechanical registration sound,
- brief neutral transition.

## Music policy

Music is sparse.

Normal puzzle solving often uses no score.

Music is reserved for:
- act transitions,
- quiet traversal,
- selected narrative reveals,
- late Record 0 reconstruction,
- final exit.

Even then, score should remain textural and restrained.

Preferred instruments/textures:
- low acoustic/electroacoustic drones,
- tape/room tone,
- sparse piano or struck metal used carefully,
- restrained strings or sustained tones,
- processed institutional mechanical recordings.

Avoid:
- emotional piano underscoring every document,
- horror ostinatos,
- heroic puzzle-solved fanfares,
- cyberpunk synthwave.

## Puzzle success

Do not play a success jingle.

Success is communicated by:
- the world entering the useful state,
- an NPC beginning the predicted reaction,
- an unlocked route becoming apparent.

A score transition may occur only at larger structural milestones.

## Act progression

### Act I
Human office sounds dominate. Nearby coworkers, printers, doors, ordinary building ambience.

### Act II
Security and route sounds become more readable: locks, readers, footsteps, service doors.

### Act III
Mechanical layers become richer: restoration machinery, conveyor motion, relays, ventilation, maintenance activity.

### Act IV
Older infrastructure changes the acoustic character: heavier doors, older motors, mechanical relays, greater reverberation. Inconsistency is expressed through era-mismatched sound sources, not horror distortion.

### Act V
Sound thins. Reconstruction emphasizes recorded fragments, room tone, and the absence of an event cue during the 39-second gap.

## Yoon Seojin voice

Seojin should be heard only a small number of times.

Her recordings are functional:
- workplace communication,
- incidental captured conversation,
- accident communications.

No explanatory audio diaries and no farewell message.

The important line remains ordinary:

> "재민 씨, 이거 제어실에서 한번만 봐줄래요?"

Do not score the line heavily on first exposure.

## Record 0 — 39-second gap

This is a major audio-design payoff.

Throughout the game, selectable recorded events acquire small, learnable interface/event cues.

At the absent transition:
- there is no event-selection tick,
- no correction artifact,
- no hidden heartbeat,
- no dramatic "error" tone.

The absence itself must be perceptible because the player has learned what recorded events normally sound like.

## Final door

The final exterior door closes with an ordinary physical latch/motor sound.

No correction tone.
No Residual Exposure cue.
No musical sting synchronized to the closure.

Allow the ordinary sound to finish before cut/credit transition.

## Accessibility

Puzzle-critical sound information requires alternatives.

Support:
- subtitles/captions for meaningful environmental cues where appropriate,
- visual state changes,
- Review history,
- adjustable dynamic range,
- separate dialogue/music/effects controls.

No puzzle should become impossible for a player who cannot rely on directional hearing.

## Mixing rules

- protect relevant event transients,
- keep background machinery below causal cues,
- use realistic occlusion but do not bury required information,
- music yields to gameplay events,
- alarm loudness must communicate urgency without becoming physically unpleasant,
- avoid excessive low-frequency rumble over long sessions.

## Production rule

Record and design reusable sound families tied to gameplay semantics before producing large quantities of decorative ambience.

Audio implementation must be tested together with puzzle readability, not added only during final polish.
