# UNDO — ChatGPT / Codex Development Workflow

Last locked: 2026-09-25

## Purpose

UNDO is intentionally developed with separate responsibilities:

- **ChatGPT conversation:** design, specification, review, debugging strategy, task decomposition.
- **GitHub repository:** canonical project memory and source of truth.
- **Codex:** scoped implementation work against repository canon.
- **Pull requests/tests:** verification boundary before changes become canonical implementation.

Chat history is not project memory.

## Core rule

> If a decision matters to future implementation, it is not canonical until the repository documentation is updated.

A useful idea discussed in chat but absent from the repository remains exploratory.

## Role separation

### ChatGPT — design / technical direction

Use ChatGPT to:
- explore game ideas,
- resolve mechanic ambiguity,
- design puzzle grammar,
- refine narrative,
- write/update canonical Markdown,
- define technical contracts,
- decompose milestones into implementation tasks,
- inspect PRs/diffs/test failures,
- identify conflicts between implementation and design.

ChatGPT should not silently reinterpret old chat as canon when repository docs disagree.

### GitHub — durable memory

GitHub stores:
- canonical Markdown,
- implementation,
- content JSON,
- tests,
- assets/asset references,
- issues/tasks where useful,
- pull-request history,
- milestone state.

A new conversation should be able to reconstruct the project from GitHub without needing the original brainstorming transcript.

### Codex — implementation engineer

Codex receives:
- one scoped task,
- repository access,
- named canonical docs,
- explicit acceptance criteria.

Codex should:
- read `AGENTS.md`,
- read relevant docs before editing,
- implement only requested scope,
- add/update tests,
- run validation,
- report ambiguity rather than inventing mechanics,
- produce a reviewable branch/PR when the environment supports it.

Codex is not the game designer by default.

## New ChatGPT conversation bootstrap

Recommended first message in a fresh project chat:

```
This is the UNDO project in the game3 repository.

Read AGENTS.md first, then docs/20_CURRENT_STATE.md and every canonical document that AGENTS.md requires for the task.

Treat the repository as the source of truth.
Do not reconstruct game rules from old chat history.

First summarize:
1. current project phase,
2. locked core mechanics,
3. current next milestone,
4. open decisions relevant to my request.

Do not implement anything until the request requires implementation.
```

If the conversation is specifically about one domain, then add the relevant files.

Example for narrative:

```
Also read:
docs/08_WORLD.md
docs/09_NARRATIVE.md
docs/10_CHARACTERS.md
```

## New Codex task bootstrap

Every implementation task should begin from repository context, not a pasted retelling of the whole game.

Base instruction:

```
Read AGENTS.md first.

Then read the canonical docs named in this task.
Repository documentation is the source of truth.

If the requested implementation conflicts with canon, stop and report the conflict instead of inventing an exception.
```

## Task format

Use this task shape:

```md
# TASK: <short implementation objective>

## Milestone
<Bootstrap / Prototype A / Prototype B / ...>

## Canonical references
- AGENTS.md
- docs/...
- docs/...

## Goal
<one concrete outcome>

## Required behavior
- ...
- ...

## Explicit non-goals
- ...
- ...

## Files/systems likely involved
- ...

## Acceptance criteria
- ...

## Tests / validation
- ...

## Completion report
Return:
1. summary,
2. changed files,
3. tests run/results,
4. known limitations,
5. any canon conflict or follow-up.
```

Do not send Codex a task like:

> "Build the Undo game."

## Scope size

Preferred Codex task size:
- one subsystem,
- one prototype increment,
- one bug,
- one content conversion,
- one refactor with explicit invariants.

Avoid combining:
- new architecture,
- several puzzles,
- UI redesign,
- art imports,
- narrative rewrite,

in one task.

## Design before code

If ChatGPT discussion changes a canonical rule:

1. resolve the design,
2. update primary Markdown,
3. update `docs/19_DECISION_LOG.md`,
4. update `docs/20_CURRENT_STATE.md` if phase/status changes,
5. only then send implementation task to Codex.

Never ask Codex to implement a rule that exists only in a chat message if it materially changes game behavior.

## Implementation before canon status

