using NaughtyAttributes;
using Terra.AI.EnemyStates;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Managers;
using Terra.Particles;
using Terra.Player;
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
        [Foldout("References")][SerializeField] private Animator _animator;
        [Foldout("Particles")][SerializeField] private ParticleComponentData _particleComponentData;
        
        [Foldout("Debug"), ReadOnly][SerializeField] private int _penetrationTargets;
        [Foldout("Debug"), ReadOnly][SerializeField] private int _damage;
        [Foldout("Debug"), ReadOnly][SerializeField] private Entity _origin;
        [Foldout("Debug"), ReadOnly][SerializeField] private LayerMask _originLayer;
        [Foldout("Debug"), ReadOnly][SerializeField] private EffectsContainer _effects;
        
        private LayerMask _playerLayer;

        private void Awake()
        {
            _playerLayer = LayerMask.NameToLayer("Player");
        }
        
        /// <summary>
        ///     Initializes the projectile with configuration data and sets its velocity.
        /// </summary>
        public void Initialize(BulletData data, Vector3 direction, Entity origin)
        {
            
            _penetrationTargets = data.penetrationPower;
            _damage = data.bulletDamage;
            _effects = data.bulletEffects;
            _origin = origin;
            _originLayer = origin.gameObject.layer;
            _rigidbody.velocity = direction.normalized * data.bulletSpeed;
            transform.forward  = direction.normalized;
            if (origin.gameObject.layer == _playerLayer)
            {
                PlayerProjectileInitialization();
            }
            else
            {
                EnemyProjectileInitialization(direction);
            }
            Destroy(gameObject, 20f);
        }

        private void EnemyProjectileInitialization(Vector3 direction)
        {
            if (direction.x < 0)
            {
                _animator.CrossFade(AnimationHashes.AttackLeft, 0.1f);
            }
            else
            {
                _animator.CrossFade(AnimationHashes.AttackRight, 0.1f);
            }
        }

        private void PlayerProjectileInitialization()
        {
            _damage += PlayerStatsManager.Instance.PlayerStats.Dexterity;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == _originLayer) return;

          
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                if (_originLayer == _playerLayer)
                {
                    CombatManager.Instance.PlayerPerformAttack(WeaponType.Ranged, _origin, target, _effects);
                }
                else
                {
                    CombatManager.Instance.PerformAttack(_origin, target, _effects,  _damage);
                    _particleComponentData.offset = transform.position;
                    VFXController.SpawnParticleInWorld(_particleComponentData);
                }
            }

            Debug.Log($"{gameObject.name}: collided with {other.gameObject.name}");
            _penetrationTargets--;
            
            if(_penetrationTargets <0) Destroy(gameObject);
        }

        private void OnValidate()
        {
            if(!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
            if(!_spriteRenderer) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if(!_animator) _animator = GetComponentInChildren<Animator>();
        }
    }
}