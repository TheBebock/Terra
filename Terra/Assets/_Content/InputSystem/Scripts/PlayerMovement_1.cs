using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement_1 : MonoBehaviour
{
    public float walkSpeed = 6f; // Prędkość poruszania się
    public float gravity = 10f; // Grawitacja -  nie wiem czy będzie nam potrzebna w grze( bo chyba wsm RigidBody to robi) ale narazie zostawiam
    public float dashSpeed = 20f; // Prędkość dasha
    public float dashDuration = 0.2f; // Długość dasha
    public float dashCooldown = 1f; // Cooldown dasha

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    // Input Actions
    private InputSystemMovement inputActions; // Klasa wygenerowana przez Input System
    private Vector2 movementInput; // Wektor wejścia dla ruchu

    private void Awake()
    {
        inputActions = new InputSystemMovement(); // Inicjalizacja klasy akcji
    }

    private void OnEnable()
    {
        // Aktywowanie akcji 
        inputActions.PlayerControls.Enable();

        // Ruch
        inputActions.PlayerControls.Movement.performed += OnMovementInput;
        inputActions.PlayerControls.Movement.canceled += OnMovementInput;

        // Dash
        inputActions.PlayerControls.Dash.performed += OnDashInput;

        // Akcje dodatkowe
        inputActions.PlayerControls.MelleAttack.performed += OnMeleeAttackInput;
        inputActions.PlayerControls.DistanceAttack.performed += OnDistanceAttackInput;
        inputActions.PlayerControls.UseItem.performed += OnUseItemInput;
        inputActions.PlayerControls.Interaction.performed += OnInteractionInput;
    }

    private void OnDisable()
    {
        // Wyłączenie akcji 
        inputActions.PlayerControls.Movement.performed -= OnMovementInput;
        inputActions.PlayerControls.Movement.canceled -= OnMovementInput;

        inputActions.PlayerControls.Dash.performed -= OnDashInput;

        inputActions.PlayerControls.MelleAttack.performed -= OnMeleeAttackInput;
        inputActions.PlayerControls.DistanceAttack.performed -= OnDistanceAttackInput;
        inputActions.PlayerControls.UseItem.performed -= OnUseItemInput;
        inputActions.PlayerControls.Interaction.performed -= OnInteractionInput;

        inputActions.PlayerControls.Disable();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isDashing) // Ruch tylko, jeśli nie dashujemy
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
        // Przekształcenie Vector2 na Vector3 do ruchu w przestrzeni 3D
        Vector3 forward = transform.forward * movementInput.y; // Ruch przód/tył
        Vector3 right = transform.right * movementInput.x; // Ruch lewo/prawo
        moveDirection = (forward + right) * walkSpeed;

        // Dodanie grawitacji
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = 0;
        }

        // Ruch postaci
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        // Odczyt wejścia z systemu (Movement -> Vector2)
        movementInput = context.ReadValue<Vector2>();
    }

    private void OnDashInput(InputAction.CallbackContext context)
    {
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
    
    private void OnMeleeAttackInput(InputAction.CallbackContext context)
    {
        Debug.Log("Performing melee attack.");
        // TO DO: Logika ataku wręcz 
    }

    private void OnDistanceAttackInput(InputAction.CallbackContext context)
    {
        Debug.Log("Performing ranged attack.");
        // TO DO: Logika dla ataku dystansowego 
    }

    private void OnUseItemInput(InputAction.CallbackContext context)
    {
        Debug.Log("Using active item.");
        // TO DO : Implementacja logiki używania przedmiotów.
    }

    private void OnInteractionInput(InputAction.CallbackContext context)
    {
        Debug.Log("Interacting with an object.");
        // Raycast w celu wykrycia obiektu do interakcji
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Debug.Log("Interacted with: " + hit.collider.name);
            // TO DO:Logika interakcji z wykrytym obiektem.
        }
    }
}
