using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using Terra.Core.Generics;
using Terra.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.Outro
{
    public class OutroController : InGameMonobehaviour
    {
        [SerializeField] private Image _creditsImage;
        [SerializeField] private CanvasGroup _thanksForPlayingGroup;

        private void Awake()
        {
            _creditsImage.color = new Color(1f, 1f, 1f, 0f);
            _thanksForPlayingGroup.alpha = 0f;
        }
        
        [UsedImplicitly]
        public void ShowCredits()
        {
            _ = CreditsAnimAsync();
        }

        private async UniTaskVoid CreditsAnimAsync()
        {
            await _creditsImage.DOFade(1, 2f).SetEase(Ease.InSine);
            await UniTask.WaitForSeconds(1f);
            await _creditsImage.DOFade(0, 2f).SetEase(Ease.InSine);
            await _thanksForPlayingGroup.DOFade(1, 2f).SetEase(Ease.InSine);
            await UniTask.WaitForSeconds(1f);
            await _thanksForPlayingGroup.DOFade(0, 2f).SetEase(Ease.InSine);
            await UniTask.WaitForSeconds(0.25f);
            
            ScenesManager.Instance?.LoadMainMenu();
        }
    }
}
