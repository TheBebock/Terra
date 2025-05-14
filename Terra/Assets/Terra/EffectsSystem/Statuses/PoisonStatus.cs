using Terra.Combat;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Statuses.Data;
using UnityEngine;

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
            Debug.Log($"{entity.name} poison damaged {Data.damagePerTick}");
            _damageable?.TakeDamage(Data.damagePerTick);
        }
        
    }
}