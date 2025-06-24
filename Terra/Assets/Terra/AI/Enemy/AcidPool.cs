using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class AcidPool : Entity, IAttachListeners
    {
        
        [SerializeField] private int _damage;
        [SerializeField, ReadOnly] private Collider _collider;
        private float _lifeDuration; 
        
        public void Init(float lifeDuration, int damage)
        {
            _damage = damage;
            _lifeDuration = lifeDuration;
            VFXController.PlayParticleOnEntity(VFXController.onSpawnParticle);
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
            VFXController.DoFadeModel(0, 2);
            await UniTask.WaitForSeconds(2.5f, cancellationToken: CancellationToken);
            
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
            if(!_collider) _collider = GetComponent<Collider>();
        }
    }
}
