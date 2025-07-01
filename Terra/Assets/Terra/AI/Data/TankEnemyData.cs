using UnityEngine;

namespace Terra.AI.Data
{
    [CreateAssetMenu(fileName = "TankEnemy_",menuName = "TheBebocks/AI/Data/TankEnemy")]
    public class TankEnemyData : MeleeEnemyData
    {
        [Min(1f)]public float attackCooldown = 5f;
        [Min(1f)] public float attackDuration;
    }   
}
