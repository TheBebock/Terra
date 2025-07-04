using Terra.Attributes;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Actions.Data;
using UnityEngine;

namespace Terra.EffectsSystem.Actions
{    
    [ActionEffect(typeof(RandomDamageActionData))]
    public class RandomDamageAction : ActionEffect<RandomDamageActionData>
    {
        protected override void OnExecute(Entity target, Entity source = null)
        {
            int damage = Mathf.RoundToInt( Random.Range(Data.damageRange.x, Data.damageRange.y));

            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}
