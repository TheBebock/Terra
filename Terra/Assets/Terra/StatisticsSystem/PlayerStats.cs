using System;
using Terra.StatisticsSystem.Definitions;


namespace Terra.StatisticsSystem
{
    [Serializable]
    public class PlayerStats : CharacterStats
    {
        
        public PlayerStats(int baseStrength, int baseMaxHealth, int baseSpeed, int baseLuck) 
            : base(baseStrength, baseMaxHealth, baseSpeed, baseLuck)
        {
        }
 
        public PlayerStats(PlayerStatsDefinition data)
            : this(data.baseStrength, data.baseMaxHealth, data.baseSpeed, data.baseLuck){}
    }
}