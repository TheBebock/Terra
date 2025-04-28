using System;
using System.Collections.Generic;
using Terra.EffectsSystem.Abstracts;


namespace Terra.EffectsSystem
{

    /// <summary>
    ///     Represents container for different statuses on entity
    /// </summary>
    [Serializable]
    public class StatusContainer
    {

        List<StatusEffectBase> _statuses = new List<StatusEffectBase>();

        public void TryAddEffect(StatusEffectBase newStatus)
        {
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
            }
        }
    }
}