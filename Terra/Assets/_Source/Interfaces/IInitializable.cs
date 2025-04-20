
/// <summary>
/// Interface for classes that require additional initialization at Awake
/// </summary>
public interface IInitializable 
{
    public void Initialize();

    public bool IsInitialized { get;}
}
