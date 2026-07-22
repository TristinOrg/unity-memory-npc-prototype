namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Represents a provider-independent dialogue response.
    /// </summary>
    public readonly struct AIResponse
    {
        /// <summary>
        /// Gets Arthur's dialogue text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Creates a dialogue response.
        /// </summary>
        /// <param name="text">Arthur's dialogue text.</param>
        public AIResponse(string text)
        {
            Text = text;
        }
    }
}
