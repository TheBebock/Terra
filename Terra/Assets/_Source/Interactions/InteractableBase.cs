using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : InGameMonoBehaviour, IInteractable
{
    public abstract bool CanBeInteractedWith { get; }
    public virtual bool CanShowVisualisation { get; set; }
    public abstract void Interact();
    public abstract void OnInteraction();
    
    public virtual string GetInteractionPrompt()
    {
        return "Press E to interact";  // Default text
    }
    
    public void ShowVisualisation()
    {
        if(!CanShowVisualisation) return;
        if (CanBeInteractedWith)
        {
            ShowAvailableVisualization();

        }
        else
        {
            ShowUnAvailableVisualization();
        }
    }

    /// <summary>
    /// Shown when is available
    /// </summary>
    protected virtual void ShowAvailableVisualization()
    {
        
    }
    
    /// <summary>
    /// Shown when interaction is not available
    /// </summary>
    protected virtual void ShowUnAvailableVisualization()
    {
        
    }

    public virtual void StopVisualization()
    {
        CanShowVisualisation = false;
    }

    
}