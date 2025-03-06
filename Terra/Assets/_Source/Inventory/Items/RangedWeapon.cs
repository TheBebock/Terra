using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Items.Definitions;
using NaughtyAttributes;
using OdinSerializer;
using UnityEngine;

namespace Inventory.Items
{
    [Serializable]
    public class RangedWeapon : Weapon
    {

        [ReadOnly] public string name;
        [OdinSerialize] public string name2 => rangedData.name;
        public RangedWeaponData rangedData;

        public RangedWeapon(string newName)
        {
            name = newName;
        }

        public void Shoot()
        {
            if (rangedData.isExplosive)
            {
                ThrowExplosive();
            }
            else
            {
                
            }
        }

        private void ThrowExplosive()
        {
            if (rangedData.projectilePrefab != null)
            {
                //TODO: throw logic
            }

            if (rangedData.explosionSound != null)
            {
                //AudioSource.PlayClipAtPoint(rangedWeaponData.explosionSound, transform.position);
            }

            if (rangedData.explosionEffect != null)
            {
                //TODO: explosion effect
            }
            //TODO: damage logic in explosion range
        }
        public void Reload()
        {
            
        }
    }
}