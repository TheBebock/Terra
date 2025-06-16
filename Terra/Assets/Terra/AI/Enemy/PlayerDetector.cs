using NaughtyAttributes;
using Terra.AI.Data.Definitions;
using Terra.Core.Generics;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class PlayerDetector : InGameMonobehaviour
    {
        [SerializeField, ReadOnly] private EnemyData _enemyData;
        [SerializeField, ReadOnly] private float _attackRange =-1;
        
        private CountdownTimer _detectionTimer;
        
        private IDetectionStrategy _detectionStrategy;
        
        public void Init(EnemyData enemyData) => _enemyData = enemyData;

        public bool CanAttackPlayer() {
            Debug.DrawLine(transform.position, PlayerManager.Instance.transform.position, Color.blue);
            return Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= _enemyData.attackRange;
        }
        
        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this._detectionStrategy = detectionStrategy;
        
        void OnDrawGizmos() {
            Gizmos.color = Color.red;
            
            // Rysowanie stożka detekcji dla RangedEnemy
            if (_enemyData is RangedEnemyData rangedData)
            {
                Vector3 forwardConeDirection = Quaternion.Euler(0, rangedData.detectionAngle / 2, 0) * transform.forward * rangedData.detectionRadius;
                Vector3 backwardConeDirection = Quaternion.Euler(0, -rangedData.detectionAngle / 2, 0) * transform.forward * rangedData.detectionRadius;
                Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
                Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
            }
        }


    }
}
