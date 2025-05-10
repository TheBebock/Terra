using System.Collections;
using NaughtyAttributes;
using Terra.Enums;
using Terra.InputManagement;
using Terra.Interfaces;
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
        [SerializeField, ReadOnly] private bool _isDashing = false;
        [SerializeField, ReadOnly]private float dashCooldownTimer = 0f;
        // Input Actions
        private InputSystem.PlayerControlsActions inputActions;
        private Vector2 movementInput;

        [Foldout("Debug"), ReadOnly] [SerializeField] private bool _isTryingMove = false;
        [SerializeField] private bool _canPlayerMove = true;
        public bool CanPlayerMove { 
            get => _canPlayerMove;
            set => _canPlayerMove = value;
        }
        public bool IsDashing { 
            get => _isDashing;
            private set => _isDashing = value;
        }

        public bool IsTryingMove
        {
            get => _isTryingMove;
            private set => _isTryingMove = value;
        }
        
        private FacingDirection currentplayerMoveDirection;
        public FacingDirection CurrentPlayerMoveDirection => currentplayerMoveDirection;



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
            if (PlayerManager.Instance.IsPlayerDead || !CanPlayerMove)
            {
                return;
            }

            if (dashCooldownTimer > 0)
            {
                dashCooldownTimer -= Time.deltaTime;
            }
        }

        public void HandleMovement()
        {
            if (PlayerManager.Instance.IsPlayerDead || !CanPlayerMove)
            {
                return;
            }
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
            ChangeMoveDirection();
        }

        private void OnMovementInput(InputAction.CallbackContext context)
        {
            if (!CanPlayerMove)
            {
                Debug.LogWarning("Player cannot move. Ignoring movement input.");
                return;
            }
            IsTryingMove = true;
            // Reads System input (Movement -> Vector2)
            movementInput = context.ReadValue<Vector2>();
        }

        private void OnMovementInputCanceled(InputAction.CallbackContext context)
        {
            IsTryingMove = false;
        }

        private void OnDashInput(InputAction.CallbackContext context)
        {
            if (!CanPlayerMove)
            {
                Debug.LogWarning("Player cannot dash. Ignoring dash input.");
                return;
            }

            if (dashCooldownTimer <= 0 && !IsDashing)
            {
                StartCoroutine(Dash());
            }
        }

        //TODO:Change to UniTask
        private IEnumerator Dash()
        {
            Debug.Log("Dashing");
            IsDashing = true;
            Vector3 dashDirection = (transform.forward * movementInput.y) + (transform.right * movementInput.x);
            float startTime = Time.time;

            while (Time.time < startTime + dashDuration)
            {
                characterController.Move(dashDirection.normalized * dashSpeed * Time.deltaTime);
                yield return null;
            }

            IsDashing = false;
            dashCooldownTimer = dashCooldown;
            yield break;
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

        private void ChangeMoveDirection()
        {
            if (movementInput.y > 0)
                currentplayerMoveDirection = FacingDirection.Up;
            if (movementInput.y < 0)
                currentplayerMoveDirection = FacingDirection.Down;
            if (movementInput.x < 0)
                currentplayerMoveDirection = FacingDirection.Left;
            if (movementInput.x > 0)
                currentplayerMoveDirection = FacingDirection.Right;
        }

        public void AttachListeners()
        {
            // Movement
            inputActions.Movement.performed += OnMovementInput;
            inputActions.Movement.canceled += OnMovementInput;
            inputActions.Movement.canceled += OnMovementInputCanceled;

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
            if (inputActions.Movement != null)
            {
                inputActions.Movement.performed -= OnMovementInput;
                inputActions.Movement.canceled -= OnMovementInput;
                inputActions.Movement.canceled -= OnMovementInputCanceled;
            }
            if(inputActions.Dash != null)
                inputActions.Dash.performed -= OnDashInput;
            if(inputActions.Interaction != null)
                inputActions.Interaction.performed -= OnInteractionInput;
        }
    }
}