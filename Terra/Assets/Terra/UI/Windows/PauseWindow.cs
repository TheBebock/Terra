using Terra.Managers;
using Terra.UI.MainMenu;
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

        public void HideButtons()
        {
            _buttonsCanvasGroup.alpha = 0;
        }

        public void ShowButtons()
        {
            _buttonsCanvasGroup.alpha = 1;
        }
        private void OpenSettings()
        {
            HideButtons();
            _settingsPanel.gameObject.SetActive(true);
            _settingsPanel.IsInMainMenu = false;
            _settingsPanel.SetDarkScreenOpacity(_settingsDarkScreenOpacity);
        }
        private void ExitToMenu()
        {
            Close();
            ScenesManager.Instance.LoadMainMenu();
        }
    }
}
