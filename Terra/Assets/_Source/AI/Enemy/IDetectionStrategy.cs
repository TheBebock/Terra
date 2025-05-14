using Terra.Utils;
using UnityEngine;

public interface IDetectionStrategy
{
    bool Execute(Transform playerTransform, Transform enemyTransform, CountdownTimer detectionTimer);
}