using UnityEngine;
using Inventory.Pickups.Definitions;

namespace Inventory.Pickups
{
    public class CrystalPickup : Pickup
    {
        public override void OnPickUp()
        {
            CrystalPickupData data = (CrystalPickupData)pickupData;
            Debug.Log($"Picked up Crystal: +{data.crystalAmount}");
            // Add crystal adding logic
        }
    }
}