using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Pickups.Definitions;
using UnityEngine;

namespace Terra.Itemization.Pickups
{
    [Serializable]
    public class HealthPickup : Pickup
    {
        [SerializeField] private HealthPickupData pickupData;
        public HealthPickupData Data => pickupData;
        
        public override string PickupName => Data.pickupName;
        public override PickupType PickupType => PickupType.Health;

        public override void OnPickUp()
        {
            HealthPickupData data = (HealthPickupData)pickupData;
            Debug.Log($"Picked up Health: +{data.healthAmount} HP");
            // Add healing logic
        }
        
    }
}