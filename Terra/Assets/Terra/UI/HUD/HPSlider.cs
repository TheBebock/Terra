using System.Globalization;
using Terra.Player;
using TMPro;
using UIExtensionPackage.UISystem.Core.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    public class HpSlider : UIObject
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _currentHealthText;
        [SerializeField] private TMP_Text _maxHealthText;
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _maxHealth;
        
        public void Init(int currentHealth, int maxHealth)
        {
            _maxHealth = maxHealth;
            _maxHealthText.SetText(_maxHealth.ToString(CultureInfo.InvariantCulture));

            _currentHealth = currentHealth;
            _currentHealthText.SetText(_currentHealth.ToString(CultureInfo.InvariantCulture));
            
            AttachListeners();
        }
        
        private void AttachListeners()
        {
            PlayerManager.Instance.PlayerEntity.HealthController.OnHealthChangedNormalized += SetSliderValue;
            PlayerManager.Instance.PlayerEntity.HealthController.OnHealthChanged += UpdateCurrentHealth;
            
            PlayerStatsManager.Instance.OnMaxHealthChanged += UpdateMaxHealth;
        }

        private void SetSliderValue(float value)
        {
            _slider.SetValueWithoutNotify(value);
        }

        private void UpdateCurrentHealth(int value)
        {
            _currentHealth = value;
            _currentHealthText.SetText(_currentHealth.ToString(CultureInfo.InvariantCulture));
        }

        private void UpdateMaxHealth(int value)
        {
            _maxHealth = value;
            _maxHealthText.SetText(_maxHealth.ToString(CultureInfo.InvariantCulture));
        }
        private void DetachListeners()
        {
            if (PlayerManager.Instance)
            {
                PlayerManager.Instance.PlayerEntity.HealthController.OnHealthChangedNormalized -= SetSliderValue;
                PlayerManager.Instance.PlayerEntity.HealthController.OnHealthChanged -= UpdateCurrentHealth;
            }

            if (PlayerStatsManager.Instance)
            {
                PlayerStatsManager.Instance.OnMaxHealthChanged -= UpdateMaxHealth;
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
