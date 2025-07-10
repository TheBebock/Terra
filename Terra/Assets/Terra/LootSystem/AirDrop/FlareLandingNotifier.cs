using System;
using NaughtyAttributes;
using Terra.Components;
using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Particles;
using UnityEngine;

namespace Terra.LootSystem.AirDrop
{
    public class FlareLandingNotifier : InGameMonobehaviour
    {
        public Action onLanded;

        private bool _hasLanded;
        public bool HasLanded => _hasLanded;

        [SerializeField] private float _smokeDurationAfterHittingGround = 6f;
        [SerializeField, ReadOnly] private VFXController _vfxController;
        [SerializeField, ReadOnly] private LightComponent _light;
        [SerializeField, ReadOnly] private Rigidbody _rigidbody;
        [SerializeField, ReadOnly] private ParticleComponent _particles;
        [SerializeField, ReadOnly] private Color _color;

        private void Awake()
        {
            _particles.Initialize();
        }

        public void Init(Color color)
        {
            _color = color;
            _vfxController.SetModelColor(_color);
            _light.SetColor(_color);
            _particles.SetParticlesStartColor(_color);
            _particles.ParticleSystem.Stop();
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
                _particles.ParticleSystem.Play();
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
            
            if(!_vfxController) _vfxController = gameObject.GetComponent<VFXController>();
            if(!_light) _light = gameObject.GetComponentInChildren<LightComponent>();
            if(!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
            if(!_particles) _particles = GetComponentInChildren<ParticleComponent>(); 
        }

#endif

    }
}