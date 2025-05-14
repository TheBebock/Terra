using System;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;
using UnityEngine;

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

        public override void PerformAttack(Vector3 position, Quaternion rotation)
        {
            
        }
        
    }
}