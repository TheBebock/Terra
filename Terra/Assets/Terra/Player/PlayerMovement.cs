using System.Collections;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Enums;
using Terra.InputSystem;
using Terra.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Player
{
    
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : InGameMonobehaviour, IAttachListeners
    {
        [Header("Movement Settings")]
        [SerializeField] private float _walkSpeed = 6f;
        [SerializeField] private float _gravity = 10f;
        [SerializeField] private float _dashSpeed = 20f;
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] private float _dashCooldown = 1f;

        [Foldout("References")] [SerializeField]private CharacterController _characterController;
        [Foldout("Debug"), ReadOnly] [SerializeField] private bool _isDashing;
        [Foldout("Debug"), ReadOnly] [SerializeField] private float _dashCooldownTimer;
        
        [Foldout("Debug"), ReadOnly] [SerializeField] private bool _isTryingMove;
        [Foldout("Debug"), ReadOnly] [SerializeField] private bool _canPlayerMove = true;
        [Foldout("Debug"), ReadOnly] [SerializeField] private Vector3 _moveDirection = Vector3.zero;
        [Foldout("Debug"), ReadOnly] [SerializeField] private Vector2 _movementInput;

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
        
        private FacingDirection _currentplayerMoveDirection;
        public FacingDirection CurrentPlayerMoveDirection => _currentplayerMoveDirection;
        

        void Update()
        {
            if (PlayerManager.Instance.IsPlayerDead || !CanPlayerMove)
            {
                return;
            }

            if (_dashCooldownTimer > 0)
            {
                _dashCooldownTimer -= Time.deltaTime;
            }
        }

        public void HandleMovement()
        {
            if (PlayerManager.Instance.IsPlayerDead || !CanPlayerMove)
            {
                return;
            }
            // Transforms 2DVector into 3DVector 
            Vector3 forward = transform.forward * _movementInput.y; // Ruch przód/tył
            Vector3 right = transform.right * _movementInput.x; // Ruch lewo/prawo
            _moveDirection = (forward + right) * _walkSpeed;

            // Adds gravity
            if (!_characterController.isGrounded)
            {
                _moveDirection.y -= _gravity * Time.deltaTime;
            }
            else
            {
                _moveDirection.y = 0;
            }

            // Character Movement
            _characterController.Move(_moveDirection * Time.deltaTime);
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
            _movementInput = context.ReadValue<Vector2>();
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

            if (_dashCooldownTimer <= 0 && !IsDashing)
            {
                StartCoroutine(Dash());
            }
        }

        //TODO:Change to UniTask
        private IEnumerator Dash()
        {
            Debug.Log("Dashing");
            IsDashing = true;
            Vector3 dashDirection = (transform.forward * _movementInput.y) + (transform.right * _movementInput.x);
            float startTime = Time.time;

            while (Time.time < startTime + _dashDuration)
            {
                _characterController.Move(dashDirection.normalized * (_dashSpeed * Time.deltaTime));
                yield return null;
            }

            IsDashing = false;
            _dashCooldownTimer = _dashCooldown;
        }
        

        private void ChangeMoveDirection()
        {
            if (_movementInput.y > 0)
                _currentplayerMoveDirection = FacingDirection.Up;
            if (_movementInput.y < 0)
                _currentplayerMoveDirection = FacingDirection.Down;
            if (_movementInput.x < 0)
                _currentplayerMoveDirection = FacingDirection.Left;
            if (_movementInput.x > 0)
                _currentplayerMoveDirection = FacingDirection.Right;
        }

        public void AttachListeners()
        {
            if (!InputManager.Instance)
            {
                Debug.LogError("Input manager not found.");
                return;
            }

            var inputActions = InputManager.Instance.PlayerControls;
            
            // Movement
            inputActions.Movement.performed += OnMovementInput;
            inputActions.Movement.canceled += OnMovementInput;
            inputActions.Movement.canceled += OnMovementInputCanceled;

            // Dash
            inputActions.Dash.performed += OnDashInput;
        }

        public void DetachListeners()
        {
            if (!InputManager.Instance) return;
            
            InputManager.Instance.PlayerControls.Movement.performed -= OnMovementInput;
            InputManager.Instance.PlayerControls.Movement.canceled -= OnMovementInput;
            InputManager.Instance.PlayerControls.Movement.canceled -= OnMovementInputCanceled;
            InputManager.Instance.PlayerControls.Dash.performed -= OnDashInput;
        }

        private void OnValidate()
        {
            if(!_characterController) _characterController = GetComponent<CharacterController>();
        }
    }
}