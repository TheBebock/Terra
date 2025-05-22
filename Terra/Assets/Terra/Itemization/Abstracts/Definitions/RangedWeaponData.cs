using Terra.Combat.Projectiles;
using UnityEngine;

namespace Terra.Itemization.Abstracts.Definitions
{
    //NOTE: It should never have [CrateAssetMenu], creating new definitions is done through Resources -> ItemsDatabase
    public class RangedWeaponData : WeaponData
    {
        public string ammoType;
        public int ammoCapacity;
        public float reloadTime;

        public bool isExplosive;
        public float explosionRadius;
        public float explosionDamage;
        
        public BulletData bulletData;
        public AudioClip firingSound;
        
    }
}