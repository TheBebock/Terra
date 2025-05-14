

using Terra.Core.Generics;
using Terra.Interfaces;

namespace Terra.Interactions
{
    /// <summary>
    /// Represents base class for object that can be interacted with
    /// </summary>
    public abstract class InteractableBase : Entity, IInteractable
    {
        public abstract bool CanBeInteractedWith { get; protected set; }
        public virtual bool CanShowVisualisation { get; set; }


        public abstract void OnInteraction();
    
        public virtual string GetInteractionPrompt()
        {
            return "Press E to interact";  // Default text
        }
    
    
        public void Interact()
        {
            if(!CanBeInteractedWith) return;

            StopVisualization();

            // Show VFX
            // Play sound
            OnInteraction();
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