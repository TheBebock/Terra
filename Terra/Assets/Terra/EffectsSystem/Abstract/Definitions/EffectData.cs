using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Components;
using Terra.Utils;
using UnityEngine;

namespace Terra.EffectsSystem.Abstract.Definitions
{

    [Serializable]
    public class EffectData : ScriptableObject
    {
        [ReadOnly] public int entityID = Constants.DefaultID;

        public Sprite effectIcon;
        public string effectName;
        public string effectDescription;
        [Min(1)] public int effectCost = 50;
        public ParticleComponent effectParticle;
        
    

        /// <summary>
        /// See description at: <see cref="ContainerType"/>
        /// </summary>
        public ContainerType containerType;
        [Tooltip("Effect to delete when applying this effect")]
        [ShowIf(nameof(ShowIncompatibleEffects))] public List<EffectData> incompatibleEffects = new();
        
        /// <summary>
        ///     Returns overall power of the effect. Can be used to compare effects of the same type.
        /// </summary>
        public virtual float GetEffectPower(){return -1;}
        
        private bool ShowIncompatibleEffects() => containerType is not ContainerType.None;
        protected void OnValidate()
        {
            if (!ShowIncompatibleEffects())
            {
                incompatibleEffects.Clear();
                return;
            }
            for (int i = incompatibleEffects.Count-1; i >= 0; i--)
            {
                if(incompatibleEffects[i] == null) continue;
                if (!ValidateIncompatibleEffect(incompatibleEffects[i]))
                {
                    incompatibleEffects.RemoveAt(i);
                }
            }
            
        }

        private bool ValidateIncompatibleEffect(EffectData incompatibleEffect)
        {
            if (incompatibleEffect != null)
            {
                if (incompatibleEffect == this)
                {
                    Debug.LogError($"Effect cannot be incompatible with self");
                    return false;
                }
                if (incompatibleEffect.containerType != containerType)
                {
                    Debug.LogError($"{incompatibleEffect} is not of type {containerType}. " +
                                   $"Only effects with the same container type ({containerType}) are supported.");
                    return false;
                }


                bool effectType = GetType().IsSubclassOf(typeof(StatusEffectData));
                bool incompatibleEffectType = GetType().IsSubclassOf(typeof(StatusEffectData));
                if (effectType != incompatibleEffectType)
                {
                    string msg = effectType == true ? $"{nameof(StatusEffectData)}" : $"{nameof(ActionEffectData)}";
                    Debug.LogError($"{incompatibleEffect} does not inherit from {msg}." +
                                   $"Only effects of type {msg} are supported.");
                    return false;
                }

                return true;
            }
            
            return false;
        }
    }
}