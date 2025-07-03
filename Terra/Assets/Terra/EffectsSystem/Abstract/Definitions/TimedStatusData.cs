using UnityEngine;

namespace Terra.EffectsSystem.Abstract.Definitions
{
    /// <summary>
    ///     Represents data for status effects that are time based 
    /// </summary>
    public abstract class TimedStatusData : StatusEffectData
    {
        [Tooltip("In seconds. -1 is infinity")] public float statusDuration = 1f;
        [Tooltip("In seconds"), Min(0.1f)] public float tickTime = 0.1f;
        public float AmountOfTicks => statusDuration / tickTime;
        //[Tooltip("Rounded up")][Min(1)] public int amountOfTicksPerSecond;

        protected override void OnValidate()
        {
            base.OnValidate();

            if (statusDuration > 0 && tickTime > statusDuration)
            {
                Debug.LogError($"{name}: Tick time cannot be greater than {statusDuration}." +
                               $"\nSetting tick time to 0.1");
                tickTime = 0.1f;
            }
        }
    }
}