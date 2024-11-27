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
        Time.timeScale = 0f; // Freezes the game
        pauseMenu.SetActive(true); // Shows the pause menu
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
        Cursor.visible = true; // Makes the cursor visible
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resumes the game
        pauseMenu.SetActive(false); // Hides the pause menu
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor again
        Cursor.visible = false; // Hides the cursor
    }
}
