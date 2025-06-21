using System;
using Terra.Core.ModifiableValue;
using Terra.StatisticsSystem.Definitions;
using UnityEngine;
using UnityEngine.Serialization;


namespace Terra.StatisticsSystem
{
    [Serializable]
    public class PlayerStats : CharacterStats
    {
        [FormerlySerializedAs("luck")] [SerializeField] private ModifiableValue _luck;
        public int Luck => _luck.Value;
        
        public PlayerStats(int baseStrength, int baseMaxHealth, int baseSpeed, int baseLuck) : base(baseStrength,baseMaxHealth,baseSpeed)
        {
            _luck = new ModifiableValue(baseLuck);
        }
 
        public PlayerStats(PlayerStatsDefinition data)
            : this(data.baseStrength, data.baseMaxHealth, data.baseSpeed, data.baseLuck){}
        

        public void AddLuckModifier(ValueModifier modifier)
        {
            _luck.AddStatModifier(modifier);
        }

        public bool RemoveLuckModifier(ValueModifier modifier)
        {
            return _luck.RemoveStatModifier(modifier);
        }
    }
}
