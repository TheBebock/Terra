using System;
using Terra.Core.Generics;
using UnityEngine;

namespace Terra.Combat.Projectiles
{
    /// <summary>
    /// Factory responsible for instantiating and initializing bullet projectiles.
    /// Implements the IBulletFactory interface to support dependency injection and unit testing.
    /// </summary>
    public static class ProjectileFactory
    {

        /// <summary>
        /// Creates a new bullet projectile at the specified position and direction,
        /// initializing it with the provided bullet data and shooter reference.
        /// </summary>
        /// <param name="data">The data defining bullet properties such as speed, damage, and lifetime.</param>
        /// <param name="position">The world position at which to spawn the projectile.</param>
        /// <param name="direction">The normalized direction in which the projectile should travel.</param>
        /// <param name="shooter">The GameObject that fired the bullet. Used to identify the source and prevent friendly fire.</param>
        /// <returns>A fully initialized <see cref="Projectile"/> instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the prefab is not assigned or does not contain a <see cref="Projectile"/> component.
        /// </exception>
        public static Projectile CreateProjectile(BulletData data, Vector3 position, Vector3 direction, Entity shooter)
        {
            if (data.bulletPrefab == null)
            {
                Debug.LogError("Received bullet data without prefab assigned");
                return null;
            }

            // Instantiate the projectile prefab at the given position with no rotation.
            Projectile projectile = UnityEngine.Object.Instantiate(data.bulletPrefab, position, Quaternion.identity);
            
            // Initialize the projectile with its configuration, direction, and owner.
            projectile.Initialize(data, direction, shooter);

            return projectile;
        }
    }
}
