using Terra.Combat;
using UnityEngine;

namespace Terra.Components
{

    /// <summary>
    /// Represents a projectile that applies damage on contact and self-destructs after its lifetime.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        private float damage;
        private float speed;

        /// <summary>
        /// Initializes the projectile with configuration data and sets its velocity.
        /// </summary>
        public void Initialize(BulletData data, Vector3 direction)
        {
            damage = data.bulletDamage;
            speed = data.bulletSpeed;

            // Schedule destruction after lifetime
            Destroy(gameObject, data.bulletLifetime);

            // Set initial velocity
            var rb = GetComponent<Rigidbody2D>();
            rb.velocity = direction.normalized * speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}