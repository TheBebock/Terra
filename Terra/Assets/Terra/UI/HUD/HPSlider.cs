using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    public class HPSlider : MonoBehaviourSingleton<HPSlider>
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _maxHealth;
        private string HealthDisplayFormat => $"{_currentHealth} {_maxHealth}";
        
        public void Init(float currentHealth, float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = currentHealth;
            _text.SetText(HealthDisplayFormat);
            
            AttachListeners();
        }
        
        private void AttachListeners()
        {
            PlayerManager.Instance.PlayerEntity.HealthController.OnHealthChangedNormalized += SetSliderValue;
            PlayerManager.Instance.PlayerEntity.HealthController.OnHealthChanged += UpdateCurrentHealth;
            PlayerManager.Instance.PlayerEntity.HealthController.OnHealthCreated += Init;
            
            PlayerStatsManager.Instance.OnMaxHealthChanged += UpdateMaxHealth;
        }

        private void SetSliderValue(float value)
        {
            _slider.SetValueWithoutNotify(value);
        }

        private void UpdateCurrentHealth(float value)
        {
            _currentHealth = value;
            _text.SetText(HealthDisplayFormat);
        }

        private void UpdateMaxHealth(float value)
        {
            _maxHealth = value;
            _text.SetText(HealthDisplayFormat);
        }
        private void DetachListeners()
        {
            if (PlayerManager.Instance)
            {
                PlayerManager.Instance.PlayerEntity.HealthController.OnHealthChangedNormalized -= SetSliderValue;
                PlayerManager.Instance.PlayerEntity.HealthController.OnHealthChanged -= UpdateCurrentHealth;
                PlayerManager.Instance.PlayerEntity.HealthController.OnHealthCreated -= Init;
            }

            if (PlayerStatsManager.Instance)
            {
                PlayerStatsManager.Instance.OnStrengthChanged += UpdateMaxHealth;
            }
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            DetachListeners();
        }

        private void OnValidate()
        {
            if (_slider == null)
            {
                _slider = GetComponent<Slider>();
            }
        }
    }
}
