using System;
using NaughtyAttributes;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Itemization.Items
{
    /// <summary>
    /// Represents an item, that has a special use case
    /// </summary>
    [Serializable]
    public class ActiveItem : Item<ActiveItemData>
    {
        
        [FormerlySerializedAs("currentCD")] [SerializeField, ReadOnly] private float _currentCd;
        
        public override ItemType ItemType => ItemType.Active;
        public bool CanBeActivated => _currentCd <= 0;
        public float CurrentCd => _currentCd;
        private float MaxCd => Data.itemCooldown;
        
        public static event Action<ActiveItem, bool> OnStatusChanged;

        public ActiveItem(ActiveItemData itemData)
        {
            // Set Data
            Data = itemData;
            // Reset cooldown on creation
            _currentCd = 0;
        }

        /// <summary>
        /// Decreases cooldown of the item and changes status when available
        /// </summary>
        public void DecreaseCooldown()
        {
            // Decrease cooldown
            _currentCd--;
            // Clamp value
            _currentCd = Mathf.Max(_currentCd, 0);
            
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
            _currentCd = MaxCd;
            
            // Invoke status change
            OnStatusChanged?.Invoke(this, false);
            Debug.Log($"Activating {Data.itemName}, Cooldown: {Data.itemName}s");
        }
        
    }
}