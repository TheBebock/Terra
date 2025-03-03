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
    public Transform itemHoldPoint;
    public TextMeshProUGUI interactionPromptText;
    public Image interactionPromptBackground;
   
    [Header("Item Inventory")]
    public GameObject currentMeleeWeapon;
    public GameObject currentRangedWeapon;
    public GameObject currentActiveItem;
    
    [Header("Combat Settings")]
    public float meleeDamage = 25f;
    public float rangedDamage = 15f;
    public float meleeAttackRange = 2f;
    public float rangedAttackRange = 50f;
    
    private PlayerMovement _playerMovement;
    private IInteractable currentInteractable;
    private float attackCooldownTimer = 0f;
    private float attackCooldown = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        currentInteractable = GetComponent<IInteractable>();
        
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
        
        if (interactionPromptBackground != null)
        {
            interactionPromptBackground.gameObject.SetActive(false);
        }
        
        // Initialize player weapons references
        _playerMovement.meleeWeapon = currentMeleeWeapon;
        _playerMovement.rangedWeapon = currentRangedWeapon;
        _playerMovement.activeItem = currentActiveItem;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForInteractables();
        HandleInteractionInput();
        //HandleCombatInput();
        //HandleEquipmentInput();
        
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
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
        if (currentInteractable != null && currentInteractable.CanBeInteractedWith)
        {
            Debug.Log("Interacting with: " + currentInteractable);
            currentInteractable.Interact();
            currentInteractable.OnInteraction();
            
            // Also check if the object is pickupable
            GameObject interactableObject = currentInteractable is MonoBehaviour monoBehaviourComponent ? monoBehaviourComponent.gameObject : null;
            
            if (interactableObject != null)
            {
                IPickupable pickupable = interactableObject.GetComponent<IPickupable>();
                if (pickupable != null && pickupable.CanBePickedUp)
                {
                    pickupable.PickUp();
                }
            }
        }
        else
        {
            Debug.Log("No valid interactable found.");
        }
    }
    
}
