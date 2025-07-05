using Terra.Attributes;
using Terra.Combat;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Statuses.Data;
using UnityEngine;

namespace Terra.EffectsSystem.Statuses
{
    [StatusEffect(typeof(RegenerationStatusData))]
    public class RegenerationStatus : TimedStatus<RegenerationStatusData>
    {
        private IHealable _healable;
        protected override void OnApply()
        {
            base.OnApply();
            if (entity.TryGetComponent(out IHealable healable))
            {
                _healable = healable;
            }
            else
            {
                Debug.LogError($"Not found {nameof(IHealable)} on entity {entity.gameObject.name}");
            }
        }

        protected override void OnStatusTick()
        {
            _healable?.Heal(Data.healPerTick);
        }
        
    }
}