using UnityEngine;
using Inventory.Pickups.Definitions;

namespace Inventory.Pickups
{
    public class AmmoPickup : Pickup
    {
        public AmmoPickupData data;

        public override PickupType PickupType { get; protected set;} = PickupType.Ammo;

        public override void OnPickUp()
        {
            Debug.Log($"Picked up Ammo: +{data.ammoAmount} bullets");
            // Add adding ammo logic
        }
    }
}
