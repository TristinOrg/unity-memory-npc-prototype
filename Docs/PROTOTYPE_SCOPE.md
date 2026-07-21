# Prototype Scope

## Status

This document defines the pre-project feasibility prototype. It deliberately has less scope than the proposed CM3070 Final Project.

## Goal

Demonstrate that structured player facts can be persisted across Unity application sessions and included in a deterministic, budget-aware dialogue context that changes an NPC response.

## Questions to answer

1. Can Unity perform the dialogue flow without blocking the main thread?
2. Can the prototype save and reload structured facts reliably?
3. Can a context builder preserve required information while trimming optional history?
4. Can the complete offline path be demonstrated with a mock provider?

## Demonstration

### First session

Player: `My name is Alex and I like swords.`

Stored facts:

```text
player.name = Alex
player.preference.weapon = swords
```

### Later session

Player: `Do you remember me?`

Expected mock response:

```text
Welcome back, Alex. I remember that you are interested in swords.
```

## Must have

- one minimal Unity scene;
- one NPC named Arthur;
- one dialogue input and transcript view;
- static NPC profile separated from mutable runtime data;
- mock AI provider;
- structured player facts;
- JSON save/load;
- deterministic context builder;
- context length budget and priority trimming;
- loading, cancellation, timeout, error and fallback states;
- small debug view showing facts and included context;
- focused tests for persistence, context trimming and invalid responses.

## Optional

- one remote LLM provider after the mock path is stable;
- provider token-count reporting;
- a simple summary of excluded recent dialogue for exploration only.

## Not included

- formal research experiments or final evaluation data;
- relationship simulation or NPC schedules;
- memory embeddings or vector storage;
- SQLite or a remote database;
- multiple NPCs or NPC-to-NPC dialogue;
- autonomous planning or generated quests;
- arbitrary model-controlled Unity actions;
- voice, facial animation, multiplayer or mobile deployment;
- a production backend or Asset Store package.

## Completion criteria

The prototype stops when the following are true:

1. the offline demonstration works from a clean checkout without an API key;
2. facts survive an application restart;
3. required facts remain in context when optional history is trimmed;
4. failure never leaves the UI permanently blocked;
5. the flow can be shown in a 30-60 second recording;
6. findings and limitations are recorded in the learning log.

## Time box

Target: 5-7 focused development days, with a maximum of approximately 20 hours. Additional ideas become notes for the formal project rather than prototype features.
