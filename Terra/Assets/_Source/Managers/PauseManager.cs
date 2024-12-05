using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu UI
    private bool isPaused = false;
    private InputSystem inputActions;

    private void Awake()
    {
        inputActions = InputManager.Instance.GetInputActions();
        inputActions.AllTime.Enable(); 
    }

    private void OnEnable()
    {
        inputActions.AllTime.Enable(); // Enable AllTime action map
    }

    private void OnDisable()
    {
        inputActions.AllTime.Disable(); // Disable AllTime action map
    }

    private void Update()
    {
        if (inputActions.AllTime.Pause.triggered)
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
        Time.timeScale = 0f; // Pause the game
        pauseMenu.SetActive(true); // Show the pause menu
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false); // Hide the pause menu
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    }
}
