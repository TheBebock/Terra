using System;
using Terra.Combat;
using Terra.Extensions;
using UnityEngine;

namespace Terra.LootSystem.AirDrop
{
    public class CrateLanding : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private LayerMask _setToLayerAfterHit;
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
        }
    }
}