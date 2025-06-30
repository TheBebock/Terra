using System;

namespace Terra.EffectsSystem.Abstract.Definitions
{

    /// <summary>
    ///     Represents data used by action effects
    /// </summary>
    [Serializable]
    public abstract class ActionEffectData : EffectData
    {
        public sealed override float GetEffectPower()
        {
            return CalculateEffectPower();
        }
        
        protected abstract float CalculateEffectPower();
    }
}
