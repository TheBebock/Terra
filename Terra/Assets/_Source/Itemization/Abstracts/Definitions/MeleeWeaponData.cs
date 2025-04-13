using NaughtyAttributes;
using UnityEngine;

namespace Terra.Itemization.Items.Definitions
{
    [CreateAssetMenu(fileName = "MeleeWeaponData_", menuName = "TheBebocks/Items/MeleeWeaponData")]
    public class MeleeWeaponData : WeaponData
    {
        public AttackType attackType;
        [ShowIf(nameof(ShowThrustInfo))]
        public Vector3 hitboxSize;
        [ShowIf(nameof(ShowSwingInfo))]
        public float sphereHitboxRadius;

        private bool ShowThrustInfo() => attackType == AttackType.Thrust;
        private bool ShowSwingInfo() => attackType == AttackType.Thrust;
    }

    public enum AttackType
    {
        Thrust,
        Swing
    }
}