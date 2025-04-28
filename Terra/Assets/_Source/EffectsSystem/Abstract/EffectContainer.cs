using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstracts;

namespace Terra.EffectsSystem
{

    
    [Serializable]
    public class EffectsContainer{

        [Expandable]
        public List<StatusEffectData> _statuses = new ();
        [Expandable]
        public List<ActionEffectData> _actions = new ();

        
        public void ExecuteActions(Entity source, Entity target)
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                //TODO:Factory for actions
                //_actions[i].Execute(source, target);
            }
        }

        public void ApplyStatuses(Entity target)
        {
            for (int i = 0; i < _statuses.Count; i++)
            {
                target.StatusContainer.TryAddEffect(_statuses[i]);
            }
        }
    }
}