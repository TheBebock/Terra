using System;
using Terra.Managers;
using UnityEngine;

namespace Terra.GameStates
{

    [Serializable]
    public class GameplayState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (WaveManager.Instance)
            {
                WaveManager.Instance.StartWaves();
                Debug.Log($"{this}: Starting Waves {WaveManager.Instance.name} + {WaveManager.Instance.gameObject.activeInHierarchy}");
            }
            else
            {
                Debug.LogError($"{this}: No Wave Manager found");
            }
        }
    }
}

