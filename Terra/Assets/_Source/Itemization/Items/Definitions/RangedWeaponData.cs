using UnityEngine;

namespace Terra.Itemization.Items.Definitions
{
    [CreateAssetMenu(fileName = "RangedWeaponData_", menuName = "TheBebocks/Items/RangedWeaponData")]
    public class RangedWeaponData : WeaponData
    {
        public string ammoType;
        public int ammoCapacity;
        public float reloadTime;

        public bool isExplosive;
        public float explosionRadius;
        public float explosionDamage;
        
        public GameObject projectilePrefab;
        public AudioClip firingSound;
        public ParticleSystem explosionEffect;
        public AudioClip explosionSound;
        
    }
}