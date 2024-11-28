using System;
using Inventory.Items.Definitions;
using Player;
using UnityEngine;

namespace Inventory.Abstracts
{
    [Serializable]
    public abstract class Item : ScriptableObject, IEquipable
    {
        public bool CanBeRemoved => data.canBeRemoved; 
        
        public ItemType itemType;
        public ItemData data { get; private set; }
        
        public virtual void OnEquip()
        {
            //TODO: add luck
            PlayerStatsManager.Instance.AddStrength(data.strengthModifiers);
            PlayerStatsManager.Instance.AddMaxHealth(data.maxHealthModifiers);
            PlayerStatsManager.Instance.AddSpeed(data.speedModifiers);
        }
        
        public virtual void OnUnEquip()
        {
            //TODO: add luck
            PlayerStatsManager.Instance.RemoveStrength(data.strengthModifiers);
            PlayerStatsManager.Instance.RemoveMaxHealth(data.maxHealthModifiers);
            PlayerStatsManager.Instance.RemoveSpeed(data.speedModifiers);
        }
    }
}