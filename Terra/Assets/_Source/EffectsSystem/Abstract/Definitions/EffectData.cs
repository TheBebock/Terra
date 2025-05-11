using System;
using NaughtyAttributes;
using Terra.Utils;
using UnityEngine;

namespace Terra.EffectsSystem.Abstracts.Definitions
{
    [Serializable]
    public class EffectData : ScriptableObject
    {
        [ReadOnly] public int entityID = Constants.DEFAULT_ID;
    }
}