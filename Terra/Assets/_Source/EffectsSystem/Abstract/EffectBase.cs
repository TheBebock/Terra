using UnityEngine;

namespace Terra.EffectsSystem.Abstracts
{

    /// <summary>
    ///     Represents base class for all effects.
    /// </summary>
    public class EffectBase : ScriptableObject
    {
        /// <summary>
        ///     Initializes effect with data
        /// </summary>
        public virtual void Initialize(EffectData effectData)
        {
        }
    }
}
