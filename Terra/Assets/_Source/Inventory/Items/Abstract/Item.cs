using System;
using Inventory.Items.Definitions;
using NaughtyAttributes;
using Player;

namespace Inventory.Abstracts
{
    //NOTE: Class cannot be abstract, because it will not serialize otherwise
    [Serializable]
    public class Item : IEquipable
    {
        public bool CanBeRemoved => Data.canBeRemoved; 
        
        [ReadOnly]public ItemType itemType;
        //TODO: Remove this Data and change OnEquip / OnUnEquip
        public ItemData Data;
        
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