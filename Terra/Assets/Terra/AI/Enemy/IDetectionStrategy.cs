using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public interface IDetectionStrategy
    {
        bool Execute(Transform playerTransform, Transform enemyTransform, CountdownTimer detectionTimer);
    }
}