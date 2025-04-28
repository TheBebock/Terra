using UnityEngine;

namespace Terra.EffectsSystem.Abstracts
{
    public class StatusEffectData : EffectData
    {
        [Tooltip("In seconds"), Min(0.1f)] public float duration;
        [Min(0.1f)] public float ticksPerSecond;
    }
}

