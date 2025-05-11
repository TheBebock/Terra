using System;
using Terra.EffectsSystem.Abstracts;
using Terra.EffectsSystem.Abstracts.Definitions;
using UnityEngine;

namespace Terra.EffectsSystem.Actions.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "PushbackData_", menuName = "TheBebocks/Actions/Data/PushbackData")]
    public class PushBackData : ActionEffectData
    {
        public float force = 10f;
    }
}


