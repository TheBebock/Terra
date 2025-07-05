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
        [SerializeField] private int _currentHealth;

        [SerializeField] private bool _canBeHealed;

        [SerializeField] private bool _immuneAfterHit;
        private int _previousMaxHealth;
        private static float _invincibilityTimeAfterHit = 0.2f;
        private static float _invincibilityTimeAfterSpawn = 0.5f;
        public bool CanBeHealed => _canBeHealed && CurrentHealth < MaxHealth;
        public int MaxHealth => _maxHealth.Value;
        public int CurrentHealth => _currentHealth;
        public bool IsImmuneAfterHit => _immuneAfterHit;
        public float NormalizedCurrentHealth => (float)_currentHealth / _maxHealth.Value;

        public bool IsDead => _currentHealth <= 0;
        public event Action OnDeath;
        public event Action<bool> OnInvincibilityChanged;
        public event Action<bool> OnCanBeHealedChanged;
        public event Action<float> OnHealthChangedNormalized;
        public event Action<int> OnHealthChanged;
        public event Action<int> OnDamaged;
        public event Action<int> OnHealed;
        
        CancellationToken _cancellationToken;
        public HealthController(ModifiableValue modifiableValue, CancellationToken cancellationToken, bool canBeHealed = false)
        {
            _maxHealth = modifiableValue;
            _currentHealth = _maxHealth.Value;
            _previousMaxHealth = _maxHealth.Value;
            _canBeHealed = canBeHealed;
            _cancellationToken = cancellationToken;
            _maxHealth.OnValueChanged += OnMaxHealthChanged;
            _ = ImmunityTimer(_invincibilityTimeAfterSpawn);
        }
        public HealthController(int maxHealthValue, CancellationToken cancellationToken, bool canBeHealed = false)
        {
            _maxHealth = new ModifiableValue(maxHealthValue);
            _currentHealth = _maxHealth.Value;
            _previousMaxHealth = _maxHealth.Value;
            _canBeHealed = canBeHealed;
            _cancellationToken = cancellationToken;
            _maxHealth.OnValueChanged += OnMaxHealthChanged;
            
            _ = ImmunityTimer(_invincibilityTimeAfterSpawn);
        }

        /// <summary>
        ///     Damages entity
        /// </summary>
        /// <param name="amount">Amount of damage</param>
        /// <param name="isPercentage">If marked as true, <see cref="amount"/> will be treated as a percentage</param>
        /// <param name="isSilent">If marked as true, <see cref="OnDamaged"/> won't be called</param>
        public void TakeDamage(int amount, bool isPercentage = false, bool isSilent = false)
        {

            if (IsDead || IsImmuneAfterHit) return;

            int calculatedValue = CalculateValue(amount, isPercentage);

            // Change health amount
            _currentHealth -= calculatedValue;


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

            _ = ImmunityTimer(_invincibilityTimeAfterHit).AttachExternalCancellation(_cancellationToken);
        }


        /// <summary>
        ///     Heals entity
        /// </summary>
        /// <param name="amount">Heal amount</param>
        /// <param name="isPercentage">If marked as true, <see cref="amount"/> will be treated as a percentage</param>
        public void Heal(int amount, bool isPercentage = false)
        {
            int calculatedValue = CalculateValue(amount, isPercentage);
            
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
        private int CalculateValue(int amount, bool isPercentage)
        {
            amount = Mathf.Abs(amount);
            
            // flat value
            if (!isPercentage)
            {
                return amount;
            }
            // Percentage value of max health
            return Mathf.RoundToInt(MaxHealth * amount.ToFactor());
        }
        
        private void OnMaxHealthChanged(int newMaxHealth)
        {
            int delta = newMaxHealth - _previousMaxHealth;

            if (delta > 0)
            {
                _currentHealth += delta;
                OnHealed?.Invoke(delta);
            }
            else if (_currentHealth > newMaxHealth)
            {
                _currentHealth = newMaxHealth;
            }

            _previousMaxHealth = newMaxHealth;

            OnHealthChanged?.Invoke(_currentHealth);
            OnHealthChangedNormalized?.Invoke(NormalizedCurrentHealth);
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

        private async UniTask ImmunityTimer(float time)
        {
            _immuneAfterHit = true;
            await UniTask.WaitForSeconds(time, cancellationToken: _cancellationToken);
            _immuneAfterHit = false;
        }

    }
}