using Terra.EffectsSystem.Abstract.Definitions;
using UnityEngine;


namespace Terra.EffectsSystem.Statuses.Data
{

    [CreateAssetMenu(fileName = "PoisonStatusData_", menuName = "TheBebocks/Statuses/Data/PoisonStatusData")]
    public class PoisonStatusData : TimedStatusData
    {
        public float damagePerTick;
    }
}