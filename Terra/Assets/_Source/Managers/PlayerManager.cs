using System;
using System.Runtime.Remoting.Messaging;
using Core.Generics;
using Core.ModifiableValue;
using Player;
using UnityEngine;
using StatisticsSystem;
using StatisticsSystem.Definitions;

namespace _Source.Managers
{
    public class PlayerManager : MonoBehaviourSingleton<PlayerManager>, IDamagable, IHealable
    {
        
        private bool _isPlayerDead =false;
        
        [Header("References")]
        [SerializeField] PlayerInventoryManager playerInventory;
        public GameObject playerPrefab;
        public Transform playerParent;
        public Transform spawnPoint;
        private GameObject _currentPlayer;
        
        public bool CanBeHealed { get; set; }
        public bool IsInvincible { get; set; }
        public bool CanBeDamaged { get; set; }
        public float CurrentHealth { get; set; }


        private bool isInitialized;
        public float MaxHealth
        {
            get =>  playerStats.MaxHealth;
        }
        
        public bool IsPlayerDead => _isPlayerDead;
        
        private PlayerStats playerStats;
        public Action OnPlayerDeath;
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if(isInitialized) return;
            isInitialized = true;
            
            if(!playerInventory) playerInventory = PlayerInventoryManager.Instance;
            playerStats = PlayerStatsManager.Instance.PlayerStats;
            SpawnPlayer();
            ResetHealth();
        }

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
        }
        
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

        public void OnDeath()
        {
            _isPlayerDead = true;
            OnPlayerDeath?.Invoke();
            Debug.Log("Player has died");
        }
        
        public void Heal(float amount)
        {
            if (isStunned)
            {
                Debug.Log("Player is stunned!");
                return;
            }
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        }
        //This will be in separate class, it is it's own separate system
        public void DealDamage(float damage, GameObject target)
        {
            // This is in comment bcs I don't how we use this and I don't want to destroy something 
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

        
        /// <summary>
        /// This should be a separe class, that holds all the possible states, such as StunState, BurnState etc.
        /// </summary>
        [Header("Player States")]
        public bool isStunned = false;

        
        //This also should be in separate class
        public void ApplyStun(float duration)
        {
            if(isStunned) return;
                isStunned = true;
                Debug.Log("Player is stunned!");
                //NOTE: Inkove's are dangerous, use UniTasks
                Invoke(nameof(RemoveStun), duration);
        }
        //This also should be in separate class
        public void RemoveStun()
        {
            isStunned = false;
            Debug.Log("Player is no longer stunned!");
        }


    }
}

