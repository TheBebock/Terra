using Core.ModifiableValue;
using Terra.Combat;
using UnityEngine;

namespace Terra.Environment
{

    /// <summary>
    /// Represents object in game world that can be damaged
    /// </summary>
    public class DamageableObject : InteractableBase, IDamagable, IInitializable
    {
        [SerializeField] private ModifiableValue maxHealth;

        private HealthController _healthController;
        public bool IsInvincible => _healthController.IsInvincible;
        public bool CanBeDamaged { get; }
        public override bool CanBeInteractedWith { get; }

        public bool IsInitialized { get; set; }

        public HealthController HealthController => _healthController;


        public void Initialize()
        {
            _healthController = new HealthController(maxHealth);
            _healthController.OnDeath += OnDeath;
        }

        public void TakeDamage(float amount)
        {
            if (!CanBeDamaged) return;
            _healthController.TakeDamage(amount);
            // Show VFX
        }


        public virtual void OnDeath()
        {
            // TODO: Show VFX

            Destroy(gameObject);

        }

        public override void Interact()
        {
            // DO nothing
        }

        public override void OnInteraction()
        {
            //DO nothing
        }
    }
}
