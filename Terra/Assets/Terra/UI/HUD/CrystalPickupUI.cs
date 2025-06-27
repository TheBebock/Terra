using System.Collections;
using Terra.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.UI.HUD
{
    public class CrystalPickupUI : MonoBehaviour
    {
        [FormerlySerializedAs("crystalSprite")] [SerializeField] private Sprite _crystalSprite; 
        [FormerlySerializedAs("crystalPickupText")] [SerializeField] private TextMeshProUGUI _crystalPickupText;
        [FormerlySerializedAs("canvasGroup")] [SerializeField] private CanvasGroup _canvasGroup; 
        
        [FormerlySerializedAs("fadeDuration")] [SerializeField] private float _fadeDuration = 0.5f;
        [FormerlySerializedAs("displayDuration")] [SerializeField] private float _displayDuration = 1.5f;

        private Coroutine _fadeRoutine;

        private void Start()
        {
            _canvasGroup.alpha = 0f;

            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnGoldChanged += HandleGoldChanged;
            }
        }
        
        private void OnDisable()
        {
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.OnGoldChanged -= HandleGoldChanged;
            }
        }
        
        private void HandleGoldChanged(int currentGold)
        {
            ShowGoldPickup(currentGold);
        }

        public void ShowGoldPickup(float amount)
        {
            if (_fadeRoutine != null)
                StopCoroutine(_fadeRoutine);

            _crystalPickupText.text = $"{amount}";
            _fadeRoutine = StartCoroutine(FadeInOut());
        }

        private IEnumerator FadeInOut()
        {
            // Fade in
            yield return Fade(0f, 1f, _fadeDuration);

            // Wait
            yield return new WaitForSeconds(_displayDuration);

            // Fade out
            yield return Fade(1f, 0f, _fadeDuration);

            _fadeRoutine = null;
        }

        private IEnumerator Fade(float from, float to, float duration)
        {
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                _canvasGroup.alpha = Mathf.Lerp(from, to, t);
                yield return null;
            }

            _canvasGroup.alpha = to;
        }
    }
}
