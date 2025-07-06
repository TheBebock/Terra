using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Managers;
using Terra.UI.HUD.PassiveItemsDisplay;
using Terra.UI.HUD.StatDisplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    public class HUDManager : MonoBehaviourSingleton<HUDManager>
    {
        [SerializeField] private CanvasGroup _gameplayHUDCanvasGroup;
        [SerializeField] private CanvasGroup _crystalCanvasGroup;
        [SerializeField] private CanvasGroup _ammoCanvasGroup;
        [SerializeField] private HpSlider _hpSlider;
        [SerializeField] private ElevatorDoors _elevatorDoors;
        [SerializeField] private Image _darkScreen;
        [SerializeField] private StatsDisplayGUI _statsDisplay;
        [SerializeField] private PassiveItemsGUI _passiveItemsGUI;
        
        [BoxGroup("Weapons")] [SerializeField] private CanvasGroup _weaponsCanvasGroup;
        [BoxGroup("Weapons")] [SerializeField] private Image _meleeIcon;
        [BoxGroup("Weapons")] [SerializeField] private Image _rangeIcon;
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

        protected override void Awake()
        {
            base.Awake();
            EventsAPI.Register<LevelIncreasedEvent>(OnLevelIncreased);
            EventsAPI.Register<WaveEndedEvent>(OnWaveEnded);
            EventsAPI.Register<OnWeaponsChangedEvent>(OnWeaponsChanged);
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

        private void OnLevelIncreased(ref LevelIncreasedEvent dummy)
        {
            int floor = WaveManager.Instance.CurrentLevel;
            _floorCounter?.SetText($"{_floorCounterText} {floor}");
        }
        private void OnWaveEnded(ref WaveEndedEvent dummy)
        {
            _floorEndText?.DOFade(1, _floorEndFadeDuration/2)
                .SetEase(Ease.OutCubic)
                .SetLoops(2, LoopType.Yoyo)
                .WithCancellation(CancellationToken);
        }

        private void OnWeaponsChanged(ref OnWeaponsChangedEvent ev)
        {
            if (ev.itemType == WeaponType.Melee)
            {
                _meleeIcon.sprite = ev.weaponSprite;
            }
            else
            {
                _rangeIcon.sprite = ev.weaponSprite;
            }
        }
        public void ShowGameplayHUD()
        {
            _gameplayHUDCanvasGroup.alpha = 1;
            ShowCrystalCounter();
            ShowAmmoCounter();
            ShowWeapons();
        }

        public void ShowUpgradeHUD()
        {
            StatsDisplay.Show();
            PassiveItemsDisplay.Show();
            ShowWeapons();
            ShowAmmoCounter();
            ShowCrystalCounter();
        }
        
        public void HideGameplayHUD()
        {
            _gameplayHUDCanvasGroup.alpha = 0;
            HideCrystalCounter();
            HideAmmoCounter();
            HideWeapons();
            StatsDisplay.Hide();
            PassiveItemsDisplay.Hide();
        }
        private void ShowCrystalCounter()
        {
            _crystalCanvasGroup.alpha = 1;
        }

        private void HideCrystalCounter()
        {
            _crystalCanvasGroup.alpha = 0; 
        }
        
        private void ShowAmmoCounter()
        {
            _ammoCanvasGroup.alpha = 1;
        }
        
        private void HideAmmoCounter()
        {
            _ammoCanvasGroup.alpha = 0;
        }

        private void ShowWeapons()
        {
            _weaponsCanvasGroup.alpha = 1;
        }
        
        private void HideWeapons()
        {
            _weaponsCanvasGroup.alpha = 0;
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            
            _darkSequence?.Kill();
            
            EventsAPI.Unregister<LevelIncreasedEvent>(OnLevelIncreased);
            EventsAPI.Unregister<WaveEndedEvent>(OnWaveEnded);
            EventsAPI.Unregister<OnWeaponsChangedEvent>(OnWeaponsChanged);

        }
    }
}
