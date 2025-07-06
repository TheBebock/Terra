using System;
using System.Collections.Generic;
using Terra.Core.ModifiableValue;
using Terra.Enums;
using Terra.Extensions;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Player;
using Terra.StatisticsSystem;
using UnityEngine;

namespace Terra.RewardSystem
{
    
    public static class ItemsComparator
    {
        public static readonly string BetterItemColor = "#00980A";
        public static readonly string WorseItemColor = "#C70000";


        public static StatsDataComparison CompareStats(StatisticType type, List<ValueModifier> modifiers)
        {
            StatsDataComparison comparison = new StatsDataComparison
            {
                isInitialized = true,
            };
            switch (type)
            {
                case StatisticType.Strength:
                    comparison.strength = CompareModifiers(0, modifiers);
                    comparison.strengthValue =
                        PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Strength, modifiers);
                    break;
                case StatisticType.MaxHealth:
                    comparison.maxHealth = CompareModifiers(0, modifiers);
                    comparison.maxHealthValue = 
                        PlayerStatsManager.Instance.GetTempStatValue(StatisticType.MaxHealth, modifiers);
                    break;
                case StatisticType.Dexterity:
                    comparison.dexterity = CompareModifiers(0, modifiers);
                    comparison.dexterityValue =
                        PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Dexterity, modifiers);
                    break;
                case StatisticType.Luck:
                    comparison.luck = CompareModifiers(0,modifiers);
                    comparison.luckValue =
                        PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Luck, modifiers);
                    break;
                case StatisticType.SwingSpeed:
                    comparison.swingSpeed = CompareModifiers(0, modifiers);
                    comparison.swingSpeedValue = 
                        PlayerStatsManager.Instance.GetTempStatValue(StatisticType.SwingSpeed, modifiers);
                    break;
                case StatisticType.MeleeRange:
                    comparison.meleeRange = CompareModifiers(0, modifiers);
                    comparison.meleeRangeValue =
                        PlayerStatsManager.Instance.GetTempStatValue(StatisticType.MeleeRange, modifiers);
                    break;
                case StatisticType.RangeCooldown:
                    comparison.rangeCooldown = CompareModifiers(0, modifiers);
                    comparison.rangeCooldownValue =
                        PlayerStatsManager.Instance.GetTempStatValue(StatisticType.RangeCooldown, modifiers);
                    break;
            }
            return comparison;
        }
        public static StatsDataComparison CompareItems(ItemData currentItem)
        {
            if (currentItem == null)
            {
                Debug.LogError("Given item to compare is null");
                return default;
            }
            
            StatsDataComparison comparison = new StatsDataComparison
            {
                isInitialized = true,
                
                strength = CompareModifiers(0, currentItem.strengthModifiers),
                strengthValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Strength, currentItem.strengthModifiers),
        
                maxHealth = CompareModifiers(0, currentItem.maxHealthModifiers),
                maxHealthValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.MaxHealth, currentItem.maxHealthModifiers),
        
                dexterity = CompareModifiers(0, currentItem.dexModifiers),
                dexterityValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Dexterity, currentItem.dexModifiers),
        
                luck = CompareModifiers(0,currentItem.luckModifiers),
                luckValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Luck, currentItem.luckModifiers),

                swingSpeed = CompareModifiers(0, currentItem.swingSpeedModifiers),
                swingSpeedValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.SwingSpeed, currentItem.swingSpeedModifiers),

                meleeRange = CompareModifiers(0, currentItem.meleeRangeModifiers),
                meleeRangeValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.MeleeRange, currentItem.meleeRangeModifiers),

                rangeCooldown = CompareModifiers(0, currentItem.rangeCooldownModifiers),
                rangeCooldownValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.RangeCooldown, currentItem.rangeCooldownModifiers),

            };
            return comparison;
        }
        
        public static StatsDataComparison CompareItems(ItemData currentItem, ItemData toCompareItem)
        {
            if (currentItem == null || toCompareItem == null)
            {
                Debug.LogError("Given items are null");
                return default;
            }
            
            StatsDataComparison comparison = new StatsDataComparison
            {
                isInitialized = true,
                
                strength = CompareModifiers(currentItem.strengthModifiers, toCompareItem.strengthModifiers),
                strengthValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Strength, toCompareItem.strengthModifiers),
        
                maxHealth = CompareModifiers(currentItem.maxHealthModifiers, toCompareItem.maxHealthModifiers),
                maxHealthValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.MaxHealth, toCompareItem.maxHealthModifiers),
        
                dexterity = CompareModifiers(currentItem.dexModifiers, toCompareItem.dexModifiers),
                dexterityValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Dexterity, toCompareItem.dexModifiers),
        
                luck = CompareModifiers(currentItem.luckModifiers, toCompareItem.luckModifiers),
                luckValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Luck, toCompareItem.luckModifiers),

                swingSpeed = CompareModifiers(currentItem.swingSpeedModifiers, toCompareItem.swingSpeedModifiers),
                swingSpeedValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.SwingSpeed, toCompareItem.swingSpeedModifiers),

                meleeRange = CompareModifiers(currentItem.meleeRangeModifiers, toCompareItem.meleeRangeModifiers),
                meleeRangeValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.MeleeRange, toCompareItem.meleeRangeModifiers),

                rangeCooldown = CompareModifiers(currentItem.rangeCooldownModifiers, toCompareItem.rangeCooldownModifiers),
                rangeCooldownValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.RangeCooldown, toCompareItem.rangeCooldownModifiers),

            };
            return comparison;
        }

        
        public static WeaponDataComparison CompareWeapons(WeaponData currentWeapon, WeaponData toCompareWeapon)
        {
            if (currentWeapon == null || toCompareWeapon == null)
            {
                Debug.LogError("Given weapons are null");
                return default;
            }
            float newWeaponStr = CalculateModifierValue(toCompareWeapon.strengthModifiers);
            float playerStr = PlayerStatsManager.Instance.PlayerStats.Strength;
            float playerStrWithoutCurrentWeapon = playerStr - CalculateModifierValue(currentWeapon.strengthModifiers);

            float newWeaponDex = CalculateModifierValue(toCompareWeapon.dexModifiers);
            float playerDex = PlayerStatsManager.Instance.PlayerStats.Dexterity;
            float playerDexWithoutCurrentWeapon = playerDex - CalculateModifierValue(currentWeapon.dexModifiers);

            float newWeaponMeleeRange = CalculateModifierValue(toCompareWeapon.meleeRangeModifiers);
            float playerMeleeRange = PlayerStatsManager.Instance.PlayerStats.MeleeRange;
            float playerMeleeRangeWithoutCurrentWeapon = playerMeleeRange - CalculateModifierValue(currentWeapon.meleeRangeModifiers);

            float newWeaponSwingSpeed = CalculateModifierValue(toCompareWeapon.swingSpeedModifiers);
            float playerSwingSpeed = PlayerStatsManager.Instance.PlayerStats.SwingSpeed;
            float playerSwingSpeedWithoutCurrentWeapon = playerSwingSpeed - CalculateModifierValue(currentWeapon.swingSpeedModifiers);

            float newWeaponRangeCooldown = CalculateModifierValue(toCompareWeapon.rangeCooldownModifiers);
            float playerRangeCooldown = PlayerStatsManager.Instance.PlayerStats.RangeCooldown;
            float playerRangeCooldownWithoutCurrentWeapon = playerRangeCooldown - CalculateModifierValue(currentWeapon.rangeCooldownModifiers);



            WeaponDataComparison comparison = new WeaponDataComparison
            {
                itemDataComparison = new StatsDataComparison
                {
                    isInitialized = true,

                    strength = CompareModifiers(currentWeapon.strengthModifiers, toCompareWeapon.strengthModifiers),
                    strengthValue = (int)(playerStrWithoutCurrentWeapon + newWeaponStr),

                    maxHealth = CompareModifiers(currentWeapon.maxHealthModifiers, toCompareWeapon.maxHealthModifiers),
                    maxHealthValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.MaxHealth, toCompareWeapon.maxHealthModifiers),

                    dexterity = CompareModifiers(currentWeapon.dexModifiers, toCompareWeapon.dexModifiers),
                    dexterityValue = (int)(playerDexWithoutCurrentWeapon + newWeaponDex),

                    luck = CompareModifiers(currentWeapon.luckModifiers, toCompareWeapon.luckModifiers),
                    luckValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Luck, toCompareWeapon.luckModifiers),

                    swingSpeed = CompareModifiers(currentWeapon.swingSpeedModifiers, toCompareWeapon.swingSpeedModifiers),
                    swingSpeedValue = (int)(playerSwingSpeedWithoutCurrentWeapon + newWeaponSwingSpeed),

                    meleeRange = CompareModifiers(currentWeapon.meleeRangeModifiers, toCompareWeapon.meleeRangeModifiers),
                    meleeRangeValue = (int)(playerMeleeRangeWithoutCurrentWeapon + newWeaponMeleeRange),

                    rangeCooldown = CompareModifiers(currentWeapon.rangeCooldownModifiers, toCompareWeapon.rangeCooldownModifiers),
                    rangeCooldownValue = (int)(playerRangeCooldownWithoutCurrentWeapon + newWeaponRangeCooldown),

                }
            };

            return comparison;
        }


        private static Comparison CompareModifiers(int valueToCompareTo, List<ValueModifier> compareModifiers)
        {
            float modifiersValue = CalculateModifierValue(compareModifiers);
            
            return CompareValue(valueToCompareTo, modifiersValue);
        }

        private static Comparison CompareModifiers(List<ValueModifier> modifiersToCompareTo, List<ValueModifier> compareModifiers)
        {
            float firstModifiersValue = CalculateModifierValue(modifiersToCompareTo);
            float secondModifiersValue = CalculateModifierValue(compareModifiers);

            return CompareValue(firstModifiersValue, secondModifiersValue);
        }

        private static Comparison CompareValue(float firstValue, float secondValue)
        {
            if(firstValue > secondValue) return Comparison.Worse;
            if(firstValue < secondValue) return Comparison.Better;
            
            return Comparison.Equal;
        }

        public static float CalculateModifierValue(List<ValueModifier> modifiers)
        {
            float finalValue = 0;
            float sumPercentAdd = 0;

            for (int i = 0; i < modifiers.Count; i++)
            {
                ValueModifier mod = modifiers[i];
                switch (mod.type)
                {
                    case StatModType.Flat:
                        finalValue += mod.value;
                        break;

                    case StatModType.PercentMult:
                        sumPercentAdd += mod.value;
                        break;
                }
            }

            if (finalValue == 0)
            {
                finalValue = sumPercentAdd.ToFactor();
            }
            else
            {
                finalValue *= (1 + sumPercentAdd.ToFactor());
            }

            return (float)Math.Round(finalValue, 4);
        }
    }
}