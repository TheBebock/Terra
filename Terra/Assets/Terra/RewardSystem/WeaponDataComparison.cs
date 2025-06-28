using System;
using Terra.Enums;

namespace Terra.RewardSystem
{
    [Serializable]
    public struct WeaponDataComparison
    {
        public Comparison damage;
        public Comparison attackCooldown;
        public StatsDataComparison itemDataComparison;

    }
}