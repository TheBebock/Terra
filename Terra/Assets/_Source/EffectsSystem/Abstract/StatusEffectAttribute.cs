using System;

namespace Terra.EffectsSystem.Abstracts
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class StatusEffectAttribute : Attribute
    {
        public Type DataType { get; }

        public StatusEffectAttribute(Type dataType)
        {
            DataType = dataType;
        }
    }
}