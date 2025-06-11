using System;
using Terra.InputSystem;
using Terra.Managers;
using Terra.UI.HUD;
using UnityEngine;

namespace Terra.GameStates
{

    [Serializable]
    public class GameplayState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            
            HUDManager.Instance?.ShowGameplayHUD();
            
            if (WaveManager.Instance)
            {
                WaveManager.Instance.StartWaves();
                Debug.Log($"{this}: Starting Waves {WaveManager.Instance.name} + {WaveManager.Instance.gameObject.activeInHierarchy}");
            }
            else
            {
                Debug.LogError($"{this}: No Wave Manager found");
            }
            
            InputManager.Instance?.SetPlayerControlsState(true);
            InputManager.Instance?.SetAllTimeControlsState(true);
            TimeManager.Instance?.ResumeTime();
            
        }
    }
}

