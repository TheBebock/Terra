using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using Core.ModifiableValue;
using NaughtyAttributes;
using StatisticsSystem;
using StatisticsSystem.Definitions;
using UnityEngine;

namespace Terra.Player
{
    public class PlayerStatsManager : MonoBehaviourSingleton<PlayerStatsManager>
    {
        [SerializeField, Expandable] private  PlayerStatsDefinition playerStatsData;
        [SerializeField, ReadOnly] private PlayerStats playerStats;
        
        public PlayerStats PlayerStats => playerStats;

        public event Action<float> OnMaxHealthChanged; 
        public event Action<float> OnStrengthChanged; 
        public event Action<float> OnDexterityChanged; 
        public event Action<float> OnLuckChanged;


        protected override void Awake()
        {
            base.Awake();
            
            if (playerStatsData == null)
            {
                Debug.LogError($"{this}: playerStatsData is null. Please attach basic stats to player.");
                return;
            }
            playerStats = new PlayerStats(playerStatsData);
        }

        public void AddStrength(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;
            
            for (int i = 0; i < modifiers.Count; i++)
            {
                playerStats.AddStrengthModifier(modifiers[i]);
            }
            
            OnStrengthChanged?.Invoke(playerStats.Strength);
        }
        
        public void RemoveStrength(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
                playerStats.RemoveStrengthModifier(modifiers[i]);
            }
            
            OnStrengthChanged?.Invoke(playerStats.Strength);
        }
        
        public void AddMaxHealth(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
               playerStats.AddMaxHealthModifier(modifiers[i]);
            }
            OnMaxHealthChanged?.Invoke(playerStats.MaxHealth);
        }
        
        public void RemoveMaxHealth(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            { 
                playerStats.RemoveMaxHealthModifier(modifiers[i]);
            }
            OnMaxHealthChanged?.Invoke(playerStats.MaxHealth);
        }
        
        public void AddDexterity(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
                playerStats.AddDexterityModifier(modifiers[i]);
            }
            OnDexterityChanged?.Invoke(playerStats.Dexterity);   
        }
        
        public void RemoveSpeed(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
                playerStats.RemoveDexterityModifier(modifiers[i]);
            }
            OnDexterityChanged?.Invoke(playerStats.Dexterity);   
        }
        
        public void AddLuck(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
                playerStats.AddLuckModifier(modifiers[i]);
            }
            
            OnLuckChanged?.Invoke(playerStats.Luck);
        }
        
        public void RemoveLuck(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
                playerStats.RemoveLuckModifier(modifiers[i]);
            }
            
            OnLuckChanged?.Invoke(playerStats.Luck);
        }

        private void OnValidate()
        {
            if (playerStatsData == null)
            {
                Debug.LogError($"{this}: playerStatsData is null. Please attach basic stats to player.");
                return;
            }
            playerStats = new PlayerStats(playerStatsData);
        }
    }
}