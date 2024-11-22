using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace AI.Data.Definitions
{
    [CreateAssetMenu(fileName = "RangedEnemy_",menuName = "TheBebocks/AI/Data/RangedEnemy")]

    public class RangedEnemyData : EnemyData
    {
        [Foldout("Statistics"), Range(0.1f, 5f)]
        public float rateOfFire = 1f;

    }
}