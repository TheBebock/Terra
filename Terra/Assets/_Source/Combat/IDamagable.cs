using Terra.Combat;

public interface IDamagable
{
    /// <summary>
    /// Whether entity can be killed
    /// </summary>
    public bool IsInvincible { get; }
    /// <summary>
    /// Whether entity can be damaged
    /// </summary>
    public bool CanBeDamaged { get; }
    /// <summary>
    /// Method for damaging entity
    /// </summary>
    public void TakeDamage(float amount, bool isPercentage = false);
    
    public HealthController HealthController { get; }
    
    /// <summary>
    /// Method used on entities death
    /// </summary>
    public void OnDeath();
}
