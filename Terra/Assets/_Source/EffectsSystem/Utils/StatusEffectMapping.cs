using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Terra.EffectsSystem.Utils
{

    /// <summary>
    ///     Holds reference types for <see cref="StatusEffectsFactory"/>
    /// </summary>
    public class StatusEffectMapping : ScriptableObject
    {
        [Serializable]
        public struct Mapping
        {
            public string dataTypeName;
            public string effectTypeName;
        }

        [ReadOnly] public List<Mapping> mappings = new();
    }
}