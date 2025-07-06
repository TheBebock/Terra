using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Enums;
using Terra.Extensions;
using Terra.GameStates;
using Terra.Managers;
using Terra.Utils;
using TMPro;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Terra.UI.Windows.RewardWindow
{
    public class RewardWindow : UIWindow
    {
        public override bool AllowMultiple { get; } = false;

        [FormerlySerializedAs("rewardToggles")] [SerializeField] List<RewardToggle> _rewardToggles = new();
        [FormerlySerializedAs("acceptButton")] [SerializeField] Button _acceptButton;
        [FormerlySerializedAs("rerollButton")] [SerializeField] Button _rerollButton;
        [SerializeField] private TMP_Text _rerollCostText;
        [SerializeField] private int _rerollCost;
        private int _leftRerolls = 3;

        private float DifficultyModifier = 1;

        private int modifiedCost = 0;

        private System.Random randomValue = new System.Random();

        [SerializeField, ReadOnly] RewardToggle _currentlyActiveToggle;

        [Foldout("Debug")] [SerializeField] private List<RewardType> _availableRewardTypes = new();
        public override void SetUp()
        {
            base.SetUp();

            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            
            SetDifficultyMultipler();

            for (int i = 0; i < _rewardToggles.Count; i++)
            {
                _rewardToggles[i].Toggle.onValueChanged.AddListener(NotifyRewardSelected);
            }
            ReAssigneAvailableRewardTypes();

            _acceptButton.onClick.AddListener(ApplyReward);
            _rerollButton.onClick.AddListener(RerollRewards);

            CheckRerollsAvailability();

            if(_rerollCost != 0)
            {
                _rerollCostText.gameObject.SetActive(true);
                _rerollCostText.text = modifiedCost.ToString();
            }
            else
            {
                _rerollCostText.gameObject.SetActive(false);
            }

            LoadRewardsData();
        }
        private void SetDifficultyMultipler()
        {
            switch (GameSettings.DefaultDifficultyLevel)
            {
                case Enums.GameDifficulty.Cyberiada: DifficultyModifier = 0.5f; break;
                case Enums.GameDifficulty.Easy: DifficultyModifier = 0.75f; break;
                case Enums.GameDifficulty.Normal: DifficultyModifier = 1; break;
            }
            modifiedCost = Mathf.RoundToInt(_rerollCost * DifficultyModifier);
        }

        private void NotifyRewardSelected(bool value)
        {
            if (value)
            {
                _acceptButton.interactable = true;
            }
            else
            {
                if(IsAnyToggleOn()) return;
                _acceptButton.interactable = false;
            }
        }
        
        private void ApplyReward()
        {
            if (!IsAnyToggleOn()) return;
            
            _currentlyActiveToggle.ApplyReward();
            _currentlyActiveToggle.Toggle.isOn = false;
            GameManager.Instance.SwitchToGameState<StartOfFloorState>();
            Close();
        }

        private void CheckRerollsAvailability()
        {
            if (_rerollCost != 0)
            {
                _rerollButton.interactable = EconomyManager.Instance.CanBuy(modifiedCost);
            }
            else
            {
                if (_leftRerolls <= 0)
                    _rerollButton.interactable = false;
                else
                    _rerollButton.interactable = true;
            }
        }

        private void CheckRewardDuplication(RewardToggle firstToggle, RewardToggle secondToggle, bool freeStatus)
        {
            if (firstToggle.RewardName.text.Equals(secondToggle.RewardName.text))
            {
                _availableRewardTypes.RemoveElement(firstToggle.RewardType);
                secondToggle.RewardType = GetRewardType(freeStatus);
                secondToggle.SetRewardData();
            }
        }

        private void SetExplicity(RewardToggle rewardToggleFromExplicit, RewardToggle rewardToggleToExplicit)
        {
            RewardToggle.ExplicedRewardData explicedData;
            explicedData.type = rewardToggleFromExplicit.RewardType;
            explicedData.rewardName = rewardToggleFromExplicit.RewardName.text;

            rewardToggleToExplicit.SetExplicedReward(explicedData);
        }

        private void RerollRewards()
        {
            _leftRerolls--;
            EconomyManager.Instance.TryToBuy(modifiedCost);
            CheckRerollsAvailability();

            if (IsAnyToggleOn())
            {
                _currentlyActiveToggle.Toggle.isOn = false;
                _currentlyActiveToggle = null;
            }

            _rewardToggles[0].RewardType = GetRewardType(freeStatus: true);
            ReAssigneAvailableRewardTypes();

            _rewardToggles[1].RewardType = GetRewardType(freeStatus: true);
            ReAssigneAvailableRewardTypes();

            _rewardToggles[2].RewardType = GetRewardType(freeStatus: false);
            ReAssigneAvailableRewardTypes();

            _rewardToggles[3].RewardType = GetRewardType(freeStatus: false);
            ReAssigneAvailableRewardTypes();

            _rewardToggles[0].SetRewardData();
            SetExplicity(_rewardToggles[0], _rewardToggles[1]);
            _rewardToggles[1].SetRewardData();
            CheckRewardDuplication(_rewardToggles[0], _rewardToggles[1], freeStatus: true);

            _rewardToggles[2].SetRewardData();
            SetExplicity(_rewardToggles[2], _rewardToggles[3]);
            _rewardToggles[3].SetRewardData();
            CheckRewardDuplication(_rewardToggles[2], _rewardToggles[3], freeStatus: false);
        }

        private void LoadRewardsData()
        {

            _rewardToggles[2].SetFreeStatus(false);
            _rewardToggles[3].SetFreeStatus(false);

            _rewardToggles[0].RewardType = GetRewardType(freeStatus: true);
            ReAssigneAvailableRewardTypes();

            _rewardToggles[1].RewardType = GetRewardType(freeStatus: true);
            ReAssigneAvailableRewardTypes();

            _rewardToggles[2].RewardType = GetRewardType(freeStatus: false);
            ReAssigneAvailableRewardTypes();

            _rewardToggles[3].RewardType = GetRewardType(freeStatus: false);
            ReAssigneAvailableRewardTypes();

            foreach (var toggle in _rewardToggles)
            {
                toggle?.Init();
            }

            _rewardToggles[0].SetRewardData();
            SetExplicity(_rewardToggles[0], _rewardToggles[1]);
            _rewardToggles[1].SetRewardData();
            CheckRewardDuplication(_rewardToggles[0], _rewardToggles[1], freeStatus: true);

            _rewardToggles[2].SetRewardData();
            SetExplicity(_rewardToggles[2], _rewardToggles[3]);
            _rewardToggles[3].SetRewardData();
            CheckRewardDuplication(_rewardToggles[2], _rewardToggles[3], freeStatus: false);
        }

        private void ReAssigneAvailableRewardTypes()
        {
            _availableRewardTypes.Clear();
            _availableRewardTypes.Add(RewardType.PassiveItem);
            _availableRewardTypes.Add(RewardType.Weapon);
            _availableRewardTypes.Add(RewardType.Effect);
        }
        
        private RewardType GetRewardType(bool freeStatus)
        {
            for (int i = _availableRewardTypes.Count-1; i >= 0; i--)
            {
                if(IsRewardTypeAvailable(_availableRewardTypes[i], freeStatus)) continue;
                
                _availableRewardTypes.RemoveAt(i);
            }
            if(_availableRewardTypes.Count == 0) return RewardType.Stats;
            
            int random = randomValue.Next(0, _availableRewardTypes.Count);

            RewardType rewardType = _availableRewardTypes[random];

            //ReAssigneAvailableRewardTypes();

            return rewardType;
        }

        private bool IsRewardTypeAvailable(RewardType rewardType, bool freeStatus)
        {
            switch (rewardType)
            {
                case RewardType.Stats: return true;
                case RewardType.PassiveItem:
                    if (freeStatus)
                        return LootManager.Instance.LootTable.FreePassiveItemsCount > 0;
                    else
                        return LootManager.Instance.LootTable.PayPassiveItemsCount > 0;
                case RewardType.Weapon:
                    if (freeStatus)
                        return LootManager.Instance.LootTable.FreeMeleeWeaponsCount 
                            + LootManager.Instance.LootTable.FreeRangedWeaponsCount > 0;
                    else
                        return LootManager.Instance.LootTable.PayMeleeWeaponsCount
                            + LootManager.Instance.LootTable.PayRangedWeaponsCount > 0;
                case RewardType.Effect:
                    if (freeStatus)
                        return false;
                    else
                        return LootManager.Instance.LootTable.StatusEffectsCount + 
                            LootManager.Instance.LootTable.ActionEffectsCount > 0;
                
                default: return false;
            }
        }
        private bool IsAnyToggleOn()
        {
            foreach (var toggle in _rewardToggles)
            {
                if (toggle.Toggle.isOn)
                {
                    _currentlyActiveToggle = toggle;
                    return true;
                }
            }
            return false;
        }
    }
}
