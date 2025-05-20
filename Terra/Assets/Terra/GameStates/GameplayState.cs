using System;
using Terra.Managers;

namespace Terra.GameStates
{

    [Serializable]
    public class GameplayState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            WaveManager.Instance.StartWaves();
        }
    }
}

