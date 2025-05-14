using _Source.AI.Data.Definitions;
using AI.Data.Definitions;
using Terra.Combat;
using Terra.Interfaces;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace _Source.AI.Enemy
{
    public class PlayerDetector : InGameMonobehaviour, IWithSetUp
    {
        [SerializeField] private EnemyData enemyData;  // Przechowujemy dane z EnemyData
        
        private PlayerManager _playerManager;
        
        private CountdownTimer _detectionTimer;
        
        private IDetectionStrategy _detectionStrategy;

        public void SetUp()
        {
            _detectionTimer = new CountdownTimer(enemyData.detectionCooldown);  // Używamy danych z EnemyData
            
            // Wybór odpowiedniej strategii detekcji
            if (enemyData is RangedEnemyData)
            {
                var rangedData = enemyData as RangedEnemyData;
                _detectionStrategy = new ConeDetectionStrategy(rangedData.detectionAngle, rangedData.detectionRadius, rangedData.innerDetectionRadius);
            }
            else if (enemyData is MeleeEnemyData)
            {
                var meleeData = enemyData as MeleeEnemyData;
                _detectionStrategy = new SphereDetectionStrategy(meleeData.detectionRadius);
            }

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
            return directionToPlayer.magnitude <= enemyData.attackRange;  // Używamy danych z EnemyData
        }
        
        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this._detectionStrategy = detectionStrategy;
        
        void OnDrawGizmos() {
            Gizmos.color = Color.red;

            // Draw a spheres for the radii
            Gizmos.DrawWireSphere(transform.position, enemyData.detectionRadius);
            // Rysowanie stożka detekcji dla RangedEnemy
            if (enemyData is RangedEnemyData rangedData)
            {
                Vector3 forwardConeDirection = Quaternion.Euler(0, rangedData.detectionAngle / 2, 0) * transform.forward * rangedData.detectionRadius;
                Vector3 backwardConeDirection = Quaternion.Euler(0, -rangedData.detectionAngle / 2, 0) * transform.forward * rangedData.detectionRadius;
                Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
                Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
            }
        }

        public void TearDown()
        {
            
        }
    }
}
