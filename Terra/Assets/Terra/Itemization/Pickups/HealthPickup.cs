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
            PlayerManager.Instance.PlayerEntity.Heal(Data.maxHealthHealAmount, true);
        }
        
        public override bool CanBePickedUp()
        {
            if (!PlayerManager.Instance) return false;
            return PlayerManager.Instance.HealthController.CurrentHealth < PlayerManager.Instance.HealthController.MaxHealth;
        }
    }
}