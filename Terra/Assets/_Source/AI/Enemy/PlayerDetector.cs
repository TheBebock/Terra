using Terra.AI.Data.Definitions;
using Terra.Data.Definitions;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class PlayerDetector : InGameMonobehaviour
    {
        [SerializeField] private EnemyData enemyData;  // Przechowujemy dane z EnemyData
        
        private PlayerManager _playerManager;
        
        private CountdownTimer _detectionTimer;
        
        private IDetectionStrategy _detectionStrategy;


        //TODO: Delete detecting player, enemy can always detect player

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
            Gizmos.DrawWireSphere(transform.position, 5f);
            // Rysowanie stożka detekcji dla RangedEnemy
            if (enemyData is RangedEnemyData rangedData)
            {
                Vector3 forwardConeDirection = Quaternion.Euler(0, rangedData.detectionAngle / 2, 0) * transform.forward * rangedData.detectionRadius;
                Vector3 backwardConeDirection = Quaternion.Euler(0, -rangedData.detectionAngle / 2, 0) * transform.forward * rangedData.detectionRadius;
                Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
                Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
            }
        }
    }
}
