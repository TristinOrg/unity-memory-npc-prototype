using System;
using System.Collections.Generic;

namespace UnityMemoryNPCPrototype.Memory
{
    /// <summary>
    /// Stores the versioned structured memory persisted for the player.
    /// </summary>
    [Serializable]
    public sealed class PlayerMemoryData
    {
        /// <summary>
        /// The current persistence schema version.
        /// </summary>
        public const int CurrentSchemaVersion = 1;

        /// <summary>
        /// Stores the schema version used to serialize this document.
        /// </summary>
        public int SchemaVersion = CurrentSchemaVersion;

        /// <summary>
        /// Stores the known structured player facts.
        /// </summary>
        public List<PlayerFact> Facts = new();

        /// <summary>
        /// Adds or replaces a fact by its stable key.
        /// </summary>
        /// <param name="key">The stable fact key.</param>
        /// <param name="value">The normalized fact value.</param>
        /// <returns>Whether the stored data changed.</returns>
        public bool SetFact(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("A fact key is required.", nameof(key));

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("A fact value is required.", nameof(value));

            for (var i = 0; i < Facts.Count; i++)
            {
                if (!string.Equals(Facts[i].Key, key, StringComparison.Ordinal))
                    continue;

                if (string.Equals(Facts[i].Value, value, StringComparison.Ordinal))
                    return false;

                Facts[i] = new PlayerFact(key, value);
                return true;
            }

            Facts.Add(new PlayerFact(key, value));
            return true;
        }

        /// <summary>
        /// Tries to read a fact by its stable key.
        /// </summary>
        /// <param name="key">The stable fact key.</param>
        /// <param name="value">The stored fact value when found.</param>
        /// <returns>Whether the fact exists.</returns>
        public bool TryGetFact(string key, out string value)
        {
            foreach (var fact in Facts)
            {
                if (!string.Equals(fact.Key, key, StringComparison.Ordinal))
                    continue;

                value = fact.Value;
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Restores safe defaults after deserialization.
        /// </summary>
        public void Normalize()
        {
            Facts ??= new List<PlayerFact>();
        }
    }
}
