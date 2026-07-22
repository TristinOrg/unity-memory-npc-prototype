using System;
using System.Threading;
using NUnit.Framework;
using UnityMemoryNPCPrototype.Dialogue;

namespace UnityMemoryNPCPrototype.Tests.EditMode
{
    /// <summary>
    /// Verifies the deterministic offline provider contract.
    /// </summary>
    public sealed class MockAIProviderTests
    {
        /// <summary>
        /// Verifies that valid input produces the fixed Arthur response.
        /// </summary>
        [Test]
        public void ValidMessageReturnsDeterministicResponse()
        {
            var provider = new MockAIProvider();
            var request = new DialogueRequest("Hello", "[message.current]\nCurrent player message: Hello");
            var response = provider.GenerateAsync(request, CancellationToken.None).GetAwaiter().GetResult();

            Assert.That(response.Text, Is.EqualTo(MockAIProvider.DefaultResponse));
        }

        /// <summary>
        /// Verifies that blank input is rejected at the provider boundary.
        /// </summary>
        [Test]
        public void BlankMessageThrowsArgumentException()
        {
            var provider = new MockAIProvider();

            try
            {
                provider.GenerateAsync(new DialogueRequest(" ", "context"), CancellationToken.None);
                Assert.Fail("Expected an ArgumentException for blank input.");
            }
            catch (ArgumentException)
            {
            }
        }

        /// <summary>
        /// Verifies that a cancelled request returns a cancelled task.
        /// </summary>
        [Test]
        public void CancelledRequestReturnsCancelledTask()
        {
            var provider = new MockAIProvider();
            var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            var request = new DialogueRequest("Hello", "context");
            var task = provider.GenerateAsync(request, cancellation.Token);

            Assert.That(task.IsCanceled, Is.True);
            cancellation.Dispose();
        }

        /// <summary>
        /// Verifies that recall is derived only from facts present in the supplied context.
        /// </summary>
        [Test]
        public void RecallUsesStructuredFactsFromContext()
        {
            var provider = new MockAIProvider();
            var context = "[fact.player.name]\nPlayer name: Alex\n\n[fact.player.preference.weapon]\nPlayer weapon preference: swords";
            var request = new DialogueRequest("Do you remember me?", context);

            var response = provider.GenerateAsync(request, CancellationToken.None).GetAwaiter().GetResult();

            Assert.That(response.Text, Is.EqualTo("Welcome back, Alex. I remember that you are interested in swords."));
        }
    }
}
