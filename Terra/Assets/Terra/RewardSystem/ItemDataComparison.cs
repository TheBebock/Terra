using System;
using Terra.Enums;

namespace Terra.RewardSystem
{    
    [Serializable]
    public struct StatsDataComparison
    {
        public bool isInitialized;
        public Comparison strength;
        public int strengthValue;
        public Comparison maxHealth;
        public int maxHealthValue;
        public Comparison dexterity;
        public int dexterityValue;
        public Comparison luck;
        public int luckValue;
    }
}