using Terra.MainMenu;
using Terra.Managers;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Terra.UI.Windows
{
    public class PauseWindow : UIWindow
    {
        public override bool AllowMultiple { get; } = false;
        
        [SerializeField] Button _resumeButton;
        [SerializeField] Button _settingsButton;
        [SerializeField] Button _exitToMenuButton;
        [SerializeField] CanvasGroup _buttonsCanvasGroup;
        [SerializeField] SettingsUI _settingsPanel;
        [SerializeField,Range(0,1f)] private float _settingsDarkScreenOpacity = 0.9f;
        public override void SetUp()
        {
            base.SetUp();
            
            _resumeButton?.onClick.AddListener(Resume);
            _settingsButton?.onClick.AddListener(OpenSettings);
            _exitToMenuButton?.onClick.AddListener(ExitToMenu);
            _settingsPanel?.CloseButton.onClick.AddListener(ShowButtons);
            
            if(_settingsPanel) _settingsPanel.gameObject.SetActive(false);

        }

        private void Resume()
        {
            TimeManager.Instance?.ResumeGame();
            Close();
        }

        private void HideButtons()
        {
            _buttonsCanvasGroup.alpha = 0;
        }

        private void ShowButtons()
        {
            _buttonsCanvasGroup.alpha = 1;
        }
        private void OpenSettings()
        {
            _settingsPanel.gameObject.SetActive(true);
            _settingsPanel.SetDarkScreenOpacity(_settingsDarkScreenOpacity);
        }
        private void ExitToMenu()
        {
            Close();
            ScenesManager.Instance.LoadMainMenu();
        }
    }
}
