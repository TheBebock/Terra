using System;
using NaughtyAttributes;
using Terra.Utils;
using UnityEngine;

namespace Terra.EffectsSystem.Abstract.Definitions
{
    [Serializable]
    public class EffectData : ScriptableObject
    {
        [ReadOnly] public int entityID = Constants.DEFAULT_ID;

        public Sprite effectIcon;
        public string effectName;
        public string effectDescription;
        public ParticleSystem effectParticle;

        public virtual string GetUIInfo()
        {
            return "";
        }
    }
}