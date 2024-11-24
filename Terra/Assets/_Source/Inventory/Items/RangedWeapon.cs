using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using Inventory.Items.Definitions;
using UnityEngine;

namespace Inventory.Items
{
    public class RangedWeapon : Weapon
    {
        public RangedWeaponData rangedWeaponData;

        public void Shoot()
        {
            if (rangedWeaponData.isExplosive)
            {
                ThrowExplosive();
            }
            else
            {
                
            }
        }

        private void ThrowExplosive()
        {
            if (rangedWeaponData.projectilePrefab != null)
            {
                //TODO: throw logic
            }

            if (rangedWeaponData.explosionSound != null)
            {
                //AudioSource.PlayClipAtPoint(rangedWeaponData.explosionSound, transform.position);
            }

            if (rangedWeaponData.explosionEffect != null)
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