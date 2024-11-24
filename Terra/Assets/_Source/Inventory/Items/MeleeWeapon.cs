using Inventory.Items.Definitions;


namespace Inventory.Items
{
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