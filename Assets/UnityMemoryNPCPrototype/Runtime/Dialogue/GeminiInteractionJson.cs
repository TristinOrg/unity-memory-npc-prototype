using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Builds and parses Gemini Interactions API JSON documents.
    /// </summary>
    public static class GeminiInteractionJson
    {
        /// <summary>
        /// Builds a single-turn text interaction request.
        /// </summary>
        /// <param name="model">The Gemini model code.</param>
        /// <param name="context">The complete provider context.</param>
        /// <returns>The serialized request document.</returns>
        public static string BuildRequest(string model, string context)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException("A Gemini model is required.", nameof(model));
            }

            if (string.IsNullOrWhiteSpace(context))
            {
                throw new ArgumentException("Provider context is required.", nameof(context));
            }

            var document = new JObject
            {
                ["model"] = model,
                ["input"] = context,
            };
            return document.ToString(Formatting.None);
        }

        /// <summary>
        /// Extracts the final text produced by a completed interaction.
        /// </summary>
        /// <param name="json">The Gemini interaction response document.</param>
        /// <returns>The final model output text.</returns>
        public static string ParseResponse(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidAIResponseException("Gemini returned an empty response document.");
            }

            var document = JObject.Parse(json);
            var status = document.Value<string>("status");
            if (!string.Equals(status, "completed", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidAIResponseException($"Gemini interaction finished with status '{status ?? "missing"}'.");
            }

            var steps = document["steps"] as JArray;
            if (steps is null)
            {
                throw new InvalidAIResponseException("Gemini response does not contain interaction steps.");
            }

            string outputText = null;
            foreach (var step in steps)
            {
                if (!string.Equals(step.Value<string>("type"), "model_output", StringComparison.Ordinal))
                {
                    continue;
                }

                var content = step["content"] as JArray;
                if (content is null)
                {
                    continue;
                }

                foreach (var part in content)
                {
                    if (string.Equals(part.Value<string>("type"), "text", StringComparison.Ordinal))
                    {
                        outputText = part.Value<string>("text");
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(outputText))
            {
                throw new InvalidAIResponseException("Gemini response does not contain model output text.");
            }

            return outputText;
        }
    }
}
