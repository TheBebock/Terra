using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Pickups.Definitions;
using UnityEngine;

namespace Terra.Itemization.Pickups
{
    [Serializable]
    public class HealthPickup : Pickup<HealthPickupData>
    {
        
        public override PickupType PickupType => PickupType.Health;

        public override void OnPickUp()
        { 
            Debug.Log($"Picked up Health: +{Data.healthAmount} HP");
            // Add healing logic
        }
        
    }
}