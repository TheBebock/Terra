using System;
using Terra.Constants;

namespace Core.ModifiableValue
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
        public readonly int Order;
        public readonly int SourceID;

        public ValueModifier(float value, StatModType type, int order, int sourceID =  Utils.DEFAULT_ID)
        {
            Value = value;
            Type = type;
            Order = order;
            SourceID = sourceID;
        }

        public ValueModifier(float value, StatModType type) : this(value, type, (int)type, Utils.DEFAULT_ID)
        {
        }

        public ValueModifier(float value, StatModType type, int order) : this(value, type, order,  Utils.DEFAULT_ID)
        {
        }

        public ValueModifier(float value, StatModType type, object source) : this(value, type, (int)type,  Utils.DEFAULT_ID)
        {
        }

    }
}
