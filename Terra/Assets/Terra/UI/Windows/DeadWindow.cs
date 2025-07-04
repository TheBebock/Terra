using Terra.Managers;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.Windows
{
    public class DeadWindow: UIWindow
    {
        public override bool AllowMultiple { get; } = false;

        [SerializeField] private GameObject _darkScreen;
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private Button _mainMenuButton;

        public override void SetUp()
        {
            base.SetUp();

            _playAgainButton?.onClick.AddListener(Restart);
            _mainMenuButton?.onClick.AddListener(LoadMainMenu);
            
        }

        private void LoadMainMenu()
        {
            Close();
            ScenesManager.Instance?.LoadMainMenu();
        }
        private async void Restart()
        {
            _darkScreen.SetActive(true);
            TimeManager.Instance.ResumeTime();
            await ScenesManager.Instance.LoadGameplay();
            Close();
        }
    }
}