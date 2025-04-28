using System;
using UnityEngine;

namespace Terra.EffectsSystem.Abstracts
{

    /// <summary>
    ///     Represents base class for all status effects
    /// </summary>
    [Serializable]
    public class StatusEffectBase : EffectBase
    {
        public void Apply()
        {
        }

        public void Update()
        {
        }

        public void Remove()
        {
        }

        protected virtual void InternalApply()
        {
        }

        protected virtual void InternalUpdate()
        {
        }

        protected virtual void InternalRemove()
        {
        }
    }

    public abstract class StatusEffect<TStatusData> : StatusEffectBase
        where TStatusData : StatusEffectData
    {

        protected abstract bool CanBeRemoved { get; }

        private TStatusData _typedData;
        public TStatusData Data => _typedData;

        public override void Initialize(EffectData effectData)
        {
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

        protected override void InternalApply()
        {
            if (_typedData == null)
            {
                Debug.LogError($"On Apply {this} data is null");
                return;
            }

            OnApply();
        }

        protected override void InternalUpdate()
        {
            if (_typedData == null)
            {
                Debug.LogError($"On Update {this} data is null");
                return;
            }

            OnUpdate();
        }

        protected override void InternalRemove()
        {
            if (_typedData == null)
            {
                Debug.LogError($"On Remove {this} data is null");
                return;
            }

            if (!CanBeRemoved) return;

            OnRemove();
        }

        protected abstract void OnApply();
        protected abstract void OnUpdate();
        protected abstract void OnRemove();
    }
}