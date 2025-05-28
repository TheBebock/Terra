using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Terra.Core.Generics;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    public class HUDManager : MonoBehaviourSingleton<HUDManager>
    {
        [SerializeField] private CanvasGroup _gameplayHUDCanvasGroup;
        [SerializeField] private HPSlider _hpSlider;
        [SerializeField] private ElevatorDoors _elevatorDoors;
        [SerializeField] private Image _darkScreen;
    
        public HPSlider HPSlider => _hpSlider;
        public ElevatorDoors ElevatorDoors => _elevatorDoors;
        public Image DarkScreen => _darkScreen;
        
        private Sequence _darkSequence;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                _ = ElevatorDoors.OpenDoors();
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _ = ElevatorDoors.CloseDoors();
            }
        }

        public void ForceSetDarkScreenAlpha(float alpha)
        {
            _darkScreen.color = new Color(0,0,0, alpha);
        }
        public async UniTask FadeInDarkScreen(float duration)
        {
            _darkSequence?.Kill();
            _darkSequence = DOTween.Sequence();
            
            _darkSequence.Append(
                _darkScreen.DOFade(1, duration)
                .SetEase(Ease.InCubic)
                );
            
            await _darkSequence.AwaitForComplete(cancellationToken: CancellationToken);
        }
        
        public async UniTask FadeOutDarkScreen(float duration)
        {
            _darkSequence?.Kill();
            _darkSequence = DOTween.Sequence();
            
            _darkSequence.Append(
                _darkScreen.DOFade(0, duration)
                    .SetEase(Ease.InCubic)
            );

            await _darkSequence.AwaitForComplete(cancellationToken: CancellationToken);
        }

        public void ShowGameplayHUD()
        {
            _gameplayHUDCanvasGroup.alpha = 1;
            _gameplayHUDCanvasGroup.interactable = true;
            _gameplayHUDCanvasGroup.blocksRaycasts = true;
        }
        
        public void HideGameplayHUD()
        {
            _gameplayHUDCanvasGroup.alpha = 0;
            _gameplayHUDCanvasGroup.interactable = false;
            _gameplayHUDCanvasGroup.blocksRaycasts = false;
        }
    }
}
