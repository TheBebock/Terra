using System.Collections.Generic;
using Terra.Combat;
using Terra.Player;
using UIExtensionPackage.UISystem.Core.Interfaces;
using UIExtensionPackage.UISystem.UI.Elements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    public class HealthUI : UIElement, IWithSetupUI, IAttachListenersUI
    {
        [FormerlySerializedAs("heartPrefab")] [SerializeField] private GameObject _heartPrefab; 
        [FormerlySerializedAs("heartSprite")] [SerializeField] private Sprite _heartSprite;    

        private HealthController _healthController;
        private List<Image> _hearts = new();  
    
        public void SetUp()
        {
            _healthController = PlayerManager.Instance?.HealthController;

            if (_healthController == null)
            {
                Debug.LogError(this + " PlayerManager health controller does not exist");
                return;
            }

            CreateHearts();
            UpdateHearts(_healthController.CurrentHealth);
        }

        public void AttachListeners()
        {
            _healthController.OnHealthChanged += OnHealthChanged;
        }

        private void CreateHearts()
        {
            int heartCount = Mathf.CeilToInt(_healthController.MaxHealth); 

            for (int i = 0; i < heartCount; i++)
            {
                GameObject heart = Instantiate(_heartPrefab, transform);
                Image image = heart.GetComponent<Image>();
                _hearts.Add(image);
            }
        }

        private void OnHealthChanged(int currentHealth)
        {
            UpdateHearts(currentHealth);
        }

        private void UpdateHearts(float currentHealth)
        {
            int fullHearts = Mathf.FloorToInt(currentHealth);

            for (int i = 0; i < _hearts.Count; i++)
            {
                _hearts[i].gameObject.SetActive(i < fullHearts);
            }
        }
    
        public void DetachListeners()
        {
            if (_healthController != null)
                _healthController.OnHealthChanged -= OnHealthChanged;
        }


        public void TearDown()
        {
            _healthController = null;
        }
    }
}
