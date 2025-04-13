using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Terra.Managers;
using UnityEngine.Serialization;

namespace Terra.UI
{
    public class CrystalPickupUI : MonoBehaviour
    {
        [SerializeField] private Sprite crystalSprite; 
        [SerializeField] private TextMeshProUGUI crystalPickupText;
        [SerializeField] private CanvasGroup canvasGroup; 
        
        [SerializeField] private float fadeDuration = 0.5f;
        [SerializeField] private float displayDuration = 1.5f;

        private Coroutine fadeRoutine;

        private void Start()
        {
            canvasGroup.alpha = 0f;

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
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            crystalPickupText.text = $"{amount}";
            fadeRoutine = StartCoroutine(FadeInOut());
        }

        private IEnumerator FadeInOut()
        {
            // Fade in
            yield return Fade(0f, 1f, fadeDuration);

            // Wait
            yield return new WaitForSeconds(displayDuration);

            // Fade out
            yield return Fade(1f, 0f, fadeDuration);

            fadeRoutine = null;
        }

        private IEnumerator Fade(float from, float to, float duration)
        {
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                canvasGroup.alpha = Mathf.Lerp(from, to, t);
                yield return null;
            }

            canvasGroup.alpha = to;
        }
    }
}
