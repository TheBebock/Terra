using System;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Actions.Data;
using UnityEngine;

namespace Terra.EffectsSystem.Actions
{
    [Serializable]
    public class PushBackAction : ActionEffect<PushBackData>
    {
        protected override void OnExecute(Entity target, Entity source = null)
        {
            Debug.Log($"PushBackAction {Data.force}");
        }
    }
}
