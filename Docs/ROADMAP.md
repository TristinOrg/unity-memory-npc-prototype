# Prototype Development Roadmap

## P0 - Repository and Unity baseline

- create the Unity project in this repository;
- confirm editor version, render pipeline, input and test framework;
- create a minimal scene without imported art dependencies;
- establish runtime and test assembly boundaries;
- record a clean compilation and test baseline.

Exit condition: the project opens in the selected Unity version, compiles without errors and has an empty passing test baseline.

## P1 - Offline dialogue vertical slice

- define Arthur's immutable profile;
- define request and response models;
- define an AI provider interface;
- implement a deterministic mock provider;
- build the minimal dialogue UI and controller;
- display loading, success and fallback states.

Exit condition: the player submits text and receives a mock Arthur response without network access.

## P2 - Structured facts and persistence

- define the player-fact representation;
- explicitly record the name and sword preference used by the demonstration;
- save versioned JSON outside Unity assets;
- reload data on a later application session;
- test round trips, missing files and malformed files.

Exit condition: the two demonstration facts survive an application restart.

## P3 - Budget-aware context builder

- define context sections and priorities;
- include Arthur's profile, required facts, recent turns and current message;
- enforce a deterministic budget;
- remove lower-priority optional content first;
- expose included and removed items in the debug view;
- test boundary and overflow cases.

Exit condition: required facts remain available under the configured budget and context construction is reproducible.

## P4 - Reliability and optional remote provider

- add cancellation and timeout behaviour;
- validate responses and provide fallback dialogue;
- add a remote provider only if the offline path is stable;
- keep credentials outside source and Unity assets;
- record feasibility findings, costs and unresolved risks.

Exit condition: the prototype satisfies `PROTOTYPE_SCOPE.md` and no new formal-project features are started.

Status: completed with the offline provider. The optional remote provider was deliberately deferred because it is not required to answer the feasibility questions.

## First implementation target

P1 is the first coding milestone. P0 must be completed first because code, packages and assembly decisions depend on the actual Unity version and project configuration.
