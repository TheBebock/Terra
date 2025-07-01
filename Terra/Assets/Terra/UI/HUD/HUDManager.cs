using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using Terra.Managers;
using Terra.UI.HUD.PassiveItemsDisplay;
using Terra.UI.HUD.StatDisplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    public class HUDManager : MonoBehaviourSingleton<HUDManager>, IAttachListeners
    {
        [SerializeField] private CanvasGroup _gameplayHUDCanvasGroup;
        [SerializeField] private HpSlider _hpSlider;
        [SerializeField] private ElevatorDoors _elevatorDoors;
        [SerializeField] private Image _darkScreen;
        [SerializeField] private StatsDisplayGUI _statsDisplay;
        [SerializeField] private PassiveItemsGUI _passiveItemsGUI;
        [BoxGroup("FloorEndText")][SerializeField] private TMP_Text _floorEndText;
        [BoxGroup("FloorEndText")][SerializeField] private float _floorEndFadeDuration = 1f;
        
        [BoxGroup("FloorCounter")][SerializeField] private TMP_Text _floorCounter;
        [BoxGroup("FloorCounter")][SerializeField] private string _floorCounterText = "Floor";

        public HpSlider HpSlider => _hpSlider;
        public ElevatorDoors ElevatorDoors => _elevatorDoors;
        public Image DarkScreen => _darkScreen;
        public StatsDisplayGUI StatsDisplay => _statsDisplay;
        public PassiveItemsGUI PassiveItemsDisplay => _passiveItemsGUI;
        
        private Sequence _darkSequence;
        
        public void AttachListeners()
        {
            EventsAPI.Register<StartOfNewFloorEvent>(OnStartOfFloor);
            EventsAPI.Register<WaveEndedEvent>(OnWaveEnded);
        }

        public void ForceSetDarkScreenAlpha(float alpha)
        {
            _darkScreen.color = new Color(0,0,0, alpha);
        }
        public async UniTask FadeInDarkScreen(float duration)
        {
            _darkSequence?.Kill();
            _darkSequence = DOTween.Sequence();
            
           await _darkSequence.Append(
                _darkScreen.DOFade(1, duration)
                .SetEase(Ease.InCubic)
                ).WithCancellation(CancellationToken);
        }
        
        public async UniTask FadeOutDarkScreen(float duration)
        {
            _darkSequence?.Kill();
            _darkSequence = DOTween.Sequence();
            
            await _darkSequence.Append(
                _darkScreen.DOFade(0, duration)
                    .SetEase(Ease.InCubic)
            ).WithCancellation(CancellationToken);
        }

        private void OnStartOfFloor(ref StartOfNewFloorEvent dummy)
        {
            int floor = WaveManager.Instance.CurrentLevel + 1;
            _floorCounter?.SetText($"{_floorCounterText} {floor}");
        }
        private void OnWaveEnded(ref WaveEndedEvent dummy)
        {
            _floorEndText?.DOFade(1, _floorEndFadeDuration/2)
                .SetEase(Ease.OutCubic)
                .SetLoops(2, LoopType.Yoyo)
                .WithCancellation(CancellationToken);
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
        
        public void DetachListeners()
        {
            EventsAPI.Unregister<StartOfNewFloorEvent>(OnStartOfFloor);
            EventsAPI.Unregister<WaveEndedEvent>(OnWaveEnded);
        }
    }
}
