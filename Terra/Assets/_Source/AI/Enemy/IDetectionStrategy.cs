using UnityEngine;

namespace _Source.AI.Enemy {
    public interface IDetectionStrategy {
        bool Execute(Transform player, Transform detector, CountdownTimer timer);
    }
}