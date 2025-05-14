using Terra.EffectsSystem;

namespace Terra.Combat
{
    /// <summary>
    /// Marks class as a damageable object
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        ///     Whether entity can be killed
        /// </summary>
        public bool IsInvincible { get; }

        /// <summary>
        /// Whether entity can be damaged
        /// </summary>
        public bool CanBeDamaged { get; }

        /// <summary>
        ///     Method for damaging entity
        /// </summary>
        public void TakeDamage(float amount, bool isPercentage = false);

        /// <summary>
        ///     Instantly kills entity
        /// </summary>
        /// <param name="isSilent">If marked as true, does not provoke OnDamaged</param>
        public void Kill(bool isSilent = true);
        
        public HealthController HealthController { get; }
        
        public StatusContainer StatusContainer { get; }
        
        /// <summary>
        ///     Method used on entities death
        /// </summary>
        /// <remarks>
        ///     Method should be implemented explicitly, to avoid it being public
        /// </remarks>
        void OnDeath();
    }
}