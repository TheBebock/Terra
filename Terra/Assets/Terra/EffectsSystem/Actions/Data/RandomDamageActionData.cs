using System;
using Terra.EffectsSystem.Abstract.Definitions;
using UnityEngine;

namespace Terra.EffectsSystem.Actions.Data
{
    
    [CreateAssetMenu(fileName = "RandomDamageData", menuName = "TheBebocks/Actions/Data/RandomDamageData")]
    public class RandomDamageActionData : ActionEffectData
    {
        public Vector2 damageRange = new Vector2(0, 50);
        protected override float CalculateEffectPower()
        {
            return damageRange.y;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if(damageRange.x < 0) damageRange.x = 0;
            if(damageRange.y < damageRange.x) damageRange.y = damageRange.x;
        }
    }
}
