using UnityEngine;


/// <summary>
/// Class that implements custom initialization pipeline
/// </summary>
public abstract class InGameMonobehaviour : MonoBehaviour
{
    private void Awake()
    {
        if (this is IInitializable initializable)
        {
            if(initializable.IsInitialized) return;
            initializable.Initialize();
            initializable.IsInitialized = true;
        }
    }

    private void Start()
    {
        if(this is IWithSetUp setup)
            setup.SetUp();
 
        if(this is IAttachListeners attachListeners)
            attachListeners.AttachListeners();
    }

    
    protected virtual void CleanUp(){}
    private void OnDestroy()
    {
        CleanUp();
        
        if(this is IAttachListeners attachListeners)
            attachListeners.DetachListeners();
        
        if(this is IWithSetUp setup)
            setup.TearDown();
    }
}
