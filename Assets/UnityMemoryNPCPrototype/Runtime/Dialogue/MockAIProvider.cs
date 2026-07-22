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
        /// Defines the marker used to locate the structured player name.
        /// </summary>
        private const string PlayerNamePrefix = "Player name: ";

        /// <summary>
        /// Defines the marker used to locate the structured weapon preference.
        /// </summary>
        private const string WeaponPreferencePrefix = "Player weapon preference: ";

        /// <summary>
        /// Generates a deterministic response without network access.
        /// </summary>
        /// <param name="request">The message and deterministic provider context.</param>
        /// <param name="cancellationToken">The token used to cancel the request.</param>
        /// <returns>The completed or cancelled dialogue task.</returns>
        public Task<AIResponse> GenerateAsync(DialogueRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<AIResponse>(cancellationToken);

            if (string.IsNullOrWhiteSpace(request.PlayerMessage))
                throw new ArgumentException("A player message is required.", nameof(request));

            if (string.IsNullOrWhiteSpace(request.Context))
                throw new ArgumentException("Provider context is required.", nameof(request));

            if (request.PlayerMessage.IndexOf("remember me", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var playerName = ReadValue(request.Context, PlayerNamePrefix);
                var weaponPreference = ReadValue(request.Context, WeaponPreferencePrefix);
                if (!string.IsNullOrWhiteSpace(playerName) && !string.IsNullOrWhiteSpace(weaponPreference))
                    return Task.FromResult(new AIResponse($"Welcome back, {playerName}. I remember that you are interested in {weaponPreference}."));
            }

            return Task.FromResult(new AIResponse(DefaultResponse));
        }

        /// <summary>
        /// Reads a single-line value from the supplied provider context.
        /// </summary>
        /// <param name="context">The complete provider context.</param>
        /// <param name="prefix">The value prefix.</param>
        /// <returns>The matched value, or null when absent.</returns>
        private static string ReadValue(string context, string prefix)
        {
            var start = context.IndexOf(prefix, StringComparison.Ordinal);
            if (start < 0)
                return null;

            start += prefix.Length;
            var end = context.IndexOf('\n', start);
            return end < 0 ? context.Substring(start).Trim() : context.Substring(start, end - start).Trim();
        }
    }
}
