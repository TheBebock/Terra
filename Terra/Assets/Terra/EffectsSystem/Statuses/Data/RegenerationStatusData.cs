using Terra.EffectsSystem.Abstract.Definitions;
using UnityEngine;


namespace Terra.EffectsSystem.Statuses.Data
{

    [CreateAssetMenu(fileName = "RegenerationStatusData", menuName = "TheBebocks/Statuses/Data/RegenerationStatusData")]
    public class RegenerationStatusData : TimedStatusData
    {
        [Min(1)]public int healPerTick = 1;

        protected override float CalculateEffectPower()
        {
            float totalDamage = Mathf.Round(AmountOfTicks * healPerTick);
            return totalDamage;
        }
    }
}