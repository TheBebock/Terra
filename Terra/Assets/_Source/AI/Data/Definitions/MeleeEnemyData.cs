using UnityEngine;

namespace _Source.AI.Data.Definitions
{
    [CreateAssetMenu(fileName = "MeleeEnemy_",menuName = "TheBebocks/AI/Data/MeleeEnemy")]

    public class MeleeEnemyData : EnemyData
    {
        
        public float attackRadius = 1.5f;
        public float attackCooldown = 1f;
        public new float detectionRadius = 5f;
        public new float attackRange = 2f;
    }
}