using Terra.Attributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Actions.Data;
using UnityEngine;

namespace Terra.EffectsSystem.Actions
{
    [ActionEffect(typeof(InstaHealData))]
    public class InstaHealAction : ActionEffect<InstaHealData>
    {
        protected override void OnExecute(Entity target, Entity source = null)
        {
            if (source == null)
            {
                Debug.LogError($"When executing action {nameof(InstaHealAction)}, source cannot be empty!");
                return;
            }
            if (source.TryGetComponent(out IHealable healable))
            {
                healable.Heal(Data.amount, Data.isPercentage);
            }
        }
    }
}
