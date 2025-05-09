using System;
using Terra.Core.Generics;
using UnityEngine;

namespace Terra.EffectsSystem.Abstracts
{

    /// <summary>
    ///     Represents base class for all status effects
    /// </summary>
    //NOTE: Class should be Abstract, but it cannot be due to serialization
    [Serializable]
    public class StatusEffectBase : EffectBase
    {
        
        protected StatusEffectBase(){}
        public void Apply()
        {
        }

        public void Update()
        {
        }

        public void Remove(bool force = false)
        {
        }

        public void Reset()
        {
            
        }
        protected virtual void InternalApply()
        {
        }

        protected virtual void InternalUpdate()
        {
        }

        protected virtual void InternalRemove(bool force = false)
        {
        }
    }

    public abstract class StatusEffect<TStatusData> : StatusEffectBase
        where TStatusData : StatusEffectData
    {
        protected abstract bool CanBeRemoved { get; }

        private TStatusData _typedData;
        protected TStatusData Data => _typedData;

        public override void Initialize(Entity target, EffectData effectData)
        {
            base.Initialize(target, effectData);
            
            if (effectData == null)
            {
                Debug.LogError(this + "ActionEffectData is null");
                return;
            }

            if (effectData is TStatusData data)
            {
                _typedData = data;
            }
            else
            {
                Debug.LogError($"{effectData} is not of type {typeof(TStatusData).Name}");
            }
        }
        

        protected sealed override void InternalApply()
        {
            if (_typedData == null)
            {
                Debug.LogError($"On Apply {this} data is null");
                return;
            }

            //TODO: Add VFX
            OnApply();
        }

        protected sealed override void InternalUpdate()
        {
            if (_typedData == null)
            {
                Debug.LogError($"On Update {this} data is null");
                return;
            }

            OnUpdate();
        }

        protected sealed override void InternalRemove(bool force = false)
        {
            if (_typedData == null)
            {
                Debug.LogError($"On Remove {this} data is null");
                return;
            }

            if (!CanBeRemoved && !force) return;

            //TODO: Remove VFX
            OnRemove();
        }

        protected abstract void OnApply();
        protected abstract void OnUpdate();
        protected abstract void OnRemove();
    }
}