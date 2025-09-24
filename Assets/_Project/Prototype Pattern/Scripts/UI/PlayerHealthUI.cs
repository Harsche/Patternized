using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PrototypePattern.Player;

namespace PrototypePattern.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        private Canvas _rootCanvas;
        private Slider _healthSlider;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;

        [SerializeField] private PlayerController _player;
        [SerializeField] private float _visibleDuration = 2f;

        private Coroutine _hideCoroutine;
        [Header("Positioning")]
        [SerializeField] private Vector3 _worldOffset = new Vector3(0f, 1.5f, 0f);

        
        private void Awake()
        {
            _rootCanvas = GetComponentInParent<Canvas>();
            _healthSlider = GetComponent<Slider>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = _canvasGroup.GetComponent<RectTransform>();

            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _healthSlider.minValue = 0f;
            _healthSlider.maxValue = 1f;
            _healthSlider.value = 1f;
        }

        private void OnEnable()
        {
            _player.OnHealthChanged += HandleHealthChanged;
        }

        private void OnDisable()
        {
            _player.OnHealthChanged -= HandleHealthChanged;
        }

        private void HandleHealthChanged(float normalized)
        {
            _healthSlider.value = normalized;

            Show();
        }

        private void LateUpdate()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {

            Camera camera = _rootCanvas.renderMode == RenderMode.ScreenSpaceCamera ? _rootCanvas.worldCamera : Camera.main;
            camera = Camera.main;

            Vector3 worldPos = _player.transform.position + _worldOffset;
            Vector3 screenPoint = camera.WorldToScreenPoint(worldPos);

            RectTransform canvasRect = _rootCanvas.transform as RectTransform;
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, camera, out localPoint))
            {
                _rectTransform.anchoredPosition = localPoint;
            }
        }

        private void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);

            _hideCoroutine = StartCoroutine(HideAfterDelay(_visibleDuration));
        }

        private IEnumerator HideAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _hideCoroutine = null;
        }
    }
}
