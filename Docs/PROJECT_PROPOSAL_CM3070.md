# CM3070 Final Project Proposal

## Project details

**Provisional title:** Design and Evaluation of a Memory- and Context-Managed NPC Framework for Consistent and Efficient Dialogue in Unity

**Student:** [Insert full name]

**Student number:** [Insert student number]

**Programme:** BSc Computer Science, University of London

**Module:** CM3070 Final Project

**Supervisor:** [Insert supervisor name when assigned]

**Date:** [Insert submission date]

## 1. Project summary

This project will design, implement and evaluate a memory- and context-managed non-player character (NPC) framework in Unity. The framework will allow an NPC to retain selected facts and events across play sessions, maintain deterministic relationship and world state, retrieve relevant memories, compress older dialogue into bounded summaries, and use a large language model (LLM) to produce context-aware dialogue within a configurable context budget. The proposed software will separate authoritative game logic from generative language generation: schedules, relationship values, world-state changes, persistence and gameplay actions will remain under deterministic program control, while the LLM will be used only to express dialogue, produce validated summaries and request a small set of allowlisted high-level actions.

The implementation will be evaluated using three controlled configurations: a memory-free baseline, a full-history condition and the proposed budget-aware selected-memory condition. The principal measures will be factual recall accuracy, contradiction rate, personality consistency, memory-retrieval precision, response validity, latency and token usage. The assessed prototype will contain one NPC, Arthur the blacksmith, in one compact Unity scene. This deliberately constrained scenario will provide an end-to-end demonstration without expanding into a complete game or large-scale agent simulation.

## 2. Background and motivation

NPC dialogue in conventional games is usually written in advance and selected through dialogue trees or rules. This gives designers control and consistency, but limits the range and personalisation of responses. LLMs can generate flexible dialogue, but a model call does not by itself provide reliable long-term memory. Supplying an entire interaction history increases token usage and latency and may introduce irrelevant context, while aggressively reducing the history may remove information required for a consistent answer. Context selection is therefore not only an implementation detail but part of the research problem. In a game, an additional problem is that fluent generated language must not be treated as authoritative gameplay state: a model may contradict stored facts or request actions that the game should not permit.

Prior work demonstrates both the promise and limitations of memory-augmented agents. Park et al. (2023) proposed generative agents that store experiences, retrieve memories and form higher-level reflections to support believable behaviour. Lewis et al. (2020) showed the wider value of combining generation with retrieved non-parametric information. In long-term dialogue, Xu et al. (2022) addressed the extraction and updating of persona memory, while Maharana et al. (2024) found that long-context and retrieval-augmented approaches improve very long-term conversational memory but still remain substantially below human performance. These studies motivate an external memory mechanism, but the proposed project has a narrower and more practical focus: a transparent, game-specific structured memory design in Unity, evaluated under real-time software constraints.

The project is also motivated by a gap between research prototypes and game engineering requirements. A game needs reproducible state transitions, inspectable debugging information, safe failure behaviour and controlled actions. This project therefore investigates a hybrid architecture in which deterministic software owns the game state and the LLM generates language from a selected, validated context.

## 3. Problem statement

An LLM-driven NPC may produce locally plausible dialogue while failing to remember earlier interactions, contradicting stored game state, or using excessive context and model requests. A practical Unity integration needs to decide what should be remembered, how relevant memories should be selected, how generated output should be constrained, and how the resulting behaviour should be evaluated rather than judged only through a demonstration.

The project addresses the following problem:

> How can a small, explainable memory and context-management system be integrated with an LLM-driven Unity NPC so that later dialogue uses relevant past interactions within a bounded context budget, without surrendering authoritative gameplay control to the model?

## 4. Research question and objectives

### 4.1 Primary research question

> To what extent can structured persistent memory and budget-aware context selection improve factual recall, personality consistency and token efficiency in LLM-driven Unity NPC interactions, compared with memory-free and full-history baselines?

### 4.2 Secondary questions

1. Can a simple retrieval method based on importance, recency and tag overlap select useful memories without a vector database?
2. How does selected memory compare with full conversation history in factual recall, contradiction rate, latency and token usage?
3. Does retaining a stable NPC profile and selected interaction context improve personality consistency across repeated questions?
4. Can structured response validation and an action allowlist prevent generated output from directly controlling arbitrary gameplay behaviour?

### 4.3 Hypotheses

