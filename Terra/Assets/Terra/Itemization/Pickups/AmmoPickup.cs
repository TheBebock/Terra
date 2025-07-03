using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Pickups.Definitions;
using Terra.Player;

namespace Terra.Itemization.Pickups
{
    [Serializable]
    public class AmmoPickup : Pickup<AmmoPickupData>
    {

        public override PickupType PickupType => PickupType.Ammo;

        public override void OnPickUp()
        {
            PlayerInventoryManager.Instance.ModifyCurrentAmmo(Data.ammoAmount);
        }

        public override bool CanBePickedUp()
        {
            if (!PlayerInventoryManager.Instance) return false;
            return PlayerInventoryManager.Instance.CurrentAmmo < PlayerInventoryManager.Instance.MaxAmmo;
        }
    }
}
