using NaughtyAttributes;
using Terra.AI.Enemy;
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
        [Foldout("References")][SerializeField] private AcidBulletSplashComponent _acidBulletSplash;
        
        
        [Foldout("Particles")][SerializeField] private ParticleComponentData _particleComponentData;
        
        
        [Foldout("Debug"), ReadOnly][SerializeField] private int _penetrationTargets;
        [Foldout("Debug"), ReadOnly][SerializeField] private int _damage;
        [Foldout("Debug"), ReadOnly][SerializeField] private Entity _origin;
        [Foldout("Debug"), ReadOnly][SerializeField] private LayerMask _originLayer;
        [Foldout("Debug"), ReadOnly][SerializeField] private EffectsContainer _effects;
        
        private LayerMask _playerLayer;
        private LayerMask _rangeBoundsLayer;
        private Vector3 _direction;

        private void Awake()
        {
            _playerLayer = LayerMask.NameToLayer("Player");
            _rangeBoundsLayer = LayerMask.NameToLayer("RangeBounds");
        }
        
        /// <summary>
        ///     Initializes the projectile with configuration data and sets its velocity.
        /// </summary>
        public void Initialize(BulletData data, Vector3 direction, Entity origin, float bulletSpeedModifier =1f)
        {
            
            _penetrationTargets = data.penetrationPower;
            _damage = data.bulletDamage;
            _effects = data.bulletEffects;
            _origin = origin;
            _originLayer = origin.gameObject.layer;
            _rigidbody.velocity = direction.normalized * (data.bulletSpeed * bulletSpeedModifier);
            transform.forward  = direction.normalized;
            _direction = direction.normalized;
            if (origin.gameObject.layer == _playerLayer)
            {
                PlayerProjectileInitialization();
            }
            else
            {
                EnemyProjectileInitialization(_direction);
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
            bool isPlayer = _originLayer == _playerLayer;
          
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                if (isPlayer)
                {
                    CombatManager.Instance.PlayerPerformAttack(WeaponType.Ranged, _origin, target, _effects);
                }
                else
                {
                    CombatManager.Instance.PerformAttack(_origin, target, _effects,  _damage);
                }
            }
            
            _penetrationTargets--;

            if (_penetrationTargets < 0)
            {
                if (!isPlayer && other.gameObject.layer != _rangeBoundsLayer)
                {
                    _particleComponentData.offset = transform.position;
                    Instantiate(_acidBulletSplash, transform.position, Quaternion.identity).Init(_direction);
                    VFXController.SpawnParticleInWorld(_particleComponentData);
                }
                Destroy(gameObject);
            }
        }

        private void OnValidate()
        {
            if(!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
            if(!_spriteRenderer) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if(!_animator) _animator = GetComponentInChildren<Animator>();
        }
    }
}