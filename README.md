# Unity Memory NPC Prototype

A minimal Unity feasibility prototype for persistent NPC memory and budget-aware dialogue context selection.

## Purpose

This repository is a pre-project technical exploration for a possible University of London CM3070 Final Project. It is intended to answer a small set of feasibility questions before the formal project begins:

1. Can a Unity client complete a reliable, cancellable dialogue request?
2. Can structured player facts survive an application restart and influence later NPC dialogue?
3. Can a deterministic context builder preserve important facts while enforcing a context budget?

This repository is not yet the assessed Final Project implementation, and results produced here are not final evaluation evidence.

## Prototype target

The player tells Arthur the blacksmith a name and a preference. These facts are stored as structured JSON. After restarting the application, a later dialogue request includes the relevant facts in a bounded context and Arthur responds accordingly.

The prototype is complete when this flow works with a mock provider, handles failure safely, and can be demonstrated without an API key. A remote provider is optional until the offline vertical slice is stable.

## Current status

A complete Unity 2022.3.62f3 feasibility prototype now covers offline dialogue, structured player-fact persistence, deterministic context budgeting and provider reliability. The `Prototype` scene stores the demonstration facts across sessions, exposes context inclusion and trimming decisions, recalls supplied facts without network access, and recovers the UI after cancellation, timeout, invalid responses or provider failures.

## Optional Gemini provider

The project remains offline by default. To test Gemini without committing a credential:

1. Copy `Config/gemini-provider.example.json` to `Config/gemini-provider.local.json`.
2. Set `UseGemini` to `true`.
3. Put your Gemini key in `ApiKey` and keep or replace the `Model` value.
4. Enter Play Mode and use the existing dialogue UI.

Files ending in `.local.json` are ignored by Git. Never place a real key in the example file, a scene, a prefab or source code. Invalid or absent local configuration safely selects the deterministic mock provider.

## Documentation

- [Prototype scope](Docs/PROTOTYPE_SCOPE.md)
- [Development roadmap](Docs/ROADMAP.md)
- [Learning log](Docs/LEARNING_LOG.md)
- [Unity project context](Docs/AI/UnityProjectContext.md)
- [CM3070 project proposal](Docs/PROJECT_PROPOSAL_CM3070.md)

## Academic boundary

Pre-project exploration will be dated and disclosed to the future supervisor. Formal requirements, implementation history, experiments and evaluation data will be created during the assessed project in accordance with the current CM3070 guidance.
