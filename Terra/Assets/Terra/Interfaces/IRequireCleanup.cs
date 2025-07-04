
namespace Terra.Interfaces
{
    /// <summary>
    /// Marks an object to be cleaned up at the end of the floor
    /// </summary>
    public interface IRequireCleanup
    {
        public void PerformCleanup();
    }
}
