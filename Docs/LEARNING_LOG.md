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
