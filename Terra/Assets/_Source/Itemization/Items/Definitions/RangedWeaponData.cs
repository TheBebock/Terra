using UnityEngine;

namespace Terra.Itemization.Items.Definitions
{
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