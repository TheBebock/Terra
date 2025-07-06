using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Extensions;
using Terra.GameStates;
using Terra.InputSystem;
using Terra.Interfaces;
using Terra.UI.Windows;
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
        private bool PauseLocks => _pauseLocks.Count > 0;
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        
        public void AttachListeners()
        {
            if(InputsManager.Instance) InputsManager.Instance.AllTimeControls.Pause.started += OnPauseInput;
            else Debug.LogError(this + " Input Manager is null");
        }

        private void OnPauseInput(InputAction.CallbackContext context)
        {
            InUpgradePanelWindow();
            ChangePauseState();
        }

        private void InUpgradePanelWindow()
        {
            if (GameManager.Instance.IsCurrentState<UpgradeGameState>())
            {
                if (UIWindowManager.Instance.TryGetWindowFromStacks(out PauseWindow window))
                {
                    window.Close();
                }
                else
                {
                    UIWindowManager.Instance.OpenWindow<PauseWindow>();
                }
            }
        }
        public void ChangePauseState()
        {

            if (!_isGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        
        public void PauseGame()
        {
            // Check can game be paused
            if(PauseLocks) return;
            
            // Set paused game flag
            _isGamePaused = true;
            // Pause time
            PauseTime();
            
            UIWindowManager.Instance?.OpenWindow<PauseWindow>();
            
            // Invoke events
            OnGamePaused?.Invoke();
        }

        public void ResumeGame()
        {
            if (PauseLocks) return;
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

        public void RemovePauseLocksAndResume()
        {
            _pauseLocks.Clear();
            _isGamePaused = false;
            ResumeTime();
        } 
        private void ChangeTimeScale(float timeScale)
        {
            if(PauseLocks) return;
            
            Time.timeScale = timeScale;
        }


        public void DetachListeners()
        {
            if(InputsManager.Instance)
                InputsManager.Instance.AllTimeControls.Pause.started -= OnPauseInput;
        }
    }
}

