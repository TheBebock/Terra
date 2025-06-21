using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Attributes;
using UnityEngine;

namespace Terra.EffectsSystem
{

    /// <summary>
    ///     Holds reference types for <see cref="StatusEffectsFactory"/>
    /// </summary>
    public class StatusEffectMapping : ScriptableObject
    {
        [ReadOnly] public List<EffectsMapping> mappings = new();
    }
}