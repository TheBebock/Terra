using System;
using Terra.EffectsSystem.Abstract.Definitions;
using UnityEngine;

namespace Terra.EffectsSystem.Actions.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "InstaHealData", menuName = "TheBebocks/Actions/Data/InstaHealData")]
    public class InstaHealData : ActionEffectData
    {
        public int amount = 10;
        public bool isPercentage = false;
        

        protected override float CalculateEffectPower()
        {
            return amount;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (isPercentage && amount < 0 || amount > 100)
            {
                Debug.LogError($"{nameof(isPercentage)} is set to true, values must be between 0 and 100.");
            }
        }
#endif
        
    }
}


