using System.Collections;
using System.Collections.Generic;
using Inventory.Pickups.Definitions;
using UnityEngine;

namespace Inventory.Pickups
{
    public class HealthPickup : Pickup
    {
        public override void OnPickUp()
        {
            HealthPickupData data = (HealthPickupData)pickupData;
            Debug.Log($"Picked up Health: +{data.healthAmount} HP");
            // Add healing logic
        }
    }
}