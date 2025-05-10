using Terra.Combat;
using Terra.EffectsSystem.Abstracts;
using Terra.EffectsSystem.Statuses.Data;

namespace Terra.EffectsSystem.Statuses
{
    [StatusEffect(typeof(PoisonStatusData))]
    public class PoisonStatus : TimedStatus<PoisonStatusData>
    {
        IDamageable _damageable;
        protected override void OnApply()
        {
            base.OnApply();
            if (entity.TryGetComponent(out IDamageable damageable))
            {
                _damageable = damageable;
                //TODO: Add visual effect
            }
        }

        protected override void OnStatusTick()
        {
            _damageable?.TakeDamage(Data.damagePerTick);
        }

        protected override void OnRemove()
        {
            base.OnRemove();
            //TODO: remove visual effect
        }
    }
}