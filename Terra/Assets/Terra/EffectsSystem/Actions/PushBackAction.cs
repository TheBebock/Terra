using Terra.Attributes;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Actions.Data;
using UnityEngine;

namespace Terra.EffectsSystem.Actions
{
    [ActionEffect(typeof(PushBackData))]
    public class PushBackAction : ActionEffect<PushBackData>
    {
        protected override void OnExecute(Entity target, Entity source = null)
        {
            if (source == null)
            {
                Debug.LogError($"Source cannot be empty on {nameof(PushBackAction)}");
                return;
            }
            Vector3 direction = (target.transform.position - source.transform.position).normalized;
            
            if (target.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(direction * Data.force, ForceMode.Impulse);
            }
        }
    }
}
