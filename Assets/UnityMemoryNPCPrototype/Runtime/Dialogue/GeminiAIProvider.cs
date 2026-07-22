using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace UnityMemoryNPCPrototype.Dialogue
{
    /// <summary>
    /// Generates dialogue through the Gemini Interactions REST API.
    /// </summary>
    public sealed class GeminiAIProvider : IAIProvider
    {
        /// <summary>
        /// Defines the Gemini Interactions API endpoint.
        /// </summary>
        private const string Endpoint = "https://generativelanguage.googleapis.com/v1beta/interactions";

        /// <summary>
        /// Stores the local API key used only in request headers.
        /// </summary>
        private readonly string mApiKey;

        /// <summary>
        /// Stores the selected Gemini model code.
        /// </summary>
        private readonly string mModel;

        /// <summary>
        /// Creates a Gemini dialogue provider.
        /// </summary>
        /// <param name="apiKey">The local Gemini API key.</param>
        /// <param name="model">The Gemini model code.</param>
        public GeminiAIProvider(string apiKey, string model)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("A Gemini API key is required.", nameof(apiKey));
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException("A Gemini model is required.", nameof(model));
            }

            mApiKey = apiKey;
            mModel = model;
        }

        /// <summary>
        /// Sends the deterministic dialogue context to Gemini.
        /// </summary>
        /// <param name="request">The message and deterministic provider context.</param>
        /// <param name="cancellationToken">The token used to cancel the request.</param>
        /// <returns>The generated Gemini dialogue response.</returns>
        public async Task<AIResponse> GenerateAsync(DialogueRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var requestJson = GeminiInteractionJson.BuildRequest(mModel, request.Context);
            using var webRequest = new UnityWebRequest(Endpoint, UnityWebRequest.kHttpVerbPOST);
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(requestJson));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("x-goog-api-key", mApiKey);
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    webRequest.Abort();
                    cancellationToken.ThrowIfCancellationRequested();
                }

                await Task.Yield();
            }

            if (webRequest.result is not UnityWebRequest.Result.Success)
            {
                throw new GeminiProviderException(webRequest.responseCode, $"Gemini request failed with HTTP status {webRequest.responseCode}.");
            }

            return new AIResponse(GeminiInteractionJson.ParseResponse(webRequest.downloadHandler.text));
        }
    }
}
