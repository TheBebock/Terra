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
        
        [Tooltip("Amount of additional targets before destroying")]
        [Min(0)]public int penetrationPower;
        
        [Tooltip("Movement speed of the bullet (units per second)")]
        [Min(2)]public float bulletSpeed;

        [Tooltip("Damage dealt by the bullet on hit")]
        [Min(0)]public int bulletDamage;
    }
}