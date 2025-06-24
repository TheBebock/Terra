using NaughtyAttributes;
using System;
using Terra.EffectsSystem;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.Enums;
using Terra.Extensions;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Items;
using Terra.Managers;
using Terra.Player;
using Terra.RewardSystem;
using TMPro;
using UIExtensionPackage.UISystem.Core.Base;
using UIExtensionPackage.UISystem.Core.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI
{
    public class RewardToggle: UIObject, IWithSetup
    {
        [SerializeField] private TMP_Text rewardName;
        [SerializeField] private TMP_Text rewardDescription;

        [SerializeField] private Image rewardIcon;

        [SerializeField] private Toggle rewardToggle;
        [SerializeField] private RewardData rewardData;

        public Toggle Toggle => rewardToggle;
        
        private WeaponReward weaponReward = new();
        private StatsReward statsReward = new();
        private EffectData effectReward = default;
        private Type effectType;
        private ActiveItem activeItemReward = default;
        private PassiveItem passiveItemReward = default;

        private WeaponDataComparison weaponDataComparison;
        private ItemDataComparison itemDataComparison;

        [SerializeField, ReadOnly] private RewardType rewardType;

        public RewardType RewardType { get { return rewardType; } set { rewardType = value; } }

        public void SetUp()
        {
            ChooseRewardData();
        }

        private void ChooseRewardData()
        {
            switch(rewardType)
            {
                case RewardType.Stats: 
                    statsReward.AddRandomStat();
                    LoadStatsData();
                    break;
                case RewardType.Weapon: 
                    GetRandomWeaponRandomType(); 
                    break;
                case RewardType.Effect:
                    GetRandomEffect();
                    break;
                case RewardType.ActiveItem:
                    GetRandomItem(rewardType);
                    break;
                case RewardType.PassiveItem:
                    GetRandomItem(rewardType);
                    break;
            }
        }

        private void SetNewRewardType()
        {
            RewardType = (Enums.RewardType)UnityEngine.Random.Range(1, 5);
            SetUp();
        }

        private void GetRandomItem(RewardType rewardType)
        {
            if(rewardType == RewardType.ActiveItem)
            {
                activeItemReward = LootManager.Instance.LootTable.GetRandomActiveItem();
                if(activeItemReward == null)
                {
                    SetNewRewardType();
                    return;
                }

                if (PlayerInventoryManager.Instance.ActiveItem.Data == null)
                    itemDataComparison = ItemsComparator.Instance.CompareItems(activeItemReward.Data);
                else
                    itemDataComparison = ItemsComparator.Instance.CompareItems(PlayerInventoryManager.Instance.ActiveItem.Data, activeItemReward.Data);
                LoadItemData(activeItemReward.Data);
            }
            else if(rewardType == RewardType.PassiveItem)
            {
                passiveItemReward = LootManager.Instance.LootTable.GetRandomPassiveItem();
                if (passiveItemReward == null)
                {
                    SetNewRewardType();
                    return;
                }

                if (PlayerInventoryManager.Instance.GetPassiveItems.Count < 1)
                    itemDataComparison = ItemsComparator.Instance.CompareItems(passiveItemReward.Data);
                else
                    itemDataComparison = ItemsComparator.Instance.CompareItems(PlayerInventoryManager.Instance.GetPassiveItems[0].Data, passiveItemReward.Data);
                LoadItemData(passiveItemReward.Data);
            }
        }

        private void LoadItemData(ItemData data)
        {
            rewardName.text = data.itemName;
            rewardDescription.text = data.itemDescription;

            LoadModifiersUIText(data);

            rewardIcon.sprite = data.itemSprite;
        }

        private void LoadModifiersUIText(ItemData data)
        {
            ItemDataComparison currentItemDataComparison = weaponDataComparison.damage != default ? weaponDataComparison.itemDataComparison : itemDataComparison;

            if (data.maxHealthModifiers.Count > 0)
            {
                float totalValue = 0;

                foreach (var modifier in data.maxHealthModifiers)
                {
                    totalValue += modifier.Value;
                }
                rewardDescription.text += MarkStatisticText(currentItemDataComparison.maxHealth, $"\nMax Health: {totalValue}");
            }

            if (data.luckModifiers.Count > 0)
            {
                float totalValue = 0;

                foreach (var modifier in data.luckModifiers)
                {
                    totalValue += modifier.Value;
                }
                rewardDescription.text += MarkStatisticText(currentItemDataComparison.luck, $"\nLuck: {totalValue}");
            }

            if (data.strengthModifiers.Count > 0)
            {
                float totalValue = 0;

                foreach (var modifier in data.strengthModifiers)
                {
                    totalValue += modifier.Value;
                }
                rewardDescription.text += MarkStatisticText(currentItemDataComparison.strength, $"\nStrength: {totalValue}");
            }

            if (data.speedModifiers.Count > 0)
            {
                float totalValue = 0;

                foreach (var modifier in data.speedModifiers)
                {
                    totalValue += modifier.Value;
                }
                rewardDescription.text += MarkStatisticText(currentItemDataComparison.speed, $"\nSpeed: {totalValue}");
            }
        }

        private void GetRandomEffect()
        {
            switch (UnityEngine.Random.Range(0, 2))
            {
                case 0: 
                    effectReward = TemporaryEffectsContainer.Instance.actionEffectDatas?.GetRandomElement<ActionEffectData>();
                    if (effectReward != null)
                        effectType = typeof(ActionEffectData);
                    else
                        TurnOffToogle();
                    break;
                case 1:
                    effectReward = TemporaryEffectsContainer.Instance.statusEffectDatas?.GetRandomElement<StatusEffectData>();
                    if(effectReward != null)
                    effectType = typeof(StatusEffectData);
                    else
                        TurnOffToogle();
                    break;
            }

            LoadEffectData(effectReward);
             
        }

        private void TurnOffToogle()
        {
            gameObject.SetActive(false);
        }

        private void GetRandomWeaponRandomType()
        {
            int rand = UnityEngine.Random.Range(0, 1);
            
            if (rand == 0)
            {
                var randomWeapon = LootManager.Instance.LootTable.GetRandomMeleeWeapon();
                weaponDataComparison = ItemsComparator.Instance.CompareWeapons(PlayerInventoryManager.Instance.MeleeWeapon.Data, randomWeapon?.Data);
                LoadWeaponData(randomWeapon?.Data);
                weaponReward.MeleeWeapon = randomWeapon;
            }
            else
            {
                var randomWeapon = LootManager.Instance.LootTable.GetRandomRangedWeapon();
                weaponDataComparison = ItemsComparator.Instance.CompareWeapons(PlayerInventoryManager.Instance.RangedWeapon.Data, randomWeapon?.Data);
                LoadWeaponData(randomWeapon?.Data);
                weaponReward.RangedWeapon = randomWeapon;
            }
        }

        private void LoadWeaponData<TData>(TData data) where TData: WeaponData
        {
            rewardName.text = data.itemName;
            rewardDescription.text = data.itemDescription;
            rewardDescription.text += "\n";
            rewardDescription.text += MarkStatisticText(weaponDataComparison.damage, $"\nDamage: {data.damage}");
            LoadModifiersUIText(data);
            rewardDescription.text += "\n";

            if (data.effects.actions.Count > 0 || data.effects.statuses.Count > 0)
            {
                rewardDescription.text += $"\nEffects: \n";

                for (int i = 0; i < data.effects.actions.Count; i++)
                {
                    rewardDescription.text += $"{data.effects.actions[i].effectName}";
                    if (i < data.effects.actions.Count - 1 || data.effects.statuses.Count > 0) rewardDescription.text += ", ";
                }

                for (int i = 0; i < data.effects.statuses.Count; i++)
                {
                    rewardDescription.text += $"{data.effects.statuses[i].effectName}";
                    if (i < data.effects.statuses.Count - 1) rewardDescription.text += ", ";
                }
            }

            rewardIcon.sprite = data.itemSprite;
        }

        private void LoadStatsData()
        {
            rewardName.text = statsReward.RewardName;
            rewardDescription.text = statsReward.RewardDescription;
        }

        private void LoadEffectData(EffectData effectData)
        {
            rewardName.text = effectData.effectName;
            rewardDescription.text = effectData.effectDescription;
            rewardIcon.sprite = effectData.effectIcon;  
        }

        private string MarkStatisticText(Comparison comparedStatistic, string textToMark)
        {
            string newTextWithMark = "";
            switch (comparedStatistic)
            {
                case Comparison.Worse: newTextWithMark += $"<color={ItemsComparator.Instance.worseItemColor}>"; break;
                case Comparison.Equal: break;
                case Comparison.Better: newTextWithMark += $"<color={ItemsComparator.Instance.betterItemColor}>"; break;
            }
            newTextWithMark += $"{textToMark}";
            newTextWithMark += "</color>";

            return newTextWithMark;
        }

        public void ApplyReward()
        {
            switch (rewardType)
            {
                case RewardType.Stats: 
                    statsReward.ApplyReward(); 
                    break;
                case RewardType.Weapon:
                    weaponReward.ApplyReward();
                    break;
                case RewardType.Effect:
                    if (effectType.Equals(typeof(ActionEffectData)))
                        PlayerInventoryManager.Instance.MeleeWeapon.Data.effects.actions.Add(effectReward as ActionEffectData);
                    else
                        PlayerInventoryManager.Instance.MeleeWeapon.Data.effects.statuses.Add(effectReward as StatusEffectData);
                    break;
                case RewardType.ActiveItem:
                    PlayerInventoryManager.Instance.TryToEquipItem(activeItemReward);
                    break;
                case RewardType.PassiveItem:
                    PlayerInventoryManager.Instance.TryToEquipItem(passiveItemReward);
                    break;
            }
        }
        public void TearDown()
        {
            
        }
    }
}