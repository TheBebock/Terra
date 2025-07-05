using Terra.Attributes;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Actions.Data;
using Terra.Player;
using UnityEngine;

namespace Terra.EffectsSystem.Actions
{
    [ActionEffect(typeof(InstaHealData))]
    public class InstaHealPlayerAction : ActionEffect<InstaHealData>
    {
        protected override void OnExecute(Entity target, Entity source = null)
        {
            if (PlayerManager.Instance?.PlayerEntity == null)
            {
                Debug.LogError($"Player Entity not found while executing {nameof(InstaHealPlayerAction)}");
                return;
            }

            PlayerManager.Instance.PlayerEntity.Heal(Data.amount, Data.isPercentage);
        }
    }
}
