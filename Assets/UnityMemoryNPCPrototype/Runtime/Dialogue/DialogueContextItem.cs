namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Identifies one optional dialogue context item.
    /// </summary>
    public readonly struct DialogueContextItem
    {
        /// <summary>
        /// Gets the stable item identifier used by diagnostics.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the item content supplied to the provider.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Creates an optional context item.
        /// </summary>
        /// <param name="id">The stable diagnostic identifier.</param>
        /// <param name="content">The provider-facing content.</param>
        public DialogueContextItem(string id, string content)
        {
            Id = id;
            Content = content;
        }
    }
}
