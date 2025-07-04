using System;
using NaughtyAttributes;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using TMPro;
using UIExtensionPackage.UISystem.Core.Base;
using UIExtensionPackage.UISystem.Core.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    public class BossHpSlider : UIObject, IAttachListeners
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField, ReadOnly] private bool _canShow;
        private int _currentHp;
        private int _maxHp;
        
        private void Awake()
        {
            _slider.minValue = 0f;
            _slider.maxValue = 1f;
            _slider.interactable = false;
        }

        public void AttachListeners()
        {
            EventsAPI.Register<OnBossSpawnedEvent>(OnBossSpawnedEvent);
            EventsAPI.Register<OnBossStartedMovingEvent>(OnBossStartMovingEvent);
            EventsAPI.Register<OnBossDamagedEvent>(OnBossDamagedEvent);
        }


        private void Show()
        {
            _canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
        }
        private void OnBossSpawnedEvent(ref OnBossSpawnedEvent spawnedEvent)
        {
            _canShow = true;
            _maxHp = spawnedEvent.bossHp;
            _currentHp = _maxHp;
            UpdateHpText();
        }
        
        private void OnBossStartMovingEvent(ref OnBossStartedMovingEvent dummy)
        {
            if (_canShow)
            {
                Show();
            }
        }

        private void OnBossDamagedEvent(ref OnBossDamagedEvent damagedEvent)
        {
            _currentHp -= damagedEvent.damage;
            _slider.value = damagedEvent.normalizedDamage;
            UpdateHpText();
        }

        private void UpdateHpText()
        {
            _hpText.text = $"{_currentHp}/{_maxHp}";
        }
        
        public void DetachListeners()
        {
            EventsAPI.Unregister<OnBossSpawnedEvent>(OnBossSpawnedEvent);
            EventsAPI.Unregister<OnBossStartedMovingEvent>(OnBossStartMovingEvent);
            EventsAPI.Unregister<OnBossDamagedEvent>(OnBossDamagedEvent);
        }
    }
}
