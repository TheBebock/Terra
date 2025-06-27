using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Extensions;
using Terra.InputSystem;
using Terra.Interfaces;
using Terra.UI;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Managers
{
    
    /// <summary>
    /// Handles changing TimeScale and pausing game
    /// </summary>
    public class TimeManager : PersistentMonoSingleton<TimeManager>, IAttachListeners
    {

        [Foldout("Debug"), ReadOnly] [SerializeField]private bool _isGamePaused;
    
        public bool IsGamePaused => _isGamePaused;
    
        private List<object> _pauseLocks = new ();
    
        public bool CanBePaused => !IsGamePaused && _pauseLocks.Count <= 0;
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        
        public void AttachListeners()
        {
            if(InputManager.Instance) InputManager.Instance.AllTimeControls.Pause.performed += OnPauseInput;
            else Debug.LogError(this + " Input Manager is null");
        }
        
        private void OnPauseInput(InputAction.CallbackContext context) => ChangePauseState();
        
        
        public void ChangePauseState()
        {
        
            bool pauseState = !_isGamePaused;
            
        
            if(pauseState) PauseGame();
            else ResumeGame();
        }
        
        public void PauseGame()
        {
            // Check can game be paused
            if(!CanBePaused) return;

            UIWindowManager.Instance?.OpenWindow<PauseWindow>();
            
            // Set paused game flag
            _isGamePaused = true;
            // Pause time
            PauseTime();
            
            // Invoke events
            OnGamePaused?.Invoke();
        }

        public void ResumeGame()
        {
            // Set paused game flag
            _isGamePaused = false;
            // Resume time
            ResumeTime();
            
            UIWindowManager.Instance?.CloseWindow<PauseWindow>();

            // Invoke events
            OnGameResumed?.Invoke();
        }
        
        
        public void AddPauseLock(object lockObject) => _pauseLocks.Add(lockObject);

        public void RemovePauseLock(object lockObject) => _pauseLocks.RemoveElement(lockObject);
        
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

