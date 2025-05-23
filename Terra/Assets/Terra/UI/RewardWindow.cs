using System.Collections.Generic;
using Terra.GameStates;
using Terra.Managers;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI
{
    public class RewardWindow : UIWindow
    {
        public override bool AllowMultiple { get; } = false;

        [SerializeField] List<RewardToggle> rewardToggles = new();
        [SerializeField] Button acceptButton;

        [SerializeField] RewardToggle currentlyActiveToogle;

        public override void SetUp()
        {
            base.SetUp();

            LoadRewardsData();

            acceptButton.onClick.AddListener(ApplyReward);
        }

        private void ApplyReward()
        {
            if (!IsAnyToggleOn()) return;

            
            
            // TODO: Apply reward for player
            currentlyActiveToogle.ApplyReward();

            GameManager.Instance.SwitchToGameState<GameplayState>();
            Close();
        }

        private void LoadRewardsData()
        {
            rewardToggles[0].RewardType = Enums.RewardType.Stats;
            rewardToggles[1].RewardType = Enums.RewardType.Stats;
            rewardToggles[2].RewardType = (Enums.RewardType)Random.Range(1, 5);
            rewardToggles[3].RewardType = (Enums.RewardType)Random.Range(1, 5);

            foreach (var toggle in rewardToggles)
            {
                toggle?.SetUp();
            }
        }
        
        private bool IsAnyToggleOn()
        {
            foreach (var toggle in rewardToggles)
            {
                if (toggle.GetToggleStatus())
                {
                    currentlyActiveToogle = toggle;
                    return true;
                }
            }
            return false;
        }
    }
}