- **H1:** The selected-memory condition will achieve higher factual recall and a lower contradiction rate than the memory-free baseline.
- **H2:** The selected-memory condition will use fewer input tokens than the full-history condition while retaining comparable factual recall.
- **H3:** Supplying a stable personality profile through the context manager will produce more personality-consistent responses than the memory-free baseline.

### 4.4 Objectives

1. Review research on generative agents, retrieval-augmented generation, long-term dialogue memory and AI-driven game characters.
2. Define a structured model for NPC profiles, mutable state, player facts, interaction events and relationship state.
3. Design a modular Unity architecture separating gameplay, memory, persistence, dialogue orchestration and AI-provider integration.
4. Implement a reproducible vertical slice using a mock AI provider before connecting a real provider.
5. Implement JSON persistence, an explainable memory-retrieval algorithm and a budget-aware context builder.
6. Implement bounded conversation summarisation after the basic retrieval path is operational, with validation and fallback to unsummarised recent turns.
7. Implement constrained AI request and response schemas, failure handling and an allowlisted action gateway.
8. Create a debugging view showing state, retrieved memories, included and excluded context, response validity, latency and token usage.
9. Compare memory-free, full-history and selected-memory configurations through controlled experiments and automated tests.
10. Critically evaluate the design, results, limitations, ethical considerations and possible future improvements.

## 5. Scope

### 5.1 Core deliverable

The assessed prototype will contain:

- one Unity demonstration scene representing a small blacksmith workshop;
- one NPC, Arthur, with a configurable identity, personality and communication style;
- a serialisable runtime state separate from the static NPC profile;
- factual memories, event memories and deterministic relationship values;
- short-term conversational context and persistent JSON save/load support;
- an explainable memory retriever using importance, recency and tag overlap;
- a budget-aware context builder with deterministic priorities, section limits and Top-K memory selection;
- bounded summaries of older dialogue, generated only when a configured threshold is exceeded and never used as authoritative game state;
- a mock AI provider for offline development and repeatable tests;
- one real LLM-provider adapter, subject to service availability;
- a dialogue orchestrator that constructs a bounded context;
- a validated structured response format and allowlisted action gateway;
- graceful timeout, cancellation, malformed-output and network-failure handling;
- an in-game or editor debugging panel;
- automated tests for non-scene-specific core logic;
- memory-free, full-history and selected-memory experimental configurations;
- a final report, source code and demonstration video.

### 5.2 Demonstration scenario

During the first interaction, the player tells Arthur a name and a preference for swords. The player then completes one predefined event by delivering iron, which deterministically changes the forge state and Arthur's relationship value and creates a structured event memory. After a save/reload cycle or an advance in game time, the player speaks to Arthur again. In the memory-enabled condition, the system retrieves relevant facts and events and supplies them to the dialogue generator. In the baseline condition, persistent memories are omitted. The debug panel shows the evidence used for each response.

### 5.3 Explicit exclusions

The core project will not include multiple autonomous NPCs, NPC-to-NPC conversations, generated quests, unrestricted model-controlled gameplay, speech recognition, speech synthesis, facial animation, multiplayer, mobile deployment, local model inference, a vector database, a production cloud platform or an Asset Store release. SQLite, semantic embedding retrieval, adaptive or learned context optimisation, extended NPC scheduling and a participant study are stretch goals only and will not be required for project completion.

## 6. Proposed system design

### 6.1 Architectural layers

The software will use six responsibilities:

1. **Unity presentation and gameplay:** player interaction, dialogue UI, animation and deterministic scene behaviour.
2. **NPC domain:** profile, runtime state, relationship state and domain rules.
3. **Memory and persistence:** structured memories, retrieval and versioned save/load.
4. **Dialogue orchestration and context management:** prioritisation, summarisation, selection and formatting of a bounded dialogue context.
5. **AI-provider integration:** a provider-independent interface with mock and remote implementations.
6. **Validation and debugging:** schema validation, action authorisation, logging and experiment data.

The core models and algorithms will be kept independent of scene objects where practical so that they can be tested using the Unity Test Framework.

### 6.2 Data model

Static NPC data will include a stable NPC identifier, display name, role, biography, personality traits, communication style, long-term goal, knowledge boundaries and allowed actions. Mutable runtime state will include current activity, relationship values, recent conversational context, known player facts and timestamps. Static authoring data may use a Unity ScriptableObject, but mutable state will not be stored in the shared asset.

