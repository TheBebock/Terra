using System;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract.Definitions;

namespace Terra.EffectsSystem.Abstract
{

    /// <summary>
    ///     Represents base class for all effects.
    /// </summary>
    [Serializable]
    public class EffectBase
    {
        protected Entity entity;



        public virtual float GetEffectPower => -1;

        /// <summary>
        ///     Initializes effect with data
        /// </summary>
        public virtual void Initialize(Entity target, EffectData effectData)
        {
            entity = target;
        }
    }
}
