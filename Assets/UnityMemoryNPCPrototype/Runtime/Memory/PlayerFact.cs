using System;

namespace UnityMemoryNPCPrototype.Memory
{
    /// <summary>
    /// Represents one structured fact known about the player.
    /// </summary>
    [Serializable]
    public struct PlayerFact
    {
        /// <summary>
        /// Stores the stable fact key.
        /// </summary>
        public string Key;

        /// <summary>
        /// Stores the normalized fact value.
        /// </summary>
        public string Value;

        /// <summary>
        /// Creates a structured player fact.
        /// </summary>
        /// <param name="key">The stable fact key.</param>
        /// <param name="value">The normalized fact value.</param>
        public PlayerFact(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
