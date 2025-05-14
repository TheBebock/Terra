using AI.Data.Definitions;

namespace _Source.AI.Data.Definitions
{
    public abstract class EnemyData : AIData
    {
        public float detectionCooldown = 0.2f;
        public float detectionRadius;
        public float attackRange;
    }
}