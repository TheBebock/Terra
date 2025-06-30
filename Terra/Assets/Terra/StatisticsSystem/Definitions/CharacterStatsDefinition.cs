using UnityEngine;

namespace Terra.StatisticsSystem.Definitions
{
    public abstract class CharacterStatsDefinition : ScriptableObject
    {
        [Header("Character Stats")]
        public int baseStrength = 1;
        public int baseMaxHealth = 1;
        public int baseDexterity = 1;
    }
}

