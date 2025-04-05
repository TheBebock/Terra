/// <summary>
/// Marks class as a healable
/// </summary>
public interface IHealable 
{
    /// <summary>
    /// Heal entity by given amount
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(float amount);
}
