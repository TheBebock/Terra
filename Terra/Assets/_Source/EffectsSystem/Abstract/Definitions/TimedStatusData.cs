using UnityEngine;

namespace Terra.EffectsSystem.Abstracts.Definitions
{
    /// <summary>
    ///     Represents data for status effects that are time based 
    /// </summary>
    public abstract class TimedStatusData : StatusEffectData
    {
        [Tooltip("In seconds"), Min(1)] public float statusDuration;
        
        [Tooltip("Rounded up")][Min(1)] public int amountOfTicksPerSecond;
    }
}