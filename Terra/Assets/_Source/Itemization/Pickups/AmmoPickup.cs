using System;
using UnityEngine;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Pickups.Definitions;

namespace Terra.Itemization.Pickups
{
    [Serializable]
    public class AmmoPickup : Pickup<AmmoPickupData>
    {

        public override PickupType PickupType => PickupType.Ammo;

        public override void OnPickUp()
        {
            Debug.Log($"Picked up Ammo: +{Data.ammoAmount} bullets");
            //TODO: Add adding ammo logic
        }
    }
}
