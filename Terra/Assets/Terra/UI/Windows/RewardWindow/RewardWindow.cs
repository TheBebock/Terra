using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Enums;
using Terra.GameStates;
using Terra.Managers;
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
        private static int LeftRerolls = 3;

        [SerializeField, ReadOnly] RewardToggle _currentlyActiveToggle;

        private List<RewardType> _availableRewardTypes = new();
        public override void SetUp()
        {
            base.SetUp();

            for (int i = 0; i < _rewardToggles.Count; i++)
            {
                _rewardToggles[i].Toggle.onValueChanged.AddListener(NotifyRewardSelected);
            }
            _availableRewardTypes.Clear();
            _availableRewardTypes.Add(RewardType.PassiveItem);
            _availableRewardTypes.Add(RewardType.Weapon);
            _availableRewardTypes.Add(RewardType.Effect);
            
            _acceptButton.onClick.AddListener(ApplyReward);
            _rerollButton.onClick.AddListener(RerollRewards);

            CheckRerollsAvailability();

            if(_rerollCost != 0)
            {
                _rerollCostText.gameObject.SetActive(true);
                _rerollCostText.text = _rerollCost.ToString();
            }
            else
            {
                _rerollCostText.gameObject.SetActive(false);
            }

            LoadRewardsData();
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
            
            // TODO: Apply reward for player
            _currentlyActiveToggle.ApplyReward();
            _currentlyActiveToggle.Toggle.isOn = false;
            GameManager.Instance.SwitchToGameState<StartOfFloorState>();
            Close();
        }

        private void CheckRerollsAvailability()
        {
            if (_rerollCost != 0)
            {
                _rerollButton.interactable = EconomyManager.Instance.CanBuy(_rerollCost);
            }
            else
            {
                if (LeftRerolls <= 0)
                    _rerollButton.interactable = false;
                else
                    _rerollButton.interactable = true;
            }
        }

        private void RerollRewards()
        {
            LeftRerolls--;
            EconomyManager.Instance.TryToBuy(_rerollCost);
            CheckRerollsAvailability();

            if (IsAnyToggleOn())
            {
                _currentlyActiveToggle.Toggle.isOn = false;
                _currentlyActiveToggle = null;
            }

            _rewardToggles[0].RewardType = GetRewardType(freeStatus: true);
            _rewardToggles[1].RewardType = GetRewardType(freeStatus: true);
            _rewardToggles[2].RewardType = GetRewardType(freeStatus: false);
            _rewardToggles[3].RewardType = GetRewardType(freeStatus: false);

            _rewardToggles[0].SetRewardData();
            _rewardToggles[1].SetRewardData();
            _rewardToggles[2].SetRewardData();
            _rewardToggles[3].SetRewardData();
        }

        private void LoadRewardsData()
        {

            _rewardToggles[2].SetFreeStatus(false);
            _rewardToggles[3].SetFreeStatus(false);

            _rewardToggles[0].RewardType = GetRewardType(freeStatus: true);
            _rewardToggles[1].RewardType = GetRewardType(freeStatus: true);
            _rewardToggles[2].RewardType = GetRewardType(freeStatus: false);
            _rewardToggles[3].RewardType = GetRewardType(freeStatus: false);

            foreach (var toggle in _rewardToggles)
            {
                toggle?.Init();
            }
        }
        
        private RewardType GetRewardType(bool freeStatus)
        {
            for (int i = _availableRewardTypes.Count-1; i >= 0; i--)
            {
                if(IsRewardTypeAvailable(_availableRewardTypes[i], freeStatus)) continue;
                
                _availableRewardTypes.RemoveAt(i);
            }
            if(_availableRewardTypes.Count == 0) return RewardType.Stats;
            
            int random = Random.Range(0, _availableRewardTypes.Count);
            
            return _availableRewardTypes[random];
            
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
