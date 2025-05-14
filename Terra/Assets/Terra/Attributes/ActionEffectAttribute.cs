using System;

namespace Terra.Attributes
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ActionEffectAttribute : Attribute
    {
        public Type DataType { get; }

        public ActionEffectAttribute(Type dataType)
        {
            DataType = dataType;
        }
    }
}