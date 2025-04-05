using System;
using System.Collections;
using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.InputManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Managers
{
    public class TimeManager : MonoBehaviourSingleton<TimeManager>
    {
        private bool isPaused;
        public bool IsPaused => isPaused;
        public event Action OnTimePaused;
        public event Action OnTimeResumed;
        
        private void Start()
        {
            if(InputManager.Instance) InputManager.Instance.AllTimeControls.Pause.performed += OnPauseInput;
            else Debug.LogError(this + " Input Manager is null");
        }

        protected override void CleanUp()
        {
            if(InputManager.Instance)
                InputManager.Instance.AllTimeControls.Pause.performed -= OnPauseInput;
        }
        

        private void OnPauseInput(InputAction.CallbackContext context)
        {
            if (isPaused) Resume();
            else Pause();
            
        }
        
        public void Pause()
        {
            isPaused = true;
            Time.timeScale = 0;
            OnTimePaused?.Invoke();
        }

        public void Resume()
        {
            isPaused = false;
            Time.timeScale = 1;
            OnTimeResumed?.Invoke();
        }
    }
}

