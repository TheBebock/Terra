using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.Managers;
using UnityEngine;

namespace Terra.Combat.Projectiles
{
    /// <summary>
    ///     Represents a projectile that moves in 3D space, applies damage on contact.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [Foldout("References")][SerializeField] private SpriteRenderer _spriteRenderer;
        [Foldout("References")][SerializeField] private Rigidbody _rigidbody;
        
        [Foldout("Debug"), ReadOnly][SerializeField] private int _penetrationTargets;
        [Foldout("Debug"), ReadOnly][SerializeField] private float _damage;
        [Foldout("Debug"), ReadOnly][SerializeField] private Entity _origin;
        [Foldout("Debug"), ReadOnly][SerializeField] private LayerMask _originLayer;
        [Foldout("Debug"), ReadOnly][SerializeField] private EffectsContainer _effects;
        
        /// <summary>
        /// Initializes the projectile with configuration data and sets its velocity.
        /// </summary>
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        public void Initialize(BulletData data, Vector3 direction, Entity origin)
        {
            if(data.bulletSprite) _spriteRenderer.sprite = data.bulletSprite;
            _penetrationTargets = data.penetrationPower;
            _damage = data.bulletDamage;
            _effects = data.bulletEffects;
            _origin = origin;
            _originLayer = origin.gameObject.layer;
            _rigidbody.velocity = direction.normalized * data.bulletSpeed;
            transform.forward  = direction.normalized;
            
            Destroy(gameObject, 10f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == _originLayer) return;
            
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                CombatManager.Instance.PerformAttack(_origin, target, _effects,  _damage);
            }

            _penetrationTargets--;
            
            if(_penetrationTargets <0) Destroy(gameObject);
        }

        private void OnValidate()
        {
            if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        }
    }
}