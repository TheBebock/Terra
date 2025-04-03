using System;
using Terra.Player;
using Terra.Core.Generics;
using NaughtyAttributes;
using StatisticsSystem;
using UnityEngine;

namespace Terra.Player
{
    public class PlayerManager : MonoBehaviourSingleton<PlayerManager>, IDamagable, IHealable, IWithSetUp
    {
        
        [Foldout("Debug")][SerializeField, ReadOnly] private bool _isPlayerDead = false;

        [Foldout("References")] [SerializeField] PlayerInventoryManager playerInventory;
        [Foldout("References")][SerializeField] PlayerMovement playerMovement;
        [Foldout("References")][SerializeField] Animator playerAnimator;
        
        
        private StateMachine.StateMachine _stateMachine;

        public bool CanBeHealed { get; set; }
        public bool IsInvincible { get; set; }
        public bool CanBeDamaged { get; set; }
        public float CurrentHealth { get; set; }

        public bool IsInitialized { get; private set; } = false;

        public float MaxHealth
        {
            get =>  playerStats.MaxHealth;
        }
        
        public bool IsPlayerDead => _isPlayerDead;
        
        private PlayerStats playerStats;    
        
        public PlayerMovement PlayerMovement => playerMovement;
        public PlayerInventoryManager PlayerInventory => playerInventory;
        
        public event Action OnPlayerDeath;
        private void Start()
        {
            SetUp();
        }
        
        public void SetUp()
        {
            if(IsInitialized) return;
            IsInitialized = true;
            
            _stateMachine = new StateMachine.StateMachine();
            
            //TODO: Add states and transitions
            
            if(PlayerInventoryManager.Instance) playerInventory = PlayerInventoryManager.Instance;
            if(PlayerStatsManager.Instance) playerStats = PlayerStatsManager.Instance.PlayerStats;
            
            ResetHealth();
        }
        /*
        public void SpawnPlayer()
        {
            if (playerPrefab == null || spawnPoint == null)
            {
                Debug.LogError("PlayerPrefab or SpawnPoint is not assigned!");
                return;
            }

            if (_currentPlayer != null)
            {
                Destroy(_currentPlayer);
            }
            _currentPlayer = Instantiate(playerPrefab,spawnPoint.position, spawnPoint.rotation);
            ResetHealth();
        }*/
        
        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
        }
        
        
        public void TakeDamage(float amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);
            if (CurrentHealth <= 0)
            {
                OnDeath();
            }
        }

        // Move to state PlayerDeathState
        public void OnDeath()
        {
            _isPlayerDead = true;
            OnPlayerDeath?.Invoke();
            Debug.Log("Player has died");
        }
        
        public void Heal(float amount)
        {
            if(!CanBeHealed) return;
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        }
        
        //This will be in separate class, it is it's own separate system
        public void DealDamage(float damage, GameObject target)
        {

            /*
         if (isStunned)
        {
            Debug.Log("Player is stunned and cannot attack!");
            return;
        }
        var targetManager = target.GetComponent<TargetManager>();
        if (targetManager != null)
        {
            targetManager.TakeDamage(damage);
            Debug.Log($"Player dealt {damage} damage to {target.name}.");
        }
        else
        {
            Debug.LogWarning("Target does not have a TargetManager component!");
        }
             */
        }


        //Clean up data if needed
        public void TearDown()
        {
            
        }
    }
}

