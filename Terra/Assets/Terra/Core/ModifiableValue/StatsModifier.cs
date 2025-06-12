using System;
using NaughtyAttributes;
using Terra.ID;
using Terra.Utils;
using UnityEngine;

namespace Terra.Core.ModifiableValue
{
    public enum StatModType: int
    {
        Flat = 100,
        PercentMult = 300,
    }

    [Serializable]
    public class ValueModifier 
    {
        public int Value;
        public StatModType Type;
        [HideInInspector, ReadOnly] public int SourceID;
        [HideInInspector, ReadOnly] public readonly int Order;

        public ValueModifier(int value, StatModType type, int sourceID = Constants.DEFAULT_ID)
        {
            Value = value;
            Type = type;
            Order = (int)type;
            SourceID = sourceID;
        }
        
        public ValueModifier(int value, StatModType type, object source) : this(value, type)
        {
            // Check for null
            if (source == null) return;
            // Check is object an IUniquable
            if (source is not IUniqueable unique) return;

             SourceID = unique.Identity;
        }
        
    }
}
