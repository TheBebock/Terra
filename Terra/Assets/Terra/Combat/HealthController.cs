using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Terra.Core.ModifiableValue;
using Terra.Extensions;
using UnityEngine;

namespace Terra.Combat
{
    /// <summary>
    /// Handles entities health management
    /// </summary>
    [Serializable]
    public class HealthController
    {

        [SerializeField] private ModifiableValue _maxHealth;
        [SerializeField] private float _currentHealth;

        [SerializeField] private bool _canBeHealed;
        [SerializeField] private bool _isInvincible;

        [SerializeField] private bool _immuneAfterHit;
        
        private static float _invincibilityTimeAfterHit = 0.2f;
        public bool CanBeHealed => _canBeHealed;
        public bool IsInvincible => _isInvincible;
        public float MaxHealth => _maxHealth.Value;
        public float CurrentHealth => _currentHealth;
        public bool IsImmuneAfterHit => _immuneAfterHit;
        public float NormalizedCurrentHealth => _currentHealth / _maxHealth.Value;

        public bool IsDead => _currentHealth <= 0;
        public event Action OnDeath;
        public event Action<bool> OnInvincibilityChanged;
        public event Action<bool> OnCanBeHealedChanged;
        public event Action<float> OnHealthChangedNormalized;
        public event Action<float> OnHealthChanged;
        public event Action<float> OnDamaged;
        public event Action<float> OnHealed;
        
        CancellationToken _cancellationToken;
        public HealthController(ModifiableValue modifiableValue, CancellationToken cancellationToken, bool canBeHealed = false)
        {
            _maxHealth = modifiableValue;
            _currentHealth = _maxHealth.Value;
            _canBeHealed = canBeHealed;
            _cancellationToken = cancellationToken;
        }
        public HealthController(float maxHealthValue, CancellationToken cancellationToken, bool canBeHealed = false)
        {
            _maxHealth = new ModifiableValue(maxHealthValue);
            _currentHealth = _maxHealth.Value;
            _canBeHealed = canBeHealed;
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        ///     Damages entity
        /// </summary>
        /// <param name="amount">Amount of damage</param>
        /// <param name="isPercentage">If marked as true, <see cref="amount"/> will be treated as a percentage</param>
        /// <param name="isSilent">If marked as true, <see cref="OnDamaged"/> won't be called</param>
        public void TakeDamage(float amount, bool isPercentage = false, bool isSilent = false)
        {

            if (IsDead || IsImmuneAfterHit) return;

            float calculatedValue = CalculateValue(amount, isPercentage);

            // Change health amount
            _currentHealth -= calculatedValue;

            // Clamp value, if invincible then set health to 1
            _currentHealth = Mathf.Max(_currentHealth, IsInvincible ? 1f : 0f);


            OnHealthChanged?.Invoke(_currentHealth);
            OnHealthChangedNormalized?.Invoke(NormalizedCurrentHealth);

            if (_currentHealth <= 0f)
            {
                OnDeath?.Invoke();
                return;
            }

            if (!isSilent)
            {
                OnDamaged?.Invoke(calculatedValue);
            }

            _ = ImmunityTimer().AttachExternalCancellation(_cancellationToken);
        }


        /// <summary>
        ///     Heals entity
        /// </summary>
        /// <param name="amount">Heal amount</param>
        /// <param name="isPercentage">If marked as true, <see cref="amount"/> will be treated as a percentage</param>
        public void Heal(float amount, bool isPercentage = false)
        {
            float calculatedValue = CalculateValue(amount, isPercentage);
            
            // Increase current health
            _currentHealth += calculatedValue;

            // Clamp to max health
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth.Value);
            
            // Invoke event
            OnHealed?.Invoke(calculatedValue);
            OnHealthChanged?.Invoke(_currentHealth);
            OnHealthChangedNormalized?.Invoke(NormalizedCurrentHealth);
        }

        /// <summary>
        ///     Returns calculated value, either flat of percentage of max health
        /// </summary>
        private float CalculateValue(float amount, bool isPercentage)
        {
            amount = Mathf.Abs(amount);
            
            // flat value
            if (!isPercentage)
            {
                return amount;
            }
            // Percentage value of max health
            return MaxHealth * amount.ToFactor();
        }
        /// <summary>
        ///     Change invincibility state
        /// </summary>
        public void SetInvincible(bool invincible)
        {
            // Invoke event if status is different
            if(_isInvincible != invincible) 
                OnInvincibilityChanged?.Invoke(invincible);
 
            _isInvincible = invincible;
 
        }

        /// <summary>
        /// Change healable state
        /// </summary>
        public void SetCanBeHealed(bool healed)
        {
            // Invoke event if status is different
            if(_canBeHealed != healed) 
                OnCanBeHealedChanged?.Invoke(healed);
            
            _canBeHealed = healed;
        }

        /// <summary>
        /// Resets health to max
        /// </summary>
        /// <param name="isSilent">Does not proke heal VFX when true</param>
        public void ResetHealth(bool isSilent = true)
        {
            if (isSilent)
            {
                _currentHealth = MaxHealth;
                return;
            }     
            
            Heal(MaxHealth);
        }
        /// <summary>
        /// KIll target
        /// </summary>
        /// <param name="isSilent">Does not proke damage VFX when true</param>
        public void Kill(bool isSilent = false)
        {
            TakeDamage(MaxHealth, isSilent: isSilent);
        }

        private async UniTask ImmunityTimer()
        {
            _immuneAfterHit = true;
            await UniTask.WaitForSeconds(_invincibilityTimeAfterHit, cancellationToken: _cancellationToken);
            _immuneAfterHit = false;
        }

    }
}