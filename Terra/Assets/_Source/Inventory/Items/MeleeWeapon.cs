using System;
using Inventory.Items.Definitions;
using UnityEngine;


namespace Inventory.Items
{
    [Serializable]
    public class MeleeWeapon : Weapon
    {
        public MeleeWeaponData meleeWeaponData;
        private void OnValidate()
        {
            itemType = ItemType.Melee;
        }
        public void PerformAttack(bool isThrust)
        {
            if (isThrust)
            {
                if (meleeWeaponData.attackType == AttackType.Thrust)
                {
                    PerformThrust();
                }
            }
            else
            {
                PerformSwing();
            }
        }
        private void PerformThrust()
        {
        
        }

        private void PerformSwing()
        {
        
        }
        

    }
}