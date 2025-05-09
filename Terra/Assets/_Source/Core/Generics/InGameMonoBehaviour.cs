using Terra.ID;
using Terra.Interfaces;
using UnityEngine;


/// <summary>
/// Class that implements custom initialization pipeline
/// </summary>
public abstract class InGameMonobehaviour : MonoBehaviour, IInitializable
{
    public bool IsInitialized { get; private set; }
    
    private void Start()
    {
        if(IsInitialized) return;
        
        IsInitialized = true;
        Initialize();
        
    }

    public void Initialize()
    {
        if(this is IWithSetUp setup)
            setup.SetUp();
 
        if(this is IAttachListeners attachListeners)
            attachListeners.AttachListeners();
        if(this is IUniqueable uniqueable)
            uniqueable.RegisterID();
    }
    
    protected virtual void CleanUp(){}
    private void OnDestroy()
    {
        if(this is IAttachListeners attachListeners)
            attachListeners.DetachListeners();
        
        CleanUp();
        
        if(this is IWithSetUp setup)
            setup.TearDown();
        
        if(this is IUniqueable uniqueable)
            uniqueable.ReturnID();
    }




}
