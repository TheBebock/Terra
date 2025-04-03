using System;

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
    /// Represents logic for pickups
    /// </summary>
    [Serializable]
    public abstract class Pickup
    {
        public abstract string PickupName { get; }
        public abstract PickupType PickupType { get; }
        public abstract void OnPickUp(); 

    }
}

