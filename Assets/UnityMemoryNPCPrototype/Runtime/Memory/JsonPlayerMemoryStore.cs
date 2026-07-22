using System;
using System.IO;
using UnityEngine;

namespace UnityMemoryNPCPrototype.Memory
{
    /// <summary>
    /// Persists versioned player memory as human-readable JSON.
    /// </summary>
    public sealed class JsonPlayerMemoryStore
    {
        /// <summary>
        /// Stores the target JSON file path.
        /// </summary>
        private readonly string mFilePath;

        /// <summary>
        /// Creates a JSON memory store.
        /// </summary>
        /// <param name="filePath">The target JSON file path.</param>
        public JsonPlayerMemoryStore(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("A memory file path is required.", nameof(filePath));

            mFilePath = filePath;
        }

        /// <summary>
        /// Loads valid current-version memory or returns safe empty data.
        /// </summary>
        /// <returns>The loaded or recovered player memory.</returns>
        public PlayerMemoryData Load()
        {
            if (!File.Exists(mFilePath))
                return new PlayerMemoryData();

            try
            {
                var json = File.ReadAllText(mFilePath);
                var memory = JsonUtility.FromJson<PlayerMemoryData>(json);
                if (memory is null || memory.SchemaVersion != PlayerMemoryData.CurrentSchemaVersion)
                    return new PlayerMemoryData();

                memory.Normalize();
                return memory;
            }
            catch (IOException)
            {
                return new PlayerMemoryData();
            }
            catch (UnauthorizedAccessException)
            {
                return new PlayerMemoryData();
            }
            catch (ArgumentException)
            {
                return new PlayerMemoryData();
            }
        }

        /// <summary>
        /// Atomically saves current-version player memory.
        /// </summary>
        /// <param name="memory">The player memory to save.</param>
        public void Save(PlayerMemoryData memory)
        {
            if (memory is null)
                throw new ArgumentNullException(nameof(memory));

            memory.SchemaVersion = PlayerMemoryData.CurrentSchemaVersion;
            memory.Normalize();
            var directoryPath = Path.GetDirectoryName(mFilePath);
            if (!string.IsNullOrWhiteSpace(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var temporaryPath = mFilePath + ".tmp";
            File.WriteAllText(temporaryPath, JsonUtility.ToJson(memory, true));
            if (File.Exists(mFilePath))
                File.Replace(temporaryPath, mFilePath, null);
            else
                File.Move(temporaryPath, mFilePath);
        }
    }
}
