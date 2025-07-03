using System;
using Terra.Player;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Interfaces;
using Terra.StatisticsSystem;
using UnityEngine;
using UnityEngine.Serialization;

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

        public virtual int ItemCost { get; } = 0;

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
        [FormerlySerializedAs("data")] [SerializeField] private TData _data;
        public TData Data 
        { 
            get => _data;
            protected set => _data = value;
        }

        public sealed override string ItemName => _data != null ? _data.name : string.Empty;

        public sealed override Sprite ItemIcon => _data != null ? _data.itemSprite : null;

        public sealed override bool CanBeRemoved => _data == null || _data.canBeRemoved;

        public sealed override int ItemCost => _data != null ? _data.itemCost : 0;
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
            PlayerStatsManager.Instance?.AddModifiers(StatisticType.Strength, Data.strengthModifiers);
            PlayerStatsManager.Instance?.AddModifiers(StatisticType.MaxHealth, Data.maxHealthModifiers);
            PlayerStatsManager.Instance?.AddModifiers(StatisticType.Dexterity, Data.dexModifiers);
            PlayerStatsManager.Instance?.AddModifiers(StatisticType.Luck,Data.luckModifiers);
            PlayerStatsManager.Instance?.AddModifiers(StatisticType.MeleeRange, Data.meleeRangeModifiers);
            PlayerStatsManager.Instance?.AddModifiers(StatisticType.SwingSpeed, Data.swingSpeedModifiers);
            PlayerStatsManager.Instance?.AddModifiers(StatisticType.RangeCooldown, Data.rangeCooldownModifiers);
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
            PlayerStatsManager.Instance?.RemoveModifiers(StatisticType.Strength, Data.strengthModifiers);
            PlayerStatsManager.Instance?.RemoveModifiers(StatisticType.MaxHealth, Data.maxHealthModifiers);
            PlayerStatsManager.Instance?.RemoveModifiers(StatisticType.Dexterity, Data.dexModifiers);
            PlayerStatsManager.Instance?.RemoveModifiers(StatisticType.Luck,Data.luckModifiers);
            PlayerStatsManager.Instance?.RemoveModifiers(StatisticType.MeleeRange, Data.meleeRangeModifiers);
            PlayerStatsManager.Instance?.RemoveModifiers(StatisticType.SwingSpeed, Data.swingSpeedModifiers);
            PlayerStatsManager.Instance?.RemoveModifiers(StatisticType.RangeCooldown, Data.rangeCooldownModifiers);
        }
        
    }
}