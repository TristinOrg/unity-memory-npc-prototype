using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityMemoryNPCPrototype.Dialogue;
using UnityMemoryNPCPrototype.Memory;

namespace UnityMemoryNPCPrototype.Presentation
{
    /// <summary>
    /// Coordinates player input, provider requests and dialogue presentation.
    /// </summary>
    public sealed class DialogueController : MonoBehaviour
    {
        /// <summary>
        /// Receives the player's message.
        /// </summary>
        public TMP_InputField PlayerInput;

        /// <summary>
        /// Displays the current player and Arthur dialogue.
        /// </summary>
        public TMP_Text Transcript;

        /// <summary>
        /// Submits the current player message.
        /// </summary>
        public Button SubmitButton;

        /// <summary>
        /// Stores the active provider.
        /// </summary>
        private IAIProvider mProvider;

        /// <summary>
        /// Stores the active request cancellation source.
        /// </summary>
        private CancellationTokenSource mRequestCancellation;

        /// <summary>
        /// Stores the versioned player memory loaded for this application session.
        /// </summary>
        private PlayerMemoryData mPlayerMemory;

        /// <summary>
        /// Stores the JSON persistence adapter used by this controller.
        /// </summary>
        private JsonPlayerMemoryStore mMemoryStore;

        /// <summary>
        /// Creates the offline provider used by the first vertical slice.
        /// </summary>
        private void Awake()
        {
            mProvider = new MockAIProvider();
            var memoryPath = System.IO.Path.Combine(Application.persistentDataPath, "player-memory-v1.json");
            mMemoryStore = new JsonPlayerMemoryStore(memoryPath);
            mPlayerMemory = mMemoryStore.Load();
        }

        /// <summary>
        /// Registers the submit listener.
        /// </summary>
        private void OnEnable()
        {
            if (SubmitButton)
                SubmitButton.onClick.AddListener(HandleSubmitClicked);
        }

        /// <summary>
        /// Cancels active work and unregisters the submit listener.
        /// </summary>
        private void OnDisable()
        {
            if (SubmitButton)
                SubmitButton.onClick.RemoveListener(HandleSubmitClicked);

            CancelRequest();
        }

        /// <summary>
        /// Submits the current player input and presents Arthur's response.
        /// </summary>
        private async void HandleSubmitClicked()
        {
            if (!PlayerInput || !Transcript || !SubmitButton || mRequestCancellation is not null)
                return;

            var playerMessage = PlayerInput.text.Trim();
            if (string.IsNullOrWhiteSpace(playerMessage))
                return;

            CapturePlayerFacts(playerMessage);

            var requestCancellation = new CancellationTokenSource();
            mRequestCancellation = requestCancellation;
            SetInteractive(false);
            Transcript.text = $"Player: {playerMessage}\nArthur: Thinking...";

            try
            {
                var response = await mProvider.GenerateAsync(playerMessage, requestCancellation.Token);
                if (this)
                    Transcript.text = $"Player: {playerMessage}\nArthur: {response.Text}";
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
                if (this)
                    Transcript.text = $"Player: {playerMessage}\nArthur: I cannot answer right now.";
            }
            finally
            {
                if (ReferenceEquals(mRequestCancellation, requestCancellation))
                {
                    mRequestCancellation.Dispose();
                    mRequestCancellation = null;

                    if (this && isActiveAndEnabled)
                        SetInteractive(true);
                }
            }
        }

        /// <summary>
        /// Updates the input controls for the current request state.
        /// </summary>
        /// <param name="interactive">Whether the player may submit another message.</param>
        private void SetInteractive(bool interactive)
        {
            PlayerInput.interactable = interactive;
            SubmitButton.interactable = interactive;
        }

        /// <summary>
        /// Cancels and releases the active request.
        /// </summary>
        private void CancelRequest()
        {
            mRequestCancellation?.Cancel();
            mRequestCancellation?.Dispose();
            mRequestCancellation = null;
        }

        /// <summary>
        /// Extracts and persists supported structured facts without blocking dialogue on failure.
        /// </summary>
        /// <param name="playerMessage">The submitted player message.</param>
        private void CapturePlayerFacts(string playerMessage)
        {
            if (!PlayerFactExtractor.Apply(playerMessage, mPlayerMemory))
                return;

            try
            {
                mMemoryStore.Save(mPlayerMemory);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Player memory could not be saved: {exception.Message}");
            }
        }
    }
}
