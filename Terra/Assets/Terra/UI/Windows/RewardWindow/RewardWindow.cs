using System.Collections.Generic;
using NaughtyAttributes;
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

        public override void SetUp()
        {
            base.SetUp();

            for (int i = 0; i < _rewardToggles.Count; i++)
            {
                _rewardToggles[i].Toggle.onValueChanged.AddListener(NotifyRewardSelected);
            }
            LoadRewardsData();

            _acceptButton.onClick.AddListener(ApplyReward);
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
            _rewardToggles[2].RewardType = (Enums.RewardType)Random.Range(1, 5);
            _rewardToggles[3].RewardType = (Enums.RewardType)Random.Range(1, 5);

            foreach (var toggle in _rewardToggles)
            {
                toggle?.Init();
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
