using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using Inventory.Items.Definitions;
using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = "rangedWeapon_", menuName = "TheBebocks/Weapons/Ranged")]
    public class RangedWeapon : Weapon
    {
        
        public RangedWeaponData rangedData;
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