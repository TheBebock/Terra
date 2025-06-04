using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;


namespace Terra.Itemization.Items
{
    [Serializable]
    public class MeleeWeapon : Weapon<MeleeWeaponData>
    {
        public override ItemType ItemType => ItemType.Melee;

        public MeleeWeapon(MeleeWeaponData itemData)
        {
            Data = itemData;
        }
    }
}