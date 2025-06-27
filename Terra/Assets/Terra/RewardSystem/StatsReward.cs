using Terra.Player;
using Terra.Core.ModifiableValue;

using UnityEngine;
using System.Collections.Generic;

namespace Terra.RewardSystem
{
    public class StatsReward: RewardData
    {
        private List<ValueModifier> _modifiersMaxHealth = new();
        private List<ValueModifier> _modifiersDexterity = new();
        private List<ValueModifier> _modifiersStrength = new();
        private List<ValueModifier> _modifiersLuck = new();

        private int _maxFlatModifier = 5;
        //private float maxPercentAddModifier = 0.5f;
        private int _maxPercentMultModifier = 20;

        public string RewardName => rewardName;
        public string RewardDescription => rewardDescription;


        public override void ApplyReward()
        {
            if (_modifiersMaxHealth.Count > 0) PlayerStatsManager.Instance.AddMaxHealth(_modifiersMaxHealth);
            if (_modifiersDexterity.Count > 0) PlayerStatsManager.Instance.AddDexterity(_modifiersDexterity);
            if (_modifiersStrength.Count > 0) PlayerStatsManager.Instance.AddStrength(_modifiersStrength);
            if (_modifiersLuck.Count > 0) PlayerStatsManager.Instance.AddLuck(_modifiersLuck);
        }

        public void AddRandomStat()
        {
            switch(Random.Range(0, 4))
            {
                case 0: 
                    _modifiersMaxHealth.Add(AddRandomModifier());
                    rewardName = "Max Health";
                    break;
                case 1:
                    _modifiersDexterity.Add(AddRandomModifier());
                    rewardName = "Dexterity";
                    break;
                case 2: 
                    _modifiersStrength.Add(AddRandomModifier());
                    rewardName = "Strength";
                    break;
                case 3: 
                    _modifiersLuck.Add(AddRandomModifier());
                    rewardName = "Luck";
                    break;
            }
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
            char rewardSign = valueModifier.type == StatModType.Flat ? '+' : 'x';
            float rewardValue = valueModifier.type == StatModType.Flat ? valueModifier.value : 1 + valueModifier.value;

            rewardDescription = $"{rewardSign}{rewardValue:0.00}";
        }
        
    }
}