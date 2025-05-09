using Terra.Combat;
using Terra.Interfaces;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace _Source.AI.Enemy {
    public class PlayerDetector : InGameMonobehaviour, IWithSetUp
    {
        [SerializeField] float detectionAngle = 60f; // Cone in front of enemy
        [SerializeField] float detectionRadius = 10f; // Large circle around enemy
        [SerializeField] float innerDetectionRadius = 5f; // Small circle around enemy
        [SerializeField] float detectionCooldown = 0.2f; // Time between detections
        [SerializeField] float attackRange = 2f; // Distance from enemy to player to attack
        
        private PlayerManager _playerManager;
        
        private CountdownTimer _detectionTimer;
        
        private IDetectionStrategy _detectionStrategy;

        public void SetUp()
        {
            _detectionTimer = new CountdownTimer(detectionCooldown);
            _detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
            _playerManager = PlayerManager.Instance;
        }

        void Update()
        {
            _detectionTimer.Tick(Time.deltaTime);
        } 

        public bool CanDetectPlayer() {
            return _detectionTimer.IsRunning || _detectionStrategy.Execute(_playerManager.transform, transform, _detectionTimer);
        }

        public bool CanAttackPlayer() {
            var directionToPlayer = _playerManager.transform.position - transform.position;
            Debug.DrawLine(transform.position, _playerManager.transform.position, Color.blue);
            return directionToPlayer.magnitude <= attackRange;
        }
        
        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this._detectionStrategy = detectionStrategy;
        
        void OnDrawGizmos() {
            Gizmos.color = Color.red;

            // Draw a spheres for the radii
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);

            // Calculate our cone directions
            Vector3 forwardConeDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;
            Vector3 backwardConeDirection = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRadius;

            // Draw lines to represent the cone
            Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
            Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
        }



        public void TearDown()
        {
            
        }
    }
}