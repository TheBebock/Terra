using System;
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

        [ReadOnly] public string Identifier = string.Empty;
        [Tooltip("If the particle is played often, this shouldn't be marked, for example: 'OnHit' is played often")]
        [SerializeField] private bool _isDestroyable;
        [Tooltip("If marked, after the timer runs out, particles are instantly destroyed, only Stopped and destroyed after their main duration")]
        [SerializeField] private bool _fadeParticlesAfterDuration;
        [Tooltip("How long to play the particles. -1 is infinity")]
        [ShowIf(nameof(ShowParticlesDuration))][SerializeField] private float _duration;
        [Foldout("Debug")] [SerializeField] private ParticleSystem _particles;
        [Foldout("Debug")] [SerializeField] private CountdownTimer _timer;
        
        public static event Action<ParticleComponent> OnParticleDestroyed;
        
        public ParticleSystem ParticleSystem => _particles;
        public float DestroyDuration => _duration;
        public bool IsDestroyable => _isDestroyable;
        public bool IsPlaying => _particles.isPlaying;
        public bool IsLooped => _particles.main.loop;
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
            _timer.OnTimerStop += OnTimerStop;
        }

        private void OnTimerStop()
        {
           _ = StopParticles();
        }
        private async UniTaskVoid StopParticles()
        {
            if (_fadeParticlesAfterDuration)
            {
                _particles.Stop();
                await UniTask.WaitForSeconds(_particles.main.duration + 0.5f);
            }
            if(_isDestroyable) Destroy(gameObject);
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

        public void KillParticles()
        {
            _timer.Stop();
        }
        private void Update()
        {
            _timer?.Tick(Time.deltaTime);
        }

        private bool ShowParticlesDuration => _particles.main.loop;
        
        
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (!_particles)
            {
                _particles = GetComponent<ParticleSystem>();
            }
            
            Identifier = gameObject.name;
        }
#endif

        protected override void CleanUp()
        {
            base.CleanUp();
            OnParticleDestroyed?.Invoke(this);
            if(_timer != null ) _timer.OnTimerStop -= OnTimerStop;
        }
        
    }
}
