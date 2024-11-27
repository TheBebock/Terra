using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu UI
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pauzuje gre
        pauseMenu.SetActive(true); // Pokazuje pause menu
        Cursor.lockState = CursorLockMode.None; // Odblokowuje kursor
        Cursor.visible = true; // Pokazuje kursor
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Wznawia gre
        pauseMenu.SetActive(false); // Ukrywa Pause menu
        Cursor.lockState = CursorLockMode.Locked; // Blokuje kursor
        Cursor.visible = false; // Chowa kursor
    }
}