Code is not automatically "the design" because it exists.

If a prototype implementation accidentally behaves differently from docs:
- treat it as a bug or design conflict,
- do not rewrite documentation to match the accidental implementation without explicit design review.

## Prototype workflow

### Bootstrap

ChatGPT task definition
→ Codex branch
→ build/tests/headless smoke
→ review
→ merge
→ update current state

### Prototype A

Read:
- core mechanic,
- technical architecture,
- event system,
- testing,
- prototype spec.

Implement only A.

Review focuses on:
- event/state authority,
- suppression/release,
- restore,
- debug visibility.

### Prototype B

Implement persistence scenario only after A is stable.

Prototype B review has two independent checks:

**Technical**
- badge returns,
- authorization persists,
- door remains unlocked,
- guard history does not rewind.

**Human**
- player understands why.

Passing unit tests alone does not greenlight the game.

### Prototype C

Focus on:
- memory contradiction,
- reaction evaluation,
- route predictability,
- newly generated events.

Do not expand NPC AI into a general behavior simulation.

## PR review with ChatGPT

When reviewing a PR, provide repository/PR reference and ask ChatGPT to inspect:

1. diff versus canonical docs,
2. architecture boundary violations,
3. test coverage,
4. hidden puzzle-specific exceptions,
5. scope creep,
6. likely regressions,
7. docs that now need update.

Review should cite concrete files/lines from the diff rather than relying on general impressions.

## Failure-debug workflow

When a test/game behavior fails:

1. collect exact failing test/log,
2. identify stable EntityId/EventId/PuzzleId,
3. inspect domain state before presentation,
4. determine whether failure is:
   - domain rule,
   - content data,
   - scene binding,
   - NPC execution,
   - presentation only,
5. fix the lowest correct layer.

Do not start by patching the visible Godot scene if the semantic model is wrong.

## Context-window protection

Long chats are not the storage layer.

When a conversation becomes long:
- write finalized decisions to GitHub,
- update CURRENT_STATE,
- start a fresh chat when useful.

A fresh chat that reads the repository is preferred over relying on the model to remember hundreds of earlier turns.

## Branch and task naming

Recommended branch examples:

```
build/bootstrap-godot-dotnet
proto/a-basic-correction
proto/b-persistence
proto/c-npc-reaction
core/event-resolver
fix/masked-event-resolution
content/pz-a2-05-missing
```

Task names should describe outcome rather than agent.

Avoid:
- `codex-work`
- `ai-changes`
- `chatgpt-fix`

The implementation matters, not which model produced it.

## Generated code policy

AI-generated code receives the same standards as human-written code.

It must:
- compile,
- pass tests,
- follow architecture,
- be understandable in review,
- avoid unnecessary dependencies,
- avoid duplicated abstractions,
- preserve canonical behavior.

"Codex wrote it" is never a reason to merge unreviewed code.

## Documentation granularity

Do not update every Markdown file after every small code change.

Update docs when:
- behavior changes,
- architecture changes,
- milestone status changes,
- a formerly open decision becomes locked,
- content registry status changes materially.

Ordinary bug fixes that restore documented behavior usually need tests/commit history, not a design rewrite.

## CURRENT_STATE discipline

`docs/20_CURRENT_STATE.md` answers:

- What phase are we in?
- What is complete?
- What is next?
- What is still open?
- What must not be started yet?

Keep it concise enough that every new conversation can read it first.

It is not a chronological diary.

## Decision Log discipline

`docs/19_DECISION_LOG.md` records important locked decisions.

It is not:
- every brainstorm,
- every commit,
- every small parameter.

Use it for decisions future developers/models might otherwise reopen accidentally.

## Chat shorthand after setup

Once a new chat has read the repo, short commands are acceptable.

Examples:

> Continue the current UNDO design milestone.

> Prepare the next Codex task from CURRENT_STATE.

> Review the latest Prototype A PR against canon.

But the model should still check repository state if the answer depends on implementation that may have changed.

## Golden rule

ChatGPT may forget the conversation.

Codex may receive a fresh task.

A developer may return months later.

**The repository must still be sufficient to understand what UNDO is and what to do next.**
