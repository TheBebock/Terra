using Terra.Utils;
using UnityEngine;

namespace _Source.AI.Enemy
{
    public class ConeDetectionStrategy : IDetectionStrategy
    {
        private float _detectionAngle;
        private float _detectionRadius;
        private float _innerDetectionRadius;

        public ConeDetectionStrategy(float detectionAngle, float detectionRadius, float innerDetectionRadius)
        {
            _detectionAngle = detectionAngle;
            _detectionRadius = detectionRadius;
            _innerDetectionRadius = innerDetectionRadius;
        }

        public bool Execute(Transform playerTransform, Transform enemyTransform, CountdownTimer detectionTimer)
        {
            Vector3 directionToPlayer = playerTransform.position - enemyTransform.position;
            float angleToPlayer = Vector3.Angle(enemyTransform.forward, directionToPlayer);

            return angleToPlayer <= _detectionAngle / 2 && directionToPlayer.magnitude <= _detectionRadius;
        }
    }
}