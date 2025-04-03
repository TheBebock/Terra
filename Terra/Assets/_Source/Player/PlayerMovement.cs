using System.Collections;
using Terra.InputManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Player
{
    
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, IAttachListeners
    {
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float gravity = 10f;
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 1f;

        private CharacterController characterController;
        private Vector3 moveDirection = Vector3.zero;
        private bool isDashing = false;
        private float dashCooldownTimer = 0f;
        public GameObject meleeWeapon;
        public GameObject rangedWeapon;
        public GameObject activeItem;
        // Input Actions
        private InputSystem.PlayerControlsActions inputActions;
        private Vector2 movementInput;
        
        private bool _canPlayerMove = true;
        public bool CanPlayerMove { get { return _canPlayerMove; } set { _canPlayerMove = value; } }

        private void OnDestroy()
        {
            DetachListeners();
        }

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            inputActions = InputManager.Instance.PlayerControls;
            AttachListeners();
        }

        void Update()
        {
            if (!PlayerManager.Instance.IsPlayerDead || !CanPlayerMove)
            {
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
            if (!CanPlayerMove)
            {
                Debug.LogWarning("Player cannot move. Ignoring movement input.");
                return;
            }

            // Reads System input (Movement -> Vector2)
            movementInput = context.ReadValue<Vector2>();
        }

        private void OnDashInput(InputAction.CallbackContext context)
        {
            if (!CanPlayerMove)
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

        public void AttachListeners()
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

        public void DetachListeners()
        {
            inputActions.Movement.performed -= OnMovementInput;
            inputActions.Movement.canceled -= OnMovementInput;
            inputActions.Dash.performed -= OnDashInput;
            inputActions.Interaction.performed -= OnInteractionInput;
        }
    }
}