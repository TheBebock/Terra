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

        [SerializeField] private Image rewardSprite;

        [SerializeField] private Toggle rewardToggle;

        public bool GetToggleStatus()
        {
            return rewardToggle.isOn;
        }

        public void SetUp()
        {
            // TODO: Setup reward data
        }

        public void TearDown()
        {
            
        }
    }
}