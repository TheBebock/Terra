using System;
using NaughtyAttributes;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;
using UnityEngine;

namespace Terra.Itemization.Items
{
    /// <summary>
    /// Represents an item, that has a special use case
    /// </summary>
    [Serializable]
    public class ActiveItem : Item<ActiveItemData>
    {
        
        [SerializeField, ReadOnly] private float currentCD = 0;
        
        public override ItemType ItemType => ItemType.Active;
        public bool CanBeActivated => currentCD <= 0;
        public float CurrentCD => currentCD;
        private float MaxCD => Data.itemCooldown;
        
        public static event Action<ActiveItem, bool> OnStatusChanged;

        public ActiveItem(ActiveItemData itemData)
        {
            // Set Data
            Data = itemData;
            // Reset cooldown on creation
            currentCD = 0;
        }

        /// <summary>
        /// Decreases cooldown of the item and changes status when available
        /// </summary>
        public void DecreaseCooldown()
        {
            // Decrease cooldown
            currentCD--;
            // Clamp value
            currentCD = Mathf.Max(currentCD, 0);
            
            // Invoke event if the item can be activated
            if(CanBeActivated)
                OnStatusChanged?.Invoke(this, true);
        }

        /// <summary>
        /// Uses item
        /// </summary>
        public void ActivateItem()
        {
            // Fail-safe
            if(!CanBeActivated) return;
            
            // Use particular item
            Data.ActivateItem();

            // Update cooldown
            currentCD = MaxCD;
            
            // Invoke status change
            OnStatusChanged?.Invoke(this, false);
            Debug.Log($"Activating {Data.itemName}, Cooldown: {Data.itemName}s");
        }
        
    }
}