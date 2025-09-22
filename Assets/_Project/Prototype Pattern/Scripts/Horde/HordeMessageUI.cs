using System.Collections;
using TMPro;
using UnityEngine;

namespace PrototypePattern.Horde
{
    public class HordeMessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private float _fadeDuration = 0.25f;

        private Coroutine _messageCoroutine;
        [SerializeField] private CanvasGroup _canvasGroup;

        /// <summary>Initializes the message UI and hides it on awake.</summary>
        private void Awake()
        {
            _messageText.text = string.Empty;

            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        /// <summary>Shows a message for the given duration with fade in/out.</summary>
        public void ShowMessage(string message, float duration = 2f)
        {
            StopMessageCoroutine();
            _messageCoroutine = StartCoroutine(MessageRoutine(message, duration));
        }


        /// <summary>Stops any current message and clears the UI immediately.</summary>
        public void Stop()
        {
            StopMessageCoroutine();

            _canvasGroup.alpha = 0f;
            _messageText.text = string.Empty;
        }

        /// <summary>Stops the active message coroutine if one is running.</summary>
        private void StopMessageCoroutine()
        {
            if (_messageCoroutine != null)
            {
                StopCoroutine(_messageCoroutine);
                _messageCoroutine = null;
            }
        }

        /// <summary>Fades the canvas group alpha between two values over a duration.</summary>
        private IEnumerator Fade(float fromAlpha, float toAlpha, float duration)
        {
            if (duration <= 0f)
            {
                _canvasGroup.alpha = toAlpha;
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
                yield return null;
            }
            _canvasGroup.alpha = toAlpha;
        }

        /// <summary>Coroutine that displays the message and handles fading and timing.</summary>
        private IEnumerator MessageRoutine(string message, float totalDuration)
        {
            _messageText.text = message;

            float fadeIn = _fadeDuration;
            float fadeOut = _fadeDuration;
            float hold = Mathf.Max(0f, totalDuration - fadeIn - fadeOut);

            float alphaHidden = 0f;
            float alphaVisible = 1f;

            yield return StartCoroutine(Fade(alphaHidden, alphaVisible, fadeIn));

            if (hold > 0f) yield return new WaitForSeconds(hold);

            yield return StartCoroutine(Fade(alphaVisible, alphaHidden, fadeOut));

            _messageText.text = string.Empty;
            _messageCoroutine = null;
        }
        /// <summary>Changes the alignment of the message text.</summary>
        public void ChangeTextAlignment(TextAlignmentOptions textAlignment)
        {
            _messageText.alignment = textAlignment;
        }
    }
}
