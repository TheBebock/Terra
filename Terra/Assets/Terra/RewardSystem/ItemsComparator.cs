using System;
using System.Collections.Generic;
using Terra.Core.ModifiableValue;
using Terra.Enums;
using Terra.Extensions;
using Terra.Itemization.Abstracts.Definitions;
using UIExtensionPackage.UISystem.Core.Generics;

namespace Terra.RewardSystem
{
    public class ItemsComparator:PersistentSingleton<ItemsComparator>
    {
        public readonly string betterItemColor = "green";
        public readonly string worseItemColor = "red";


        public ItemDataComparison CompareItems(ItemData currentItem)
        {
            ItemDataComparison comparison = new ItemDataComparison
            {
                strength = CompareModifiers(currentItem.strengthModifiers, 0),
                maxHealth = CompareModifiers(currentItem.maxHealthModifiers, 0),
                speed = CompareModifiers(currentItem.speedModifiers, 0),
                luck = CompareModifiers(currentItem.luckModifiers, 0)
            };
            return comparison;
        }
        
        public ItemDataComparison CompareItems(ItemData currentItem, ItemData toCompareItem)
        {
            ItemDataComparison comparison = new ItemDataComparison
            {
                strength = CompareModifiers(currentItem.strengthModifiers, toCompareItem.strengthModifiers),
                maxHealth = CompareModifiers(currentItem.maxHealthModifiers, toCompareItem.maxHealthModifiers),
                speed = CompareModifiers(currentItem.speedModifiers, toCompareItem.speedModifiers),
                luck = CompareModifiers(currentItem.luckModifiers, toCompareItem.luckModifiers)
            };
            return comparison;
        }
        
        public WeaponDataComparison CompareWeapons(WeaponData currentWeapon, WeaponData toCompareWeapon)
        {
            WeaponDataComparison comparison = new WeaponDataComparison
            {
                damage = CompareValue(currentWeapon.damage, toCompareWeapon.damage),
                attackCooldown = CompareValue(currentWeapon.attackCooldown, toCompareWeapon.attackCooldown),
                itemDataComparison = new ItemDataComparison
                {
                    strength = CompareModifiers(currentWeapon.strengthModifiers, toCompareWeapon.strengthModifiers),
                    maxHealth = CompareModifiers(currentWeapon.maxHealthModifiers, toCompareWeapon.maxHealthModifiers),
                    speed = CompareModifiers(currentWeapon.speedModifiers, toCompareWeapon.speedModifiers),
                    luck = CompareModifiers(currentWeapon.luckModifiers, toCompareWeapon.luckModifiers)
                }
            };

            return comparison;
        }

        private Comparison CompareValue(float firstValue, float secondValue)
        {
            if(firstValue > secondValue) return Comparison.Worse;
            if(firstValue < secondValue) return Comparison.Better;
            
            return Comparison.Equal;
        }
        
        private Comparison CompareModifiers(List<ValueModifier> firstModifiersList, int value)
        {
            var modifiersValue = CalculateModifierValue(firstModifiersList);

            return CompareValue(value, modifiersValue);
        }

        private Comparison CompareModifiers(List<ValueModifier> firstModifiersList, List<ValueModifier> secornModifiersList)
        {
            var firstModifiersValue = CalculateModifierValue(firstModifiersList);
            var secondModifiersValue = CalculateModifierValue(secornModifiersList);

            return CompareValue(firstModifiersValue, secondModifiersValue);
        }

        private int CalculateModifierValue(List<ValueModifier> modifiers)
        {
            float finalValue = 1;
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
            finalValue *= (1 + sumPercentAdd.ToFactor());

            return (int)Math.Round(finalValue, 4);
        }
    }
}