using System;
using System.IO;
using NUnit.Framework;
using UnityMemoryNPCPrototype.Dialogue;

namespace UnityMemoryNPCPrototype.Tests.EditMode
{
    /// <summary>
    /// Verifies Gemini configuration and Interactions API JSON handling without network access.
    /// </summary>
    public sealed class GeminiProviderTests
    {
        /// <summary>
        /// Stores the isolated configuration path used by each test.
        /// </summary>
        private string mConfigurationPath;

        /// <summary>
        /// Creates an isolated local configuration path.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            mConfigurationPath = Path.Combine(Path.GetTempPath(), $"gemini-provider-{Guid.NewGuid():N}.local.json");
        }

        /// <summary>
        /// Removes the isolated local configuration file.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (File.Exists(mConfigurationPath))
            {
                File.Delete(mConfigurationPath);
            }
        }

        /// <summary>
        /// Verifies that missing local configuration preserves the offline provider path.
        /// </summary>
        [Test]
        public void MissingConfigurationDisablesGemini()
        {
            var configuration = GeminiProviderConfiguration.Load(mConfigurationPath);

            Assert.That(configuration.UseGemini, Is.False);
            Assert.That(configuration.ApiKey, Is.Null);
        }

        /// <summary>
        /// Verifies that the local template remains offline while its credential is empty.
        /// </summary>
        [Test]
        public void DisabledConfigurationDoesNotRequireApiKey()
        {
            File.WriteAllText(mConfigurationPath, "{\"UseGemini\":false,\"ApiKey\":\"\",\"Model\":\"gemini-test\"}");

            var configuration = GeminiProviderConfiguration.Load(mConfigurationPath);

            Assert.That(configuration.UseGemini, Is.False);
            Assert.That(configuration.ApiKey, Is.Null);
            Assert.That(configuration.Model, Is.EqualTo("gemini-test"));
        }

        /// <summary>
        /// Verifies that enabled local configuration loads its credential and model fields.
        /// </summary>
        [Test]
        public void EnabledConfigurationLoadsLocalFields()
        {
            File.WriteAllText(mConfigurationPath, "{\"UseGemini\":true,\"ApiKey\":\"local-key\",\"Model\":\"gemini-test\"}");

            var configuration = GeminiProviderConfiguration.Load(mConfigurationPath);

            Assert.That(configuration.UseGemini, Is.True);
            Assert.That(configuration.ApiKey, Is.EqualTo("local-key"));
            Assert.That(configuration.Model, Is.EqualTo("gemini-test"));
        }

        /// <summary>
        /// Verifies that enabled configuration rejects an empty credential field.
        /// </summary>
        [Test]
        public void EnabledConfigurationRequiresApiKey()
        {
            File.WriteAllText(mConfigurationPath, "{\"UseGemini\":true,\"ApiKey\":\"\",\"Model\":\"gemini-test\"}");

            try
            {
                GeminiProviderConfiguration.Load(mConfigurationPath);
                Assert.Fail("Expected enabled Gemini configuration to require ApiKey.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Verifies that request JSON carries the selected model and deterministic context.
        /// </summary>
        [Test]
        public void RequestJsonContainsModelAndContext()
        {
            var json = GeminiInteractionJson.BuildRequest("gemini-test", "Arthur says \"hello\".");

            Assert.That(json, Does.Contain("\"model\":\"gemini-test\""));
            Assert.That(json, Does.Contain("Arthur says \\\"hello\\\"."));
        }

        /// <summary>
        /// Verifies extraction of the final text from a completed interaction response.
        /// </summary>
        [Test]
        public void CompletedInteractionReturnsModelOutputText()
        {
            const string ResponseJson = "{\"status\":\"completed\",\"steps\":[{\"type\":\"thought\"},{\"type\":\"model_output\",\"content\":[{\"type\":\"text\",\"text\":\"Welcome back.\"}]}]}";

            var response = GeminiInteractionJson.ParseResponse(ResponseJson);

            Assert.That(response, Is.EqualTo("Welcome back."));
        }
    }
}
