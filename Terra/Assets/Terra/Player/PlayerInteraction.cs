using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerInteraction : InGameMonobehaviour, IAttachListeners
    {
        [Header("Interaction Settings")] 
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactionLayer;
    

        private IInteractable _currentInteractable;


        private List<IInteractable> _nearbyInteractables = new List<IInteractable>();
    
    
        private void Update()
        {
            UpdateNearestInteractable();
        }

        private void AddInteractable(IInteractable interactable)
        {
            if (interactable != null && !_nearbyInteractables.Contains(interactable))
            {
                _nearbyInteractables.Add(interactable);
                UpdateNearestInteractable();
            }
        }

        private void RemoveInteractable(IInteractable interactable)
        {
            if (interactable != null && _nearbyInteractables.Contains(interactable))
            {
                _nearbyInteractables.Remove(interactable);

                // If deleted object was the one chosesn
                if (_currentInteractable == interactable)
                {
                    _currentInteractable = null;
                }

                UpdateNearestInteractable();
            }
        }

        private void UpdateNearestInteractable()
        {
            if (_nearbyInteractables.Count == 0)
            {
                _currentInteractable = null;
                return;
            }

            // Find the nearest object 
            float closestDistance = float.MaxValue;
            IInteractable closestInteractable = null;

            foreach (var interactable in _nearbyInteractables)
            {
                if (!interactable.CanBeInteractedWith) continue;

                // Get the GameObject of the interactable
                GameObject interactableObj = (interactable as MonoBehaviour)?.gameObject;
                if (interactableObj == null) continue;

                float distance = Vector3.Distance(transform.position, interactableObj.transform.position);

                // Check if object is within interaction distance
                if (distance <= interactionDistance && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }

            // when new object is found
            if (closestInteractable != null && closestInteractable != _currentInteractable)
            {
                _currentInteractable = closestInteractable;
            }
            else if (closestInteractable == null)
            {
                _currentInteractable = null;
            }
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if(context.performed) Interact();
        }

        private void Interact()
        {
            if (_currentInteractable != null && _currentInteractable.CanBeInteractedWith)
            {
                Debug.Log("Interakcja z: " + _currentInteractable);
                _currentInteractable.Interact();

                GameObject interactableObject = (_currentInteractable as MonoBehaviour)?.gameObject;

                if (interactableObject != null)
                {
                    IPickupable pickupable = interactableObject.GetComponent<IPickupable>();
                    if (pickupable != null && pickupable.CanBePickedUp)
                    {
                        pickupable.PickUp();
                        RemoveInteractable(_currentInteractable);
                    }
                }
            }
            else
            {
                Debug.Log("Nie znaleziono prawidłowego obiektu interaktywnego.");
            }
        }

        public void AttachListeners()
        {
            InputManager.Instance.PlayerControls.Interaction.performed += OnInteract;
        }

        public void DetachListeners()
        {
            if(InputManager.Instance)
                InputManager.Instance.PlayerControls.Interaction.performed += OnInteract;
        }
    }
}