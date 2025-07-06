using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.EffectsSystem;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.EffectsSystem.Actions;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Items;
using Terra.Managers;
using Terra.Player;
using Terra.RewardSystem;
using Terra.StatisticsSystem;
using TMPro;
using UIExtensionPackage.UISystem.Core.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.Windows.RewardWindow
{
    [RequireComponent(typeof(Toggle))]
    public class RewardToggle: UIObject
    {
        [Serializable]
        internal struct RandomStatSpriteData
        {
            public StatisticType type;
            public Sprite sprite;
        }
        [SerializeField] RandomStatSpriteData[] _spriteData;
        [SerializeField] private TMP_Text _rewardName;
        [SerializeField] private TMP_Text _rewardDescription;
        [SerializeField] private TMP_Text _costDisplay;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private string _freeText = "FREE!";
        
        [SerializeField, ReadOnly] private Toggle _rewardToggle;
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

        [SerializeField, ReadOnly] private bool _freeStatus = true;
        public bool FreeStatus => _freeStatus;

        private int _rewardCost = 0;
        public RewardType RewardType { get { return _rewardType; } set { _rewardType = value; } }

        private static List<int> _availableRewards = new();

        private System.Random randomValue = new System.Random();

        public void Init()
        {
            SetRewardData();
            
            _rewardToggle.onValueChanged.AddListener(OnToggle);
        }

        public void SetRewardData()
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

            SetToggleInteractable();
        }

        private void ClearComparisons()
        {
            _weaponDataComparison = new();
            _itemDataComparison = new();
            _statsDataComparison = new();
        }

        public void SetFreeStatus(bool freeStatus)
        {
            _freeStatus = freeStatus;
        }

        private void TurnOffToggle()
        {
            gameObject.SetActive(false);
        }
        private void SetToggleInteractable()
        {
            _rewardToggle.interactable = EconomyManager.Instance.CanBuy(_rewardCost);
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
            ClearComparisons();
            switch (_rewardType)
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
            RewardType = RewardType.Stats;
            SetRewardData();
        }

        private void GetRandomItem(RewardType rewardType)
        {
            if(rewardType == RewardType.ActiveItem)
            {
                _activeItemReward = LootManager.Instance.LootTable.GetRandomActiveItem(_freeStatus);
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
                _passiveItemReward = LootManager.Instance.LootTable.GetRandomPassiveItem(_freeStatus);
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

            _costDisplay.text = GetCostDisplayText(data.itemCost);
            LoadModifiersUIText(data);

            _rewardIcon.sprite = data.itemSprite;
        }

        private void LoadModifiersUIText(ItemData data)
        {
            StatsDataComparison currentItemDataComparison = _weaponDataComparison.itemDataComparison.isInitialized ? 
                _weaponDataComparison.itemDataComparison : _itemDataComparison;

            if (data.maxHealthModifiers.Count > 0)
            {
                float totalValue = ItemsComparator.CalculateModifierValue(data.maxHealthModifiers);
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.maxHealth, $"{CheckAddPercentMark(totalValue)}");
                _rewardDescription.text += " Max Health";
            }

            if (data.luckModifiers.Count > 0)
            {
                float totalValue = ItemsComparator.CalculateModifierValue(data.luckModifiers);
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.luck, $"{CheckAddPercentMark(totalValue)}");
                _rewardDescription.text += " Luck";
            }

            if (data.strengthModifiers.Count > 0)
            {
                float totalValue = ItemsComparator.CalculateModifierValue(data.strengthModifiers);
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.strength, $"{CheckAddPercentMark(totalValue)}");
                _rewardDescription.text += " Strength";
            }

            if (data.dexModifiers.Count > 0)
            {
                float totalValue = ItemsComparator.CalculateModifierValue(data.dexModifiers);
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.dexterity, $"{CheckAddPercentMark(totalValue)}");
                _rewardDescription.text += " Dexterity";
            }

            if(data.swingSpeedModifiers.Count > 0)
            {
                float totalValue = ItemsComparator.CalculateModifierValue(data.swingSpeedModifiers);
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.swingSpeed, $"{CheckAddPercentMark(totalValue)}");
                _rewardDescription.text += " Swing Speed";
            }

            if (data.meleeRangeModifiers.Count > 0)
            {
                float totalValue = ItemsComparator.CalculateModifierValue(data.meleeRangeModifiers);
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.meleeRange, $"{CheckAddPercentMark(totalValue)}");
                _rewardDescription.text += " Range";
            }

            if (data.rangeCooldownModifiers.Count > 0)
            {
                float totalValue = ItemsComparator.CalculateModifierValue(data.rangeCooldownModifiers);
                _rewardDescription.text += MarkStatisticText(currentItemDataComparison.rangeCooldown, $"{CheckAddPercentMark(totalValue)}");
                _rewardDescription.text += " Cooldown";
            }
        }

        private string CheckAddPercentMark(float value)
        {
            if (value % 1 != 0)
            {
                string stringToMark = (value * 100).ToString();
                stringToMark += "%";
                return stringToMark;
            }
            else
            {
                return value.ToString();
            }
        }

        private void GetRandomEffect()
        {
            _availableRewards.Clear();
            if (LootManager.Instance.LootTable.ActionEffectsCount > 0)
            {
                _availableRewards.Add(0);
            }

            if (LootManager.Instance.LootTable.StatusEffectsCount > 0)
            {
                _availableRewards.Add(1);
            }
            int rand = randomValue.Next(0, _availableRewards.Count);

            switch (rand)
            {
                case 0: 
                    _effectReward = LootManager.Instance.LootTable.GetRandomActionEffect();
                    if (_effectReward != null)
                        _effectType = typeof(ActionEffectData);
                    else
                        TurnOffToggle();
                    break;
                case 1:
                    _effectReward = LootManager.Instance.LootTable.GetRandomStatusEffect();
                    if(_effectReward != null)
                        _effectType = typeof(StatusEffectData);
                    else
                        TurnOffToggle();
                    break;
            }

            LoadEffectData(_effectReward);
             
        }



        private void GetRandomWeaponRandomType()
        {
            _availableRewards.Clear();
            if (LootManager.Instance.LootTable.MeleeWeaponsCount > 0)
            {
                _availableRewards.Add(0);
            }

            if (LootManager.Instance.LootTable.RangedWeaponsCount > 0)
            {
                _availableRewards.Add(1);
            }


            
            int rand = randomValue.Next(0, _availableRewards.Count);

            if (rand == 0)
            {
                var randomWeapon = LootManager.Instance.LootTable.GetRandomMeleeWeapon(_freeStatus);
                _weaponDataComparison = ItemsComparator.CompareWeapons(PlayerInventoryManager.Instance.MeleeWeapon.Data, randomWeapon?.Data);
                LoadWeaponData(randomWeapon?.Data);
                _weaponReward.MeleeWeapon = randomWeapon;
                _weaponReward.WeaponType = WeaponType.Melee;
            }
            else
            {
                var randomWeapon = LootManager.Instance.LootTable.GetRandomRangedWeapon(_freeStatus);
                _weaponDataComparison = ItemsComparator.CompareWeapons(PlayerInventoryManager.Instance.RangedWeapon.Data, randomWeapon?.Data);
                LoadWeaponData(randomWeapon?.Data);
                _weaponReward.RangedWeapon = randomWeapon;
                _weaponReward.WeaponType = WeaponType.Ranged;
            }
        }

        private void LoadWeaponData<TData>(TData data) where TData: WeaponData
        {
            _rewardName.text = data.itemName;
            _rewardDescription.text = data.itemDescription;

            if(data.WeaponType == WeaponType.Ranged)
            {
                _rewardDescription.text += $"Max Ammo: {(data as RangedWeaponData).ammoCapacity}\n";
                _rewardDescription.text += $"Penetration: {(data as RangedWeaponData).bulletData.penetrationPower}\n";
            }

            _costDisplay.text = GetCostDisplayText(data.itemCost);
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
            _rewardDescription.text = MarkStatisticText(Comparison.Better,_statsReward.RewardDescription);
            _rewardIcon.sprite = GetStatSprite(_statsReward.StatType);
            _statsDataComparison = _statsReward.Comparison;
            _costDisplay.text = GetCostDisplayText(0);
        }

        private void LoadEffectData(EffectData effectData)
        {
            _rewardName.text = effectData.effectName;
            _rewardDescription.text = effectData.effectDescription;
            _rewardIcon.sprite = effectData.effectIcon;  
            _costDisplay.text = GetCostDisplayText(effectData.effectCost);
        }

        private Sprite GetStatSprite(StatisticType statisticType)
        {
            for (int i = 0; i < _spriteData.Length; i++)
            {
                if(_spriteData[i].type == statisticType) return _spriteData[i].sprite;
            }

            return null;
        }
        private string MarkStatisticText(Comparison comparedStatistic, string textToMark)
        {
            string newTextWithMark = "";
            switch (comparedStatistic)
            {
                case Comparison.Worse: newTextWithMark += $"<color={ItemsComparator.WorseItemColor}>\n"; break;
                case Comparison.Equal: newTextWithMark += "\n"; break;
                case Comparison.Better: newTextWithMark += $"<color={ItemsComparator.BetterItemColor}>\n+"; break;
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
                    ApplyEffectReward(_effectReward);
                    break;
                case RewardType.ActiveItem:
                    PlayerInventoryManager.Instance?.TryToEquipItem(_activeItemReward);
                    LootManager.Instance.LootTable.RemoveActiveItem(_activeItemReward);
                    break;
                case RewardType.PassiveItem:
                    PlayerInventoryManager.Instance?.TryToEquipItem(_passiveItemReward);
                    LootManager.Instance.LootTable.RemovePassiveItem(_passiveItemReward);
                    break;
            }
            EconomyManager.Instance.TryToBuy(_rewardCost);
        }

        private void ApplyEffectReward(EffectData effectData)
        {
            if (effectData is StatusEffectData statusEffectData)
            {
                ApplyStatusEffectReward(statusEffectData);
            }

            if (effectData is ActionEffectData actionEffectData)
            {
                ApplyActionEffectReward(actionEffectData);
            }
        }

        private void ApplyStatusEffectReward(StatusEffectData statusEffectData)
        {
            if (statusEffectData.containerType == ContainerType.None)
            {
                PlayerManager.Instance.PlayerEntity?.StatusContainer.TryAddEffect(statusEffectData);
            }
            else
            {
                PlayerManager.Instance.PlayerAttackController?.AddNewAttackStatusEffect(statusEffectData);
            }
            LootManager.Instance.LootTable.RemoveStatusEffect(statusEffectData);
        }

        private void ApplyActionEffectReward(ActionEffectData actionEffectData)
        {
            if (actionEffectData.containerType == ContainerType.None)
            {
                ActionEffectsDatabase.Instance.ExecuteAction(actionEffectData, PlayerManager.Instance.PlayerEntity);
            }
            else
            {
                PlayerManager.Instance.PlayerAttackController?.AddNewAttackActionEffect(actionEffectData);
            }
            LootManager.Instance.LootTable.RemoveActionEffect(actionEffectData);
        }

        private string GetCostDisplayText(int cost)
        {
            _rewardCost = cost;
            return cost == 0 ? _freeText : $"{_rewardCost}"; 
        }
    }
}