using _Source.Components;
using UnityEngine;

namespace _Source.AI.Enemy
{
    /// <summary>
    /// Defines a factory responsible for creating and initializing bullet projectiles.
    /// </summary>
    public interface IBulletFactory
    {
        /// <summary>
        /// Creates a new bullet projectile with the specified data, position, and direction.
        /// </summary>
        /// <param name="data">The bullet data used to initialize the projectile (e.g. speed, damage).</param>
        /// <param name="position">The world position where the projectile should be spawned.</param>
        /// <param name="direction">The direction in which the projectile should move.</param>
        /// <returns>An initialized <see cref="Projectile"/> instance.</returns>
        Projectile CreateBullet(BulletData data, Vector3 position, Vector3 direction, GameObject shooter);
    }
}