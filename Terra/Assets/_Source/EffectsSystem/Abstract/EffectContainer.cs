using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstracts;

namespace Terra.EffectsSystem
{
    [Serializable]
    public struct ActionDataContainer
    {
        public ActionEffectBase actionEffect;
        [Expandable]
        public EffectData actionEffectData;

        public void Execute(Entity source, Entity target) => actionEffect.Execute( source,  target);
        public void Initialize() => actionEffect.Initialize(actionEffectData);
    }
    
    [Serializable]
    public struct StatusDataContainer
    {
        public StatusEffectBase statusEffect;
        [Expandable]
        public EffectData statusEffectData;

        public void Apply() => statusEffect.Apply();
        public void Initialize() => statusEffect.Initialize(statusEffectData);
    }
    
    [Serializable]
    public class EffectsContainer{

        
        public List<StatusDataContainer> _statuses = new ();
        public List<ActionDataContainer> _actions = new ();


        public void InitializeEffects()
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                _actions[i].Initialize();
            }
            
            for (int i = 0; i < _statuses.Count; i++)
            {
                _statuses[i].Initialize();
            }
        }
        
        public void ExecuteActions(Entity source, Entity target)
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                _actions[i].Execute(source, target);
            }
        }

        public void ApplyStatuses(Entity target)
        {
            for (int i = 0; i < _statuses.Count; i++)
            {
                target.StatusContainer.TryAddEffect(_statuses[i].statusEffect);
            }
        }
    }
}