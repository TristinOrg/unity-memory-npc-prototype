using System.Threading;
using System.Threading.Tasks;
using UnityMemoryNPCPrototype.Dialogue;

namespace UnityMemoryNPCPrototype.Tests.EditMode
{
    /// <summary>
    /// Supplies a configured task for provider reliability tests.
    /// </summary>
    public sealed class StubAIProvider : IAIProvider
    {
        /// <summary>
        /// Stores the task returned for every request.
        /// </summary>
        private readonly Task<AIResponse> mResponseTask;

        /// <summary>
        /// Creates a stub provider.
        /// </summary>
        /// <param name="responseTask">The task returned for every request.</param>
        public StubAIProvider(Task<AIResponse> responseTask)
        {
            mResponseTask = responseTask;
        }

        /// <summary>
        /// Returns the configured provider task.
        /// </summary>
        /// <param name="request">The ignored dialogue request.</param>
        /// <param name="cancellationToken">The ignored cancellation token.</param>
        /// <returns>The configured provider task.</returns>
        public Task<AIResponse> GenerateAsync(DialogueRequest request, CancellationToken cancellationToken)
        {
            return mResponseTask;
        }
    }
}
