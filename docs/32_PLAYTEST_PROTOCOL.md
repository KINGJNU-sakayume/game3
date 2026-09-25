# UNDO — Prototype and Puzzle Playtest Protocol

Last locked: 2026-09-25

## Purpose

UNDO depends on players forming the correct causal mental model.

A technically correct prototype can still fail as a game if players interpret corrections as:
- arbitrary magic,
- inconsistent time travel,
- bugs,
- hidden scripting.

Playtesting therefore evaluates understanding, prediction, pacing, and desire to experiment.

## General rule

Observe first. Explain later.

Do not teach internal terms such as:
- Consequence Persistence,
- causal decoupling,
- state channel,
- EventLedger,

before testing whether the game communicates them.

## Tester briefing

For early mechanic prototypes, tell testers only what they would naturally know from the game.

Avoid briefing:

> "Undo only changes the direct event and downstream consequences persist."

That would invalidate the central comprehension test.

Suitable briefing:

> "This is an early puzzle prototype. Play naturally and say what you're thinking if you're comfortable. Some visuals/audio are placeholders."

## Think-aloud

Think-aloud is useful, but do not constantly interrogate during a key realization.

Record:
- what object they focus on,
- what rule they seem to assume,
- when their prediction changes,
- what they think caused an NPC reaction.

If recording video/audio, obtain appropriate consent.

## Intervention policy

During a test:
- do not solve the puzzle for the player,
- do not correct a wrong theory immediately,
- note where the environment fails to teach.

Intervene only when:
- prototype bug blocks progress,
- tester is stuck long enough that no new information is being generated,
- accessibility/tooling issue prevents meaningful test.

Record every intervention.

## Prototype A measures

### Question

Does the player understand that they are not directly operating the door, but suppressing a recorded close event?

Observe:
- attempts to directly interact with door,
- discovery of Residual Exposure,
- Review Mode comprehension,
- reaction to suppression,
- understanding of release.

Post-test questions:
1. What happened when you used Undo on the door?
2. Did the worker go back in time?
3. What did releasing the correction do?
4. What part of the interface told you the door was actionable?

Failure signs:
- player believes Undo teleported NPC,
- player believes the door simply has a magical open command,
- player cannot distinguish suppression from ordinary interaction,
- release appears arbitrary.

## Prototype B measures

### Primary question

Does the player discover and correctly explain Consequence Persistence?

Do not name the rule before asking.

Post-state should be:
- badge back at original location,
- authorization remains granted,
- door remains unlocked,
- guard remains in present location/routine.

### Post-test questions

Ask open-endedly:

1. What do you think happened to the badge?
2. Why do you think the door stayed unlocked?
3. If you undid the badge action earlier, before it was scanned, what would you expect?
4. What other kind of thing would you try Undo on after seeing this?
5. Did any part look like a bug?

Question 3 is especially useful: it tests prediction rather than retrospective explanation.

### Success indicators

Strong:
- tester distinguishes individual recorded actions,
- tester predicts timing affects which downstream events have already become independent,
- tester proposes new causal experiments.

Weak:
- tester memorizes "badge comes back, door stays open" but cannot generalize.

Failure:
- tester explains behavior as arbitrary exception,
- tester expects complete time rewind,
- tester cannot predict a similar case.

## Prototype B greenlight

Do not reduce this to one percentage.

Greenlight requires converging evidence that:
- most testers can form a correct causal explanation with limited exposure,
- mistaken models can be corrected through level/visual/audio design rather than long text,
- testers show interest in experimenting,
- the mechanic remains understandable across repeated attempts,
- technical behavior is stable enough that real bugs are not teaching false rules.

If confusion is widespread:
1. inspect presentation,
2. inspect event timing/animation commit clarity,
3. inspect level composition,
4. inspect terminology/prompts,
5. only then reconsider the rule itself.

Do not immediately add tutorial paragraphs.

## Prototype C measures

### Primary question

Does the NPC reaction feel predictable rather than scripted behind the player's back?

Observe:
- whether tester knows what the guard believes/expects,
- whether they understand when the guard can notice contradiction,
- whether the reaction path makes sense,
- whether they attribute behavior to their correction.

Post-test questions:
1. Why did the guard change route?
2. When do you think the guard noticed the problem?
3. Would you expect the guard to react immediately from another room? Why?
4. What would happen if the badge were restored before the check point?

Failure signs:
- "The game just decided the guard should move."
- reaction occurs before any plausible observation,
- tester cannot see/hear the observation point,
- NPC route looks random.

## Timing tests

