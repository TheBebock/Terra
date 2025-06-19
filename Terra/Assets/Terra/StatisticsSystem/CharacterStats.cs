using System;
using NaughtyAttributes;
using Terra.Core.ModifiableValue;
using UnityEngine;

namespace Terra.StatisticsSystem 
{
    
    /// <summary>
    /// Base class that contains statistics for characters
    /// </summary>
    [Serializable]
    public class CharacterStats
    {
        
        [Foldout("Debug")][SerializeField] private ModifiableValue strength;
        [Foldout("Debug")][SerializeField] private ModifiableValue maxHealth;
        [Foldout("Debug")][SerializeField] private ModifiableValue dexterity;
        [Foldout("Debug")][SerializeField] private ModifiableValue luck;
        
        public CharacterStats(int baseStrength, int baseMaxHealth, int baseSpeed, int baseLuck)
        {
            strength = new ModifiableValue(baseStrength);
            maxHealth = new ModifiableValue(baseMaxHealth);
            dexterity = new ModifiableValue(baseSpeed);
            luck = new ModifiableValue(baseLuck);
        }

        public int Strength => strength.Value;
        public int MaxHealth => maxHealth.Value;
        public int Dexterity => dexterity.Value;
        public float Luck => luck.Value;

        public ModifiableValue ModifiableMaxHealth => maxHealth;
        
        public void AddStrengthModifier(ValueModifier modifier)
        {
            strength.AddStatModifier(modifier);
        }

        public void AddMaxHealthModifier(ValueModifier modifier)
        {
            maxHealth.AddStatModifier(modifier);
        }

        public void AddDexterityModifier(ValueModifier modifier)
        {
            dexterity.AddStatModifier(modifier);
        }
        public bool RemoveStrengthModifier(ValueModifier modifier)
        {
            return strength.RemoveStatModifier(modifier);
        }

        public bool RemoveMaxHealthModifier(ValueModifier modifier)
        {
            return maxHealth.RemoveStatModifier(modifier);
        }

        public bool RemoveDexterityModifier(ValueModifier modifier)
        {
            return dexterity.RemoveStatModifier(modifier);
        }
        public void AddLuckModifier(ValueModifier modifier)
        {
            luck.AddStatModifier(modifier);
        }

        public bool RemoveLuckModifier(ValueModifier modifier)
        {
            return luck.RemoveStatModifier(modifier);
        }
    }
}
