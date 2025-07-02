using System;
using Terra.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _helpPanel;

        private void Awake()
        {
            if(_settingsPanel)_settingsPanel?.SetActive(false);
            if(_helpPanel) _helpPanel?.SetActive(false);
            _startGameButton?.onClick.AddListener(StartGameplayScene);
            _exitGameButton?.onClick.AddListener(QuitGame);
        }
        
        
        public void ShowSettings()
        {
            _settingsPanel?.SetActive(true);
        }
        public void ShowHelp()
        {
            _helpPanel?.SetActive(true);
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