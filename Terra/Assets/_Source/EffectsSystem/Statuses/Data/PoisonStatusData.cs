using Terra.EffectsSystem.Abstracts;
using Terra.EffectsSystem.Abstracts.Definitions;
using UnityEngine;


namespace Terra.EffectsSystem.Statuses.Data
{

    [CreateAssetMenu(fileName = "PoisonStatusData_", menuName = "TheBebocks/Statuses/Data/PoisonStatusData")]
    public class PoisonStatusData : TimedStatusData
    {
        public float damagePerTick;
    }
}