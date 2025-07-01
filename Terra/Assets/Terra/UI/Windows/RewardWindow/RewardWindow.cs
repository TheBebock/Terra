using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Enums;
using Terra.GameStates;
using Terra.Managers;
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

        private void LoadRewardsData()
        {
            _rewardToggles[0].RewardType = Enums.RewardType.Stats;
            _rewardToggles[1].RewardType = Enums.RewardType.Stats;
            _rewardToggles[2].RewardType = GetRewardType();
            _rewardToggles[3].RewardType = GetRewardType();

            foreach (var toggle in _rewardToggles)
            {
                toggle?.Init();
            }
        }
        
        private RewardType GetRewardType()
        {
            for (int i = _availableRewardTypes.Count-1; i >= 0; i--)
            {
                if(IsRewardTypeAvailable(_availableRewardTypes[i])) continue;
                
                _availableRewardTypes.RemoveAt(i);
            }
            if(_availableRewardTypes.Count == 0) return RewardType.Stats;
            
            int random = Random.Range(0, _availableRewardTypes.Count);
            
            return _availableRewardTypes[random];
            
        }

        private bool IsRewardTypeAvailable(RewardType rewardType)
        {
            switch (rewardType)
            {
                case RewardType.Stats: return true;
                case RewardType.PassiveItem:
                    return LootManager.Instance.LootTable.PassiveItemsCount > 0;
                case RewardType.Weapon:
                    return LootManager.Instance.LootTable.MeleeWeaponsCount 
                        + LootManager.Instance.LootTable.RangedWeaponsCount > 0;
                case RewardType.Effect:
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
