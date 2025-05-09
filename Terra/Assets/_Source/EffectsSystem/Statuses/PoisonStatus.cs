using Terra.EffectsSystem.Abstracts;
using Terra.EffectsSystem.Statuses.Data;

namespace Terra.EffectsSystem.Statuses
{
    [StatusEffect(typeof(PoisonStatusData))]
    public class PoisonStatus : TimedStatus<PoisonStatusData>
    {
        IDamagable _damagable;
        protected override void OnApply()
        {
            base.OnApply();
            if (entity.TryGetComponent(out IDamagable damagable))
            {
                _damagable = damagable;
                //TODO: Add visual effect
            }
        }

        protected override void OnStatusTick()
        {
            _damagable?.TakeDamage(Data.damagePerTick);
        }

        protected override void OnRemove()
        {
            base.OnRemove();
            //TODO: remove visual effect
        }
    }
}