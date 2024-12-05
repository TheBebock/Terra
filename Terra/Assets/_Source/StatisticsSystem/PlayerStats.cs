using System;
using UnityEngine;
using Core.ModifiableValue;
using StatisticsSystem.Definitions;

namespace StatisticsSystem
{
    [Serializable]
    public class PlayerStats : CharacterStats
    {
        [SerializeField] private ModifiableValue luck;
        public float Luck => luck.Value;
        
        public PlayerStats(float basestrength, float basemaxHealth, float basespeed, float baseLuck) 
            : base(basestrength, basemaxHealth, basespeed)
        {
            luck = new ModifiableValue(baseLuck);
        }
 
        public PlayerStats(PlayerStatsDefinition data)
            : this(data.baseStrength, data.baseMaxHealth, data.baseSpeed, data.baseLuck){}
        

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