using System;
using NaughtyAttributes;
using UnityEngine;

namespace Terra.LootSystem.AirDrop
{
    public class FlareLandingNotifier : MonoBehaviour
    {
        public Action onLanded;

        private bool _hasLanded;
        public bool HasLanded => _hasLanded;
        
        [SerializeField, ReadOnly] Rigidbody _rigidbody;

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
            }
        }

        private void OnValidate()
        {
            if(!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        }
    }
}