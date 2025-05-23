using Terra.Player;
using Terra.Core.ModifiableValue;

using UnityEngine;
using System.Collections.Generic;

namespace Terra.RewardSystem
{
    public class StatsReward: RewardData
    {
        public StatsReward() { }

        private List<ValueModifier> modifiersMaxHealth = new();
        private List<ValueModifier> modifiersDexterity = new();
        private List<ValueModifier> modifiersStrength = new();
        private List<ValueModifier> modifiersLuck = new();

        private int maxFlatModifier = 5;
        private float maxPercentAddModifier = 0.5f;
        private float maxPercentMultModifier = 0.5f;

        public string RewardName => rewardName;
        public string RewardDescription => rewardDescription;


        public override void ApplyReward()
        {
            if (modifiersMaxHealth.Count > 0) PlayerStatsManager.Instance.AddMaxHealth(modifiersMaxHealth);
            if (modifiersDexterity.Count > 0) PlayerStatsManager.Instance.AddDexterity(modifiersDexterity);
            if (modifiersStrength.Count > 0) PlayerStatsManager.Instance.AddStrength(modifiersStrength);
            if (modifiersLuck.Count > 0) PlayerStatsManager.Instance.AddLuck(modifiersLuck);
        }

        public void AddRandomStat()
        {
            switch(Random.Range(0, 3))
            {
                case 0: 
                    modifiersMaxHealth.Add(AddRandomModifier());
                    rewardName = "Max Health";
                    break;
                case 1:
                    modifiersDexterity.Add(AddRandomModifier());
                    rewardName = "Dexterity";
                    break;
                case 2: 
                    modifiersStrength.Add(AddRandomModifier());
                    rewardName = "Strength";
                    break;
                case 3: 
                    modifiersLuck.Add(AddRandomModifier());
                    rewardName = "Luck";
                    break;
                default: break;
            }
        }

        public ValueModifier AddRandomModifier()
        {
            StatModType statModType = (StatModType)(Random.Range(1, 3) * 100);
            float modifierMaxValue = 0;

            switch (statModType)
            {
                case StatModType.Flat: 
                    modifierMaxValue = maxFlatModifier; 
                    break;
                case StatModType.PercentAdd:
                    modifierMaxValue = maxPercentAddModifier;
                    break;
                case StatModType.PercentMult:
                    modifierMaxValue = maxPercentMultModifier;
                    break;
                default: break;
            }

            ValueModifier valueModifier = new ValueModifier(value: Random.Range(1, modifierMaxValue), type: statModType);
            SetUIDescription(valueModifier);

            return valueModifier;
        }

        private void SetUIDescription(ValueModifier valueModifier)
        {
            char rewardSign = valueModifier.Type == StatModType.PercentMult ? '*' : '+';
            float rewardValue = valueModifier.Value;
            char rewardPercentSign = valueModifier.Type == StatModType.Flat ? ' ' : '%';

            rewardDescription = $"{rewardSign}{rewardValue}{rewardPercentSign}";
        }
        
    }
}