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


        public RangedWeapon(RangedWeaponData itemData)
        {
            Data = itemData;
        }

        public override void OnEquip()
        {
            base.OnEquip();
            
            PlayerInventoryManager.Instance.SetMaxAmmo(Data.ammoCapacity);
            PlayerInventoryManager.Instance.ModifyCurrentAmmo(0);
        }
    }
}