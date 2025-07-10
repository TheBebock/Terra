using System;
using JetBrains.Annotations;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.Particles;
using UnityEngine;

namespace Terra.EffectsSystem.Abstract
{

    /// <summary>
    ///     Represents base class for all status effects
    /// </summary>
    //NOTE: Class should be Abstract, but it cannot be due to serialization
    [Serializable]
    public class StatusEffectBase : EffectBase
    {
        [UsedImplicitly]
        [SerializeField, ReadOnly] private string _name;
        
        protected StatusEffectBase(){}
        public void Apply()
        {
            Type type = GetType();
            _name = type.Name;
            InternalApply();
        }

        public void Update()
        {
            InternalUpdate();
        }

        public bool TryRemove(bool force = false)
        {
           return InternalRemove(force);
        }

        public virtual void Reset()
        {
        }
        protected virtual void InternalApply()
        {
        }

        protected virtual void InternalUpdate()
        {
        }

        protected virtual bool InternalRemove(bool force = false)
        {
            return false;
        }
        protected virtual void InternalReset()
        {
        }
    }

    public abstract class StatusEffect<TStatusData> : StatusEffectBase
        where TStatusData : StatusEffectData
    {
        protected abstract bool CanBeRemoved { get; }

        private TStatusData _typedData;
        public TStatusData Data => _typedData;
        public sealed override int EffectCost => _typedData.effectCost;

        public sealed override float GetEffectPower => Data.GetEffectPower();


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

            if (Data.effectParticle)
            {
                VFXController.SpawnAndAttachParticleToEntityOnAnchor(entity, Data.effectParticle);
            }
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

        protected sealed override bool InternalRemove(bool force = false)
        {
            if (_typedData == null)
            {
                Debug.LogError($"On Remove {this} data is null");
                return false;
            }

            if (!CanBeRemoved && !force) return false;

            if (Data.effectParticle)
            {
                VFXController.RemoveParticleFromEntity(entity, Data.effectParticle);
            }
            OnRemove();
            
            return true;
        }
        
        protected sealed override void InternalReset()
        {
            if (_typedData == null)
            {
                Debug.LogError($"On Update {this} data is null");
                return;
            }

            OnReset();
        }

        protected abstract void OnApply();
        protected abstract void OnUpdate();
        protected abstract void OnRemove();
        protected abstract void OnReset();
    }
}