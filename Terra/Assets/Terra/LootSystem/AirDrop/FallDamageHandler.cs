using Terra.Combat;
using Terra.Core.Generics;
using Terra.Interfaces;
using UnityEngine;

namespace Terra.LootSystem.AirDrop
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Entity))]
    public class AirdropDamageHandler : MonoBehaviour
    {
        [Header("Fall Damage")]
        [SerializeField] private float fallSpeedThreshold = -10f;
        [SerializeField] private float selfDamageMultiplier = 2f;

        [Header("Collision Damage")]
        [SerializeField] private float collisionDamage = 15f;
        [SerializeField] private LayerMask damageLayers;

        private Rigidbody _rb;
        private Entity _entity;
        private IDamageable _selfDamageable;

        private float _highestFallSpeed;
        private bool _wasGroundedLastFrame;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _entity = GetComponent<Entity>();
            _selfDamageable = GetComponent<IDamageable>();
        }

        void Update()
        {
            bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

            if (isGrounded)
            {
                if (!_wasGroundedLastFrame && _highestFallSpeed < fallSpeedThreshold)
                {
                    float selfDamage = Mathf.Abs(_highestFallSpeed) * selfDamageMultiplier;
                    if (_selfDamageable != null)
                        _selfDamageable.TakeDamage(selfDamage);
                }

                _highestFallSpeed = 0f;
            }
            else
            {
                _highestFallSpeed = Mathf.Min(_highestFallSpeed, _rb.velocity.y);
            }

            _wasGroundedLastFrame = isGrounded;
        }

        void OnCollisionEnter(Collision collision)
        {
            // Sprawdź warstwę
            if (((1 << collision.gameObject.layer) & damageLayers) == 0)
                return;

            IDamageable other = collision.gameObject.GetComponent<IDamageable>();
            if (other != null)
            {
                other.TakeDamage(collisionDamage);

                if (_selfDamageable != null)
                {
                    _selfDamageable.TakeDamage(collisionDamage);
                }
            }
            _rb.velocity = Vector3.zero;
            _rb.isKinematic = true;
        }
        void OnTriggerEnter(Collider other)
        {
            // Sprawdź warstwę
            if (((1 << other.gameObject.layer) & damageLayers) == 0)
                return;

            IDamageable otherDamageable = other.GetComponent<IDamageable>();
            if (otherDamageable != null)
            {
                otherDamageable.TakeDamage(collisionDamage);

                if (_selfDamageable != null)
                {
                    _selfDamageable.TakeDamage(collisionDamage);
                }
            }
        }
    }
}
