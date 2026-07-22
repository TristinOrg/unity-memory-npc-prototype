using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Enforces a provider-independent deadline and validates dialogue responses.
    /// </summary>
    public sealed class ReliableAIProvider : IAIProvider
    {
        /// <summary>
        /// Stores the provider that performs the underlying request.
        /// </summary>
        private readonly IAIProvider mInnerProvider;

        /// <summary>
        /// Stores the maximum request duration.
        /// </summary>
        private readonly TimeSpan mTimeout;

        /// <summary>
        /// Creates a reliable provider wrapper.
        /// </summary>
        /// <param name="innerProvider">The provider that performs the underlying request.</param>
        /// <param name="timeout">The maximum request duration.</param>
        public ReliableAIProvider(IAIProvider innerProvider, TimeSpan timeout)
        {
            mInnerProvider = innerProvider ?? throw new ArgumentNullException(nameof(innerProvider));
            if (timeout <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeout));

            mTimeout = timeout;
        }

        /// <summary>
        /// Generates and validates a response within the configured deadline.
        /// </summary>
        /// <param name="request">The message and deterministic provider context.</param>
        /// <param name="cancellationToken">The token used to cancel the request.</param>
        /// <returns>The validated dialogue response.</returns>
        public async Task<AIResponse> GenerateAsync(DialogueRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var providerCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            using var timeoutCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var providerTask = mInnerProvider.GenerateAsync(request, providerCancellation.Token);
            var timeoutTask = Task.Delay(mTimeout, timeoutCancellation.Token);
            var completedTask = await Task.WhenAny(providerTask, timeoutTask).ConfigureAwait(false);

            if (!ReferenceEquals(completedTask, providerTask))
            {
                cancellationToken.ThrowIfCancellationRequested();
                providerCancellation.Cancel();
                throw new TimeoutException($"The dialogue provider exceeded the {mTimeout.TotalSeconds:0.###}-second deadline.");
            }

            timeoutCancellation.Cancel();
            var response = await providerTask.ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(response.Text))
                throw new InvalidAIResponseException("The dialogue provider returned an empty response.");

            return new AIResponse(response.Text.Trim());
        }
    }
}
