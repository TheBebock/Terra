using System;
using System.Collections.Generic;
using Terra.Core.ModifiableValue;
using Terra.Enums;
using Terra.Extensions;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Player;
using Terra.StatisticsSystem;

namespace Terra.RewardSystem
{
    
    public static class ItemsComparator
    {
        public static readonly string BetterItemColor = "green";
        public static readonly string WorseItemColor = "red";


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
            }
            return comparison;
        }
        public static StatsDataComparison CompareItems(ItemData currentItem)
        {
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
            };
            return comparison;
        }
        
        public static StatsDataComparison CompareItems(ItemData currentItem, ItemData toCompareItem)
        {
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
            };
            return comparison;
        }

        
        public static WeaponDataComparison CompareWeapons(WeaponData currentWeapon, WeaponData toCompareWeapon)
        {
            int newWeaponStr = CalculateModifierValue(toCompareWeapon.strengthModifiers);
            int playerStr = PlayerStatsManager.Instance.PlayerStats.Strength;
            int playerStrWithoutCurrentWeapon = playerStr - CalculateModifierValue(currentWeapon.strengthModifiers);
            
            WeaponDataComparison comparison = new WeaponDataComparison
            {
                attackCooldown = CompareValue(currentWeapon.attackCooldown, toCompareWeapon.attackCooldown),
                itemDataComparison = new StatsDataComparison
                {
                    isInitialized = true,
                    
                    strength = CompareModifiers(currentWeapon.strengthModifiers, toCompareWeapon.strengthModifiers),
                    strengthValue =  playerStrWithoutCurrentWeapon + newWeaponStr,
                    
                    maxHealth = CompareModifiers(currentWeapon.maxHealthModifiers, toCompareWeapon.maxHealthModifiers),
                    maxHealthValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.MaxHealth, toCompareWeapon.maxHealthModifiers),
                    
                    dexterity = CompareModifiers(currentWeapon.dexModifiers, toCompareWeapon.dexModifiers),
                    dexterityValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Dexterity, toCompareWeapon.dexModifiers),
                    
                    luck = CompareModifiers(currentWeapon.luckModifiers, toCompareWeapon.luckModifiers),
                    luckValue = PlayerStatsManager.Instance.GetTempStatValue(StatisticType.Luck, toCompareWeapon.luckModifiers),

                }
            };

            return comparison;
        }


        private static Comparison CompareModifiers(int valueToCompareTo, List<ValueModifier> compareModifiers)
        {
            var modifiersValue = CalculateModifierValue(compareModifiers);
            
            return CompareValue(valueToCompareTo, modifiersValue);
        }

        private static Comparison CompareModifiers(List<ValueModifier> modifiersToCompareTo, List<ValueModifier> compareModifiers)
        {
            var firstModifiersValue = CalculateModifierValue(modifiersToCompareTo);
            var secondModifiersValue = CalculateModifierValue(compareModifiers);

            return CompareValue(firstModifiersValue, secondModifiersValue);
        }

        private static Comparison CompareValue(float firstValue, float secondValue)
        {
            if(firstValue > secondValue) return Comparison.Worse;
            if(firstValue < secondValue) return Comparison.Better;
            
            return Comparison.Equal;
        }

        private static int CalculateModifierValue(List<ValueModifier> modifiers)
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
                finalValue = sumPercentAdd;
            }
            else
            {
                finalValue *= (1 + sumPercentAdd.ToFactor());
            }

            return (int)Math.Round(finalValue, 4);
        }
    }
}