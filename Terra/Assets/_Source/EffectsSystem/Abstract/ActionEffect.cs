using System;
using Terra.Core.Generics;
using UnityEngine;

namespace Terra.EffectsSystem.Abstracts
{
    /// <summary>
    ///     Represents base class for action effects
    /// </summary>
    [Serializable]
    ///NOTE: Class should be abstract, but due to serialization it cannot
    public class ActionEffectBase : EffectBase
    {
        public void Execute(Entity target, Entity source = null)
        {
            InternalExecute(target, source);
        }

        protected virtual void InternalExecute(Entity target, Entity source = null)
        {
        }
    }


    /// <summary>
    ///     Represents action effect with data
    /// </summary>
    [Serializable]
    public abstract class ActionEffect<TActionData> : ActionEffectBase
        where TActionData : ActionEffectData
    {
        private TActionData _typedData;
        public TActionData Data => _typedData;

        public override void Initialize(EffectData actionEffectData)
        {
            if (actionEffectData == null)
            {
                Debug.LogError(this + "ActionEffectData is null");
                return;
            }

            if (actionEffectData is TActionData data)
            {
                _typedData = data;
            }
            else
            {
                Debug.LogError($"{actionEffectData} is not of type {typeof(TActionData).Name}");
            }
        }

        
        protected override void InternalExecute(Entity target, Entity source = null)
        {
            if (_typedData == null)
            {
                Debug.LogError($"{this} data is null");
                return;
            }

            OnExecute(target, source);
        }

        protected abstract void OnExecute(Entity target, Entity source = null);
    }
}
