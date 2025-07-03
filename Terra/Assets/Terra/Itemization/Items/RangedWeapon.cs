using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Player;

namespace Terra.Itemization.Items
{
    [Serializable]
    public class RangedWeapon : Weapon<RangedWeaponData>
    {
        public override ItemType ItemType => ItemType.Ranged;

        private int _heldAmmo;
        public RangedWeapon(RangedWeaponData itemData)
        {
            Data = itemData;
            _heldAmmo = itemData.ammoCapacity;
        }

        public override void OnEquip()
        {
            base.OnEquip();
            
            PlayerInventoryManager.Instance.SetMaxAmmo(Data.ammoCapacity);
            PlayerInventoryManager.Instance.ModifyCurrentAmmo(_heldAmmo);
            _heldAmmo = 0;
        }
    }
}