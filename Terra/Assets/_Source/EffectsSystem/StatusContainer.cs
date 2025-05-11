using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstracts;
using Terra.EffectsSystem.Utils;
using UnityEngine;


namespace Terra.EffectsSystem
{

    /// <summary>
    ///     Represents container for different statuses on entity
    /// </summary>
    [Serializable]
    public class StatusContainer
    {

        [SerializeField, ReadOnly] private Entity _entity;
        [SerializeField, ReadOnly] private List<StatusEffectBase> _statuses = new List<StatusEffectBase>();

        public StatusContainer(Entity entity)
        {
            _entity = entity;
        }
        
        public void TryAddEffect(StatusEffectData statusEffectData)
        {
            StatusEffectBase statusEffect = StatusEffectsFactory.CreateStatusEffect(_entity, statusEffectData);

            TryAddEffect(statusEffect);
        }
        
        public void TryAddEffect(StatusEffectBase newStatus)
        {
            if (newStatus == null)
            {
                Debug.LogError($"{this} received status can't be null!");
                return;
            }
            
            // No effect found
            if (!_statuses.TryFind(s => s.Equals(newStatus), out StatusEffectBase currentStatus))
            {
                newStatus.Apply();
                _statuses.Add(newStatus);
                return;
            }

            
            //TODO: Compare current and new status, prolong status and update values if higher

        }

        public void UpdateEffects()
        {
            for (int i = 0; i < _statuses.Count; ++i)
            {
                _statuses[i].Update();
                _statuses[i].TryRemove();
            }
        }
    }
}