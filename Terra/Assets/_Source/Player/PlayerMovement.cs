using System.Collections;
using System.Collections.Generic;
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
    private InputSystem inputActions; 
    private Vector2 movementInput; 

    private void Awake()
    {
        inputActions = InputManager.Instance.GetInputActions();
    }

    private void OnEnable()
    {
        if (!InputManager.Instance.IsPlayerAlive())
        {
            Debug.LogWarning("Player is dead. Input will not be enabled.");
            return; 
        }
        
        //Activates Actions
        inputActions.PlayerControls.Enable();

        // Movement
        inputActions.PlayerControls.Movement.performed += OnMovementInput;
        inputActions.PlayerControls.Movement.canceled += OnMovementInput;

        // Dash
        inputActions.PlayerControls.Dash.performed += OnDashInput;

        // Extra Actions
        //inputActions.PlayerControls.MeleeAttack.performed += OnMeleeAttackInput;
        //inputActions.PlayerControls.DistanceAttack.performed += OnDistanceAttackInput;
        //inputActions.PlayerControls.UseItem.performed += OnUseItemInput;
        inputActions.PlayerControls.Interaction.performed += OnInteractionInput;
    }

    private void OnDisable()
    {
        // Disables Actions
        inputActions.PlayerControls.Movement.performed -= OnMovementInput;
        inputActions.PlayerControls.Movement.canceled -= OnMovementInput;

        inputActions.PlayerControls.Dash.performed -= OnDashInput;

        //inputActions.PlayerControls.MelleAttack.performed -= OnMeleeAttackInput;
        //inputActions.PlayerControls.DistanceAttack.performed -= OnDistanceAttackInput;
        //inputActions.PlayerControls.UseItem.performed -= OnUseItemInput;
        inputActions.PlayerControls.Interaction.performed -= OnInteractionInput;

        inputActions.PlayerControls.Disable();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!InputManager.Instance.IsPlayerAlive() || !InputManager.Instance.CanPlayerMove())
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
        if (!InputManager.Instance.IsPlayerAlive())
        {
            Debug.LogWarning("Player is dead. Ignoring interaction.");
            return;
        }
        // TODO Space to write Interaction section
        Debug.Log("Interacting with an object.");
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Debug.Log("Interacted with: " + hit.collider.name);
           
        }
    }
}
