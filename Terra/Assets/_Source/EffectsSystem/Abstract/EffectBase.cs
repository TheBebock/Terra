using System;
using UnityEngine;

namespace Terra.EffectsSystem.Abstracts
{

    /// <summary>
    ///     Represents base class for all effects.
    /// </summary>
    [Serializable]
    public class EffectBase
    {
        /// <summary>
        ///     Initializes effect with data
        /// </summary>
        public virtual void Initialize(EffectData effectData)
        {
        }
    }
}
