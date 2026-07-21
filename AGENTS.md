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

## Project C# conventions

- Use four spaces for indentation and do not use tabs.
- Do not add copyright, author, date or description header blocks to C# files.
- Write code comments and XML documentation comments in English by default.
- Add English XML documentation to types, fields, properties, methods and parameters.
- Keep one type per `.cs` file and match the file name to the type name.
- Explicitly declare private members with `private` and mark methods `static` when they do not access instance state.
- Prefer `var` when the local type is clear, keep single expressions on one line where practical, and remove unused `using` directives.
- Do not use LINQ or closures that capture locals or `this`.
- Prefer `struct` for small data values, handles, results and queries; prefer `readonly struct` for immutable values.
- A simple one-statement `if` may omit braces when its statement is on the following line. Multiple statements and complex loop bodies require braces.
- Put a blank line before an explanatory comment within a method and use one space after `//`.
- Use target-typed `new()` only for member field or property initializers. Spell out the constructed type inside methods.
- Do not explicitly initialize fields to their type default.

## Naming

- Types use `PascalCase`; enums use `E` plus `PascalCase`.
- Methods and properties use `PascalCase`.
- Public fields use `PascalCase`.
- Private instance fields use `mPascalCase`; private static fields use `sPascalCase`.
- Constants use `PascalCase`.

## Unity and performance rules

- Do not use `?.` or `??` for `UnityEngine.Object` lifetime checks. Use Unity's implicit Boolean conversion.
- Prefer `?.` and `??` for ordinary C# reference types.
- Use `is A or B` and `is not (A or B)` when comparing one value with multiple constants.
- Use `foreach` when an index is not required; use `for` for genuine index-based work, reverse removal or two-pointer algorithms.
- Avoid reflection, per-frame allocation, repeated `GetComponent`, hierarchy searches, synchronous resource loading and unbounded collection growth on hot paths.
- Cache reusable data, but do not add pooling to low-frequency objects without evidence that it is needed.

## Architecture and serialization

- Prefer explicit dependencies and constructor injection. Avoid global mutable state, unnecessary singletons and overlapping managers.
- Every mutable state value must have a clear owner and predictable lifecycle.
- Introduce abstractions only for real variation, boundaries or testing needs; keep public APIs small and avoid unrelated refactors.
- Fields that must persist in scenes or assets are public serialized fields. Keep non-serialized runtime state private.
- Use `FormerlySerializedAs` when renaming existing serialized fields.
- Pair subscriptions with unsubscriptions and release operation handles on success, failure and cancellation paths.
- Async operations must account for cancellation, owner destruction and application exit.
- Runtime assemblies must not reference `UnityEditor`.

## Testing and Git

- Run targeted tests first, then expand to EditMode, PlayMode or Player Build validation according to risk.
- Prefer EditMode tests for pure C# logic and PlayMode tests for MonoBehaviour, lifecycle, scene and prefab behavior.
- Do not disable tests, delete failing cases or suppress errors to obtain a passing result.
- Distinguish introduced regressions, pre-existing failures and environment failures. Compilation alone is not complete feature validation.
- Use Conventional Commits. Before committing, run `git pull --rebase --autostash`, inspect scope, run `git diff --check`, and exclude logs, caches and temporary artifacts.
- When the user requests automatic publishing, commit and push only after validation succeeds.
