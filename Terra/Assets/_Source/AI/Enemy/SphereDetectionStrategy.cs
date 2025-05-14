using Terra.Utils;
using UnityEngine;

namespace _Source.AI.Enemy
{
    public class SphereDetectionStrategy : IDetectionStrategy
    {
        private float _detectionRadius;

        public SphereDetectionStrategy(float detectionRadius)
        {
            _detectionRadius = detectionRadius;
        }

        public bool Execute(Transform playerTransform, Transform enemyTransform, CountdownTimer detectionTimer)
        {
            float distanceToPlayer = Vector3.Distance(playerTransform.position, enemyTransform.position);
            return distanceToPlayer <= _detectionRadius;
        }
    }
}