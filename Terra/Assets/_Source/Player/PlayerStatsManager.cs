using System.Collections;
using System.Collections.Generic;
using StatisticsSystem;
using Core.Generics;
using Core.ModifiableValue;
using UnityEngine;

namespace Player
{
    public class PlayerStatsManager : MonoBehaviourSingleton<PlayerStatsManager>
    {
        private CharacterStats characterStats;
        [SerializeField] private float baseStrength;
        [SerializeField] private float baseMaxHealth;
        [SerializeField] private float baseSpeed;
        
        protected override void Awake()
        {
            base.Awake();
            characterStats = new CharacterStats(baseStrength, baseMaxHealth, baseSpeed);
        }

        public void AddStrength(ValueModifier modifier)
        {
            characterStats.AddStrengthModifier(modifier);
        }
        public void AddStrength(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                characterStats.AddStrengthModifier(modifiers[i]);
            }
        }
        
        public void RemoveStrength(ValueModifier modifier)
        {
            characterStats.RemoveStrengthModifier(modifier);
        }
        public void RemoveStrength(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                RemoveStrength(modifiers[i]);
            }
        }

        public void AddMaxHealth(ValueModifier modifier)
        {
            characterStats.AddMaxHealthModifier(modifier);
        }
        public void AddMaxHealth(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                AddMaxHealth(modifiers[i]);
            }
        }

        public void RemoveMaxHealth(ValueModifier modifier)
        {
            characterStats.RemoveMaxHealthModifier(modifier);
        }
        public void RemoveMaxHealth(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            { 
                RemoveMaxHealth(modifiers[i]);
            }
        }

        public void AddSpeed(ValueModifier modifier)
        {
            characterStats.AddSpeedModifier(modifier);
        }
        public void AddSpeed(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                AddSpeed(modifiers[i]);
            }
        }

        public void RemoveSpeed(ValueModifier modifier)
        {
            characterStats.RemoveSpeedModifier(modifier);
        }
        public void RemoveSpeed(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                RemoveSpeed(modifiers[i]);
            }
        }


    }
}