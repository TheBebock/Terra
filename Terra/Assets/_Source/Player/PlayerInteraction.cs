using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Terra.Player;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    
    public TextMeshProUGUI interactionPromptText;
    public Image interactionPromptBackground;
    
    private PlayerMovement _playerMovement;
    private IInteractable _currentInteractable;
    private float _attackCooldownTimer = 0f;
    private float _attackCooldown = 0.5f;
    
    private List<IInteractable> _nearbyInteractables = new List<IInteractable>();
    
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
        
        if (interactionPromptBackground != null)
        {
            interactionPromptBackground.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Sprawdź najbliższy interaktywny obiekt
        UpdateNearestInteractable();
        HandleInteractionInput();
        
        if (_attackCooldownTimer > 0)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }
    }
    
    // Method to find new interaction
    public void AddInteractable(IInteractable interactable)
    {
        if (interactable != null && !_nearbyInteractables.Contains(interactable))
        {
            _nearbyInteractables.Add(interactable);
            UpdateNearestInteractable();
        }
    }
    
    // Method to delete interactions
    public void RemoveInteractable(IInteractable interactable)
    {
        if (interactable != null && _nearbyInteractables.Contains(interactable))
        {
            _nearbyInteractables.Remove(interactable);
            
            // If deleted object was the one chosesn
            if (_currentInteractable == interactable)
            {
                _currentInteractable = null;
                
                if (interactionPromptText != null)
                {
                    interactionPromptText.gameObject.SetActive(false);
                }
                
                if (interactionPromptBackground != null)
                {
                    interactionPromptBackground.gameObject.SetActive(false);
                }
            }
            
            UpdateNearestInteractable();
        }
    }
    
    private void UpdateNearestInteractable()
{
    if (_nearbyInteractables.Count == 0)
    {
        _currentInteractable = null;
        
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
        
        if (interactionPromptBackground != null)
        {
            interactionPromptBackground.gameObject.SetActive(false);
        }
        
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
        
        if (interactionPromptText != null)
        {
            string promptText = _currentInteractable.GetInteractionPrompt();
            if (string.IsNullOrEmpty(promptText))
            {
                promptText = "Naciśnij E, aby wejść w interakcję";
            }
            
            interactionPromptText.text = promptText;
            interactionPromptText.gameObject.SetActive(true);
            
            if (interactionPromptBackground != null)
            {
                interactionPromptBackground.gameObject.SetActive(true);
            }
        }
    }
    else if (closestInteractable == null)
    {
        _currentInteractable = null;
        
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
        
        if (interactionPromptBackground != null)
        {
            interactionPromptBackground.gameObject.SetActive(false);
        }
    }
}
    
    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E - interaction
        {
            Interact();
        }
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
}