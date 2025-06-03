using System;
using Terra.Itemization.Pickups.Definitions;
using UnityEngine;

namespace Terra.Itemization.Abstracts
{
    /// <summary>
    /// Represents type of pickup
    /// </summary>
    public enum PickupType
    {
        Health = 0,
        Ammo = 1,
        Crystal = 2
    }
    
    /// <summary>
    /// Represents all pickups
    /// </summary>
    [Serializable]
    public class PickupBase
    {
        public virtual string PickupName { get; }
        public virtual PickupType PickupType { get; }
        
        public virtual Sprite ItemIcon { get; }
        public virtual float DropRate { get; }

        public virtual void OnPickUp() { }

    }
    
    /// <summary>
    /// Represents logic for pickups
    /// </summary>
    [Serializable]
    public class Pickup<TData> : PickupBase
    where TData : PickupData
    {
        [SerializeField] private TData _data;
        public TData Data => _data;

        public sealed override float DropRate => _data.dropRateChance;
        public sealed override string PickupName => _data.pickupName;
        public sealed override Sprite ItemIcon => _data.pickupSprite;
    }
}

