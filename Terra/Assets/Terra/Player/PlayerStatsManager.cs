using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using NaughtyAttributes;
using Terra.Core.ModifiableValue;
using Terra.Extensions;
using Terra.StatisticsSystem;
using Terra.StatisticsSystem.Definitions;
using UnityEngine;

namespace Terra.Player
{
    public class PlayerStatsManager : MonoBehaviourSingleton<PlayerStatsManager>
    {
        [SerializeField, Expandable] private  PlayerStatsDefinition _playerStatsData;
        [Foldout("Debug"), ReadOnly] [SerializeField] private PlayerStats _playerStats;
        
        public PlayerStats PlayerStats => _playerStats;

        public event Action<float> OnMaxHealthChanged; 
        public event Action<float> OnStrengthChanged; 
        public event Action<float> OnDexterityChanged; 
        public event Action<float> OnLuckChanged;


        protected override void Awake()
        {
            base.Awake();
            
            if (_playerStatsData == null)
            {
                Debug.LogError($"{this}: playerStatsData is null. Please attach basic stats to player.");
                return;
            }
            _playerStats = new PlayerStats(_playerStatsData);
        }

        public void AddStrength(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;
            
            for (int i = 0; i < modifiers.Count; i++)
            {
                _playerStats.AddStrengthModifier(modifiers[i]);
            }
            
            OnStrengthChanged?.Invoke(_playerStats.Strength);
        }
        
        public void RemoveStrength(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
                _playerStats.RemoveStrengthModifier(modifiers[i]);
            }
            
            OnStrengthChanged?.Invoke(_playerStats.Strength);
        }
        
        public void AddMaxHealth(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
               _playerStats.AddMaxHealthModifier(modifiers[i]);
            }
            OnMaxHealthChanged?.Invoke(Mathf.RoundToInt(_playerStats.MaxHealth));
        }
        
        public void RemoveMaxHealth(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            { 
                _playerStats.RemoveMaxHealthModifier(modifiers[i]);
            }
            OnMaxHealthChanged?.Invoke(Mathf.RoundToInt(_playerStats.MaxHealth));
        }
        
        public void AddDexterity(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
                _playerStats.AddDexterityModifier(modifiers[i]);
            }
            OnDexterityChanged?.Invoke(_playerStats.Dexterity);   
        }
        
        public void RemoveSpeed(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
                _playerStats.RemoveDexterityModifier(modifiers[i]);
            }
            OnDexterityChanged?.Invoke(_playerStats.Dexterity);   
        }
        
        public void AddLuck(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
                _playerStats.AddLuckModifier(modifiers[i]);
            }
            
            OnLuckChanged?.Invoke(_playerStats.Luck);
        }
        
        public void RemoveLuck(List<ValueModifier> modifiers)
        {
            if(modifiers.IsNullOrEmpty()) return;

            for (int i = 0; i < modifiers.Count; i++)
            {
                _playerStats.RemoveLuckModifier(modifiers[i]);
            }
            
            OnLuckChanged?.Invoke(_playerStats.Luck);
        }

        private void OnValidate()
        {
            if (_playerStatsData == null)
            {
                return;
            }
            _playerStats = new PlayerStats(_playerStatsData);
        }
    }
}