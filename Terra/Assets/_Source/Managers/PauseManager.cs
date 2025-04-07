using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using UnityEngine;

public class PauseManager : MonoBehaviourSingleton<PauseManager>
{

    [Foldout("Debug"), ReadOnly] [SerializeField]private bool isGamePaused = false;
    
    public bool IsGamePaused => isGamePaused;
    
    private List<object> saveLocks = new List<object>();
    
    public bool CanBePaused => !isGamePaused && saveLocks.Count <= 0;
    
    public void ChangePauseState()
    {
        
        bool pauseState = !isGamePaused;
        
        // Additional logic
        
        if(pauseState) PauseGame();
        
        else ResumeGame();
        
    }
    
    private void PauseGame()
    {
        
    }

    private void ResumeGame()
    {
        
    }
}