For release-timing puzzles record:
- whether player understands release as a deliberate verb,
- whether execution window feels fair,
- number of failed attempts after solution is understood.

If a player understands the solution but repeatedly fails execution:
- timing window/presentation is the problem,
- do not classify it as puzzle difficulty.

## Waiting tests

Record:
- time from entering space to first useful observable event,
- repeated cycle length,
- time spent idle after player understands what they are waiting for.

Target routines generally surface key events within roughly 20–60 seconds.

If waiting dominates:
- shorten cycle,
- move observation anchor,
- preserve meaningful state between cycles,
- improve event access.

Do not simply add a fast-forward mechanic before fixing level pacing.

## UI tests

Ask testers to identify:
- current actionable object,
- currently maintained correction,
- whether capacity is full,
- current/previous state in Review.

Watch for:
- searching screen corners for HUD,
- mistaking amber for generic interactable highlight,
- treating Review as a separate menu/cyberspace,
- missing active correction after looking away.

If persistent slot HUD becomes necessary, add it only after evidence demonstrates world-only communication is insufficient.

## Art-direction test

Show representative slice screenshots/clips without explaining the style.

Ask:
- Where do you think this place is?
- What kind of institution is it?
- What era does it feel like?
- What genre do you expect?

Failure signals:
- "AI lab,"
- "cyberpunk facility,"
- "abandoned hospital horror,"
- "generic backrooms,"
- "Portal-style test lab,"

unless accompanied by stronger correct institutional reading.

## Narrative tests

Do not ask:

> "Did you understand the theme about inaction?"

Instead ask factual reconstruction:

- Who was Yoon Seojin?
- How did she know Jaemin?
- What happened in B3?
- What did Jaemin do?
- What did he not do?
- Why can Record 0 not be corrected like other events?
- Is it certain that acting immediately would have saved her?

If factual answers are wrong, narrative delivery failed.

Interpretive answers may differ.

## 39-second gap test

Critical requirement:
- player attempts or considers correction,
- discovers there is no event,
- reaches the meaning without an explanatory speech.

Afterwards ask:

> "What was different about that part of the record?"

Strong answer:
- there was no action/state change to Undo.

If testers think:
- system is broken,
- they lack enough energy,
- it is a locked special event,
- the game arbitrarily disabled Undo,

presentation needs revision.

## Ending tests

Do not label outcomes during testing.

Ask:
- What did your final actions actually do to Record 0?
- Does the physical record exist?
- Does the institution know/index it?
- Why did you choose that sequence?

Players may interpret the meaning differently. That is acceptable.

What must remain clear is the factual resulting state.

## Accessibility playtests

Test correction readability under:
- reduced color reliance,
- increased Residual Exposure strength,
- captions enabled,
- no directional-audio dependence,
- reduced camera motion,
- alternate hold/toggle input.

Do not treat accessibility options as Beta-only polish.

## Logging / telemetry for internal tests

Useful local development metrics:
- puzzle start/completion timestamp,
- restores,
- correction commits/releases,
- event selected,
- time in Review,
- hint requests,
- NPC cycle count before solution.

Do not infer player psychology automatically from a metric.

Metrics support observation; they do not replace interviews.

## Test report template

For each session record:

```
Build / commit:
Tester familiarity:
Prototype / puzzle:
Duration:
Completion:
Interventions:

Observed mental model:
Key confusion:
Key successful realization:
Unexpected behavior:
Timing/waiting notes:
UI notes:
Audio notes:
Accessibility notes:

Post-test factual answers:
Post-test prediction answers:

Bugs:
Design issues:
Suggested follow-up:
```

## Severity classification

### P0 — invalidates test
Crash, softlock, wrong causal state, NPC random divergence, reset failure.

### P1 — core comprehension failure
Mechanic consistently read incorrectly; key reaction appears arbitrary.

### P2 — major friction
Excessive waiting, unclear Review selection, correction state difficult to track.

### P3 — polish/readability
Minor wording, animation timing, audio mix, small visual hierarchy issue.

Fix P0 before collecting more interpretation data.

## Comparison discipline

When changing presentation after a failed test:
- change as few variables as practical,
- retest,
- avoid simultaneously rewriting UI, level geometry, sound, and rule timing.

Otherwise it becomes difficult to know what fixed comprehension.

## Greenlight ownership

Prototype tests inform the decision; they do not mechanically make it.

The design review should consider together:
- test observations,
- technical stability,
- production complexity,
- player curiosity,
- whether the mechanic still supports the planned puzzle grammar.

Do not continue full production merely because sunk cost is high.
