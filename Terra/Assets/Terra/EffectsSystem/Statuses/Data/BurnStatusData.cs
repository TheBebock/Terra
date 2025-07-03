using Terra.EffectsSystem.Abstract.Definitions;
using UnityEngine;


namespace Terra.EffectsSystem.Statuses.Data
{

    [CreateAssetMenu(fileName = "BurnStatusData_", menuName = "TheBebocks/Statuses/Data/BurnStatusData")]
    public class BurnStatusData : TimedStatusData
    {
        [Min(1)]public int damagePerTick = 1;

        protected override float CalculateEffectPower()
        {
            float totalDamage = Mathf.Round(AmountOfTicks * damagePerTick);
            return totalDamage;
        }
    }
}