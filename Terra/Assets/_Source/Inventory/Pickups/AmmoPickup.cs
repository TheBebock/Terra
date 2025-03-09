using UnityEngine;
using Inventory.Pickups.Definitions;

namespace Inventory.Pickups
{
    public class AmmoPickup : Pickup
    {
        public override void OnPickUp()
        {
            AmmoPickupData data = (AmmoPickupData)pickupData;
            Debug.Log($"Picked up Ammo: +{data.ammoAmount} bullets");
            // Add adding ammo logic
        }
    }
}
