using System.Collections;
using System.Collections.Generic;
using _Source.AI.Data.Definitions;
using _Source.AI.Enemy;
using NaughtyAttributes;
using UnityEngine;

namespace AI.Data.Definitions
{
    [CreateAssetMenu(fileName = "RangedEnemy_",menuName = "TheBebocks/AI/Data/RangedEnemy")]

    public class RangedEnemyData : EnemyData
    {
        public float attackCooldown = 1.5f;
        public BulletFactory bulletFactory;
        public BulletData bulletData;
        public float detectionAngle = 60f;
        public float detectionRadius = 10f;
        public float innerDetectionRadius = 5f;
        public float attackRange = 2f;

    }
}