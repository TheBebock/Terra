using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement_1 : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseActions();
        HandleKeyboardActions();

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        if (isDashing) return;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? walkSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? walkSpeed * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (!characterController.isGrounded)
        {
            moveDirection.y = movementDirectionY - gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = 0; // Reset vertical movement if grounded
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void HandleMouseActions()
    {
        if (Input.GetMouseButtonDown(0)) // LMB - melee attack
        {
            PerformMeleeAttack();
        }

        if (Input.GetMouseButton(1)) // RMB - ranged attack
        {
            PerformRangedAttack();
        }
    }

    private void HandleKeyboardActions()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Q - active item
        {
            UseActiveItem();
        }

        if (Input.GetKeyDown(KeyCode.E)) // E - interaction
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0) // Left Shift - dash
        {
            StartCoroutine(Dash());
        }
    }

    private void PerformMeleeAttack()
    {
        Debug.Log("Performing melee attack.");
        // Logic for melee attack can be implemented here (e.g., animations or damage application).
    }

    private void PerformRangedAttack()
    {
        Debug.Log("Performing ranged attack.");
        // Logic for ranged attack can be implemented here (e.g., projectile shooting).
    }

    private void UseActiveItem()
    {
        Debug.Log("Using active item.");
        // Implement logic for using an active item here.
    }

    private void Interact()
    {
        Debug.Log("Interacting.");
        // Raycast to detect objects to interact with.
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Debug.Log("Interacted with: " + hit.collider.name);
            // Add logic for interacting with the detected object.
        }
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dashing.");
        isDashing = true;
        Vector3 dashDirection = moveDirection;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            characterController.Move(dashDirection.normalized * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }
}
