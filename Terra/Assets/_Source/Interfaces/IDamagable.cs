public interface IDamagable
{
    public bool IsInvincible { get; set; }
    public bool IsDamagable { get; set; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public void TakeDamage(float damage);
}
