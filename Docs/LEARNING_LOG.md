# Learning and Decision Log

This file records what was attempted, why it was done, how it was validated and what was learned. It is not formal CM3070 experiment evidence.

## 2026-07-21 - Repository baseline

### Situation

The GitHub repository was cloned to `D:\unity-memory-npc-prototype`. At inspection time it contained only the initial README and was not yet a Unity project.

### Decision

Create the documentation and scope boundary before generating Unity assets. Do not claim a Unity version, render pipeline, package set or architecture until the actual project exists.

### Why

Those properties are determined by the Unity project configuration. Guessing them would create documentation that appears authoritative but is not supported by repository evidence.

### Validation

The expected Unity markers were absent:

- `Assets/`
- `Packages/manifest.json`
- `ProjectSettings/ProjectVersion.txt`

### Next lesson

After the Unity project is created, run repository onboarding again and generate `Docs/AI/UnityProjectContext.md` from the real configuration.

## 2026-07-21 - Unity project onboarding baseline

### Situation

The repository now contains the three authoritative Unity project markers: `Assets/`, `Packages/manifest.json` and `ProjectSettings/ProjectVersion.txt`. Before implementing the dialogue flow, the actual environment and missing foundations needed to be identified.

### Decision

Complete a read-only onboarding and record the result in `Docs/AI/UnityProjectContext.md`. Treat repository configuration as authoritative and do not use the available Unity MCP's live scene or test data because its connected Editor instance belongs to another project.

### Why

Architecture and validation claims must be traceable to evidence from the system being studied. Mixing evidence from another Unity project would make the technical record unreliable. The onboarding also exposes the smallest P0 work required before feature code: create a scene, establish assembly/test boundaries and verify compilation.

### Validation

- Unity version is 2022.3.62f3.
- The project uses the Built-in Render Pipeline and Legacy Input Manager.
- Unity Test Framework is available through the locked Development feature.
- There are no first-party scripts, assembly definitions, scenes, build scenes or tests.
- At the initial inspection, the only connected Unity MCP instance targeted `D:/ChatRoom/ChatRoom.Unity`, not this repository.
- After installing and resolving MCPForUnity, this project appeared as its own selectable MCP instance.
- Unity regenerated `packages-lock.json` with MCPForUnity and the deliberately retained packages as depth-zero dependencies.

### Next lesson

Validate the refreshed Editor Console and test discovery, then create the smallest P0 assembly, test and scene foundation before implementing the offline dialogue vertical slice.

## 2026-07-21 - Minimal package and Unity MCP baseline

### Situation

The new Unity project inherited general-purpose template packages that the feasibility prototype did not use. MCPForUnity had been added, but the manifest and lock file had not yet reached a verified, consistent state.

### Decision

Pin MCPForUnity v10.1.0 and Unity Test Framework 1.1.33 directly. Retain uGUI and TextMesh Pro for the P1 dialogue interface. Remove Collab Proxy, the Development feature bundle, Timeline, Visual Scripting and the stale Code Coverage settings.

### Why

Direct dependencies should express actual project intent. A smaller package surface reduces import time, transitive dependencies, version conflicts and unexplained implementation choices while preserving the UI, testing and Editor automation capabilities required by the roadmap.

### Validation

- Unity regenerated `Packages/packages-lock.json` from the updated manifest.
- The lock file contains MCPForUnity, Unity Test Framework, TextMesh Pro and uGUI as the only non-built-in depth-zero packages.
- MCPForUnity exposes `unity-memory-npc-prototype` as a distinct Editor instance.
- The connected Editor returned to idle with no compilation pending and no Console errors or warnings.
- The EditMode runner completed successfully with zero first-party tests, confirming test infrastructure availability but not feature coverage.

### Next lesson

Package minimalism is not removing everything possible; it is retaining every dependency that supports an explicit requirement and being able to explain that mapping.

## 2026-07-21 - P0 compilation, test and startup baseline

### Situation

