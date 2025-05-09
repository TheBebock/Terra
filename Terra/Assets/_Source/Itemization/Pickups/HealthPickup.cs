using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Pickups.Definitions;
using Terra.Player;

namespace Terra.Itemization.Pickups
{
    [Serializable]
    public class HealthPickup : Pickup<HealthPickupData>
    {
        
        public override PickupType PickupType => PickupType.Health;

        public override void OnPickUp()
        {
            PlayerManager.Instance.PlayerEntity.Heal(Data.healthAmount);
        }
        
    }
}