using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Pickups.Definitions;
using UnityEngine;

namespace Inventory.Pickups
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] protected PickupData pickupData;
        public virtual PickupType PickupType { get; }
        public virtual void OnPickUp() {}
        
    }
}

