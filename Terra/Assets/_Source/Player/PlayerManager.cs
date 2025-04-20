using System;
using Terra.Core.Generics;
using NaughtyAttributes;
using StatisticsSystem;
using Terra.Combat;
using UnityEngine;
using Terra.StateMachine.PlayerStates;
using Terra.InputManagement;
using Terra.Interfaces;
using Terra.StateMachine;

namespace Terra.Player
{
    /// <summary>
    /// Represents a player
    /// </summary>
    public class PlayerManager : MonoBehaviourSingleton<PlayerManager>, IDamagable, IHealable, IWithSetUp, IAttachListeners
    {
        
        [Foldout("Debug")][SerializeField, ReadOnly] private HealthController healthController;
        [Foldout("Debug")][SerializeField, ReadOnly] private bool _isPlayerDead = false;
        [Foldout("Debug")][SerializeField, ReadOnly] PlayerAttackController playerAttackController;
        [Foldout("Debug")][SerializeField, ReadOnly] private StateMachine.StateMachine _stateMachine;
        
        [Foldout("References")] [SerializeField] PlayerInventoryManager playerInventory;
        [Foldout("References")][SerializeField] PlayerMovement playerMovement;
        [Foldout("References")][SerializeField] Animator playerAnimator;


        public Vector3 CurrentPosition => transform.position;

        public bool CanBeDamaged { get; set; } = true;
        public bool CanBeHealed => healthController.CanBeHealed;
        public bool IsInvincible => healthController.IsInvincible;
        public float MaxHealth => healthController.MaxHealth;
        public float CurrentHealth => healthController.CurrentHealth;
        private PlayerStats _playerStats;  
        public bool IsPlayerDead => _isPlayerDead;
        
        public HealthController HealthController => healthController;


        public PlayerMovement PlayerMovement => playerMovement;
        public PlayerInventoryManager PlayerInventory => playerInventory;
        public PlayerAttackController PlayerAttackController => playerAttackController;
        
        public event Action OnPlayerDeath;

        protected override void Awake()
        {
            base.Awake();
            
             CanBeDamaged = true;
            _stateMachine = new StateMachine.StateMachine();

            // Set states
            LocomotionState locomotionState = new LocomotionState(this, playerAnimator);
            StunState stunState = new StunState(this, playerAnimator);
            DashState dashState = new DashState(this, playerAnimator);
            DeathState deathState = new DeathState(this, playerAnimator);
            MeleeAttackState meleeAttackState = new MeleeAttackState(this, playerAnimator);
            RangedAttackState rangedAttackState = new RangedAttackState(this, playerAnimator);

            // Set transitions
            _stateMachine.AddTransition(stunState, locomotionState, new FuncPredicate(() => playerMovement.CanPlayerMove));
            _stateMachine.AddTransition(locomotionState, dashState, new FuncPredicate(() => playerMovement.IsDashing));
            _stateMachine.AddTransition(dashState, locomotionState, new FuncPredicate(() => !playerMovement.IsDashing));

            _stateMachine.AddAnyTransition(stunState, new FuncPredicate(() => !playerMovement.CanPlayerMove && !IsPlayerDead));
            _stateMachine.AddAnyTransition(deathState, new FuncPredicate(() => IsPlayerDead));

            _stateMachine.AddTransition(locomotionState, meleeAttackState, new FuncPredicate(() => playerAttackController.IsTryingPerformMeleeAttack));
            _stateMachine.AddTransition(meleeAttackState, locomotionState, new FuncPredicate(() => !playerAttackController.IsTryingPerformMeleeAttack));
            _stateMachine.AddTransition(locomotionState, rangedAttackState, new FuncPredicate(() => playerAttackController.IsTryingPerformDistanceAttack));
            _stateMachine.AddTransition(rangedAttackState, locomotionState, new FuncPredicate(() => !playerAttackController.IsTryingPerformDistanceAttack));
            

            _stateMachine.SetState(locomotionState);

        }

        public void SetUp()
        {
            if (PlayerInventoryManager.Instance) playerInventory = PlayerInventoryManager.Instance;
            if(PlayerStatsManager.Instance) _playerStats = PlayerStatsManager.Instance.PlayerStats;
            
            healthController = new HealthController(_playerStats.ModifiableMaxHealth, true);
            
            if(InputManager.Instance) playerAttackController = new PlayerAttackController(InputManager.Instance.PlayerControls, this);
            else Debug.LogError(this + " Input Manager not found.");
            
            ResetHealth();
        }
        
        public void AttachListeners()
        {
            healthController.OnDeath += OnDeath;
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        public void ResetHealth(bool isSilent = true) => healthController.ResetHealth(isSilent);
        public void KIll(bool isSilent = true) => healthController.KIll(isSilent);

        
        public void OnDeath()
        {
            _isPlayerDead = true;
            CanBeDamaged = false;
            OnPlayerDeath?.Invoke();
        }
        public void TakeDamage(float amount)
        {
            if(!CanBeDamaged) return;
            
            Debug.Log($"{gameObject.name} took {amount} damage");
            healthController.TakeDamage(amount);
        } 
        
        public void Heal(float amount)
        {
            if(!CanBeHealed) return;
            healthController.Heal(amount);
        }
        

        public void TearDown()
        {
            healthController = null;
            playerAttackController.DetachListeners();
            playerAttackController = null;
        }


        public void DetachListeners()
        {
            healthController.OnDeath -= OnDeath;
        }
    }
}

