namespace Terra.Combat
{
    /// <summary>
    /// Marks class as a healable
    /// </summary>
    public interface IHealable 
    {
        /// <summary>
        /// Heal entity by given amount
        /// </summary>
        public void Heal(float amount, bool isPercentage = false);
    }
}
