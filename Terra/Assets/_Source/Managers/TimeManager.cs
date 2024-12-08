using System;
using System.Collections;
using System.Collections.Generic;
using Core.Generics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class TimeManager : MonoBehaviourSingleton<TimeManager>
    {
        
        public Action OnTimePaused;
        public Action OnTimeResumed;
        private bool isPaused;

        public bool IsPaused => isPaused;
        private void OnEnable()
        {
            InputManager.Instance.GetInputActions().AllTime.Pause.performed += OnPauseInput;
        }

        private void OnDisable()
        {
            InputManager.Instance.GetInputActions().AllTime.Pause.performed -= OnPauseInput;
        }

        private void OnPauseInput(InputAction.CallbackContext context)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
        public void Pause()
        {
            Time.timeScale = 0;
            OnTimePaused?.Invoke();
        }

        public void Resume()
        {
            Time.timeScale = 1;
            OnTimeResumed?.Invoke();
        }
    }
}

