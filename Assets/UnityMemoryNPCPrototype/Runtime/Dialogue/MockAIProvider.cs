using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Provides deterministic offline dialogue for development and testing.
    /// </summary>
    public sealed class MockAIProvider : IAIProvider
    {
        /// <summary>
        /// The deterministic response returned for valid input.
        /// </summary>
        public const string DefaultResponse = "The forge is open. What do you need?";

        /// <summary>
        /// Generates a deterministic response without network access.
        /// </summary>
        /// <param name="playerMessage">The normalized player message.</param>
        /// <param name="cancellationToken">The token used to cancel the request.</param>
        /// <returns>The completed or cancelled dialogue task.</returns>
        public Task<AIResponse> GenerateAsync(string playerMessage, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<AIResponse>(cancellationToken);

            if (string.IsNullOrWhiteSpace(playerMessage))
                throw new ArgumentException("A player message is required.", nameof(playerMessage));

            return Task.FromResult(new AIResponse(DefaultResponse));
        }
    }
}
