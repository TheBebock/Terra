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
        [SerializeField, Expandable] private PlayerStatsDefinition _playerStatsData;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private PlayerStats _playerStats;

        public PlayerStats PlayerStats => _playerStats;

        public event Action<int> OnMaxHealthChanged;
        public event Action<int> OnStrengthChanged;
        public event Action<int> OnDexterityChanged;
        public event Action<int> OnLuckChanged;
        public event Action<int> OnSwingSpeedChanged;
        public event Action<int> OnRangeCooldownChanged;
        public event Action<int> OnMeleeRangeChanged;
        public event Action<StatisticType, int> OnStatValueChanged;


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

        public int GetStatValue(StatisticType type)
        {
            switch (type)
            {
                case StatisticType.Strength: return _playerStats.Strength;
                case StatisticType.Dexterity: return _playerStats.Dexterity;
                case StatisticType.MaxHealth: return _playerStats.MaxHealth;
                case StatisticType.Luck: return _playerStats.Luck;
                case StatisticType.MeleeRange: return _playerStats.MeleeRange;
                case StatisticType.RangeCooldown: return _playerStats.RangeCooldown;
                case StatisticType.SwingSpeed: return _playerStats.SwingSpeed;
            }

            return -1;
        }

        public int GetTempStatValue(StatisticType type, List<ValueModifier> modifiers)
        {
            ModifiableValue tempValue = default;
            switch (type)
            {
                case StatisticType.Strength:
                    tempValue = new ModifiableValue(_playerStats.Strength);
                    break;
                case StatisticType.Dexterity:
                    tempValue = new ModifiableValue(_playerStats.Dexterity);
                    break;
                case StatisticType.MaxHealth:
                    tempValue = new ModifiableValue(_playerStats.MaxHealth);
                    break;
                case StatisticType.Luck:
                    tempValue = new ModifiableValue(_playerStats.Luck);
                    break;
                case StatisticType.MeleeRange:
                    tempValue = new ModifiableValue(_playerStats.MeleeRange);
                    break;
                case StatisticType.RangeCooldown:
                    tempValue = new ModifiableValue(_playerStats.RangeCooldown);
                    break;
                case StatisticType.SwingSpeed:
                    tempValue = new ModifiableValue(_playerStats.SwingSpeed);
                    break;

                default:
                    Debug.LogError($"{this}: unknown stat type: {type}");
                    return 0;
            }

            for (int i = 0; i < modifiers.Count; i++)
            {
                tempValue.AddStatModifier(modifiers[i]);
            }

            return tempValue.Value;
        }

        public void AddModifiers(StatisticType statType, List<ValueModifier> modifiers)
        {
            if (modifiers.IsNullOrEmpty()) return;

            switch (statType)
            {
                case StatisticType.Strength:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.AddStrengthModifier(modifiers[i]);
                    }

                    OnStrengthChanged?.Invoke(_playerStats.Strength);
                    OnStatValueChanged?.Invoke(StatisticType.Strength, _playerStats.Strength);
                    break;

                case StatisticType.Dexterity:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.AddDexterityModifier(modifiers[i]);
                    }

                    OnDexterityChanged?.Invoke(_playerStats.Dexterity);
                    OnStatValueChanged?.Invoke(StatisticType.Dexterity, _playerStats.Dexterity);
                    break;

                case StatisticType.MaxHealth:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.AddMaxHealthModifier(modifiers[i]);
                    }

                    OnMaxHealthChanged?.Invoke(_playerStats.MaxHealth);
                    OnStatValueChanged?.Invoke(StatisticType.MaxHealth, _playerStats.MaxHealth);
                    break;

                case StatisticType.Luck:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.AddLuckModifier(modifiers[i]);
                    }

                    OnLuckChanged?.Invoke(_playerStats.Luck);
                    OnStatValueChanged?.Invoke(StatisticType.Luck, _playerStats.Luck);
                    break;

                case StatisticType.MeleeRange:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.AddMeleeRangeModifier(modifiers[i]);
                    }

                    OnMeleeRangeChanged?.Invoke(_playerStats.MeleeRange);
                    OnStatValueChanged?.Invoke(StatisticType.MeleeRange, _playerStats.MeleeRange);
                    break;

                case StatisticType.RangeCooldown:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.AddRangeCooldownModifier(modifiers[i]);
                    }

                    OnRangeCooldownChanged?.Invoke(_playerStats.RangeCooldown);
                    OnStatValueChanged?.Invoke(StatisticType.RangeCooldown, _playerStats.RangeCooldown);
                    break;

                case StatisticType.SwingSpeed:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.AddSwingSpeedModifier(modifiers[i]);
                    }

                    OnSwingSpeedChanged?.Invoke(_playerStats.SwingSpeed);
                    OnStatValueChanged?.Invoke(StatisticType.SwingSpeed, _playerStats.SwingSpeed);
                    break;

                default:
                    Debug.LogError($"{this}: AddModifiers called with unknown stat type: {statType}");
                    break;
            }
        }


        public void RemoveModifiers(StatisticType statType, List<ValueModifier> modifiers)
        {
            if (modifiers.IsNullOrEmpty()) return;

            switch (statType)
            {
                case StatisticType.Strength:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.RemoveStrengthModifier(modifiers[i]);
                    }

                    OnStrengthChanged?.Invoke(_playerStats.Strength);
                    OnStatValueChanged?.Invoke(StatisticType.Strength, _playerStats.Strength);
                    break;

                case StatisticType.Dexterity:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.RemoveDexterityModifier(modifiers[i]);
                    }

                    OnDexterityChanged?.Invoke(_playerStats.Dexterity);
                    OnStatValueChanged?.Invoke(StatisticType.Dexterity, _playerStats.Dexterity);
                    break;

                case StatisticType.MaxHealth:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.RemoveMaxHealthModifier(modifiers[i]);
                    }

                    OnMaxHealthChanged?.Invoke(_playerStats.MaxHealth);
                    OnStatValueChanged?.Invoke(StatisticType.MaxHealth, _playerStats.MaxHealth);
                    break;

                case StatisticType.Luck:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.RemoveLuckModifier(modifiers[i]);
                    }

                    OnLuckChanged?.Invoke(_playerStats.Luck);
                    OnStatValueChanged?.Invoke(StatisticType.Luck, _playerStats.Luck);
                    break;

                case StatisticType.MeleeRange:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.RemoveMeleeRangeModifier(modifiers[i]);
                    }

                    OnMeleeRangeChanged?.Invoke(_playerStats.MeleeRange);
                    OnStatValueChanged?.Invoke(StatisticType.MeleeRange, _playerStats.MeleeRange);
                    break;

                case StatisticType.RangeCooldown:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.RemoveRangeCooldownModifier(modifiers[i]);
                    }

                    OnRangeCooldownChanged?.Invoke(_playerStats.RangeCooldown);
                    OnStatValueChanged?.Invoke(StatisticType.RangeCooldown, _playerStats.RangeCooldown);
                    break;

                case StatisticType.SwingSpeed:
                    for (int i = 0; i < modifiers.Count; i++)
                    {
                        _playerStats.RemoveSwingSpeedModifier(modifiers[i]);
                    }

                    OnSwingSpeedChanged?.Invoke(_playerStats.SwingSpeed);
                    OnStatValueChanged?.Invoke(StatisticType.SwingSpeed, _playerStats.SwingSpeed);
                    break;

                default:
                    Debug.LogError($"{this}: RemoveModifiers called with unknown stat type: {statType}");
                    break;
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (_playerStatsData == null)
            {
                return;
            }

            _playerStats = new PlayerStats(_playerStatsData);
        }
#endif
        
    }
}