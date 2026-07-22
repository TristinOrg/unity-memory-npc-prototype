namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Carries the current message and its deterministic provider context.
    /// </summary>
    public readonly struct DialogueRequest
    {
        /// <summary>
        /// Gets the normalized player message.
        /// </summary>
        public string PlayerMessage { get; }

        /// <summary>
        /// Gets the complete context supplied to the provider.
        /// </summary>
        public string Context { get; }

        /// <summary>
        /// Creates a dialogue request.
        /// </summary>
        /// <param name="playerMessage">The normalized player message.</param>
        /// <param name="context">The complete provider context.</param>
        public DialogueRequest(string playerMessage, string context)
        {
            PlayerMessage = playerMessage;
            Context = context;
        }
    }
}
