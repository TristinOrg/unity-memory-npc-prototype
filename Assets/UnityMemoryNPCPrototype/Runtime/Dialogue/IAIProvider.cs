using System.Threading;
using System.Threading.Tasks;

namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Defines the provider boundary used to generate NPC dialogue.
    /// </summary>
    public interface IAIProvider
    {
        /// <summary>
        /// Generates a response for a player message.
        /// </summary>
        /// <param name="playerMessage">The normalized player message.</param>
        /// <param name="cancellationToken">The token used to cancel the request.</param>
        /// <returns>The generated dialogue response.</returns>
        Task<AIResponse> GenerateAsync(string playerMessage, CancellationToken cancellationToken);
    }
}
