using System;

namespace UnityMemoryNPCPrototype.Memory
{
    /// <summary>
    /// Extracts the two explicit structured facts required by the demonstration.
    /// </summary>
    public static class PlayerFactExtractor
    {
        /// <summary>
        /// The supported player-name phrase.
        /// </summary>
        private const string NamePrefix = "my name is ";

        /// <summary>
        /// The supported preference phrase.
        /// </summary>
        private const string PreferencePrefix = "i like ";

        /// <summary>
        /// Extracts supported facts from a player message.
        /// </summary>
        /// <param name="playerMessage">The original player message.</param>
        /// <param name="memory">The structured memory to update.</param>
        /// <returns>Whether any fact changed.</returns>
        public static bool Apply(string playerMessage, PlayerMemoryData memory)
        {
            if (string.IsNullOrWhiteSpace(playerMessage))
                return false;

            if (memory is null)
                throw new ArgumentNullException(nameof(memory));

            var changed = false;
            var nameStart = playerMessage.IndexOf(NamePrefix, StringComparison.OrdinalIgnoreCase);
            if (nameStart >= 0)
            {
                nameStart += NamePrefix.Length;
                var nameEnd = playerMessage.IndexOf(" and ", nameStart, StringComparison.OrdinalIgnoreCase);
                if (nameEnd < 0)
                    nameEnd = playerMessage.Length;

                var name = TrimFactValue(playerMessage.Substring(nameStart, nameEnd - nameStart));
                if (!string.IsNullOrWhiteSpace(name))
                    changed |= memory.SetFact(PlayerFactKeys.Name, name);
            }

            var preferenceStart = playerMessage.IndexOf(PreferencePrefix, StringComparison.OrdinalIgnoreCase);
            if (preferenceStart >= 0)
            {
                preferenceStart += PreferencePrefix.Length;
                var preference = TrimFactValue(playerMessage.Substring(preferenceStart));
                if (!string.IsNullOrWhiteSpace(preference))
                    changed |= memory.SetFact(PlayerFactKeys.WeaponPreference, preference);
            }

            return changed;
        }

        /// <summary>
        /// Removes surrounding whitespace and sentence punctuation from a fact value.
        /// </summary>
        /// <param name="value">The extracted text value.</param>
        /// <returns>The normalized fact value.</returns>
        private static string TrimFactValue(string value)
        {
            return value.Trim().TrimEnd('.', ',', '!', '?');
        }
    }
}
