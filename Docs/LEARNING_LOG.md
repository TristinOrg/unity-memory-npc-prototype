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
