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
        private ItemDataComparison _betterComparison; 
        protected override void Awake()
        {
            base.Awake();
            _betterComparison = new ItemDataComparison
            {
                strength = Comparison.Better,
                maxHealth = Comparison.Better,
                speed = Comparison.Better,
                luck = Comparison.Better
            };
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

        public ItemDataComparison GetAllBetterComparisonItem()
        {
            return _betterComparison;
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
            switch(secondValue - firstValue)
            {
                case <0: return Comparison.Worse;
                case 0: return Comparison.Equal;
                case >0: return Comparison.Better;

                default: return 0;
            }
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
                switch (mod.Type)
                {
                    case StatModType.Flat:
                        finalValue += mod.Value;
                        break;

                    case StatModType.PercentMult:
                        sumPercentAdd += mod.Value;
                        break;
                }
            }
            finalValue *= (1 + sumPercentAdd.ToFactor());

            return (int)Math.Round(finalValue, 4);
        }
    }
}