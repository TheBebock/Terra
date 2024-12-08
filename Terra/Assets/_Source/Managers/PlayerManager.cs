using UnityEngine;
using StatisticsSystem;

namespace _Source.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance{get; private set;}
        
        [Header("Players Stats")]
        public PlayerStats playerStats;
        
        [Header("Player States")]
        public bool isStunned = false;
        // Add some more like: slow, burn etc
        
        [Header("References")]
        public GameObject playerPrefab;
        public Transform playerParent;
        public Transform spawnPoint;
        private GameObject _currentPlayer;
        
        private float _currentHealth;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);// Optional if manager it's supposed to be global
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            playerStats = new PlayerStats(10f,100f,5f,3f);
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

        public void TakeDamage(float damage)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, playerStats.MaxHealth);
            if (_currentHealth <= 0)
            {
                Debug.Log("Player Dead");
                SpawnPlayer();
            }
        }
        

        public void Heal(float amount)
        {
            if (isStunned)
            {
                Debug.Log("Player is stunned!");
                return;
            }
            _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, playerStats.MaxHealth);
        }

        public void ResetHealth()
        {
            _currentHealth = playerStats.MaxHealth;
        }
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

        public void ApplyStun(float duration)
        {
            if(isStunned) return;
                isStunned = true;
                Debug.Log("Player is stunned!");
                Invoke(nameof(RemoveStun), duration);
        }

        public void RemoveStun()
        {
            isStunned = false;
            Debug.Log("Player is no longer stunned!");
        }
    }
}

