using Terra.Attributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Actions.Data;
using UnityEngine;

namespace Terra.EffectsSystem.Actions
{
    [ActionEffect(typeof(HealOnHitData))]
    public class HealOnHitAction : ActionEffect<HealOnHitData>
    {
        protected override void OnExecute(Entity target, Entity source = null)
        {
            if (source == null)
            {
                Debug.LogError($"When executing action {nameof(HealOnHitAction)}, source cannot be empty!");
                return;
            }
            if (source.TryGetComponent(out IHealable healable))
            {
                healable.Heal(Data.healAmount);
            }
        }
    }
}
