using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface IInteractable
{
    
    /// <summary>
    /// Checks if available for interaction
    /// </summary>
    public bool CanBeInteractedWith { get; }
    /// <summary>
    /// Called on interaction
    /// </summary>
    public void Interact();

    /// <summary>
    /// Called after interaction
    /// </summary>
    public void OnInteraction();

    /// <summary>
    /// Called when is ready to be interacted with by Player
    /// </summary>
    public void ShowVisualisation();
    /// <summary>
    /// Shown when interaction is not available
    /// </summary>
    [UsedImplicitly]public void ShowAvailableVisualization();
    
    /// <summary>
    /// Shown when is available
    /// </summary>
    [UsedImplicitly]public void ShowUnAvailableVisualization();

    /// <summary>
    /// Stop all visualisation
    /// </summary>
    public void StopVisualization();

}