A memory record will contain an identifier, NPC identifier, memory type, textual description, structured metadata, creation time, last-access time, importance value, source event and relevance tags. Important game facts such as the player's name, completion of the iron-delivery event and the relationship score will remain structured rather than existing only in natural-language chat history.

### 6.3 Memory retrieval

The core project will use a deliberately simple ranking method:

```text
score = wi * importance + wr * recency + wt * tagOverlap
```

The retriever will first filter records by NPC and applicable memory type, include required structured facts, rank candidate event memories and return a bounded number of results. The weights and result limit will be configurable but fixed during comparative experiments. The debug view will expose the selected records and score components. This makes the method reproducible and supports analysis of retrieval errors. Semantic embeddings will not be required for the core research question.

### 6.4 Budget-aware context management

The context builder will organise information in an explicit priority order: safety and response instructions, NPC profile, authoritative relationship and world state, required player facts, Top-K retrieved events, recent dialogue turns and the current player message. Each section will have a configurable limit and the complete request will have a model-specific token budget. When the budget is exceeded, lower-priority optional content will be removed before required facts or the current message.

Older dialogue will be compressed only after a configured threshold is reached. A generated summary will be stored as non-authoritative narrative context and will be validated for size and format. Structured facts and game state will remain the source of truth. If summarisation fails, the system will retain a bounded recent-turn window rather than blocking dialogue. The debugging view will report which context elements were included, summarised or removed and their token contribution.

### 6.5 Dialogue generation and safe actions

The dialogue orchestrator will combine the NPC profile, current relationship summary, relevant world state, selected memories, recent conversation and current player message. The model will return a constrained object containing dialogue text, an intent, an emotion label and one requested action drawn from a fixed enumeration. The application will parse and validate the response before use. The action gateway will reject unknown, disallowed or invalid actions. Generated code, method names, object paths and arbitrary reflection will never be executed.

### 6.6 Persistence and failure handling

JSON will be used for the assessed implementation because it is adequate for the limited dataset, easy to inspect and allows effort to be focused on the research question. Repository interfaces will permit a future SQLite implementation without making it part of the core scope. Save data will contain a schema version. Missing or corrupt data, service unavailability, timeouts and malformed model output will produce logged, safe fallback behaviour.

## 7. Development methodology

The project will follow an iterative, risk-first approach. Each iteration will leave a working demonstration:

1. establish the project charter, literature plan, evaluation plan and repository;
2. build a dialogue vertical slice with the mock provider;
3. add versioned JSON persistence;
4. add structured memory and explainable retrieval;
5. add deterministic context budgeting and context-selection diagnostics;
6. add bounded conversation summarisation and its fallback path;
7. add relationship state, the iron-delivery event and safe actions;
8. add the real provider while retaining the mock implementation;
9. implement all baseline configurations and the evaluation harness;
10. conduct experiments, analyse results and complete the report and video.

Source control will be used throughout. Automated tests and documentation will be updated with each relevant iteration. A risk register and decision log will record major scope and design choices.

Before formal project approval, a separate minimal feasibility prototype may be used to test Unity-to-provider communication, JSON persistence and deterministic context trimming. Its purpose will be to identify technical risks rather than to produce final evaluation evidence. Any pre-project work will be dated, disclosed to the supervisor and clearly distinguished from the assessed implementation and experiments. The formal project will retain its own requirements, design rationale, implementation history and evaluation dataset in accordance with current University guidance.

To protect the schedule, requirements will be prioritised as follows:

- **Must:** one NPC vertical slice, structured facts and events, JSON persistence, deterministic context budget, Top-K retrieval, three experiment modes, validation, tests and evaluation;
- **Should:** bounded older-dialogue summarisation, relationship state, debugging visualisation and one safe predefined gameplay action;
- **Could:** SQLite persistence, extended scheduling or a small ethics-approved participant evaluation;
- **Will not:** vector databases, multiple autonomous NPCs, generated quests, voice, multiplayer or production backend infrastructure.

If schedule risk emerges, the Should items will be reduced before any Must item or evaluation activity. In particular, deterministic context selection will remain the core method if generated summarisation proves unreliable.

## 8. Testing and evaluation

### 8.1 Software testing

Automated unit or EditMode tests will cover:

- memory creation, filtering, ranking and result limits;
- save/load round trips and schema-version handling;
- deterministic relationship changes;
- context construction and context-size limits;
- deterministic priority trimming, token-budget enforcement and summarisation fallback;
- response parsing and validation;
- rejection of unknown or unauthorised actions;
- cancellation, timeout and provider-failure behaviour;
- separation between different NPC or player identifiers where applicable.

