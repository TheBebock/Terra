using System;
using Terra.EffectsSystem.Abstract.Definitions;
using UnityEngine;

namespace Terra.EffectsSystem.Actions.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "HealOnHitData", menuName = "TheBebocks/Actions/Data/HealOnHitData")]
    public class HealOnHitData : ActionEffectData
    {
        public int healAmount;

        protected override float CalculateEffectPower()
        {
            return healAmount;
        }
    }
}


