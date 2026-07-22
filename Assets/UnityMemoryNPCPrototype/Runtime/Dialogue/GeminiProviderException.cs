using System;

namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Reports a failed Gemini HTTP request without exposing credential data.
    /// </summary>
    public sealed class GeminiProviderException : Exception
    {
        /// <summary>
        /// Gets the HTTP response code, or zero when no response was received.
        /// </summary>
        public long ResponseCode { get; }

        /// <summary>
        /// Creates a Gemini provider exception.
        /// </summary>
        /// <param name="responseCode">The HTTP response code.</param>
        /// <param name="message">The sanitized failure description.</param>
        public GeminiProviderException(long responseCode, string message) : base(message)
        {
            ResponseCode = responseCode;
        }
    }
}
