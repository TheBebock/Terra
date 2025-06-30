using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.EffectsSystem.Actions;
using Terra.Extensions;

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


        public void AddNewStatusEffect(StatusEffectData statusToAdd, List<EffectData> statusesToDelete = null)
        {
            if (statusesToDelete != null)
            {
                foreach (var statusToDelete in statusesToDelete)
                {
                    var match = statuses.FirstOrDefault(a => a.GetType() == statusToDelete.GetType());
                    if (match != null)
                    {
                        statuses.Remove(match);
                    }
                }
            }
            
            // If there exists a status of same type that is stronger, delete the current one, otherwise skip the new one
            if (statuses.TryFind(s =>  s.GetType() == statusToAdd.GetType(), out StatusEffectData existingStatus))
            {
                if (statusToAdd.GetEffectPower() > existingStatus.GetEffectPower())
                {
                    statuses.RemoveElement(existingStatus);
                    statuses.AddUnique(statusToAdd);
                }
                return;
            }
            
            statuses.AddUnique(statusToAdd);
        }
        
        public void AddNewActionEffect(ActionEffectData actionToAdd, List<EffectData> actionsToDelete = null)
        {
            if (actionsToDelete != null)
            {
                foreach (var actionToDelete in actionsToDelete)
                {
                    var match = actions.FirstOrDefault(a => a.GetType() == actionToDelete.GetType());
                    if (match != null)
                    {
                        actions.Remove(match);
                    }
                }
            }

            // If there exists an action of same type that is stronger, replace the current effect, otherwise skip the new one
            if (actions.TryFind(a =>  a.GetType() == actionToAdd.GetType(), out ActionEffectData existingAction))
            {
                if (actionToAdd.GetEffectPower() > existingAction.GetEffectPower())
                {
                    actions.RemoveElement(existingAction);
                    actions.AddUnique(actionToAdd);
                }
                return;
            }
            actions.AddUnique(actionToAdd);
        }
        
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