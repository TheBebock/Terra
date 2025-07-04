using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
using Terra.Utils;
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
        [SerializeField]  private float _lifeDuration = 10f;
        [SerializeField, ReadOnly] private CountdownTimer _deathTimer;

        private CancellationTokenSource _scaleCts;
        public void AttachListeners()
        {
            EventsAPI.Register<StartOfNewFloorEvent>(OnStartOfNewFloor);
        }
        
        public void Init(float lifeDuration, int damage)
        {
            _damage = damage;
            _lifeDuration = lifeDuration;
            _deathTimer = new CountdownTimer(_lifeDuration);
            _deathTimer.OnTimerStop += OnTimerStop;
            _deathTimer.Start();
            VFXController.SpawnAndAttachParticleToEntity(this, VFXcontroller.onSpawnParticle);
            transform.localScale = Vector3.zero;
            DoScale(new(1f,1f,1f), 1f);
        }

        private void Update()
        {
            _deathTimer?.Tick(Time.deltaTime);
        }

        private void OnTimerStop()
        {
            _ = StartDeathAnim();
            VFXController.SpawnAndAttachParticleToEntity(this, VFXcontroller.onSpawnParticle);
            
        }
        public void ResetDeathTimer()
        { 
            Debug.Log($"Before Reset death timer: {_deathTimer.Time} | {_deathTimer.IsRunning}");
            _deathTimer.ResetTime();
            Debug.Log($"After Reset death timer: {_deathTimer.Time} | {_deathTimer.IsRunning}");
        }

        public void DoScale(Vector3 newScale, float duration)
        {
            _ = DoScaleAsync(newScale, duration);
        }
        
        private async UniTaskVoid DoScaleAsync(Vector3 newScale, float duration)
        {
            _scaleCts?.Cancel();
            _scaleCts?.Dispose();
            _scaleCts = new CancellationTokenSource();
            
            try
            {
                ScaleLights(duration);
                await transform
                    .DOScale(newScale, duration)
                    .WithCancellation(_scaleCts.Token);
            }
            finally
            {
                transform.localScale = newScale;
            }
        }

        private void ScaleLights(float duration)
        {
            float scale = transform.localScale.y > transform.localScale.z ? 
                transform.localScale.y : transform.localScale.z;
            for (int i = 0; i < _lights.Count; i++)
            {
                _lights[i].DoSetRange(scale, duration);
            }
        }
        private void OnStartOfNewFloor(ref StartOfNewFloorEvent @event)
        {
            Destroy(gameObject);
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
        
        public void DetachListeners()
        {
            EventsAPI.Unregister<StartOfNewFloorEvent>(OnStartOfNewFloor);
        }
        protected override void CleanUp()
        {
            base.CleanUp();
            _deathTimer.OnTimerStop -= OnTimerStop;
        }

#if UNITY_EDITOR
        
        protected override void OnValidate()
        {
            base.OnValidate();
            _lights.Clear();
            _lights = GetComponentsInChildren<LightComponent>().ToList();
            if(!_collider) _collider = GetComponent<Collider>();
        }
#endif
    }
}
