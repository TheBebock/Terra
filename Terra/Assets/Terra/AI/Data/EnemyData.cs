using UnityEngine.Serialization;

namespace Terra.AI.Data
{
    public abstract class EnemyData : AIData
    {
        [FormerlySerializedAs("attackRange")] public float normalAttackRange;
    }
}