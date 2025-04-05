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

        public virtual void OnPickUp() { }

    }
    
    /// <summary>
    /// Represents logic for pickups
    /// </summary>
    [Serializable]
    public class Pickup<TData> : PickupBase
    where TData : PickupData
    {
        [SerializeField] private TData data;
        public TData Data => data;

        public override string PickupName => Data.pickupName;
        public override Sprite ItemIcon => Data.pickupSprite;
    }
}

