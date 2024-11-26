using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Pickups.Definitions;
using UnityEngine;

namespace Inventory.Pickups
{
    [Serializable]
    public class Pickup
    {
        [SerializeField] PickupData pickupData;

        public virtual void OnPickUp()
        {
            
        }
        
    }

}

