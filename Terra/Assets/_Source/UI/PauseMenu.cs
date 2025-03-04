using Terra.Managers;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu; // Reference to the pause menu UI
    
    
    private void Start()
    {
        TimeManager.Instance.OnTimePaused += ShowPause;
        TimeManager.Instance.OnTimeResumed += HidePause;

    }

    private void OnDestroy()
    {
        TimeManager.Instance.OnTimePaused -= ShowPause;
        TimeManager.Instance.OnTimeResumed -= HidePause;
    }
    

    public void ShowPause()
    {
        pauseMenu.SetActive(true); 
        Cursor.visible = true;
    }
    
    public void HidePause()
    {
        pauseMenu.SetActive(false);
        Cursor.visible = false;
    }
    
    
}
