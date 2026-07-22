# Unity Project Context

<!-- unity-onboarding:generated:start -->

## Project Summary

- Project root: `D:\unity-memory-npc-prototype`
- Purpose: pre-project feasibility prototype for one persistent-memory NPC dialogue flow; not the assessed CM3070 implementation or formal experiment.
- Current state: complete offline feasibility prototype with an optional locally configured Gemini Interactions API provider.
- Last analyzed: 2026-07-22
- Last analyzed commit: `5e738b2`
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
| `Assets/Scenes/Prototype.unity` | Single startup scene containing the camera, light and offline dialogue UI. | Confirmed | Unity scene hierarchy |
| `Assets/UnityMemoryNPCPrototype/Runtime/` | Runtime assembly and prototype code. | Confirmed | assembly definition and source files |
| `Assets/UnityMemoryNPCPrototype/Presentation/` | Scene-facing dialogue controller and UI integration. | Confirmed | assembly definition and source files |
| `Assets/UnityMemoryNPCPrototype/Tests/EditMode/` | Editor-only tests that reference the runtime assembly. | Confirmed | assembly definition and source files |
| `Packages/` | Reproducible Unity package declarations and lock data. | Confirmed | `Packages/manifest.json`, `Packages/packages-lock.json` |
| `ProjectSettings/` | Unity editor and player settings. | Confirmed | project files |
| `Docs/` | Scope, roadmap, proposal and learning/decision records. | Confirmed | repository documentation |

## Assembly Boundaries

- `UnityMemoryNPCPrototype.Runtime`: runtime code boundary; may use UnityEngine but must not reference UnityEditor.
- `UnityMemoryNPCPrototype.Presentation`: Unity UI and scene integration; references Runtime, TextMesh Pro and uGUI.
- `UnityMemoryNPCPrototype.Tests.EditMode`: Editor-only NUnit test assembly with a one-way reference to Runtime.
- Dependency direction: Presentation and tests may reference Runtime; Runtime must not depend on scene objects, Presentation or test code.

## Scenes And Startup Flow

- Build scenes: `Assets/Scenes/Prototype.unity`, enabled at build index 0.
- Startup scene: `Prototype`.
- Scene loading flow: direct startup through Build Settings; no scene-loading system is needed for the one-scene prototype.

## Architecture

| Pattern | Finding | Confidence | Evidence |
| --- | --- | --- | --- |
| Dependency direction | Presentation may depend on domain/provider abstractions; core data and algorithms should avoid scene dependencies. | Confirmed requirement | `AGENTS.md` |
| Delivery strategy | Implement an offline mock-provider dialogue vertical slice before persistence, context budgeting or a remote provider. | Confirmed requirement | `Docs/ROADMAP.md`, `Docs/PROTOTYPE_SCOPE.md` |
| Provider boundary | `DialogueController` depends on `IAIProvider`; requests carry the current message and already-built context, so providers do not own memory selection. | Confirmed | first-party source |
| Presentation | A scene-bound controller owns UI interaction, request cancellation and safe fallback presentation. | Confirmed | first-party source and scene |
| Structured memory | Player name and weapon preference are stored as stable key/value facts rather than raw dialogue alone. | Confirmed | first-party source and tests |
| Persistence | Schema-v1 JSON is stored at `Application.persistentDataPath/player-memory-v1.json` using temporary-file replacement. | Confirmed | first-party source and runtime validation |
| Context selection | Required instructions, Arthur profile, supported facts and current message precede optional recent turns under a deterministic 600-character budget. | Confirmed | first-party source and tests |
| Provider reliability | `ReliableAIProvider` applies a five-second deadline, preserves caller cancellation and rejects empty responses before presentation. | Confirmed | first-party source, tests and runtime validation |
| Remote provider | `GeminiAIProvider` uses the Gemini Interactions REST API when ignored local configuration explicitly enables it; missing or invalid configuration falls back to Mock. | Confirmed implementation | first-party source and offline tests |

## Coding Conventions

- Follow repository and global `AGENTS.md` rules.
- Keep one C# class per file, add English XML documentation, omit source file headers, avoid LINQ and capturing closures, and use the specified Unity object null semantics.
- Serialized authoring fields are public under the global rules. Core data and testable logic should avoid `UnityEngine.Object` dependencies where practical.

## Testing And Validation

- Unity Test Framework is available through the locked Development feature.
- EditMode tests: 27 first-party tests cover context, memory, extraction repair, provider reliability, Gemini boundaries and the project baseline; 27 passed on 2026-07-22.
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
- Confirmed: the startup scene contains the camera, directional light, dialogue Canvas, EventSystem and serialized DialogueController and is enabled at build index 0.
- Confirmed: Runtime and EditMode test assembly names and folder boundaries are established.
- Confirmed: submitting `Hello` through the real scene Button produces `Player: Hello` and the deterministic Arthur response in Play Mode.
- Confirmed: submitting the demonstration sentence persists schema-v1 `player.name` and `player.preference.weapon` facts that reload in a later Play Mode session.
- Limitation: the character budget is an inspectable provider-independent proxy; a real tokenizer belongs with a future remote provider rather than this offline feasibility slice.
- Limitation: the Gemini HTTP path compiles and its configuration and JSON boundaries are tested, but a live request cannot be validated until the user supplies a local API key.
- Limitation: no target Player build has been produced; local configuration currently targets Editor development from the repository root.
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
