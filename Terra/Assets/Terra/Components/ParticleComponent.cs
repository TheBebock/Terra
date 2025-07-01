using System;
using System.Drawing.Design;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Utils;
using UnityEngine;

namespace Terra.Components
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleComponent : InGameMonobehaviour
    { 
        
        [Tooltip("If the particle is played often, this shouldn't be marked, for example: 'OnHit' is played often")]
        [SerializeField] private bool _isDestroyable;
        [SerializeField] private float _duration;
        [Foldout("Debug")] [SerializeField] private ParticleSystem _particles;
        private CountdownTimer _timer;
        
        public static event Action<ParticleComponent> OnParticleDestroyed;
        
        public float MainParticlesDuration => _particles.main.duration;
        public void Initialize(float newDuration = 0f)
        {
            _particles.Play();
            if(!_isDestroyable) return;
            
            if(newDuration > 0f) _duration = newDuration;
            if (_particles.main.loop)
            {
                _timer = new CountdownTimer(_duration);
            }
            else
            {
                _timer = new CountdownTimer(_particles.main.duration);
            }
            // If _duration is -1, it means that particles should exist infinitely
            if(!Mathf.Approximately(_duration, -1f)) _timer.Start();
        }

        private void OnTimerStop()
        {
           _ = StopParticles();
        }
        private async UniTaskVoid StopParticles()
        {
            _particles.Stop();
            await UniTask.WaitForSeconds(_particles.main.duration + 0.5f);
            Destroy(gameObject);
        }
        public void ResetTimer()
        {
            if(!_isDestroyable) _particles.Play();
            _timer?.ResetTime();
        }

        public void ResetTimer(float newDuration)
        {
            if(!_isDestroyable) _particles.Play();
            _timer?.ResetTime(newDuration);
        }

        public void RestartTimer(float newDuration)
        {
            _timer.Restart(newDuration);
        }
        
        private void Update()
        {
            _timer?.Tick(Time.deltaTime);
        }

        private void OnValidate()
        {
            if (!_particles)
            {
                _particles = GetComponent<ParticleSystem>();
            }
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            OnParticleDestroyed?.Invoke(this);
            _timer.OnTimerStop -= OnTimerStop;
        }
        
    }
}
