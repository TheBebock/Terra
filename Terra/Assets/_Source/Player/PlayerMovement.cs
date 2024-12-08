using System.Collections;
using System.Collections.Generic;
using _Source.Managers;
using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 6f; 
    public float gravity = 10f; 
    public float dashSpeed = 20f; 
    public float dashDuration = 0.2f; 
    public float dashCooldown = 1f; 

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    // Input Actions
    private InputSystem.PlayerControlsActions inputActions; 
    private Vector2 movementInput; 

    private void Awake()
    {
        inputActions = InputManager.Instance.GetInputActions().PlayerControls;
    }

    private void OnEnable()
    {

        // Movement
        inputActions.Movement.performed += OnMovementInput;
        inputActions.Movement.canceled += OnMovementInput;

        // Dash
        inputActions.Dash.performed += OnDashInput;

        // Extra Actions
        //inputActions.PlayerControls.MeleeAttack.performed += OnMeleeAttackInput;
        //inputActions.PlayerControls.DistanceAttack.performed += OnDistanceAttackInput;
        //inputActions.PlayerControls.UseItem.performed += OnUseItemInput;
        inputActions.Interaction.performed += OnInteractionInput;
    }

    private void OnDisable()
    {
        inputActions.Movement.performed -= OnMovementInput;
        inputActions.Movement.canceled -= OnMovementInput;
        inputActions.Dash.performed -= OnDashInput;
        inputActions.Interaction.performed -= OnInteractionInput;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!PlayerManager.Instance.IsPlayerDead|| !InputManager.Instance.CanPlayerMove())
        {
            Debug.Log("Player cannot move or is dead.");
            return; 
        }
        
        if (!isDashing) // Movement only if we don't dash
        {
            HandleMovement();
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        // Transforms 2DVector into 3DVector 
        Vector3 forward = transform.forward * movementInput.y; // Ruch przód/tył
        Vector3 right = transform.right * movementInput.x; // Ruch lewo/prawo
        moveDirection = (forward + right) * walkSpeed;

        // Adds gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = 0;
        }

        // Character Movement
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        if (!InputManager.Instance.CanPlayerMove())
        {
            Debug.LogWarning("Player cannot move. Ignoring movement input.");
            return;
        }
        // Reads System input (Movement -> Vector2)
        movementInput = context.ReadValue<Vector2>();
    }

    private void OnDashInput(InputAction.CallbackContext context)
    {
        if (!InputManager.Instance.CanPlayerMove())
        {
            Debug.LogWarning("Player cannot dash. Ignoring dash input.");
            return;
        }
        
        if (dashCooldownTimer <= 0 && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    //TODO:Change to UniTask
    private IEnumerator Dash()
    {
        Debug.Log("Dashing");
        isDashing = true;
        Vector3 dashDirection = (transform.forward * movementInput.y) + (transform.right * movementInput.x);
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            characterController.Move(dashDirection.normalized * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }
    
    private void OnInteractionInput(InputAction.CallbackContext context)
    {
        if (!PlayerManager.Instance.IsPlayerDead)
        {
            Debug.LogWarning("Player is dead. Ignoring interaction.");
            return;
        }
        // TODO Space to write Interaction section
        
        //NOTE: I belive it would be better if player could interact with objects without facing them ~JM
        Debug.Log("Interacting with an object.");
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Debug.Log("Interacted with: " + hit.collider.name);
           
        }
    }
}
