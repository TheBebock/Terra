using System;
using _Source.Components;
using UnityEngine;

namespace _Source.AI.Enemy
{
    /// <summary>
    /// Factory responsible for instantiating and initializing bullet projectiles.
    /// Implements the IBulletFactory interface to support dependency injection and unit testing.
    /// </summary>
    public class BulletFactory : IBulletFactory
    {
        /// <summary>
        /// The projectile prefab used to instantiate new bullets.
        /// </summary>
        private readonly GameObject _projectilePrefab;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulletFactory"/> class with the specified projectile prefab.
        /// </summary>
        /// <param name="prefab">The GameObject to be used as the bullet projectile prefab.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided prefab is null.</exception>
        public BulletFactory(GameObject prefab)
        {
            _projectilePrefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
        }

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
        public Projectile CreateBullet(BulletData data, Vector3 position, Vector3 direction, GameObject shooter)
        {
            if (_projectilePrefab == null)
                throw new InvalidOperationException("BulletFactory prefab is not set.");

            // Instantiate the projectile prefab at the given position with no rotation.
            var go = UnityEngine.Object.Instantiate(_projectilePrefab, position, Quaternion.identity);

            // Try to get the Projectile component from the instantiated object.
            var proj = go.GetComponent<Projectile>();

            // Ensure the prefab has a valid Projectile component.
            if (proj == null)
            {
                UnityEngine.Object.Destroy(go);
                throw new InvalidOperationException("Projectile prefab does not contain a Projectile component.");
            }

            // Initialize the projectile with its configuration, direction, and owner.
            proj.Initialize(data, direction, shooter);

            return proj;
        }
    }
}
