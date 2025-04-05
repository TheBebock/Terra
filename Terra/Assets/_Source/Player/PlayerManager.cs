using System;
using Core.ModifiableValue;
using Terra.Player;
using Terra.Core.Generics;
using NaughtyAttributes;
using StatisticsSystem;
using Terra.Combat;
using UnityEngine;

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
            
            //TODO: Add states and transitions
            
            if(PlayerInventoryManager.Instance) playerInventory = PlayerInventoryManager.Instance;
            if(PlayerStatsManager.Instance) _playerStats = PlayerStatsManager.Instance.PlayerStats;
            
            healthController = new HealthController(_playerStats.ModifiableMaxHealth, true);

            ResetHealth();
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

