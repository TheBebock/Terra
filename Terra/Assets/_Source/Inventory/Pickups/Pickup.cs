using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Pickups.Definitions;
using UnityEngine;

namespace Inventory.Pickups
{
    [Serializable]
    public abstract class Pickup : ScriptableObject
    {
        [SerializeField] PickupData pickupData;
        public PickupType pickupType;
        public virtual void OnPickUp()
        {
            if (pickupType == PickupType.Health)
            {
                //add health
            }
            if (pickupType == PickupType.Ammo)
            {
                //add ammo
            }
            if (pickupType == PickupType.Crystal)
            {
                //add crystal
            }
        }
        
    }

}

