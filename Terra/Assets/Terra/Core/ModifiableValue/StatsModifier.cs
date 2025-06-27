using System;
using NaughtyAttributes;
using Terra.ID;
using Terra.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Core.ModifiableValue
{
    public enum StatModType
    {
        Flat = 100,
        PercentMult = 300,
    }

    [Serializable]
    public class ValueModifier 
    {
        [FormerlySerializedAs("Value")] public int value;
        [FormerlySerializedAs("Type")] public StatModType type;
        [FormerlySerializedAs("SourceID")] [HideInInspector, ReadOnly] public int sourceID;
        public readonly int order;

        public ValueModifier(int value, StatModType type, int sourceID = Constants.DefaultID)
        {
            this.value = value;
            this.type = type;
            order = (int)type;
            this.sourceID = sourceID;
        }
        
        public ValueModifier(int value, StatModType type, object source) : this(value, type)
        {
            // Check for null
            if (source == null) return;
            // Check is object an IUniquable
            if (source is not IUniqueable unique) return;

             sourceID = unique.Identity;
        }
        
    }
}
