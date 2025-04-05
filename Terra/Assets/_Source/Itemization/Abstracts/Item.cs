using System;
using Terra.Player;
using NaughtyAttributes;
using Terra.Itemization.Items.Definitions;
using Terra.Itemization.Interfaces;
using Terra.LootSystem;
using UnityEngine;

namespace Terra.Itemization.Abstracts
{
    /// <summary>
    /// Represents base class for all items
    /// </summary>
    /// <remarks>Class should be an abstract, do not create instances of this class</remarks>
    [Serializable]
    public class ItemBase
    {
        public virtual ItemType ItemType { get; }
        public virtual string ItemName { get; } 

        public virtual Sprite ItemIcon { get; } = null;
        public virtual bool CanBeRemoved { get; protected set; }
        
        protected ItemBase() { }
        
    }
    
    /// <summary>
    /// Represents logic for item.
    /// </summary>
    [Serializable]
    public abstract class Item<T> : ItemBase, IEquipable
    where T: ItemData
    { 
        [SerializeField] private T data;
        public T Data 
        { 
            get => data;
            protected set => data = value;
        }

        public override string ItemName => data.itemName;

        public override Sprite ItemIcon => data.itemSprite;

        public virtual void OnEquip()
        {
            PlayerStatsManager.Instance.AddStrength(Data.strengthModifiers);
            PlayerStatsManager.Instance.AddMaxHealth(Data.maxHealthModifiers);
            PlayerStatsManager.Instance.AddSpeed(Data.speedModifiers);
            PlayerStatsManager.Instance.AddLuck(Data.luckModifiers);
        }
        
        public virtual void OnUnEquip()
        {
            PlayerStatsManager.Instance.RemoveStrength(Data.strengthModifiers);
            PlayerStatsManager.Instance.RemoveMaxHealth(Data.maxHealthModifiers);
            PlayerStatsManager.Instance.RemoveSpeed(Data.speedModifiers);
            PlayerStatsManager.Instance.RemoveLuck(Data.luckModifiers);
        }

        [ContextMenu("Add To Loot Table")]
        public void AddToLootTable()
        {
            LootTable.AddItemToLootTable(this);
        }
        
    }
}