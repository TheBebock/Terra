using UnityEngine;

namespace Terra.StatisticsSystem.Definitions
{
    public abstract class CharacterStatsDefinition : ScriptableObject
    {
        [Header("Character Stats")]
        public int baseStrength;
        public int baseMaxHealth;
        public int baseSpeed;
    }
}

