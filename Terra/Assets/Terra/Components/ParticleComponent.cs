using System;
using NaughtyAttributes;
using Terra.Utils;
using UnityEngine;

namespace Terra.Components
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleComponent : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [Foldout("Debug")] [SerializeField] private ParticleSystem _particles;
        private CountdownTimer _timer;
        
        public static event Action<ParticleComponent> OnParticleDestroyed;
        public void Initialize(float newDuration = 0f)
        {
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
            _timer.OnTimerStop += () => Destroy(gameObject);
        }
        
        public void ResetTimer() => _timer.Reset();
        public void ResetTimer(float newDuration) => _timer.Reset(newDuration);

        private void Update()
        {
            _timer?.Tick(Time.deltaTime);
        }

        private void OnDestroy()
        {
            OnParticleDestroyed?.Invoke(this);
        }
    }
}
