using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityMemoryNPCPrototype.Dialogue;

namespace UnityMemoryNPCPrototype.Tests.EditMode
{
    /// <summary>
    /// Verifies provider-independent timeout, cancellation and response validation.
    /// </summary>
    public sealed class ReliableAIProviderTests
    {
        /// <summary>
        /// Verifies that valid response text is normalized before presentation.
        /// </summary>
        [Test]
        public void ValidResponseIsTrimmed()
        {
            var innerProvider = new StubAIProvider(Task.FromResult(new AIResponse("  Ready.  ")));
            var provider = new ReliableAIProvider(innerProvider, TimeSpan.FromSeconds(1));

            var response = provider.GenerateAsync(CreateRequest(), CancellationToken.None).GetAwaiter().GetResult();

            Assert.That(response.Text, Is.EqualTo("Ready."));
        }

        /// <summary>
        /// Verifies that an empty provider response is rejected.
        /// </summary>
        [Test]
        public void EmptyResponseThrowsInvalidAIResponseException()
        {
            var innerProvider = new StubAIProvider(Task.FromResult(new AIResponse(" ")));
            var provider = new ReliableAIProvider(innerProvider, TimeSpan.FromSeconds(1));

            try
            {
                provider.GenerateAsync(CreateRequest(), CancellationToken.None).GetAwaiter().GetResult();
                Assert.Fail("Expected an InvalidAIResponseException for empty dialogue.");
            }
            catch (InvalidAIResponseException)
            {
            }
        }

        /// <summary>
        /// Verifies that a provider exceeding its deadline reports a timeout.
        /// </summary>
        [Test]
        public void SlowProviderThrowsTimeoutException()
        {
            var pendingResponse = new TaskCompletionSource<AIResponse>();
            var innerProvider = new StubAIProvider(pendingResponse.Task);
            var provider = new ReliableAIProvider(innerProvider, TimeSpan.FromMilliseconds(20));

            try
            {
                provider.GenerateAsync(CreateRequest(), CancellationToken.None).GetAwaiter().GetResult();
                Assert.Fail("Expected a TimeoutException for a slow provider.");
            }
            catch (TimeoutException)
            {
            }
        }

        /// <summary>
        /// Verifies that caller cancellation remains distinct from timeout.
        /// </summary>
        [Test]
        public void CallerCancellationThrowsOperationCanceledException()
        {
            var pendingResponse = new TaskCompletionSource<AIResponse>();
            var innerProvider = new StubAIProvider(pendingResponse.Task);
            var provider = new ReliableAIProvider(innerProvider, TimeSpan.FromSeconds(1));
            var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            try
            {
                provider.GenerateAsync(CreateRequest(), cancellation.Token).GetAwaiter().GetResult();
                Assert.Fail("Expected an OperationCanceledException for caller cancellation.");
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                cancellation.Dispose();
            }
        }

        /// <summary>
        /// Verifies that provider exceptions remain available to the presentation fallback path.
        /// </summary>
        [Test]
        public void ProviderExceptionIsPreserved()
        {
            var expectedException = new InvalidOperationException("Provider failed.");
            var innerProvider = new StubAIProvider(Task.FromException<AIResponse>(expectedException));
            var provider = new ReliableAIProvider(innerProvider, TimeSpan.FromSeconds(1));

            try
            {
                provider.GenerateAsync(CreateRequest(), CancellationToken.None).GetAwaiter().GetResult();
                Assert.Fail("Expected the provider exception to be preserved.");
            }
            catch (InvalidOperationException exception)
            {
                Assert.That(exception, Is.SameAs(expectedException));
            }
        }

        /// <summary>
        /// Creates a representative valid request.
        /// </summary>
        /// <returns>The provider request.</returns>
        private static DialogueRequest CreateRequest()
        {
            return new DialogueRequest("Hello", "[message.current]\nCurrent player message: Hello");
        }
    }
}
