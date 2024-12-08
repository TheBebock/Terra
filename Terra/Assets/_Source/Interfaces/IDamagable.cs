public interface IDamagable : IHasHealth
{
    /// <summary>
    /// Whether entity can be killed
    /// </summary>
    public bool IsInvincible { get; set; }
    /// <summary>
    /// Whether entity can be damaged
    /// </summary>
    public bool CanBeDamaged { get; set; }
    /// <summary>
    /// Method for damaging entity
    /// </summary>
    public void TakeDamage(float amount);

    /// <summary>
    /// Method used on entities death
    /// </summary>
    public void OnDeath();
}
