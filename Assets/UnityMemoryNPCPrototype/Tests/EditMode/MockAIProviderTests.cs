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
            var response = provider.GenerateAsync("Hello", CancellationToken.None).GetAwaiter().GetResult();

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
                provider.GenerateAsync(" ", CancellationToken.None);
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

            var task = provider.GenerateAsync("Hello", cancellation.Token);

            Assert.That(task.IsCanceled, Is.True);
            cancellation.Dispose();
        }
    }
}
