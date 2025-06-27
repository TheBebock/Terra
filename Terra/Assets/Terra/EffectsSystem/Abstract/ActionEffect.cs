using System;
using JetBrains.Annotations;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract.Definitions;
using UnityEngine;

namespace Terra.EffectsSystem.Abstract
{
    /// <summary>
    ///     Represents base class for action effects
    /// </summary>
    [Serializable]
    //NOTE: Class should be abstract, but due to serialization it cannot
    public class ActionEffectBase : EffectBase
    {
        [UsedImplicitly]
        [SerializeField, ReadOnly] private string _effectType;
        public ActionEffectBase()
        {
            _effectType = GetType().Name;
        }
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

        public sealed override void Initialize(Entity target, EffectData actionEffectData)
        {
            base.Initialize(target, actionEffectData);
            
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

        
        protected sealed override void InternalExecute(Entity target, Entity source = null)
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
