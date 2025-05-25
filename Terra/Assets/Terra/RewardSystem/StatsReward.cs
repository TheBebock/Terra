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
        //private float maxPercentAddModifier = 0.5f;
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
            switch(Random.Range(0, 4))
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
            float modifierMaxValue = 0;
            StatModType statModType = default;
            ValueModifier valueModifier = default;

            switch (Random.Range(0, 2))
            {
                case 0: 
                    modifierMaxValue = maxFlatModifier;
                    statModType = StatModType.Flat;
                    valueModifier = new ValueModifier(value: Random.Range(1, (int)modifierMaxValue), type: statModType);
                    break;
                case 1:
                    modifierMaxValue = maxPercentMultModifier;
                    statModType = StatModType.PercentMult;
                    valueModifier = new ValueModifier(value: Random.Range(1, modifierMaxValue), type: statModType);
                    break;
                default: break;
            }

            
            SetUIDescription(valueModifier);

            return valueModifier;
        }

        private void SetUIDescription(ValueModifier valueModifier)
        {
            char rewardSign = valueModifier.Type == StatModType.Flat ? '+' : 'x';
            float rewardValue = valueModifier.Type == StatModType.Flat ? (int)valueModifier.Value : 1 + valueModifier.Value;

            rewardDescription = $"{rewardSign}{rewardValue:0.00}";
        }
        
    }
}