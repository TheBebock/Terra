using Terra.EffectsSystem.Abstracts;
using Terra.EffectsSystem.Statuses.Data;
using UnityEngine;

namespace Terra.EffectsSystem.Statuses
{

    [CreateAssetMenu(fileName = "PoisonStatus", menuName = "TheBebocks/Statuses/PoisonStatus")]
    public class PoisonStatus : StatusEffect<PoisonStatusData>
    {
        protected override bool CanBeRemoved { get; }

        protected override void OnApply()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnRemove()
        {
            throw new System.NotImplementedException();
        }
    }
}