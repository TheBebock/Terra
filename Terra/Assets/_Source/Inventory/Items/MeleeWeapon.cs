using Inventory.Items.Definitions;
using UnityEngine;


namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "meleeWeapon_", menuName = "TheBebocks/Weapons/Melee")]
    public class MeleeWeapon : Weapon
    {
        public MeleeWeaponData meleeWeaponData;

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