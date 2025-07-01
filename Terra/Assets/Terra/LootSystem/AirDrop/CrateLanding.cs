using NaughtyAttributes;
using Terra.Combat;
using Terra.Components;
using Terra.Extensions;
using Terra.Managers;
using Terra.Particles;
using UnityEngine;

namespace Terra.LootSystem.AirDrop
{
    public class CrateLanding : MonoBehaviour
    {
        [SerializeField] private AudioClip _dropSound;
        [SerializeField] private LayerMask _setToLayerAfterHit;
        [SerializeField, ReadOnly] private AudioSource _audioSource;
        [SerializeField, ReadOnly] private Rigidbody _rb;
        [SerializeField] private ParticleComponent _particles;
        private AirdropDamageHandler _damageHandler;
        private IDamageable _selfDamageable;
        private bool _isHit;
        
        private void Awake()
        { 
            _damageHandler = GetComponentInChildren<AirdropDamageHandler>();           
            _selfDamageable = GetComponent<IDamageable>();

            if (!_damageHandler)
            {
                Debug.LogError($"{nameof(AirdropDamageHandler)} not found as a child of a {gameObject.name}");
            }
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Crate collided with: {collision.gameObject.name}");

            if (collision.gameObject.CompareTag("Flare"))
            {
                Debug.Log("Crate hit flare — destroying flare.");
                Destroy(collision.gameObject);
            }
            _rb.velocity = Vector3.zero;
            _rb.isKinematic = true;
            Destroy(_damageHandler.gameObject);
            
            if (_isHit)
            {
                _selfDamageable.Kill(false);
            }
            else
            {
                gameObject.SetLayer(_setToLayerAfterHit, true);
                if (_dropSound) AudioManager.Instance.PlaySFXAtSourceOnce(_dropSound, _audioSource);
                if (_particles) VFXController.SpawnParticleInWorld(_particles, transform.position, Quaternion.Euler(-90,0,0));
            }

            Destroy(this);
        }

        public void ObjectHitSomething()
        {
            _isHit = true;
        }
        private void OnValidate()
        {
            if(!_rb) _rb = GetComponent<Rigidbody>();
            if(!_audioSource) _audioSource = GetComponent<AudioSource>();
        }
    }
}