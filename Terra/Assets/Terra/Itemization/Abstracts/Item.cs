using System;
using Terra.Player;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Interfaces;
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
    public abstract class Item<TData> : ItemBase, IEquipable, IItem<TData>
    where TData: ItemData
    { 
        [SerializeField] private TData data;
        public TData Data 
        { 
            get => data;
            protected set => data = value;
        }

        public sealed override string ItemName => data.itemName;

        public sealed override Sprite ItemIcon => data.itemSprite;

        public sealed override bool CanBeRemoved => data.canBeRemoved;

        public virtual void OnEquip()
        {
            if (Data == null)
            {
                Debug.LogError($"{this} data was null. It should never happen.");
                return;
            }

            if (!PlayerManager.Instance)
            {
                Debug.LogError($"{this}: Player Manager not found, cannot add stat modifiers.");
            }
            PlayerStatsManager.Instance?.AddStrength(Data.strengthModifiers);
            PlayerStatsManager.Instance?.AddMaxHealth(Data.maxHealthModifiers);
            PlayerStatsManager.Instance?.AddDexterity(Data.speedModifiers);
            PlayerStatsManager.Instance?.AddLuck(Data.luckModifiers);
        }
        
        public void OnUnEquip()
        {
            if (Data == null)
            {
                Debug.LogError($"{this} data was null. It should never happen.");
                return;
            }
            if (!PlayerManager.Instance)
            {
                Debug.LogError($"{this}: Player Manager not found, cannot remove stat modifiers.");
            }
            PlayerStatsManager.Instance?.RemoveStrength(Data.strengthModifiers);
            PlayerStatsManager.Instance?.RemoveMaxHealth(Data.maxHealthModifiers);
            PlayerStatsManager.Instance?.RemoveSpeed(Data.speedModifiers);
            PlayerStatsManager.Instance?.RemoveLuck(Data.luckModifiers);
        }
        
    }
}