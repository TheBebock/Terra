using Terra.Components;
using UnityEngine;


/// <summary>
/// Class that represents object inside the game world
/// </summary>
[RequireComponent(typeof(LookAtCameraComponent))]
public abstract class InGameMonoBehaviour : MonoBehaviour
{
    private void Awake()
    {
        if (this is IInitializable initializable)
        {
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
