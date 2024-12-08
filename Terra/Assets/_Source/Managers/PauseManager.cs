using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu UI
    private bool isPaused = false;
    private InputSystem inputActions;

    private void Awake()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("InputManager.Instance is null! Ensure InputManager is in the scene.");
        }
        else
        {
            inputActions = InputManager.Instance.GetInputActions();
            
        }
    }

    private void OnEnable()
    {
        inputActions.AllTime.Pause.performed += OnPauseInput;
        inputActions.AllTime.Enable();
        
    }

    private void OnDisable()
    {
        inputActions.AllTime.Pause.performed -= OnPauseInput;
        inputActions.AllTime.Disable();
    }

    private void OnPauseInput(InputAction.CallbackContext context)
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
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
    
}
