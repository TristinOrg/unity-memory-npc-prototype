using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Stores local Gemini provider selection and credentials.
    /// </summary>
    public sealed class GeminiProviderConfiguration
    {
        /// <summary>
        /// Gets whether Gemini should replace the offline mock provider.
        /// </summary>
        public bool UseGemini { get; }

        /// <summary>
        /// Gets the local Gemini API key.
        /// </summary>
        public string ApiKey { get; }

        /// <summary>
        /// Gets the Gemini model code.
        /// </summary>
        public string Model { get; }

        /// <summary>
        /// Creates a Gemini provider configuration.
        /// </summary>
        /// <param name="useGemini">Whether Gemini should replace the offline provider.</param>
        /// <param name="apiKey">The local Gemini API key.</param>
        /// <param name="model">The Gemini model code.</param>
        public GeminiProviderConfiguration(bool useGemini, string apiKey, string model)
        {
            UseGemini = useGemini;
            ApiKey = apiKey;
            Model = model;
        }

        /// <summary>
        /// Loads local provider configuration without logging credential content.
        /// </summary>
        /// <param name="filePath">The ignored local configuration file path.</param>
        /// <returns>The parsed provider configuration.</returns>
        public static GeminiProviderConfiguration Load(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("A Gemini configuration path is required.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                return new GeminiProviderConfiguration(false, null, null);
            }

            var document = JObject.Parse(File.ReadAllText(filePath));
            var useGemini = document.Value<bool?>(nameof(UseGemini)) ?? false;
            var apiKey = document.Value<string>(nameof(ApiKey));
            var model = document.Value<string>(nameof(Model));
            if (!useGemini)
            {
                return new GeminiProviderConfiguration(false, null, model);
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("Gemini is enabled but ApiKey is empty in the local configuration.");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new InvalidOperationException("Gemini is enabled but Model is empty in the local configuration.");
            }

            return new GeminiProviderConfiguration(true, apiKey.Trim(), model.Trim());
        }
    }
}
