using System;
using System.Collections.Generic;
using System.Text;
using UnityMemoryNPCPrototype.Memory;

namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Builds deterministic provider context within a transparent character budget.
    /// </summary>
    public sealed class DialogueContextBuilder
    {
        /// <summary>
        /// Defines the default prototype context budget.
        /// </summary>
        public const int DefaultCharacterBudget = 600;

        /// <summary>
        /// Stores the separator inserted between context sections.
        /// </summary>
        private const string SectionSeparator = "\n\n";

        /// <summary>
        /// Stores the configured character budget.
        /// </summary>
        private readonly int mCharacterBudget;

        /// <summary>
        /// Creates a context builder.
        /// </summary>
        /// <param name="characterBudget">The maximum number of context characters.</param>
        public DialogueContextBuilder(int characterBudget = DefaultCharacterBudget)
        {
            if (characterBudget <= 0)
                throw new ArgumentOutOfRangeException(nameof(characterBudget));

            mCharacterBudget = characterBudget;
        }

        /// <summary>
        /// Builds context in fixed priority order and trims the oldest optional turns first.
        /// </summary>
        /// <param name="memory">The structured player memory.</param>
        /// <param name="recentTurns">The recent turns ordered from oldest to newest.</param>
        /// <param name="playerMessage">The current normalized player message.</param>
        /// <returns>The context and its inspectable inclusion decisions.</returns>
        public DialogueContextBuildResult Build(PlayerMemoryData memory, IReadOnlyList<DialogueContextItem> recentTurns, string playerMessage)
        {
            if (memory is null)
                throw new ArgumentNullException(nameof(memory));

            if (recentTurns is null)
                throw new ArgumentNullException(nameof(recentTurns));

            if (string.IsNullOrWhiteSpace(playerMessage))
                throw new ArgumentException("A player message is required.", nameof(playerMessage));

            var requiredItems = new List<DialogueContextItem>
            {
                new DialogueContextItem("instructions", "Instructions: Reply as Arthur in one concise sentence. Use only supplied player facts."),
                new DialogueContextItem("npc.profile", "NPC profile: Arthur is a practical village blacksmith."),
            };
            AddFact(memory, requiredItems, PlayerFactKeys.Name, "fact.player.name", "Player name");
            AddFact(memory, requiredItems, PlayerFactKeys.WeaponPreference, "fact.player.preference.weapon", "Player weapon preference");
            requiredItems.Add(new DialogueContextItem("message.current", $"Current player message: {playerMessage}"));

            var selectedTurns = SelectRecentTurns(requiredItems, recentTurns);
            var includedIds = new List<string>();
            var removedIds = new List<string>();
            var context = Compose(requiredItems, selectedTurns, includedIds);

            foreach (var turn in recentTurns)
            {
                if (!ContainsId(selectedTurns, turn.Id))
                    removedIds.Add(turn.Id);
            }

            return new DialogueContextBuildResult(context, mCharacterBudget, includedIds.ToArray(), removedIds.ToArray());
        }

        /// <summary>
        /// Adds a supported structured fact when it is available.
        /// </summary>
        /// <param name="memory">The structured player memory.</param>
        /// <param name="items">The required item collection.</param>
        /// <param name="factKey">The stable memory key.</param>
        /// <param name="itemId">The stable context item identifier.</param>
        /// <param name="label">The provider-facing fact label.</param>
        private static void AddFact(PlayerMemoryData memory, ICollection<DialogueContextItem> items, string factKey, string itemId, string label)
        {
            if (memory.TryGetFact(factKey, out var value))
                items.Add(new DialogueContextItem(itemId, $"{label}: {value}"));
        }

        /// <summary>
        /// Selects the newest suffix of optional turns that fits the remaining budget.
        /// </summary>
        /// <param name="requiredItems">The required context items.</param>
        /// <param name="recentTurns">The optional turns ordered oldest to newest.</param>
        /// <returns>The selected turns in chronological order.</returns>
        private List<DialogueContextItem> SelectRecentTurns(IReadOnlyList<DialogueContextItem> requiredItems, IReadOnlyList<DialogueContextItem> recentTurns)
        {
            var requiredLength = CalculateLength(requiredItems);
            if (requiredLength > mCharacterBudget)
                throw new InvalidOperationException($"Required context uses {requiredLength} characters but the budget is {mCharacterBudget}.");

            var selectedTurns = new List<DialogueContextItem>();
            var usedLength = requiredLength;
            for (var i = recentTurns.Count - 1; i >= 0; i--)
            {
                var additionalLength = SectionSeparator.Length + Format(recentTurns[i]).Length;
                if (usedLength + additionalLength > mCharacterBudget)
                    break;

                selectedTurns.Insert(0, recentTurns[i]);
                usedLength += additionalLength;
            }

            return selectedTurns;
        }

        /// <summary>
        /// Composes required sections with recent turns before the current message.
        /// </summary>
        /// <param name="requiredItems">The required context items.</param>
        /// <param name="selectedTurns">The selected optional turns.</param>
        /// <param name="includedIds">The destination for included identifiers.</param>
        /// <returns>The complete provider context.</returns>
        private static string Compose(IReadOnlyList<DialogueContextItem> requiredItems, IReadOnlyList<DialogueContextItem> selectedTurns, ICollection<string> includedIds)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < requiredItems.Count - 1; i++)
                Append(builder, requiredItems[i], includedIds);

            foreach (var turn in selectedTurns)
                Append(builder, turn, includedIds);

            Append(builder, requiredItems[requiredItems.Count - 1], includedIds);
            return builder.ToString();
        }

        /// <summary>
        /// Appends one formatted context item and records its identifier.
        /// </summary>
        /// <param name="builder">The context text builder.</param>
        /// <param name="item">The item to append.</param>
        /// <param name="includedIds">The destination for included identifiers.</param>
        private static void Append(StringBuilder builder, DialogueContextItem item, ICollection<string> includedIds)
        {
            if (builder.Length > 0)
                builder.Append(SectionSeparator);

            builder.Append(Format(item));
            includedIds.Add(item.Id);
        }

        /// <summary>
        /// Calculates the formatted length of a context item collection.
        /// </summary>
        /// <param name="items">The context items.</param>
        /// <returns>The formatted character count.</returns>
        private static int CalculateLength(IReadOnlyList<DialogueContextItem> items)
        {
            var length = 0;
            for (var i = 0; i < items.Count; i++)
            {
                if (i > 0)
                    length += SectionSeparator.Length;

                length += Format(items[i]).Length;
            }

            return length;
        }

        /// <summary>
        /// Formats one context item with an inspectable identifier marker.
        /// </summary>
        /// <param name="item">The item to format.</param>
        /// <returns>The formatted context section.</returns>
        private static string Format(DialogueContextItem item)
        {
            return $"[{item.Id}]\n{item.Content}";
        }

        /// <summary>
        /// Checks whether a context item identifier exists in a collection.
        /// </summary>
        /// <param name="items">The context items.</param>
        /// <param name="id">The identifier to locate.</param>
        /// <returns>Whether the identifier exists.</returns>
        private static bool ContainsId(IReadOnlyList<DialogueContextItem> items, string id)
        {
            foreach (var item in items)
            {
                if (string.Equals(item.Id, id, StringComparison.Ordinal))
                    return true;
            }

            return false;
        }
    }
}
