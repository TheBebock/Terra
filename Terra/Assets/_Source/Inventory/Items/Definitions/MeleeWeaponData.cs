using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Items.Definitions
{
    [CreateAssetMenu(fileName = "MeleeWeaponData_", menuName = "TheBebocks/Items/MeleeWeaponData")]
    public class MeleeWeaponData : WeaponData
    {
        public AttackType attackType;
        public float thrustSpeed;
        public float thrustRange;
        public float swingSpeed;
        public float knockback;
        public AnimationClip thrustAnimationClip;
        public AnimationClip swingAnimationClip;
    }

    public enum AttackType
    {
        Thrust,
        Swing
    }
}