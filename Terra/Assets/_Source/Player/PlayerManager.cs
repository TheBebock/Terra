using System;
using Terra.Core.Generics;
using NaughtyAttributes;
using Terra.Combat;
using Terra.FSM;
using UnityEngine;
using Terra.Interfaces;
using Terra.Player.PlayerStates;

namespace Terra.Player
{
    /// <summary>
    /// Represents a player
    /// </summary>
    public class PlayerManager : MonoBehaviourSingleton<PlayerManager>, IWithSetUp
    {
        [Foldout("Debug")][SerializeField, ReadOnly] private bool _isPlayerDead;

        [Foldout("Debug")] [SerializeField, ReadOnly]
        private PlayerAttackController _playerAttackController;
        [Foldout("Debug")][SerializeField, ReadOnly] private StateMachine _stateMachine;
        
        [Foldout("References")] [SerializeField] private PlayerMovement _playerMovement;
        [Foldout("References")] [SerializeField] private Animator _playerAnimator;
        [Foldout("References")] [SerializeField] private PlayerEntity _playerEntity;


        public Vector3 CurrentPosition => transform.position;
        
        public bool IsPlayerDead => _isPlayerDead;
        
        public PlayerEntity PlayerEntity =>  _playerEntity;
        public HealthController HealthController => PlayerEntity.HealthController;

        public PlayerMovement PlayerMovement => _playerMovement;
        public PlayerAttackController PlayerAttackController => _playerAttackController;
        
        public event Action OnPlayerDeath;


        public IState GetLastState()
        {
            return _stateMachine.GetPreviousState();
        }

        public void SetUp()
        {
            //NOTE: Needs to be in SetUp, because it caches references to managers
            _playerAttackController = new PlayerAttackController(false);
            _playerAttackController.AttachListeners();
            
            _stateMachine = new StateMachine();

            // Set states
            IdleState idleState = new IdleState(this, _playerAnimator);
            LocomotionState locomotionState = new LocomotionState(this, _playerAnimator);
            StunState stunState = new StunState(this, _playerAnimator);
            DashState dashState = new DashState(this, _playerAnimator);
            DeathState deathState = new DeathState(this, _playerAnimator);
            MeleeAttackState meleeAttackState = new MeleeAttackState(this, _playerAnimator);
            RangedAttackState rangedAttackState = new RangedAttackState(this, _playerAnimator);

            // Set transitions
            _stateMachine.AddTransition(stunState, locomotionState, new FuncPredicate(() => _playerMovement.CanPlayerMove));
            _stateMachine.AddTransition(locomotionState, dashState, new FuncPredicate(() => _playerMovement.IsDashing));
            _stateMachine.AddTransition(dashState, locomotionState, new FuncPredicate(() => !_playerMovement.IsDashing));
            _stateMachine.AddTransition(idleState, locomotionState, new FuncPredicate(() => _playerMovement.IsTryingMove));
            _stateMachine.AddTransition(locomotionState, idleState, new FuncPredicate(() => !_playerMovement.IsTryingMove));

            _stateMachine.AddAnyTransition(stunState, new FuncPredicate(() => !_playerMovement.CanPlayerMove && !IsPlayerDead));
            _stateMachine.AddAnyTransition(deathState, new FuncPredicate(() => IsPlayerDead));

            _stateMachine.AddTransition(locomotionState, meleeAttackState, new FuncPredicate(() => _playerAttackController.IsTryingPerformMeleeAttack));
            _stateMachine.AddTransition(locomotionState, rangedAttackState, new FuncPredicate(() => _playerAttackController.IsTryingPerformDistanceAttack));
            _stateMachine.AddTransition(idleState, meleeAttackState, new FuncPredicate(() => _playerAttackController.IsTryingPerformMeleeAttack));
            _stateMachine.AddTransition(idleState, rangedAttackState, new FuncPredicate(() => _playerAttackController.IsTryingPerformDistanceAttack));

            _stateMachine.AddTransition(meleeAttackState, idleState, new FuncPredicate(() => !_playerAttackController.IsTryingPerformMeleeAttack));
            _stateMachine.AddTransition(rangedAttackState, idleState, new FuncPredicate(() => !_playerAttackController.IsTryingPerformDistanceAttack));
            

            _stateMachine.SetState(idleState);
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        public void OnPlayerDeathNotify()
        {
            _isPlayerDead = true;
            OnPlayerDeath?.Invoke();
        } 

        public void TearDown()
        {
            _playerAttackController.DetachListeners();
            _playerAttackController = null;
        }

        private void OnValidate()
        {
            if (_playerEntity == null)
            {
                if (TryGetComponent(out PlayerEntity playerEntity))
                {
                    _playerEntity = playerEntity;
                }
                else
                {
                    Debug.LogError($"{this}: reference to PlayerEntity is missing.");
                }
            }
        }
    }
}

