using Terra.InputSystem;

namespace Terra.GameStates
{
    public class PlayerDeadState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            InputManager.Instance?.SetPlayerControlsState(false);
        }
    }
}
