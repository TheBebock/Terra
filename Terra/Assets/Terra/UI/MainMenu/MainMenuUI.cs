using NaughtyAttributes;
using Terra.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [BoxGroup("Buttons")][SerializeField] private Button _startGameButton;
        [BoxGroup("Buttons")][SerializeField] private Button _helpButton;
        [BoxGroup("Buttons")][SerializeField] private Button _settingsButton;
        [BoxGroup("Buttons")][SerializeField] private Button _creditsButton;
        [BoxGroup("Buttons")][SerializeField] private Button _exitGameButton;
        
        [SerializeField] private SettingsUI _settingsPanel;
        [SerializeField] private GameObject _helpPanel;
        [SerializeField] private GameObject _creditsPanel;

        private void Awake()
        {
            if(_settingsPanel)_settingsPanel?.gameObject.SetActive(false);
            if(_helpPanel) _helpPanel?.SetActive(false);
            if(_creditsPanel) _creditsPanel?.SetActive(false);
            
            _startGameButton?.onClick.AddListener(StartGameplayScene);
            _helpButton?.onClick.AddListener(OnHelpBtnClicked);
            _settingsButton?.onClick.AddListener(ShowSettings);
            _creditsButton?.onClick.AddListener(OnCreditsBtnClicked);
            _exitGameButton?.onClick.AddListener(QuitGame);
        }
        
        
        private void ShowSettings()
        {
            if(!_settingsPanel) return;
            
            _settingsPanel.IsInMainMenu = true;
            _settingsPanel.SetEnable(true);
        }

        private void OnHelpBtnClicked()
        {
            if(_helpPanel) _helpPanel?.SetActive(true);
        }
        
        private void OnCreditsBtnClicked()
        {
            if(_creditsPanel) _creditsPanel?.SetActive(true);
        }
        private void StartGameplayScene()
        {
            _ = ScenesManager.Instance.LoadGameplay();
        }

        private void QuitGame()
        {
            Application.Quit();
            Debug.Log("Quit Game");
        }
    }
}