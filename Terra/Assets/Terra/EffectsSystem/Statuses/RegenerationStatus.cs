using Terra.Attributes;
using Terra.Combat;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Statuses.Data;

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
        }

        protected override void OnStatusTick()
        {
            _healable?.Heal(Data.healPerTick);
        }
        
    }
}