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

        [SerializeField] private InteractionIcon _interactionIconObject;
        [SerializeField] private bool _showInteractionIconWithoutPlayerNearby;
        [SerializeField] private bool _canBeInteractedWith;
        public virtual bool CanBeInteractedWith => _canBeInteractedWith;
        
        public abstract void OnInteraction();
        
    
        public void Interact()
        {
            if(!CanBeInteractedWith) return;

            StopVisualization();
            
            OnInteraction();
        }

        public void ChangeInteractibility(bool canBeInteractedWith)
        {
            _canBeInteractedWith = canBeInteractedWith;
            OnInteractableStateChanged(canBeInteractedWith);
        }
        protected virtual void OnInteractableStateChanged(bool interactableState)
        {
            if (interactableState == false)
            {
                StopVisualization(true);
            }
            else
            {
                if(_showInteractionIconWithoutPlayerNearby) ShowVisualization();
            }
        }
        
        public void ShowVisualization()
        {
            if (CanBeInteractedWith)
            {
                _interactionIconObject.gameObject.SetActive(true);
                OnShowVisualization();
            }
        }

        protected virtual void OnShowVisualization() { }
        public virtual void StopVisualization(bool force = false)
        {
            if (force)
            {
                _interactionIconObject.gameObject.SetActive(false);
                OnStopVisualization();
                return;
            }

            if (_showInteractionIconWithoutPlayerNearby == false)
            {
                _interactionIconObject.gameObject.SetActive(false);
                OnStopVisualization();
            }
        }
        
        protected virtual void OnStopVisualization() { }
    }
}