using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Pickups.Definitions;
using Terra.Player;
using UnityEngine;

namespace Terra.Itemization.Pickups
{
    [Serializable]
    public class HealthPickup : Pickup<HealthPickupData>
    {
        
        public override PickupType PickupType => PickupType.Health;

        public override void OnPickUp()
        {
            PlayerManager.Instance.Heal(Data.healthAmount);
        }
        
    }
}