public interface IInitializable 
{
    public void Initialize();

    public bool IsInitialized { get; }
}
