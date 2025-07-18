using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Extensions;
using Terra.InputSystem;
using Terra.Interactions;
using Terra.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Player
{
    public class PlayerInteraction : InGameMonobehaviour, IAttachListeners
    {
        [Foldout("Debug"), ReadOnly][SerializeField] private InteractableBase _currentInteractable;
        [Foldout("Debug"), ReadOnly][SerializeField] private List<InteractableBase> _nearbyInteractables = new();
    
        public void AttachListeners()
        {
            InputsManager.Instance.PlayerControls.Interaction.performed += OnInteract;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter: {other.name}");
            if (other.TryGetComponent(out IInteractable interactable))
            {
                _nearbyInteractables.AddUnique(interactable as InteractableBase);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                _nearbyInteractables.RemoveElement(interactable as InteractableBase);
            }
        }

        private void Update()
        {
            UpdateNearestInteractable();
        }

        private void UpdateNearestInteractable()
        {
            if (_nearbyInteractables.Count == 0)
            {
                _currentInteractable?.StopVisualization();
                _currentInteractable = null;
                return;
            }

            // Find the nearest object 
            float closestDistance = float.MaxValue;
            InteractableBase closestInteractable = null;

            foreach (var interactable in _nearbyInteractables)
            {
                if (!interactable.CanBeInteractedWith) continue;

                float distance = Vector3.Distance(transform.position, interactable.transform.position);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }

            if (closestInteractable != _currentInteractable)
            {
                ChangeCurrentInteractable(closestInteractable);
            }
        }

        private void ChangeCurrentInteractable(InteractableBase newInteractable)
        {
            _currentInteractable?.StopVisualization();
            _currentInteractable = newInteractable;
            _currentInteractable?.ShowVisualization();
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            Interact();
        }

        private void Interact()
        {
            if (_currentInteractable != null && _currentInteractable.CanBeInteractedWith)
            {
                Debug.Log("Interakcja z: " + _currentInteractable);
                _currentInteractable.Interact();
                _nearbyInteractables.Remove(_currentInteractable);
                _currentInteractable = null;
            }
            else
            {
                Debug.Log("Nie znaleziono prawidłowego obiektu interaktywnego.");
            }
        }
        

        public void DetachListeners()
        {
            if(InputsManager.Instance)
                InputsManager.Instance.PlayerControls.Interaction.started += OnInteract;
        }
    }
}