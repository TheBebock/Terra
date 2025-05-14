using System.Collections;
using System.Collections.Generic;
using _Source.AI.Data.Definitions;
using UnityEngine;

namespace AI.Data.Definitions
{
    [CreateAssetMenu(fileName = "MeleeEnemy_",menuName = "TheBebocks/AI/Data/MeleeEnemy")]

    public class MeleeEnemyData : EnemyData
    {
        
        public float attackRadius = 1.5f;
        public float attackCooldown = 1f;
        public float detectionRadius = 5f;
        public float attackRange = 2f;
    }
}