using System;
using Core.ModifiableValue;
using UnityEngine;

namespace Terra.Combat
{
    /// <summary>
    /// Handles entities health management
    /// </summary>
    [Serializable]
    public class HealthController
    {

        [SerializeField] private ModifiableValue maxHealth;
        [SerializeField] private float currentHealth=10;

        [SerializeField] private bool canBeHealed = false;
        [SerializeField] private bool isInvincible = false;

        public bool CanBeHealed => canBeHealed;
        public bool IsInvincible => isInvincible;
        public float MaxHealth => maxHealth.Value;
        public float CurrentHealth => currentHealth;
        public bool IsDead => currentHealth <= 0;
        public event Action OnDeath;
        public event Action<bool> OnInvincibilityChanged;
        public event Action<bool> OnCanBeHealedChanged;
        public event Action<float> OnHealthChanged;
        public event Action<float> OnDamaged;
        public event Action<float> OnHealed;


        public HealthController(ModifiableValue modifiableValue, bool canBeHealed = false)
        {
            maxHealth = modifiableValue;
            currentHealth = maxHealth.Value;
            this.canBeHealed = canBeHealed;
        }
        public HealthController(float maxHealthValue, bool canBeHealed = false)
        {
            maxHealth = new ModifiableValue(maxHealthValue);
            currentHealth = maxHealth.Value;
            this.canBeHealed = canBeHealed;
        }

        public void TakeDamage(float amount)
        {
            // Change health amount
            currentHealth -= amount;


            
            // Clamp value, if invincible then set health to 1
            currentHealth = Mathf.Max(currentHealth, IsInvincible ? 1f : 0f);
            
            OnHealthChanged?.Invoke(currentHealth);
            
            if (currentHealth <= 0f) OnDeath?.Invoke();
            else OnDamaged?.Invoke(amount);
        }

        /// <summary>
        /// Heals entity
        /// </summary>
        public void Heal(float amount)
        {
            // Increase current health
            currentHealth += amount;

            // Invoke event
            OnHealed?.Invoke(amount);


            // Clamp to max health
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.Value);
            
            OnHealthChanged?.Invoke(currentHealth);


        }

        /// <summary>
        /// Change invincibility state
        /// </summary>
        public void SetInvincible(bool invincible)
        {
            // Invoke event if status is different
            if(isInvincible != invincible) 
                OnInvincibilityChanged?.Invoke(invincible);
 
            isInvincible = invincible;
 
        }

        /// <summary>
        /// Change healable state
        /// </summary>
        public void SetCanBeHealed(bool healed)
        {
            // Invoke event if status is different
            if(canBeHealed != healed) 
                OnCanBeHealedChanged?.Invoke(healed);
            
            canBeHealed = healed;
        }

        /// <summary>
        /// Resets health to max
        /// </summary>
        /// <param name="isSilent">Does not proke heal VFX when true</param>
        public void ResetHealth(bool isSilent = true)
        {
            if (isSilent)
            {
                currentHealth = MaxHealth;
                return;
            }     
            
            Heal(MaxHealth);
        }
        /// <summary>
        /// KIll target
        /// </summary>
        /// <param name="isSilent">Does not proke damage VFX when true</param>
        public void KIll(bool isSilent = false)
        {
            if (isSilent)
            {
                OnDeath?.Invoke();
                return;
            }

            TakeDamage(MaxHealth);
        }

    }
}