Integration tests and scripted manual tests will cover the complete first-interaction, event, save/reload and later-interaction sequence.

### 8.2 Experimental comparison

Three configurations will be compared:

- **A - Memory-free baseline:** NPC profile, current world state and current player message, without persistent memory retrieval.
- **B - Full-history baseline:** the same profile and state plus all available conversation history within the provider's maximum permitted context.
- **C - Proposed selected-memory system:** the same profile and state plus required structured facts, Top-K event memories, recent turns and a bounded older-dialogue summary within a fixed context budget.

A small test set of approximately 15-20 scripted scenarios will be created. Each scenario will define prior facts and events, a later prompt, expected factual elements, personality-relevant expectations and prohibited contradictions. Stochastic model conditions will be repeated several times with the same model, parameters and output limit. Exact sample size and repeat count will be determined through a small pilot while remaining within cost and time constraints. The main confirmatory comparison will be A versus C; B will provide a bounded efficiency and ablation comparison rather than expanding into a separate research project.

### 8.3 Metrics

The principal metrics will be:

- **factual recall accuracy:** correctly recalled required facts divided by required facts;
- **contradiction rate:** responses conflicting with stored facts or authoritative world state;
- **personality consistency:** adherence to a predefined Arthur personality rubric across a fixed question set;
- **retrieval Precision@K:** relevant retrieved memories divided by all retrieved memories;
- **structured-response validity rate:** responses accepted by schema validation;
- **unsafe-action rejection rate:** invalid requests correctly blocked by the gateway;
- **save/load correctness:** successful recovery across defined persistence tests;
- **latency:** mean and 95th-percentile end-to-end response time;
- **usage:** input/output tokens and estimated cost per interaction when available.
- **context reduction:** percentage reduction in input tokens for C relative to B.

Descriptive statistics will be reported for all measures. Appropriate paired tests or non-parametric alternatives will be considered only if the final sample and distribution justify inferential analysis; otherwise the report will avoid overstating statistical significance. Representative failure cases will be analysed qualitatively to distinguish retrieval, generation and state-management errors.

### 8.4 Success criteria

The core project will be considered successful if:

1. the complete demonstration sequence operates across a save/reload cycle;
2. the proposed condition achieves materially higher factual recall and/or lower contradiction than the baseline in the controlled test set;
3. the selected-memory condition substantially reduces input context relative to full history without a material loss of factual recall;
4. personality consistency is assessed using a predefined and reproducible rubric;
5. the retriever, context builder and action gateway produce inspectable, reproducible decisions;
6. invalid output and unavailable services fail safely;
7. performance and usage results are reported transparently, including negative findings.

The software implementation can still be evaluated successfully if the experimental effect is small or mixed, provided the method is sound and the result is critically analysed.

## 9. Ethical, legal and security considerations

The core technical evaluation will use fictional characters and scripted interactions and therefore will not require collecting personal participant data. If a user study is later proposed, it will proceed only after confirming University of London ethics requirements and obtaining any necessary approval. Participation would be voluntary, use informed consent, collect minimal data and permit withdrawal.

The demonstration will warn users not to enter personal or sensitive information. Only data necessary for the experiment will be sent to the selected model provider. Provider terms and data-retention settings will be reviewed and documented. API credentials will be stored outside committed source and Unity assets. Logs and submitted datasets will exclude secrets and unnecessary personal information.

The system will treat generated language as untrusted input. Schema validation and the action gateway will prevent arbitrary Unity operations. The report will disclose the use of third-party models, libraries, assets and AI-assisted development in accordance with the University's current academic-integrity rules. No generated material will be represented as independently authored research evidence without verification.

## 10. Resources and feasibility

The project requires Unity, C#, the Unity Test Framework, Git, a JSON serialisation approach and access to one LLM API for the final experiment. Most development and all deterministic tests can use the mock provider, reducing cost and protecting the schedule from network availability. The demonstration will target a desktop build. Existing Unity development experience reduces implementation risk, while the constrained scope preserves sufficient time for literature review, evaluation and academic writing.

The largest technical risks are inconsistent model output, provider availability, unreliable summaries, scope growth and weak experimental evidence. These will be mitigated through structured validation, offline mocks, deterministic context trimming, summary fallbacks, strict exclusions, an early evaluation harness and baselines that can be executed with the same software.

## 11. Work plan

