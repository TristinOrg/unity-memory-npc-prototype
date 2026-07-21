# Unity Project Context

<!-- unity-onboarding:generated:start -->

## Project Summary

- Project root: `D:\unity-memory-npc-prototype`
- Purpose: pre-project feasibility prototype for one persistent-memory NPC dialogue flow; not the assessed CM3070 implementation or formal experiment.
- Current state: valid Unity P0 baseline with a minimal startup scene, explicit runtime/EditMode test assemblies and one environment test.
- Last analyzed: 2026-07-21
- Last analyzed commit: `f9ecfbac3c9991a9a4bb277e04d9955336368458`
- Package baseline was updated after the analyzed commit: MCPForUnity is now a direct pinned dependency, unused template packages were removed, and Unity regenerated a consistent lock file.

## Confirmed Environment

- Unity version: 2022.3.62f3, revision `96770f904ca7` (Unity 2022 LTS).
- Render pipeline: Built-in Render Pipeline. No URP/HDRP package or Scriptable Render Pipeline asset reference is present.
- Input system: Legacy Input Manager (`activeInputHandler: 0`); the Input System package is absent.
- Intended target: desktop; project settings default to 1920x1080. The exact release platform and build configuration are not yet established.

## Important Packages And Frameworks

| Area | Finding | Confidence | Evidence |
| --- | --- | --- | --- |
| UI | uGUI 1.0.0 and TextMesh Pro 3.0.7 are direct dependencies. | Confirmed | `Packages/manifest.json` |
| Testing | Unity Test Framework 1.1.33 is a direct dependency. No first-party tests exist yet. | Confirmed | `Packages/manifest.json`, `Packages/packages-lock.json`, `Assets/` |
| Render pipeline | No URP or HDRP dependency is present. | Confirmed | `Packages/manifest.json`, `Packages/packages-lock.json` |
| Input | No Input System package is present. | Confirmed | `Packages/manifest.json`, `Packages/packages-lock.json` |
| Networking | No networking package or first-party networking code is present. | Confirmed | `Packages/manifest.json`, `Assets/` |
| Unity MCP | MCPForUnity v10.1.0 is pinned as a Git dependency, resolved by Unity, and exposes this project as a selectable MCP instance. | Confirmed | `Packages/manifest.json`, `Packages/packages-lock.json`, MCP instance resource |

## Directory Structure

| Path | Purpose | Confidence | Evidence |
| --- | --- | --- | --- |
| `Assets/Scenes/Prototype.unity` | Minimal startup scene containing one Camera and one Directional Light. | Confirmed | Unity scene hierarchy |
| `Assets/UnityMemoryNPCPrototype/Runtime/` | Runtime assembly and prototype code. | Confirmed | assembly definition and source files |
| `Assets/UnityMemoryNPCPrototype/Tests/EditMode/` | Editor-only tests that reference the runtime assembly. | Confirmed | assembly definition and source files |
| `Packages/` | Reproducible Unity package declarations and lock data. | Confirmed | `Packages/manifest.json`, `Packages/packages-lock.json` |
| `ProjectSettings/` | Unity editor and player settings. | Confirmed | project files |
| `Docs/` | Scope, roadmap, proposal and learning/decision records. | Confirmed | repository documentation |

## Assembly Boundaries

- `UnityMemoryNPCPrototype.Runtime`: runtime code boundary; may use UnityEngine but must not reference UnityEditor.
- `UnityMemoryNPCPrototype.Tests.EditMode`: Editor-only NUnit test assembly with a one-way reference to Runtime.
- Expected P1 direction: presentation and tests may reference Runtime; Runtime must not depend on scene objects or test code.

## Scenes And Startup Flow

- Build scenes: `Assets/Scenes/Prototype.unity`, enabled at build index 0.
- Startup scene: `Prototype`.
- Scene loading flow: direct startup through Build Settings; no scene-loading system is needed for the one-scene prototype.

