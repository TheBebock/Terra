using Terra.Combat.Projectiles;
using UnityEngine;

namespace Terra.AI.Data.Definitions
{
    [CreateAssetMenu(fileName = "RangedEnemy_",menuName = "TheBebocks/AI/Data/RangedEnemy")]

    public class RangedEnemyData : EnemyData
    {
        public BulletData bulletData;
        public float keepDistanceFromPlayer = 6f;

        private void OnValidate()
        {
            if (keepDistanceFromPlayer > attackRange)
            {
                Debug.LogError($"{nameof(keepDistanceFromPlayer)} needs to be lower than {nameof(attackRange)}");
                keepDistanceFromPlayer = attackRange / 2;
            }
        }
    }
}