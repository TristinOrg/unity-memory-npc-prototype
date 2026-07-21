# Project instructions

## Project phase

This repository is currently a minimal feasibility prototype, not the final CM3070 assessed implementation.

## Scope discipline

- Maintain one NPC, one scene and one end-to-end dialogue flow.
- Implement the mock-provider path before any remote provider.
- Store important player information as structured facts, not only raw dialogue.
- Use deterministic context priority and trimming.
- Do not add embeddings, a vector database, multiple NPCs, autonomous planning, voice, multiplayer or a production backend.
- Do not collect formal experiment results during the feasibility phase.

## Development workflow

Before each feature:

1. Explain the problem and why the change is needed.
2. Identify the smallest coherent implementation.
3. State affected files and expected behaviour.
4. Implement tests for non-scene-specific logic where practical.
5. Validate compilation, tests and the demonstration flow.
6. Update `Docs/LEARNING_LOG.md` when a design decision or lesson is material.

## Architecture rule

Unity scene and UI code may depend on the prototype domain and provider abstractions. Core data, context selection and persistence logic should not depend on scene objects where avoidable.
