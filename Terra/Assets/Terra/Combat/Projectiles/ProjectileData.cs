using Terra.EffectsSystem.Abstract;
using UnityEngine;

namespace Terra.Combat.Projectiles
{
    /// <summary>
    /// Configuration data for a bullet: speed, damage and lifetime.
    /// </summary>
    [System.Serializable]
    public struct BulletData
    {
        public Projectile bulletPrefab;
        
        public EffectsContainer bulletEffects;
        
        [Tooltip("Movement speed of the bullet (units per second)")]
        public float bulletSpeed;

        [Tooltip("Damage dealt by the bullet on hit")]
        public float bulletDamage;

        [Tooltip("Lifetime of the bullet in seconds before auto-destroy")]
        public float bulletLifetime;
    }
}