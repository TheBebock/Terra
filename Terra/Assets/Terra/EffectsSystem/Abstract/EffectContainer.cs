using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.EffectsSystem.Actions;

namespace Terra.EffectsSystem.Abstract
{

    /// <summary>
    ///     Holds status and actions effects
    /// </summary>
    [Serializable]
    public class EffectsContainer
    {

        [Expandable]
        public List<StatusEffectData> statuses = new ();
        [Expandable]
        public List<ActionEffectData> actions = new ();

        
        public void ExecuteActions(Entity source, Entity target)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                ActionEffectsDatabase.Instance.ExecuteAction(actions[i], target, source);
            }
        }

        public void ApplyStatuses(IDamageable target)
        {
            for (int i = 0; i < statuses.Count; i++)
            {
                target.StatusContainer.TryAddEffect(statuses[i]);
            }
        }
    }
}