using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.Extensions;
using Terra.Managers;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemyColliderComponent : InGameMonobehaviour
    {
        [SerializeField, ReadOnly] private Collider _collider;

        [Foldout("Debug"), ReadOnly][SerializeField] private int _damage;
        [Foldout("Debug"), ReadOnly][SerializeField] private Entity _entity;
        [Foldout("Debug"), ReadOnly][SerializeField] private EffectsContainer _effectsContainer = null;
        
        private CountdownTimer _damageTimer;

        public void EnableCollider()
        {
            _damageables.Clear();
            _collider.enabled = true;
            _damageTimer.Restart();
        }

        public void DisableCollider()
        {
            _damageables.Clear();
            _collider.enabled = false;
            _damageTimer.Stop();
        }
        List<IDamageable> _damageables = new();
        public void Init(Entity entity, int damage, EffectsContainer effectsContainer = null)
        {
            _entity = entity;
            _damage = damage;
            _effectsContainer = effectsContainer;
            _damageTimer = new CountdownTimer(0.25f);
            _damageTimer.OnTimerStop += OnDamageTimerStop;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                _damageables.AddUnique(damageable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                _damageables.RemoveElement(damageable);
            }
        }

        private void Update()
        {
            _damageTimer.Tick(Time.deltaTime);
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
            for (int i = _damageables.Count-1; i >= 0; i--)
            {
                if (_damageables[i] == null)
                {
                    _damageables.RemoveAt(i);
                    continue;
                }
                _damageables[i].TakeDamage(_damage);
            }
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _damageTimer.OnTimerStop -= OnDamageTimerStop;
        }

        private void OnValidate()
        {
            if(!_collider) _collider = GetComponent<Collider>();
        }
    }
}
