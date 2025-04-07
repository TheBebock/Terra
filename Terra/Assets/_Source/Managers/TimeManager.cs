using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.InputManagement;
using Terra.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Managers
{
    public class TimeManager : MonoBehaviourSingleton<TimeManager>, IAttachListeners
    {

        [Foldout("Debug"), ReadOnly] [SerializeField]private bool isGamePaused = false;
    
        public bool IsGamePaused => isGamePaused;
    
        private List<object> saveLocks = new List<object>();
    
        public bool CanBePaused => !isGamePaused && saveLocks.Count <= 0;
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        
        public event Action<bool> OnGamePauseStateChanged;
        
        public void AttachListeners()
        {
            if(InputManager.Instance) InputManager.Instance.AllTimeControls.Pause.performed += OnPauseInput;
            else Debug.LogError(this + " Input Manager is null");
        }
        
        private void OnPauseInput(InputAction.CallbackContext context) => ChangePauseState();
        
        
        public void ChangePauseState()
        {
        
            bool pauseState = !isGamePaused;
        
            // Additional logic here
        
            if(pauseState) PauseGame();
            else ResumeGame();
        }
        
        public void PauseGame()
        {
            // Check can game be paused
            if(!CanBePaused) return;
            
            // Set paused game flag
            isGamePaused = true;
            // Pause time
            PauseTime();
            
            // Invoke events
            OnGamePaused?.Invoke();
            OnGamePauseStateChanged?.Invoke(isGamePaused);
        }

        public void ResumeGame()
        {
            // Set paused game flag
            isGamePaused = false;
            // Resume time
            ResumeTime();
            
            // Invoke events
            OnGameResumed?.Invoke();
            OnGamePauseStateChanged?.Invoke(isGamePaused);
        }
        
        public void PauseTime() => ChangeTimeScale(0f);
        public void ResumeTime() => ChangeTimeScale(1f);
        public void ChangeTimeScale(float timeScale) => Time.timeScale = timeScale;
        


        public void DetachListeners()
        {
            if(InputManager.Instance)
                InputManager.Instance.AllTimeControls.Pause.performed -= OnPauseInput;
        }
    }
}

