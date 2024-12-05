using UnityEngine;

namespace StatisticsSystem.Definitions
{
    public abstract class CharacterStatsDefinition : ScriptableObject
    {
        [Header("Character Stats")]
        public float baseStrength;
        public float baseMaxHealth;
        public float baseSpeed;
    }
}

