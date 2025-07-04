using System;
using Terra.InputSystem;
using Terra.Managers;

namespace Terra.GameStates
{

    /// <summary>
    /// Empty game state, used on default
    /// </summary>
    [Serializable]
    public class DefaultGameState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            TimeManager.Instance.ResumeTime();
            InputsManager.Instance.SetAllTimeControlsState(false);
            InputsManager.Instance.SetPlayerControlsState(false);
        }
    }
}

