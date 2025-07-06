using System;
using Terra.Core.ModifiableValue;
using Terra.Enums;
using Terra.StatisticsSystem.Definitions;
using Terra.Utils;
using UnityEngine;


namespace Terra.StatisticsSystem
{
    [Serializable]
    public class PlayerStats : CharacterStats
    {
        [SerializeField] private ModifiableValue _luck;
        [SerializeField] private ModifiableValue _swingSpeed;
        [SerializeField] private ModifiableValue _meleeRange;
        [SerializeField] private ModifiableValue _rangeCooldown;
        public int Luck => _luck.Value;
        public int SwingSpeed => _swingSpeed.Value;
        public int MeleeRange => _meleeRange.Value;
        public int RangeCooldown => _rangeCooldown.Value;
        public ModifiableValue MaxHealthValue => base.ModifiableMaxHealth;

        
        public PlayerStats(int baseStrength, int baseMaxHealth, int baseSpeed, int baseLuck,
            int swingSpeed, int meleeRange, int rangeCooldown) : base(baseStrength,baseMaxHealth,baseSpeed)
        {
            _luck = new ModifiableValue(baseLuck);
            _swingSpeed = new ModifiableValue(swingSpeed);
            _meleeRange = new ModifiableValue(meleeRange);
            _rangeCooldown = new ModifiableValue(rangeCooldown);
        }

        public PlayerStats(PlayerStatsDefinition data)
            : this(data.baseStrength, data.baseMaxHealth, data.baseDexterity, data.baseLuck,
                data.baseSwingSpeed, data.baseMeleeRange, data.baseRangeCooldown)
        {

            int modifier = 1;
            switch (GameSettings.DefaultDifficultyLevel)
            {
                case GameDifficulty.Easy : 
                    modifier = 2;
                    break;
                case GameDifficulty.Cyberiada : 
                    modifier = 3;
                    break;
            }
            _maxHealth = new ModifiableValue(data.baseMaxHealth * modifier);
                
        }
        

        // Luck
        public void AddLuckModifier(ValueModifier modifier)
        {
            _luck.AddStatModifier(modifier);
        }

        public bool RemoveLuckModifier(ValueModifier modifier)
        {
            return _luck.RemoveStatModifier(modifier);
        }

        // Swing Speed
        public void AddSwingSpeedModifier(ValueModifier modifier)
        {
            _swingSpeed.AddStatModifier(modifier);
        }

        public bool RemoveSwingSpeedModifier(ValueModifier modifier)
        {
            return _swingSpeed.RemoveStatModifier(modifier);
        }

        // Melee Range
        public void AddMeleeRangeModifier(ValueModifier modifier)
        {
            _meleeRange.AddStatModifier(modifier);
        }

        public bool RemoveMeleeRangeModifier(ValueModifier modifier)
        {
            return _meleeRange.RemoveStatModifier(modifier);
        }

        // Range Cooldown
        public void AddRangeCooldownModifier(ValueModifier modifier)
        {
            _rangeCooldown.AddStatModifier(modifier);
        }

        public bool RemoveRangeCooldownModifier(ValueModifier modifier)
        {
            return _rangeCooldown.RemoveStatModifier(modifier);
        }
    }
}
