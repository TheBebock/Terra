using System;
using NaughtyAttributes;
using Terra.EffectsSystem;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Extensions;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Items;
using Terra.Managers;
using Terra.Player;
using Terra.RewardSystem;
using TMPro;
using UIExtensionPackage.UISystem.Core.Base;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Terra.UI.Windows.RewardWindow
{
    public class RewardToggle: UIObject
    {
        [FormerlySerializedAs("rewardName")] [SerializeField] private TMP_Text _rewardName;
        [FormerlySerializedAs("rewardDescription")] [SerializeField] private TMP_Text _rewardDescription;

        [FormerlySerializedAs("rewardIcon")] [SerializeField] private Image _rewardIcon;

        [FormerlySerializedAs("rewardToggle")] [SerializeField] private Toggle _rewardToggle;
        private RewardData _rewardData;

        public Toggle Toggle => _rewardToggle;
        
        private WeaponReward _weaponReward = new();
        private StatsReward _statsReward = new();
        private EffectData _effectReward;
        private Type _effectType;
        private ActiveItem _activeItemReward;
        private PassiveItem _passiveItemReward;

        [SerializeField, ReadOnly]
        private WeaponDataComparison _weaponDataComparison;
        [SerializeField, ReadOnly]
        private StatsDataComparison _itemDataComparison;
        [SerializeField, ReadOnly]
        private StatsDataComparison _statsDataComparison;
        private OnRewardSelected _onRewardSelected;
        [SerializeField, ReadOnly] private RewardType _rewardType;

        public RewardType RewardType { get { return _rewardType; } set { _rewardType = value; } }

        public void Init()
        {
            ChooseRewardData();
            
            _onRewardSelected = new OnRewardSelected();
            if (_weaponDataComparison.itemDataComparison.isInitialized)
            {
                _onRewardSelected.comparison = _weaponDataComparison.itemDataComparison;
            }
            else if(_itemDataComparison.isInitialized )
            {
                _onRewardSelected.comparison = _itemDataComparison;
            }
            else
            {
                _onRewardSelected.comparison = _statsDataComparison;
            }
            
            _rewardToggle.onValueChanged.AddListener(OnToggle);
        }

        private void OnToggle(bool isOn)
        {
            if (isOn)
            {
                EventsAPI.Invoke(ref _onRewardSelected);
            }
            else
            {
                EventsAPI.Invoke<OnRewardUnselected>();
            }
        }
        private void ChooseRewardData()
        {
            switch(_rewardType)
            {
                case RewardType.Stats: 
                    _statsReward.AddRandomStat();
                    LoadStatsData();
                    break;
                case RewardType.Weapon: 
                    GetRandomWeaponRandomType(); 
                    break;
                case RewardType.Effect:
                    GetRandomEffect();
                    break;
                case RewardType.ActiveItem:
                    GetRandomItem(_rewardType);
                    break;
                case RewardType.PassiveItem:
                    GetRandomItem(_rewardType);
                    break;
            }
        }

        private void SetNewRewardType()
        {
            RewardType = (RewardType)UnityEngine.Random.Range(1, 5);
            Init();
        }

        private void GetRandomItem(RewardType rewardType)
        {
            if(rewardType == RewardType.ActiveItem)
            {
                _activeItemReward = LootManager.Instance.LootTable.GetRandomActiveItem();
                if(_activeItemReward == null)
                {
                    SetNewRewardType();
                    return;
                }

                if (PlayerInventoryManager.Instance.ActiveItem.Data == null)
                    _itemDataComparison = ItemsComparator.CompareItems(_activeItemReward.Data);
                else
                    _itemDataComparison = ItemsComparator.CompareItems(PlayerInventoryManager.Instance.ActiveItem.Data, _activeItemReward.Data);
                LoadItemData(_activeItemReward.Data);
            }
            else if(rewardType == RewardType.PassiveItem)
            {
                _passiveItemReward = LootManager.Instance.LootTable.GetRandomPassiveItem();
                if (_passiveItemReward == null)
                {
                    SetNewRewardType();
                    return;
                }
                
                _itemDataComparison = ItemsComparator.CompareItems(_passiveItemReward.Data);
                
                LoadItemData(_passiveItemReward.Data);
            }
        }

        private void LoadItemData(ItemData data)
        {
            _rewardName.text = data.itemName;
            _rewardDescription.text = data.itemDescription;

            LoadModifiersUIText(data);

            _rewardIcon.sprite = data.itemSprite;
        }

        private void LoadModifiersUIText(ItemData data)
        {
            StatsDataComparison currentItemDataComparison = _weaponDataComparison.damage != default ? _weaponDataComparison.itemDataComparison : _itemDataComparison;

            if (data.maxHealthModifiers.Count > 0)
            {
                float totalValue = 0;

                foreach (var modifier in data.maxHealthModifiers)
                {
                    totalValue += modifier.value;
                }
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.maxHealth, $"\nMax Health: {totalValue}");
            }

            if (data.luckModifiers.Count > 0)
            {
                float totalValue = 0;

                foreach (var modifier in data.luckModifiers)
                {
                    totalValue += modifier.value;
                }
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.luck, $"\nLuck: {totalValue}");
            }

            if (data.strengthModifiers.Count > 0)
            {
                float totalValue = 0;

                foreach (var modifier in data.strengthModifiers)
                {
                    totalValue += modifier.value;
                }
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.strength, $"\nStrength: {totalValue}");
            }

            if (data.dexModifiers.Count > 0)
            {
                float totalValue = 0;

                foreach (var modifier in data.dexModifiers)
                {
                    totalValue += modifier.value;
                }
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.dexterity, $"\nSpeed: {totalValue}");
            }
        }

        private void GetRandomEffect()
        {
            switch (UnityEngine.Random.Range(0, 2))
            {
                case 0: 
                    _effectReward = TemporaryEffectsContainer.Instance.actionEffectDatas?.GetRandomElement<ActionEffectData>();
                    if (_effectReward != null)
                        _effectType = typeof(ActionEffectData);
                    else
                        TurnOffToogle();
                    break;
                case 1:
                    _effectReward = TemporaryEffectsContainer.Instance.statusEffectDatas?.GetRandomElement<StatusEffectData>();
                    if(_effectReward != null)
                        _effectType = typeof(StatusEffectData);
                    else
                        TurnOffToogle();
                    break;
            }

            LoadEffectData(_effectReward);
             
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
                _weaponDataComparison = ItemsComparator.CompareWeapons(PlayerInventoryManager.Instance.MeleeWeapon.Data, randomWeapon?.Data);
                LoadWeaponData(randomWeapon?.Data);
                _weaponReward.MeleeWeapon = randomWeapon;
            }
            else
            {
                var randomWeapon = LootManager.Instance.LootTable.GetRandomRangedWeapon();
                _weaponDataComparison = ItemsComparator.CompareWeapons(PlayerInventoryManager.Instance.RangedWeapon.Data, randomWeapon?.Data);
                LoadWeaponData(randomWeapon?.Data);
                _weaponReward.RangedWeapon = randomWeapon;
            }
        }

        private void LoadWeaponData<TData>(TData data) where TData: WeaponData
        {
            _rewardName.text = data.itemName;
            _rewardDescription.text = data.itemDescription;
            _rewardDescription.text += "\n";
            _rewardDescription.text += MarkStatisticText(_weaponDataComparison.damage, $"\nDamage: {data.damage}");
            LoadModifiersUIText(data);
            _rewardDescription.text += "\n";

            if (data.effects.actions.Count > 0 || data.effects.statuses.Count > 0)
            {
                _rewardDescription.text += $"\nEffects: \n";

                for (int i = 0; i < data.effects.actions.Count; i++)
                {
                    _rewardDescription.text += $"{data.effects.actions[i].effectName}";
                    if (i < data.effects.actions.Count - 1 || data.effects.statuses.Count > 0) _rewardDescription.text += ", ";
                }

                for (int i = 0; i < data.effects.statuses.Count; i++)
                {
                    _rewardDescription.text += $"{data.effects.statuses[i].effectName}";
                    if (i < data.effects.statuses.Count - 1) _rewardDescription.text += ", ";
                }
            }

            _rewardIcon.sprite = data.itemSprite;
        }

        private void LoadStatsData()
        {
            _rewardName.text = _statsReward.RewardName;
            _rewardDescription.text = _statsReward.RewardDescription;
            _statsDataComparison = _statsReward.Comparison;
        }

        private void LoadEffectData(EffectData effectData)
        {
            _rewardName.text = effectData.effectName;
            _rewardDescription.text = effectData.effectDescription;
            _rewardIcon.sprite = effectData.effectIcon;  
        }

        private string MarkStatisticText(Comparison comparedStatistic, string textToMark)
        {
            string newTextWithMark = "";
            switch (comparedStatistic)
            {
                case Comparison.Worse: newTextWithMark += $"<color={ItemsComparator.WorseItemColor}>"; break;
                case Comparison.Equal: break;
                case Comparison.Better: newTextWithMark += $"<color={ItemsComparator.BetterItemColor}>"; break;
            }
            newTextWithMark += $"{textToMark}";
            newTextWithMark += "</color>";

            return newTextWithMark;
        }

        public void ApplyReward()
        {
            switch (_rewardType)
            {
                case RewardType.Stats: 
                    _statsReward.ApplyReward(); 
                    break;
                case RewardType.Weapon:
                    _weaponReward.ApplyReward();
                    break;
                case RewardType.Effect:
                    if (_effectType.Equals(typeof(ActionEffectData)))
                        PlayerInventoryManager.Instance.MeleeWeapon.Data.effects.actions.Add(_effectReward as ActionEffectData);
                    else
                        PlayerInventoryManager.Instance.MeleeWeapon.Data.effects.statuses.Add(_effectReward as StatusEffectData);
                    break;
                case RewardType.ActiveItem:
                    PlayerInventoryManager.Instance.TryToEquipItem(_activeItemReward);
                    break;
                case RewardType.PassiveItem:
                    PlayerInventoryManager.Instance.TryToEquipItem(_passiveItemReward);
                    break;
            }
        }
    }
}