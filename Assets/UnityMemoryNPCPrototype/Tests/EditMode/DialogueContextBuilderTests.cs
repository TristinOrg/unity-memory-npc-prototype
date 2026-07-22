using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityMemoryNPCPrototype.Dialogue;
using UnityMemoryNPCPrototype.Memory;

namespace UnityMemoryNPCPrototype.Tests.EditMode
{
    /// <summary>
    /// Verifies deterministic context priority and budget enforcement.
    /// </summary>
    public sealed class DialogueContextBuilderTests
    {
        /// <summary>
        /// Verifies that structured facts and the current message remain required context.
        /// </summary>
        [Test]
        public void RequiredFactsAndCurrentMessageAreIncluded()
        {
            var memory = CreateMemory();
            var builder = new DialogueContextBuilder(600);

            var result = builder.Build(memory, Array.Empty<DialogueContextItem>(), "Do you remember me?");

            Assert.That(result.Context, Does.Contain("Player name: Alex"));
            Assert.That(result.Context, Does.Contain("Player weapon preference: swords"));
            Assert.That(result.Context, Does.Contain("Current player message: Do you remember me?"));
            Assert.That(result.UsedCharacters, Is.LessThanOrEqualTo(result.Budget));
        }

        /// <summary>
        /// Verifies that trimming preserves the newest suffix of optional turns.
        /// </summary>
        [Test]
        public void BudgetTrimsOldestRecentTurnsFirst()
        {
            var memory = CreateMemory();
            var turns = new List<DialogueContextItem>
            {
                new DialogueContextItem("turn.old", new string('a', 100)),
                new DialogueContextItem("turn.new", "Arthur: newest"),
            };
            var builder = new DialogueContextBuilder(400);

            var result = builder.Build(memory, turns, "Hello");

            Assert.That(result.IncludedItemIds, Does.Contain("turn.new"));
            Assert.That(result.RemovedItemIds, Does.Contain("turn.old"));
            Assert.That(result.UsedCharacters, Is.LessThanOrEqualTo(400));
        }

        /// <summary>
        /// Verifies that identical inputs produce identical context and diagnostics.
        /// </summary>
        [Test]
        public void IdenticalInputsProduceIdenticalResults()
        {
            var memory = CreateMemory();
            var turns = new List<DialogueContextItem>
            {
                new DialogueContextItem("turn.0", "Player: Hello"),
            };
            var builder = new DialogueContextBuilder();

            var first = builder.Build(memory, turns, "Remember me?");
            var second = builder.Build(memory, turns, "Remember me?");

            Assert.That(second.Context, Is.EqualTo(first.Context));
            Assert.That(second.IncludedItemIds, Is.EqualTo(first.IncludedItemIds));
            Assert.That(second.RemovedItemIds, Is.EqualTo(first.RemovedItemIds));
        }

        /// <summary>
        /// Verifies that an impossible required budget fails explicitly.
        /// </summary>
        [Test]
        public void TooSmallBudgetThrowsInvalidOperationException()
        {
            var builder = new DialogueContextBuilder(10);

            try
            {
                builder.Build(CreateMemory(), Array.Empty<DialogueContextItem>(), "Hello");
                Assert.Fail("Expected an InvalidOperationException for an impossible required budget.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Creates representative structured memory for context tests.
        /// </summary>
        /// <returns>The populated player memory.</returns>
        private static PlayerMemoryData CreateMemory()
        {
            var memory = new PlayerMemoryData();
            memory.SetFact(PlayerFactKeys.Name, "Alex");
            memory.SetFact(PlayerFactKeys.WeaponPreference, "swords");
            return memory;
        }
    }
}
