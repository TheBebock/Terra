using Terra.Core.Generics;
using Terra.Interfaces;
using UnityEngine;

namespace Terra.Interactions
{
    /// <summary>
    /// Represents base class for object that can be interacted with
    /// </summary>
    public abstract class InteractableBase : Entity, IInteractable
    {
        [SerializeField] private bool _canBeInteractedWith;
        public virtual bool CanBeInteractedWith => _canBeInteractedWith;
        public virtual bool CanShowVisualisation { get; set; }


        
        public abstract void OnInteraction();
    
        public void Interact()
        {
            if(!CanBeInteractedWith) return;

            StopVisualization();

            // Show VFX
            // Play sound
            OnInteraction();
        }

        public void ChangeInteractibility(bool canBeInteractedWith)
        {
            _canBeInteractedWith = canBeInteractedWith;
            OnInteractableStateChanged(canBeInteractedWith);
        }
        protected virtual void OnInteractableStateChanged(bool interactableState)
        {
            
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
}