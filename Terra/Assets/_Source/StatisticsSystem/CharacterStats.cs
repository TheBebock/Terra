using System;
using Core.ModifiableValue;
using NaughtyAttributes;
using UnityEngine;

namespace StatisticsSystem 
{
    
    /// <summary>
    /// Base class that contains statistics for characters
    /// </summary>
    [Serializable]
    public class CharacterStats
    {
        
        [Foldout("Debug")][SerializeField] private ModifiableValue strength;
        [Foldout("Debug")][SerializeField] private ModifiableValue maxHealth;
        [Foldout("Debug")][SerializeField] private ModifiableValue speed;
        
        public CharacterStats(float basestrength, float basemaxHealth, float basespeed)
        {
            strength = new ModifiableValue(basestrength);
            maxHealth = new ModifiableValue(basemaxHealth);
            speed = new ModifiableValue(basespeed);
        }

        public float Strength => strength.Value;
        public float MaxHealth => maxHealth.Value;
        public float Speed => speed.Value;

        public ModifiableValue ModifiableMaxHealth => maxHealth;
        
        public void AddStrengthModifier(ValueModifier modifier)
        {
            strength.AddStatModifier(modifier);
        }

        public void AddMaxHealthModifier(ValueModifier modifier)
        {
            maxHealth.AddStatModifier(modifier);
        }

        public void AddSpeedModifier(ValueModifier modifier)
        {
            speed.AddStatModifier(modifier);
        }
        public bool RemoveStrengthModifier(ValueModifier modifier)
        {
            return strength.RemoveStatModifier(modifier);
        }

        public bool RemoveMaxHealthModifier(ValueModifier modifier)
        {
            return maxHealth.RemoveStatModifier(modifier);
        }

        public bool RemoveSpeedModifier(ValueModifier modifier)
        {
            return speed.RemoveStatModifier(modifier);
        }
    }
}
