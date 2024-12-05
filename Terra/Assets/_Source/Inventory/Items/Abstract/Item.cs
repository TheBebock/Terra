using System;
using Inventory.Items.Definitions;
using Player;
using StatisticsSystem.Definitions;
using UnityEngine;

namespace Inventory.Abstracts
{
    [Serializable]
    public abstract class Item : ScriptableObject, IEquipable
    {
        public bool CanBeRemoved => Data.canBeRemoved; 
        
        public ItemType itemType;
        public ItemData Data { get; private set; }
        
        //TODO: When PlayerStatsManager get improves, this will need a change
        public virtual void OnEquip()
        {
            PlayerStatsManager.Instance.AddLuck(Data.luckModifiers);
            PlayerStatsManager.Instance.AddStrength(Data.strengthModifiers);
            PlayerStatsManager.Instance.AddMaxHealth(Data.maxHealthModifiers);
            PlayerStatsManager.Instance.AddSpeed(Data.speedModifiers);
        }
        
        public virtual void OnUnEquip()
        {
            PlayerStatsManager.Instance.RemoveLuck(Data.luckModifiers);
            PlayerStatsManager.Instance.RemoveStrength(Data.strengthModifiers);
            PlayerStatsManager.Instance.RemoveMaxHealth(Data.maxHealthModifiers);
            PlayerStatsManager.Instance.RemoveSpeed(Data.speedModifiers);
        }
    }
}