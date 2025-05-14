using System.Collections.Generic;
using Terra.Combat;
using Terra.Player;
using UIExtensionPackage.UISystem.Core.Interfaces;
using UIExtensionPackage.UISystem.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    public class HealthUI : UIElement, IWithSetup, IAttachListeners
    {
        [SerializeField] private GameObject heartPrefab; 
        [SerializeField] private Sprite heartSprite;    

        private HealthController healthController;
        private List<Image> hearts = new();  
    
        public void SetUp()
        {
            healthController = PlayerManager.Instance?.HealthController;

            if (healthController == null)
            {
                Debug.LogError(this + " PlayerManager health controller does not exist");
                return;
            }

            CreateHearts();
            UpdateHearts(healthController.CurrentHealth);
        }

        public void AttachListeners()
        {
            healthController.OnHealthChanged += OnHealthChanged;
        }

        private void CreateHearts()
        {
            int heartCount = Mathf.CeilToInt(healthController.MaxHealth); 

            for (int i = 0; i < heartCount; i++)
            {
                GameObject heart = Instantiate(heartPrefab, transform);
                Image image = heart.GetComponent<Image>();
                hearts.Add(image);
            }
        }

        private void OnHealthChanged(float currentHealth)
        {
            UpdateHearts(currentHealth);
        }

        private void UpdateHearts(float currentHealth)
        {
            int fullHearts = Mathf.FloorToInt(currentHealth);

            for (int i = 0; i < hearts.Count; i++)
            {
                hearts[i].gameObject.SetActive(i < fullHearts);
            }
        }
    
        public void DetachListeners()
        {
            if (healthController != null)
                healthController.OnHealthChanged -= OnHealthChanged;
        }


        public void TearDown()
        {
            healthController = null;
        }
    }
}