The plan is aligned with the module's 25-week structure and should be adjusted to the actual teaching calendar.

| Weeks | Planned work | Evidence/deliverable |
|---|---|---|
| 1-2 | Refine problem, scope, ethics and initial literature search | Project charter, source matrix, risk register |
| 3-4 | Complete proposal and design first experiment scenarios | Formative project proposal |
| 5-6 | Establish Unity project, assemblies, tests and mock-provider vertical slice | Working offline dialogue prototype |
| 7-8 | Implement structured models and versioned JSON persistence | Tested save/load of facts and state |
| 9-10 | Implement initial retriever, context budget and architecture documentation | Preliminary report and inspectable retrieval prototype |
| 11-12 | Implement bounded summarisation, relationship change and debug view | Complete context-managed interaction sequence |
| 13-14 | Implement iron-delivery event, validation, action gateway and failure handling | Safety and reliability test results |
| 15-16 | Add real provider, three experiment modes and pilot evaluation | Draft report and pilot dataset/results |
| 17-18 | Refine software in response to tests and feedback | Feature freeze candidate |
| 19-20 | Run controlled experiments and collect metrics | Versioned experiment data and analysis notebook/script |
| 21-22 | Analyse failures; prepare for written examination | Results tables, limitations and revision notes |
| 23 | Final regression testing and reproducibility check | Release candidate and test report |
| 24 | Complete final report and record demonstration | Report draft and video draft |
| 25 | Proofread, verify submission package and submit | Final report, code and presentation video |

## 12. Expected outputs

1. A Unity project containing the Arthur demonstration and reusable framework components.
2. Source code for the domain, memory, persistence, orchestration, provider and validation layers.
3. Automated tests and a reproducible test procedure.
4. A controlled experiment dataset and results comparing memory-free, full-history and selected-memory configurations.
5. Architecture, setup, security and evaluation documentation.
6. A final academic report and demonstration video.

## 13. Expected contribution

The expected contribution is not a new foundation model, a novel universal optimisation algorithm or a general simulation of human behaviour. It is the design and evaluation of a transparent hybrid architecture for a specific real-time game context. The project will demonstrate how structured persistent memory, explainable retrieval, budget-aware context selection, bounded summarisation, deterministic authoritative state and constrained language generation can be combined in Unity. Its evaluation will provide evidence about the accuracy-efficiency trade-off relative to memory-free and full-history baselines, as well as an analysis of failure cases and practical limitations.

## 14. References

Jia, Z., Liu, Q., Li, H., Chen, Y. and Liu, J. (2025) 'Evaluating the Long-Term Memory of Large Language Models', *Findings of the Association for Computational Linguistics: ACL 2025*, pp. 19759-19777. Available at: https://aclanthology.org/2025.findings-acl.1014/.

Lewis, P. et al. (2020) 'Retrieval-Augmented Generation for Knowledge-Intensive NLP Tasks', *Advances in Neural Information Processing Systems*, 33. Available at: https://arxiv.org/abs/2005.11401.

Maharana, A., Lee, D.-H., Tulyakov, S., Bansal, M., Barbieri, F. and Fang, Y. (2024) 'Evaluating Very Long-Term Conversational Memory of LLM Agents', *Proceedings of the 62nd Annual Meeting of the Association for Computational Linguistics*, pp. 13851-13870. Available at: https://aclanthology.org/2024.acl-long.747/.

Park, J.S., O'Brien, J.C., Cai, C.J., Morris, M.R., Liang, P. and Bernstein, M.S. (2023) 'Generative Agents: Interactive Simulacra of Human Behavior', *Proceedings of the 36th Annual ACM Symposium on User Interface Software and Technology*. Available at: https://arxiv.org/abs/2304.03442.

Xu, X., Gou, Z., Wu, W., Niu, Z.-Y., Wu, H., Wang, H. and Wang, S. (2022) 'Long Time No See! Open-Domain Conversation with Long-Term Persona Memory', *Findings of the Association for Computational Linguistics: ACL 2022*, pp. 2639-2650. Available at: https://aclanthology.org/2022.findings-acl.207/.

## 15. Items to confirm before submission

- the official CM3070 proposal template, required headings and word limit;
- the assessment brief and marking rubric for the current study session;
- the required citation style;
- whether the proposal needs a formal supervisor approval section;
- the University's current policy for declaring generative-AI assistance;
- ethics requirements if any participant study is retained;
- code, dataset and video submission formats.
