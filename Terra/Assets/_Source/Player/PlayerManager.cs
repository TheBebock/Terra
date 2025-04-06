using System;
using Core.ModifiableValue;
using Terra.Player;
using Terra.Core.Generics;
using NaughtyAttributes;
using StatisticsSystem;
using Terra.Combat;
using UnityEngine;
using _Source.StateMachine.PlayerStates;
using Terra.StateMachine;

namespace Terra.Player
{
    public class PlayerManager : MonoBehaviourSingleton<PlayerManager>, IDamagable, IHealable, IWithSetUp
    {
        
        [Foldout("Debug")][SerializeField, ReadOnly] private HealthController healthController;
        [Foldout("Debug")][SerializeField, ReadOnly] private bool _isPlayerDead = false;

        [Foldout("References")] [SerializeField] PlayerInventoryManager playerInventory;
        [Foldout("References")][SerializeField] PlayerMovement playerMovement;
        [Foldout("References")][SerializeField] Animator playerAnimator;
        
        private StateMachine.StateMachine _stateMachine;

        public Vector3 CurrentPosition => transform.position;
        
        public bool CanBeDamaged { get; set; }
        public bool CanBeHealed => healthController.CanBeHealed;
        public bool IsInvincible => healthController.IsInvincible;
        public float MaxHealth => healthController.MaxHealth;
        public float CurrentHealth => healthController.CurrentHealth;
        private PlayerStats _playerStats;  
        public bool IsPlayerDead => _isPlayerDead;
        
        public HealthController HealthController => healthController;
        public PlayerMovement PlayerMovement => playerMovement;
        public PlayerInventoryManager PlayerInventory => playerInventory;
        
        public event Action OnPlayerDeath;


        public void SetUp()
        {
            _stateMachine = new StateMachine.StateMachine();

            // Set states
            LocomotionState locomotionState = new LocomotionState(this, playerAnimator);
            StunState stunState = new StunState(this, playerAnimator);
            DashState dashState = new DashState(this, playerAnimator);

            // Set transitions
            _stateMachine.AddAnyTransition(stunState, new FuncPredicate(() => !playerMovement.CanPlayerMove)); //NOTE: Ask about condition
            _stateMachine.AddTransition(stunState, locomotionState, new FuncPredicate(() => playerMovement.CanPlayerMove)); //NOTE: Ask about condition
            _stateMachine.AddTransition(locomotionState, dashState, new FuncPredicate(() => playerMovement.IsDashing));
            _stateMachine.AddTransition(dashState, locomotionState, new FuncPredicate(() => !playerMovement.IsDashing));

            if (PlayerInventoryManager.Instance) playerInventory = PlayerInventoryManager.Instance;
            if(PlayerStatsManager.Instance) _playerStats = PlayerStatsManager.Instance.PlayerStats;
            
            healthController = new HealthController(_playerStats.ModifiableMaxHealth, true);

            ResetHealth();
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


        public void TakeDamage(float amount)
        {
            if(!CanBeDamaged) return;
            healthController.TakeDamage(amount);
        } 

        // Move to state PlayerDeathState
        public void OnDeath()
        {
            CanBeDamaged = false;
            _isPlayerDead = true;
            OnPlayerDeath?.Invoke();
            Debug.Log("Player has died");
        }
        
        public void Heal(float amount)
        {
            if(!CanBeHealed) return;
            healthController.Heal(amount);
        }
        
        /// <summary>
        /// Clean up data
        /// </summary>
        public void TearDown()
        {
            
        }
    }
}