## Architecture

| Pattern | Finding | Confidence | Evidence |
| --- | --- | --- | --- |
| Dependency direction | Presentation may depend on domain/provider abstractions; core data and algorithms should avoid scene dependencies. | Confirmed requirement | `AGENTS.md` |
| Delivery strategy | Implement an offline mock-provider dialogue vertical slice before persistence, context budgeting or a remote provider. | Confirmed requirement | `Docs/ROADMAP.md`, `Docs/PROTOTYPE_SCOPE.md` |
| Implemented architecture | No production architecture exists yet. | Confirmed | `Assets/` |

## Coding Conventions

- Follow repository and global `AGENTS.md` rules; no first-party source exists from which to infer additional conventions.
- Keep one C# class per file, add required XML documentation and file headers, avoid LINQ and capturing closures, and use the specified Unity object null semantics.
- Serialized authoring fields are public under the global rules. Core data and testable logic should avoid `UnityEngine.Object` dependencies where practical.

## Testing And Validation

- Unity Test Framework is available through the locked Development feature.
- EditMode tests: one first-party environment baseline test; 1 passed, 0 failed, 0 skipped on 2026-07-21.
- PlayMode tests: no first-party tests or test assembly yet.
- CI/build validation: no CI configuration or documented command exists.
- After the package refresh, the connected Editor is idle with no compilation or domain reload pending and the Console reports no errors or warnings.
- An EditMode test run completed successfully with zero first-party tests; this validates the runner connection, not application behavior.

## Available Unity Tooling

| Capability | Status | Evidence |
| --- | --- | --- |
| Repository inspection | available | local filesystem and Git |
| Unity MCP tools | available | target instance is `unity-memory-npc-prototype@6bf076995c73b7ab` |
| Target-project Editor state/console/scenes/tests | available after selecting target instance | MCP instance resource and successful active-instance selection |
| Headless Unity executable | likely available, not validated in this onboarding | handoff records `D:/Softwares/Unity/Editor/Unity.exe` |

## Important Constraints

- Keep one NPC, one scene and one end-to-end dialogue flow.
- Complete and validate the mock-provider path before adding any remote provider.
- Important player information must become structured facts rather than only dialogue text.
- Context priority and trimming must be deterministic and inspectable.
- Do not add embeddings, vector storage, multiple NPCs, autonomous planning, voice, multiplayer or a production backend.
- Do not collect formal experiment results during feasibility work.
- Do not treat this prototype's results or implementation history as the assessed Final Project evidence.

## Unknowns And Confidence

- Confirmed: post-package-refresh Editor status is idle and the Console contains no errors or warnings.
- Confirmed: the startup scene contains exactly a Main Camera and Directional Light and is enabled at build index 0.
- Confirmed: Runtime and EditMode test assembly names and folder boundaries are established.
- Risk: README and planning documents predate project creation and must stay synchronized with the actual repository state.
- Risk: two Unity projects expose MCP instances simultaneously, so agents must explicitly select `unity-memory-npc-prototype@6bf076995c73b7ab` before reading or mutating Editor state.

## Source Files Inspected

- `AGENTS.md`
- `README.md`
- `Docs/PROJECT_PROPOSAL_CM3070.md`
- `Docs/PROTOTYPE_SCOPE.md`
- `Docs/ROADMAP.md`
- `Docs/LEARNING_LOG.md`
- `Packages/manifest.json`
- `Packages/packages-lock.json`
- `ProjectSettings/ProjectVersion.txt`
- `ProjectSettings/ProjectSettings.asset`
- `ProjectSettings/GraphicsSettings.asset`
- `ProjectSettings/QualitySettings.asset`
- `ProjectSettings/EditorBuildSettings.asset`
- first-party files under `Assets/`
- Unity MCP instance, project-info, editor-state and test resources (used only to establish that the connected Editor is a different project)

<!-- unity-onboarding:generated:end -->
