using System;
using Terra.EffectsSystem.Abstract.Definitions;
using UnityEngine;

namespace Terra.EffectsSystem.Actions.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "PushbackData_", menuName = "TheBebocks/Actions/Data/PushbackData")]
    public class PushBackData : ActionEffectData
    {
        public float force = 10f;

        protected override float CalculateEffectPower()
        {
            return force;
        }
    }
}


