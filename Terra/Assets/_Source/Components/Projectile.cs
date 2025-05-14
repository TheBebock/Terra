using _Source.AI.Enemy;
using Terra.Combat;
using UnityEngine;

namespace _Source.Components
{
    /// <summary>
    /// Represents a projectile that moves in 3D space, applies damage on contact, and self-destructs after its lifetime.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        private float _damage;
        private float _speed;
        private GameObject _shooter;
        private TeamType _shooterTeam;

        /// <summary>
        /// Initializes the projectile with configuration data and sets its velocity.
        /// </summary>
        public void Initialize(BulletData data, Vector3 direction, GameObject shooter)
        {
            _damage = data.bulletDamage;
            _speed = data.bulletSpeed;
            _shooter = shooter;

            if (shooter.TryGetComponent<ITeamMember>(out var teamMember))
            {
                _shooterTeam = teamMember.GetTeam();
            }
            else
            {
                Debug.LogWarning("Shooter does not implement ITeamMember. Defaulting to Enemy.");
                _shooterTeam = TeamType.Enemy;
            }

            Destroy(gameObject, data.bulletLifetime);

            var rb = GetComponent<Rigidbody>();
            rb.velocity = direction.normalized * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _shooter) return;

            if (other.TryGetComponent<ITeamMember>(out var otherTeam))
            {
                if (otherTeam.GetTeam() == _shooterTeam)
                {
                    // Same team - ignore
                    return;
                }
            }

            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}