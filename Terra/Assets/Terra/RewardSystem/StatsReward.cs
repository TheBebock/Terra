using System;
using Terra.Player;
using Terra.Core.ModifiableValue;
using System.Collections.Generic;
using Terra.StatisticsSystem;
using Random = UnityEngine.Random;

namespace Terra.RewardSystem
{
    public class StatsReward: RewardData
    {
        private List<ValueModifier> _modifiers = new();
        
        private int _maxFlatModifier = 5;
        private int _maxPercentMultModifier = 20;
        private StatisticType _statType;

        public StatisticType StatType => _statType;
        public string RewardName => rewardName;
        public string RewardDescription => rewardDescription;
    
        private StatsDataComparison _comparison;
        
        public ref StatsDataComparison Comparison => ref _comparison;

        public override void ApplyReward()
        {
            PlayerStatsManager.Instance.AddModifiers(_statType, _modifiers);
        }

        public void AddRandomStat()
        {
            int statTypesAmount = Enum.GetValues(typeof(StatisticType)).Length;
            _statType = (StatisticType)Random.Range(0, statTypesAmount);
            switch(_statType)
            {
                case StatisticType.MaxHealth: 
                    rewardName = "Max Health";
                    break;
                case StatisticType.SwingSpeed: 
                    rewardName = "Swing Speed";
                    break;
                case StatisticType.RangeCooldown:
                    rewardName = "Fire Rate";
                    break;
               default:
                   rewardName = _statType.ToString();
                   break;
            }
            _modifiers.Add(AddRandomModifier());
            _comparison = ItemsComparator.CompareStats(_statType, _modifiers);
        }

        public ValueModifier AddRandomModifier()
        {
            int modifierMaxValue;
            StatModType statModType;
            ValueModifier valueModifier = default;

            switch (Random.Range(0, 2))
            {
                case 0: 
                    modifierMaxValue = _maxFlatModifier;
                    statModType = StatModType.Flat;
                    valueModifier = new ValueModifier(value: Random.Range(1, modifierMaxValue), type: statModType);
                    break;
                case 1:
                    modifierMaxValue = _maxPercentMultModifier;
                    statModType = StatModType.PercentMult;
                    valueModifier = new ValueModifier(value: Random.Range(1, modifierMaxValue), type: statModType);
                    break;
            }

            
            SetUIDescription(valueModifier);

            return valueModifier;
        }

        private void SetUIDescription(ValueModifier valueModifier)
        {
            char rewardPercentageSign = '%';
            float rewardValue = valueModifier.type == StatModType.Flat ? valueModifier.value : 1 + valueModifier.value;

            string text = valueModifier.type == StatModType.Flat ?  
                $"{rewardValue}" 
                : $"{rewardValue}{rewardPercentageSign}";
            rewardDescription = text;
        }
        
    }
}