The Unity project resolved its packages but still had no first-party assembly boundary, executable scene or meaningful test. Starting P1 in that state would mix feature design with environment and project-structure failures.

### Decision

Create a runtime assembly, an Editor-only test assembly that references it, one Unity-version baseline test and one `Prototype` startup scene containing only a Camera and Directional Light. Allow the application Runtime assembly to use UnityEngine because this repository is a focused Unity prototype rather than a reusable pure-C# framework.

### Why

The assembly reference makes the intended dependency direction enforceable by the compiler. The test converts the pinned Unity version from documentation into executable evidence. The minimal scene proves the startup path without prematurely designing the dialogue interface.

### Validation

- Unity imported and compiled the new assemblies with no Console errors or warnings.
- `UnityVersionMatchesProjectRequirement` passed: 1 passed, 0 failed, 0 skipped.
- `Prototype.unity` contains exactly a Main Camera and Directional Light.
- `Prototype.unity` is enabled at build index 0.
- The Editor entered and exited Play Mode with no Console errors or warnings; no visual or dialogue behavior was claimed.

### Next lesson

P1 can now focus on one question only: whether player input can travel through a provider abstraction and produce deterministic mock dialogue in the UI.

## 2026-07-21 - P1 offline dialogue vertical slice

### Situation

The project had a validated startup and test baseline but no end-to-end player interaction. Adding memory or a remote model at this point would combine UI, async orchestration, provider integration and state-management risks.

### Decision

Implement the smallest complete offline dialogue path: TMP input, `DialogueController`, `IAIProvider`, `MockAIProvider`, `AIResponse` and transcript output. Keep provider models and contracts in Runtime and scene behavior in a separate Presentation assembly.

### Why

The provider boundary isolates unstable external AI infrastructure from Unity presentation. A deterministic mock makes the full interaction repeatable, free and usable without credentials. Directly composing the mock in the controller is proportional for one provider and avoids introducing a dependency-injection framework before it solves a real problem.

### Validation

- Unity compiled the Runtime, Presentation and EditMode test assemblies with no Console errors or warnings.
- Four EditMode tests passed: the environment baseline plus deterministic response, invalid-input and cancellation cases.
- Scene references for input, transcript and submit button are serialized and non-null.
- In Play Mode, setting the real input to `Hello` and invoking the real Button produced `Player: Hello` followed by `Arthur: The forge is open. What do you need?`.
- The runtime interaction produced no Console errors or warnings and Play Mode was exited without saving runtime state.

### Next lesson

P2 should introduce structured player facts and versioned persistence without changing the UI-to-provider dependency boundary proven here.

## 2026-07-22 - P2 structured facts and versioned persistence

### Situation

P1 could generate deterministic dialogue but retained no authoritative player state. Raw dialogue history alone would make later recall difficult to validate and would couple persistence to natural-language formatting.

### Decision

Represent the two demonstration facts as stable key/value records inside a schema-v1 document. Use a deliberately narrow deterministic extractor for the supported sentence pattern and persist the document as human-readable JSON under `Application.persistentDataPath`. Write through a temporary file and replace the previous document only after serialization succeeds.

### Why

Structured facts are inspectable, testable and independent of model wording. Schema metadata creates an explicit migration boundary. The narrow extractor proves the data flow without pretending that a hand-written parser is a general NLP solution; a future extraction strategy can replace it behind the same structured memory representation.

### Validation

- Nine EditMode tests passed, including fact replacement, deterministic extraction, JSON round trip, missing-file defaults and malformed-file preservation.
- Unity compiled with no Console errors or warnings.
- A real Play Mode submission stored `player.name = Alex` and `player.preference.weapon = swords` in schema version 1.
- After exiting and re-entering Play Mode, both facts loaded with their original values.
- The runtime test file did not exist before validation and was deleted afterward, leaving no test state in the user data directory.

### Next lesson

P3 should build a deterministic, budget-aware context from authoritative facts and the current message, then allow the mock provider to demonstrate recall without changing persistence ownership.
