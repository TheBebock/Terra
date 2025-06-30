using UnityEngine;

namespace Terra.AI.Data
{
    public class TankEnemyData : MeleeEnemyData
    {
        [SerializeField, Min(1f)] public float attackDuration;
    }   
}
