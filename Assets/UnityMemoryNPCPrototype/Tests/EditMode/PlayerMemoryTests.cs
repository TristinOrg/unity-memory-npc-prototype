using System;
using System.IO;
using NUnit.Framework;
using UnityMemoryNPCPrototype.Memory;

namespace UnityMemoryNPCPrototype.Tests.EditMode
{
    /// <summary>
    /// Verifies structured fact extraction and versioned JSON persistence.
    /// </summary>
    public sealed class PlayerMemoryTests
    {
        /// <summary>
        /// Stores the isolated directory used by each test.
        /// </summary>
        private string mDirectoryPath;

        /// <summary>
        /// Stores the isolated JSON file used by each test.
        /// </summary>
        private string mFilePath;

        /// <summary>
        /// Creates an isolated persistence location.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            mDirectoryPath = Path.Combine(Path.GetTempPath(), "UnityMemoryNPCPrototypeTests", Guid.NewGuid().ToString("N"));
            mFilePath = Path.Combine(mDirectoryPath, "player-memory-v1.json");
        }

        /// <summary>
        /// Removes the isolated persistence location.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(mDirectoryPath))
                Directory.Delete(mDirectoryPath, true);
        }

        /// <summary>
        /// Verifies that updating a key replaces its previous value.
        /// </summary>
        [Test]
        public void SetFactReplacesExistingValue()
        {
            var memory = new PlayerMemoryData();
            memory.SetFact(PlayerFactKeys.Name, "Alex");

            var changed = memory.SetFact(PlayerFactKeys.Name, "Sam");

            Assert.That(changed, Is.True);
            Assert.That(memory.Facts, Has.Count.EqualTo(1));
            Assert.That(memory.TryGetFact(PlayerFactKeys.Name, out var value), Is.True);
            Assert.That(value, Is.EqualTo("Sam"));
        }

        /// <summary>
        /// Verifies deterministic extraction of the demonstration facts.
        /// </summary>
        [Test]
        public void DemonstrationMessageExtractsNameAndWeaponPreference()
        {
            var memory = new PlayerMemoryData();

            var changed = PlayerFactExtractor.Apply("My name is Alex and I like swords.", memory);

            Assert.That(changed, Is.True);
            Assert.That(memory.TryGetFact(PlayerFactKeys.Name, out var name), Is.True);
            Assert.That(memory.TryGetFact(PlayerFactKeys.WeaponPreference, out var weapon), Is.True);
            Assert.That(name, Is.EqualTo("Alex"));
            Assert.That(weapon, Is.EqualTo("swords"));
        }

        /// <summary>
        /// Verifies that structured facts survive a JSON save and reload.
        /// </summary>
        [Test]
        public void SaveAndLoadRoundTripPreservesFacts()
        {
            var store = new JsonPlayerMemoryStore(mFilePath);
            var memory = new PlayerMemoryData();
            memory.SetFact(PlayerFactKeys.Name, "Alex");
            memory.SetFact(PlayerFactKeys.WeaponPreference, "swords");

            store.Save(memory);
            var loaded = store.Load();

            Assert.That(loaded.SchemaVersion, Is.EqualTo(PlayerMemoryData.CurrentSchemaVersion));
            Assert.That(loaded.TryGetFact(PlayerFactKeys.Name, out var name), Is.True);
            Assert.That(loaded.TryGetFact(PlayerFactKeys.WeaponPreference, out var weapon), Is.True);
            Assert.That(name, Is.EqualTo("Alex"));
            Assert.That(weapon, Is.EqualTo("swords"));
        }

        /// <summary>
        /// Verifies that a missing file produces safe current-version memory.
        /// </summary>
        [Test]
        public void MissingFileReturnsEmptyCurrentVersionMemory()
        {
            var store = new JsonPlayerMemoryStore(mFilePath);

            var loaded = store.Load();

            Assert.That(loaded.SchemaVersion, Is.EqualTo(PlayerMemoryData.CurrentSchemaVersion));
            Assert.That(loaded.Facts, Is.Empty);
        }

        /// <summary>
        /// Verifies that malformed JSON is preserved while safe memory is returned.
        /// </summary>
        [Test]
        public void MalformedFileReturnsEmptyMemoryWithoutOverwritingSource()
        {
            Directory.CreateDirectory(mDirectoryPath);
            const string MalformedJson = "{ invalid json";
            File.WriteAllText(mFilePath, MalformedJson);
            var store = new JsonPlayerMemoryStore(mFilePath);

            var loaded = store.Load();

            Assert.That(loaded.Facts, Is.Empty);
            Assert.That(File.ReadAllText(mFilePath), Is.EqualTo(MalformedJson));
        }
    }
}
