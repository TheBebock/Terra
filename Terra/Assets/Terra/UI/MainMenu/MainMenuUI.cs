using Terra.Managers;
using UnityEngine;

namespace Terra.UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        public GameObject settingsPanel;

        public void Start()
        {
            settingsPanel.SetActive(false);
        }
        
        public void ShowSettings()
        {
            settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            settingsPanel.SetActive(false);
        }
        public void StartGameplayScene()
        {
            _ = ScenesManager.Instance.LoadGameplay();
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Quit Game");
        }
    }
}