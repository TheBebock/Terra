using System;
using NaughtyAttributes;
using UnityEngine;

namespace Terra.LootSystem.AirDrop
{
    public class FlareLandingNotifier : MonoBehaviour
    {
        public Action OnLanded;

        private bool _hasLanded = false;
        public bool HasLanded => _hasLanded;
        
        [SerializeField, ReadOnly] Rigidbody _rigidbody;

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Flare collided with: {collision.gameObject.name}, layer: {collision.gameObject.layer}");

            if (!_hasLanded && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("Flare landed on ground.");
                _hasLanded = true;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
                _rigidbody.useGravity = false;
                _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                OnLanded?.Invoke();
            }
        }

        private void OnValidate()
        {
            if(!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        }
    }
}