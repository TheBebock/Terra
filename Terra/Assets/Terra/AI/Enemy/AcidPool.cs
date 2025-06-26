using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Terra.Combat;
using Terra.Components;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using Terra.Particles;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class AcidPool : Entity, IAttachListeners
    {
        
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _fadeDuration = 2f;
        [SerializeField] private float _lightsFadeDuration = 1f;
        [SerializeField, ReadOnly] private Collider _collider;
        [SerializeField] private List<LightComponent> _lights = new();
        private float _lifeDuration; 
        
        public void Init(float lifeDuration, int damage)
        {
            _damage = damage;
            _lifeDuration = lifeDuration;
            VFXController.SpawnAndAttachParticleToEntity(this, VFXcontroller.onSpawnParticle);
            _ = StartDeathAnim();
        }
        
        public void AttachListeners()
        {
            EventsAPI.Register<StartOfNewFloorEvent>(OnStartOfNewFloor);
        }

        private async UniTaskVoid StartDeathAnim()
        {
            await UniTask.WaitForSeconds(_lifeDuration, cancellationToken: CancellationToken);
         
            _collider.enabled = false;
            VFXcontroller.DoFadeModel(0, _fadeDuration);
            for (int i = 0; i < _lights.Count; i++)
            {
                _lights[i].DoFadeIntensity(0,_lightsFadeDuration);   
            }
            await UniTask.WaitForSeconds(_fadeDuration + 0.2f, cancellationToken: CancellationToken);
            
            Destroy(gameObject);
        }
        private void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent(out IDamageable damageable)) return;
            
            damageable.TakeDamage(_damage);
        }

        private void OnStartOfNewFloor(ref StartOfNewFloorEvent @event)
        {
            Destroy(gameObject);
        }

        public void DetachListeners()
        {
            EventsAPI.Unregister<StartOfNewFloorEvent>(OnStartOfNewFloor);
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            _lights.Clear();
            _lights = GetComponentsInChildren<LightComponent>().ToList();
            if(!_collider) _collider = GetComponent<Collider>();
        }
    }
}
