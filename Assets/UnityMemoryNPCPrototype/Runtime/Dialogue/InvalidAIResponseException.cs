using System;

namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Reports a provider response that cannot be presented as dialogue.
    /// </summary>
    public sealed class InvalidAIResponseException : Exception
    {
        /// <summary>
        /// Creates an invalid response exception.
        /// </summary>
        /// <param name="message">The validation failure description.</param>
        public InvalidAIResponseException(string message) : base(message)
        {
        }
    }
}
