using UnityEngine;

namespace Terra.AI.Data
{
    [CreateAssetMenu(fileName = "BossEnemy_",menuName = "TheBebocks/AI/Data/BossEnemy")]
    public class BossEnemyData : EnemyData
    {
        [Min(1f)] public float attackCooldown = 5f;
        [Min(1f)] public float attackRadius;
    }   
}
