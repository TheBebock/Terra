using System;
using System.Collections.Generic;
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

        public override void SetUp()
        {
            base.SetUp();

            foreach (var toggle in rewardToggles)
            {
                toggle?.SetUp();
            }

            acceptButton.onClick.AddListener(ApplyReward);
            acceptButton.onClick.AddListener(Resume);
        }

        private void ApplyReward()
        {
            if (!IsAnyToggleOn()) return;

            // TODO: Apply reward for player
        }

        private void Resume()
        {
            if (!IsAnyToggleOn()) return;

            TimeManager.Instance?.ResumeTime();
            Close();
        }

        private bool IsAnyToggleOn()
        {
            foreach (var toggle in rewardToggles)
            {
                if (toggle.GetToggleStatus()) return true;
            }
            return false;
        }
    }
}
