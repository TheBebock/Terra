using System;
using NaughtyAttributes;
using Terra.Core.ModifiableValue;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.StatisticsSystem 
{

    public enum StatisticType
    {
        MaxHealth = 0,
        Strength = 1,
        Dexterity = 2,
        Luck = 3,
        SwingSpeed = 4,
        MeleeRange = 5,
        RangeCooldown = 6,
    }
    
    /// <summary>
    /// Base class that contains statistics for characters
    /// </summary>
    [Serializable]
    public class CharacterStats
    {
        
        [FormerlySerializedAs("strength")] [Foldout("Debug")][SerializeField] private ModifiableValue _strength;
        [FormerlySerializedAs("maxHealth")] [Foldout("Debug")][SerializeField] private ModifiableValue _maxHealth;
        [FormerlySerializedAs("dexterity")] [Foldout("Debug")][SerializeField] private ModifiableValue _dexterity;
        
        public CharacterStats(int baseStrength, int baseMaxHealth, int baseSpeed)
        {
            _strength = new ModifiableValue(baseStrength);
            _maxHealth = new ModifiableValue(baseMaxHealth);
            _dexterity = new ModifiableValue(baseSpeed);
        }

        public int Strength => _strength.Value;
        public int MaxHealth => _maxHealth.Value;
        public int Dexterity => _dexterity.Value;

        public ModifiableValue ModifiableMaxHealth => _maxHealth;
        
        public void AddStrengthModifier(ValueModifier modifier)
        {
            _strength.AddStatModifier(modifier);
        }

        public void AddMaxHealthModifier(ValueModifier modifier)
        {
            _maxHealth.AddStatModifier(modifier);
        }

        public void AddDexterityModifier(ValueModifier modifier)
        {
            _dexterity.AddStatModifier(modifier);
        }
        public bool RemoveStrengthModifier(ValueModifier modifier)
        {
            return _strength.RemoveStatModifier(modifier);
        }

        public bool RemoveMaxHealthModifier(ValueModifier modifier)
        {
            return _maxHealth.RemoveStatModifier(modifier);
        }

        public bool RemoveDexterityModifier(ValueModifier modifier)
        {
            return _dexterity.RemoveStatModifier(modifier);
        }
        
    }
}
