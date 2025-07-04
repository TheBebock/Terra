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
        public virtual string PickupName => throw new NotImplementedException();
        public virtual AudioClip PickupSound => throw new NotImplementedException();
        public virtual PickupType PickupType => throw new NotImplementedException();
        public virtual Sprite ItemIcon => throw new NotImplementedException();
        public virtual Material ItemMaterial => throw new NotImplementedException();
        public virtual float DropRate => throw new NotImplementedException();
        public virtual void OnPickUp() { }
        public virtual bool CanBePickedUp(){return true;}

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

        public sealed override AudioClip PickupSound => _data.pickupSound;
        public sealed override float DropRate => _data.dropRateChance;
        public sealed override string PickupName => _data.pickupName;
        public sealed override Sprite ItemIcon => _data.pickupSprite;
        public sealed override Material ItemMaterial => _data.pickupMaterial;

    }
}

