using UnityEngine;

namespace Terra.AI.Data
{
    [CreateAssetMenu(fileName = "MeleeEnemy_",menuName = "TheBebocks/AI/Data/MeleeEnemy")]

    public class MeleeEnemyData : EnemyData
    {
        public float attackRadius = 1.5f;
        public float attackCooldown;

    }
}