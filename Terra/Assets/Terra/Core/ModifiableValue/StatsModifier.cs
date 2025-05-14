using System;
using NaughtyAttributes;
using Terra.ID;
using Terra.Utils;

namespace Terra.Core.ModifiableValue
{
    public enum StatModType: int
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300,
    }

    [Serializable]
    public class ValueModifier
    {
        public float Value;
        public StatModType Type;
        [ReadOnly] public int SourceID;
        [ReadOnly] public readonly int Order;

        public ValueModifier(float value, StatModType type, int sourceID = Constants.DEFAULT_ID)
        {
            Value = value;
            Type = type;
            Order = (int)type;
            SourceID = sourceID;
        }
        
        public ValueModifier(float value, StatModType type, object source) : this(value, type)
        {
            // Check for null
            if (source == null) return;
            // Check is object an IUniquable
            if (source is not IUniqueable unique) return;

             SourceID = unique.Identity;
        }
        
    }
}
