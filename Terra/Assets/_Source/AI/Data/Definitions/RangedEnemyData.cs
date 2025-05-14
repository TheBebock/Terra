using Terra.AI.Data.Definitions;
using Terra.Combat.Projectiles;
using UnityEngine;

namespace Terra.Data.Definitions
{
    [CreateAssetMenu(fileName = "RangedEnemy_",menuName = "TheBebocks/AI/Data/RangedEnemy")]

    public class RangedEnemyData : EnemyData
    {
        public BulletData bulletData;
        public float detectionAngle = 60f;
        public float detectionRadius = 10f;
        public float innerDetectionRadius = 5f;
    }
}