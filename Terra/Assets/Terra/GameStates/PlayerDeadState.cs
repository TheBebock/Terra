using Terra.InputSystem;

namespace Terra.GameStates
{
    public class PlayerDeadState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            InputsManager.Instance?.SetPlayerControlsState(false);
            InputsManager.Instance?.SetAllTimeControlsState(false);
        }
    }
}
