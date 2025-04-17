using UnityEngine;

namespace StatisticsSystem.Definitions
{
    [CreateAssetMenu(fileName = "BasicEnemyStats", menuName = "Stats/Basic Enemy Stats")]
    public class BasicEnemyStatsDefinition : EnemyStatsDefinition
    {
        [Header("Enemy Base Stats")]
        public float baseMaxHealth = 10f;

        public float BaseMaxHealth => baseMaxHealth;
    }
}