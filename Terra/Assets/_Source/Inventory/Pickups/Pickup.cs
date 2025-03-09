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
        public PickupType PickupType => pickupData.pickupType;
        public virtual void OnPickUp() {}
    }

}

