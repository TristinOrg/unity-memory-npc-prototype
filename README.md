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

The Git repository and planning documents exist. A Unity project has not yet been created in this repository.

## Documentation

- [Prototype scope](Docs/PROTOTYPE_SCOPE.md)
- [Development roadmap](Docs/ROADMAP.md)
- [Learning log](Docs/LEARNING_LOG.md)
- [CM3070 project proposal](Docs/PROJECT_PROPOSAL_CM3070.md)

## Academic boundary

Pre-project exploration will be dated and disclosed to the future supervisor. Formal requirements, implementation history, experiments and evaluation data will be created during the assessed project in accordance with the current CM3070 guidance.
