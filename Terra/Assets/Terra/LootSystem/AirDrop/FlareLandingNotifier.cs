using System;
using NaughtyAttributes;
using Terra.Components;
using Terra.Core.Generics;
using UnityEngine;

namespace Terra.LootSystem.AirDrop
{
    public class FlareLandingNotifier : InGameMonobehaviour
    {
        public Action onLanded;

        private bool _hasLanded;
        public bool HasLanded => _hasLanded;

        [SerializeField] private float _smokeDurationAfterHittingGround = 6f;
        [SerializeField, ReadOnly] private Rigidbody _rigidbody;
        [SerializeField, ReadOnly] private ParticleComponent _particles;
        
        private void Awake()
        {
            _particles.Initialize();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_hasLanded && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _hasLanded = true;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
                _rigidbody.useGravity = false;
                _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                onLanded?.Invoke();
                
                DeAttachParticles();
            }
        }

        private void DeAttachParticles()
        {
             _particles.transform.parent = null;
             _particles.RestartTimer(_smokeDurationAfterHittingGround);
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
            
            if(!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
            if(!_particles) _particles = GetComponentInChildren<ParticleComponent>(); 
        }

#endif
       
    }
}