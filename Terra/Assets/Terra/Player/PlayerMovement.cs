using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.InputSystem;
using Terra.Interfaces;
using Terra.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Player
{
    
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : InGameMonobehaviour, IAttachListeners
    {
        [Header("Movement Settings")]
        [SerializeField] private float _walkSpeed = 6f;
        [SerializeField] private float _dashSpeed = 20f;
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] private float _dashCooldown = 1f;

        [Foldout("References")] [SerializeField]private CharacterController _characterController;
        [Foldout("Debug"), ReadOnly] [SerializeField] private bool _isDashing;
        [Foldout("Debug"), ReadOnly] [SerializeField] private CountdownTimer _dashCooldownTimer;
        
        [Foldout("Debug"), ReadOnly] [SerializeField] private bool _isTryingMove;
        [Foldout("Debug"), ReadOnly] [SerializeField] private bool _canPlayerMove = true;
        [Foldout("Debug"), ReadOnly] [SerializeField] private Vector3 _moveDirection = Vector3.zero;
        [Foldout("Debug"), ReadOnly] [SerializeField] private Vector2 _movementInput;
        [Foldout("Debug"), ReadOnly] [SerializeField] private bool _isPlayerActionBlockingMovement;

        private Vector2 _dashMovementInput;
        private OnPlayerDashTimerProgressedEvent _onDashTimerProgressed;
        private FacingDirection _currentplayerMoveDirection;
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
        
        
        public FacingDirection CurrentPlayerMoveDirection => _currentplayerMoveDirection;
        

        private void Awake()
        {
            _dashCooldownTimer = new CountdownTimer(_dashCooldown);
            _onDashTimerProgressed = new();
            _onDashTimerProgressed.progress = 0f;
        }

        public void AttachListeners()
        {
            EventsAPI.Register<OnPlayerMeleeAttackPerformedEvent>(OnPlayerMeleeAttackPerformed);
            EventsAPI.Register<OnPlayerMeleeAttackEndedEvent>(OnPlayerMeleeAttackEnded);
            EventsAPI.Register<OnPlayerRangeAttackPerformedEvent>(OnPlayerRangeAttackPerformed);
            EventsAPI.Register<OnPlayerRangeAttackEndedEvent>(OnPlayerRangeAttackEnded);
            
            if (!InputsManager.Instance)
            {
                Debug.LogError("Input manager not found.");
                return;
            }

            var inputActions = InputsManager.Instance.PlayerControls;
            
            // Movement
            inputActions.Movement.performed += OnMovementInput;
            inputActions.Movement.canceled += OnMovementInput;
            inputActions.Movement.canceled += OnMovementInputCanceled;

            // Dash
            inputActions.Dash.started += OnDashInput;
        }
        
        
        void Update()
        {
            if (PlayerManager.Instance.IsPlayerDead)
            {
                return;
            }

            _dashCooldownTimer.Tick(Time.deltaTime);
            _onDashTimerProgressed.progress = _dashCooldownTimer.Progress;
            EventsAPI.Invoke(ref _onDashTimerProgressed);
        }

        public void HandleMovement()
        {
            if (PlayerManager.Instance.IsPlayerDead || !CanPlayerMove || 
                _isPlayerActionBlockingMovement)
            {
                return;
            }
            
            Vector3 vertical = transform.forward * _movementInput.y;
            Vector3 horizontal = transform.right * _movementInput.x;
            _moveDirection = (vertical + horizontal).normalized * _walkSpeed;

            _moveDirection.y = 0;
            
            // Character Movement
            _characterController.Move(_moveDirection * Time.deltaTime);
            ChangeMoveDirection();
        }

        public void PushPlayerInDirection(Vector3 direction, float force)
        {
            direction.y = 0f;
            _characterController.Move(direction * force * Time.deltaTime);
        }

        private void OnMovementInput(InputAction.CallbackContext context)
        {
            if (!CanPlayerMove)
            {
                Debug.LogWarning("Player cannot move. Ignoring movement input.");
                return;
            }
            IsTryingMove = true;
            _movementInput = context.ReadValue<Vector2>();
        }

        private void OnMovementInputCanceled(InputAction.CallbackContext context)
        {
            IsTryingMove = false;
        }

        private void OnDashInput(InputAction.CallbackContext context)
        {
            if (!CanPlayerMove || _isPlayerActionBlockingMovement)
            {
                Debug.LogWarning("Player cannot dash. Ignoring dash input.");
                return;
            }

            if (_dashCooldownTimer.IsFinished && !IsDashing)
            {
                _dashMovementInput = _movementInput;
                _ = Dash();
            }
        }
        
        private async UniTaskVoid Dash()
        {
            Debug.Log("Dashing");
            IsDashing = true;
            EventsAPI.Invoke<OnPlayerDashStartedEvent>();
            Vector3 dashDirection = (transform.forward * _dashMovementInput.y) + (transform.right * _dashMovementInput.x);
            float startTime = Time.time;

            while (Time.time < startTime + _dashDuration)
            {
                _characterController.Move(dashDirection.normalized * (_dashSpeed * Time.deltaTime));
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: CancellationToken);
            }

            IsDashing = false;
            EventsAPI.Invoke<OnPlayerDashEndedEvent>();
            _dashCooldownTimer.Restart();
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

        private void OnPlayerMeleeAttackPerformed(ref OnPlayerMeleeAttackPerformedEvent ev)
        {
            _isPlayerActionBlockingMovement = true;
        }

        private void OnPlayerMeleeAttackEnded(ref OnPlayerMeleeAttackEndedEvent ev)
        {
            _isPlayerActionBlockingMovement = false;
        }

        private void OnPlayerRangeAttackPerformed(ref OnPlayerRangeAttackPerformedEvent ev)
        {
            _isPlayerActionBlockingMovement = true;
        }
        
        private void OnPlayerRangeAttackEnded(ref OnPlayerRangeAttackEndedEvent ev)
        {
            _isPlayerActionBlockingMovement = false;
        }
        public void DetachListeners()
        {
            EventsAPI.Unregister<OnPlayerMeleeAttackPerformedEvent>(OnPlayerMeleeAttackPerformed);
            EventsAPI.Unregister<OnPlayerMeleeAttackEndedEvent>(OnPlayerMeleeAttackEnded);
            EventsAPI.Unregister<OnPlayerRangeAttackPerformedEvent>(OnPlayerRangeAttackPerformed);
            EventsAPI.Unregister<OnPlayerRangeAttackEndedEvent>(OnPlayerRangeAttackEnded);
            
            if (!InputsManager.Instance) return;
            
            InputsManager.Instance.PlayerControls.Movement.performed -= OnMovementInput;
            InputsManager.Instance.PlayerControls.Movement.canceled -= OnMovementInput;
            InputsManager.Instance.PlayerControls.Movement.canceled -= OnMovementInputCanceled;
            InputsManager.Instance.PlayerControls.Dash.started -= OnDashInput;
        }

        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if(!_characterController) _characterController = GetComponent<CharacterController>();
        }
#endif

    }
}