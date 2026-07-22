using System.Collections.Generic;

namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Describes the deterministic output and trimming decisions of a context build.
    /// </summary>
    public sealed class DialogueContextBuildResult
    {
        /// <summary>
        /// Gets the provider-facing context.
        /// </summary>
        public string Context { get; }

        /// <summary>
        /// Gets the number of characters used by the context.
        /// </summary>
        public int UsedCharacters { get; }

        /// <summary>
        /// Gets the configured character budget.
        /// </summary>
        public int Budget { get; }

        /// <summary>
        /// Gets the identifiers included in the context.
        /// </summary>
        public IReadOnlyList<string> IncludedItemIds { get; }

        /// <summary>
        /// Gets the identifiers removed by budget trimming.
        /// </summary>
        public IReadOnlyList<string> RemovedItemIds { get; }

        /// <summary>
        /// Creates a context build result.
        /// </summary>
        /// <param name="context">The provider-facing context.</param>
        /// <param name="budget">The configured character budget.</param>
        /// <param name="includedItemIds">The included diagnostic identifiers.</param>
        /// <param name="removedItemIds">The removed diagnostic identifiers.</param>
        public DialogueContextBuildResult(string context, int budget, IReadOnlyList<string> includedItemIds, IReadOnlyList<string> removedItemIds)
        {
            Context = context;
            UsedCharacters = context.Length;
            Budget = budget;
            IncludedItemIds = includedItemIds;
            RemovedItemIds = removedItemIds;
        }
    }
}
