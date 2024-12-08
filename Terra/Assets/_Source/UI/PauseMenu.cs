using System;
using Core.Generics;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu; // Reference to the pause menu UI
    
    
    private void OnEnable()
    {
        TimeManager.Instance.OnTimePaused += ShowPause;
        TimeManager.Instance.OnTimePaused += HidePause;

    }

    private void OnDisable()
    {
        TimeManager.Instance.OnTimePaused -= ShowPause;
        TimeManager.Instance.OnTimePaused -= HidePause;

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
