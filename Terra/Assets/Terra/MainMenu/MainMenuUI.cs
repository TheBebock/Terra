using Terra.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    public void Start()
    {
        ShowMainMenu();
        settingsPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        AudioManager.Instance.PlaySFX("UI_Interaction");
        settingsPanel.SetActive(true);
    }

    public void PlayGame()
    {
        AudioManager.Instance.PlaySFX("UI_Interaction");
        SceneManager.LoadScene("GameplayScene");
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX("UI_Interaction");
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void BackToMenu()
    {
        AudioManager.Instance.PlaySFX("UI_Interaction");
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}