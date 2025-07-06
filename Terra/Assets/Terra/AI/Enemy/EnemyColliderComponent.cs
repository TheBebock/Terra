using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.Extensions;
using Terra.Managers;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemyColliderComponent : InGameMonobehaviour
    {

        internal struct EntityDamageableData
        {
            public Entity entity;
            public IDamageable damageable;
        }

        [SerializeField, ReadOnly] private CapsuleCollider _collider;

        [Foldout("Debug"), ReadOnly][SerializeField] private int _damage;
        [Foldout("Debug"), ReadOnly][SerializeField] private AudioClip _attackSFX;
        [Foldout("Debug"), ReadOnly][SerializeField] private AudioSource _audioSource;

        private CountdownTimer _damageTimer;
        List<EntityDamageableData> _targets = new();

        public void EnableCollider()
        {
            _targets.Clear();
            _collider.enabled = true;
            _damageTimer.Restart();
        }

        public void DisableCollider()
        {
            _targets.Clear();
            _collider.enabled = false;
            _damageTimer.Stop();
        }
       
        public void Init(Entity entity, int damage, float _timeBetweenDealingDamage, AudioClip attackSFX, AudioSource audioSource)
        {
            _damage = damage;
            _attackSFX = attackSFX;
            _audioSource = audioSource;
            _damageTimer = new CountdownTimer(_timeBetweenDealingDamage);
            _damageTimer.OnTimerStop += OnDamageTimerStop;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                if (other.TryGetComponent(out Entity entity))
                {
                    _targets.AddUnique(new EntityDamageableData() { entity = entity, damageable = damageable });
                }
            }
        }

        private EntityDamageableData GetEntityDamageableData(Entity entity)
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                if(_targets[i].entity == entity) return _targets[i];
            }
            return default;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                if (other.TryGetComponent(out Entity entity))
                {
                    _targets.RemoveElement(GetEntityDamageableData(entity));
                }
            }
        }

        private void Update()
        {
            CheckDistance();
            _damageTimer.Tick(Time.deltaTime);
        }

        private void CheckDistance()
        {
            for (int i = _targets.Count-1; i >=  0; i--)
            {
                if (_targets[i].damageable == null)
                {
                    _targets.RemoveAt(i);
                    continue;
                }

                if (Vector3.Distance(transform.position, _targets[i].entity.transform.position) >
                    _collider.radius + 0.2f)
                {
                    _targets.RemoveAt(i);
                }
            }
        }

        private void OnDamageTimerStop()
        {
            DealDamage();
            if (_collider.enabled)
            {
                _damageTimer.Restart();
            }
        }
        
        private void DealDamage()
        {
            for (int i = _targets.Count-1; i >= 0; i--)
            {
                if (_targets[i].damageable == null)
                {
                    _targets.RemoveAt(i);
                    continue;
                }
                _targets[i].damageable.TakeDamage(_damage);
            }
            
            if(_attackSFX) AudioManager.Instance?.PlaySFXAtSource(_attackSFX, _audioSource);
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _damageTimer.OnTimerStop -= OnDamageTimerStop;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if(!_collider) _collider = GetComponent<CapsuleCollider>();
        }
#endif
        
    }
}
