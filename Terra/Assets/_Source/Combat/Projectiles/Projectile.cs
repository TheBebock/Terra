using _Source.AI.Enemy;
using Terra.Core.Generics;
using Terra.EffectsSystem;
using Terra.Managers;
using UnityEngine;

namespace Terra.Combat.Projectiles
{
    /// <summary>
    /// Represents a projectile that moves in 3D space, applies damage on contact, and self-destructs after its lifetime.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        private float _damage;
        private float _speed;
        private Entity _origin;
        private Rigidbody _rigidbody;
        private LayerMask _originLayer;
        private EffectsContainer _effects;
        /// <summary>
        /// Initializes the projectile with configuration data and sets its velocity.
        /// </summary>
        public void Initialize(BulletData data, Vector3 direction, Entity origin)
        {
            _damage = data.bulletDamage;
            _speed = data.bulletSpeed;
            _effects = data.bulletEffects;
            _origin = origin;
            
            _rigidbody.velocity = direction.normalized * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == _originLayer) return;
            
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                CombatManager.Instance.PerformAttack(_origin, target, _effects,  _damage);
            }

            Destroy(gameObject);
        }

        private void OnValidate()
        {
            if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        }
    }
}