using Terra.InputSystem;
using Terra.Managers;
using Terra.UI.HUD;
using Terra.UI.Windows.RewardWindow;
using UIExtensionPackage.UISystem.UI.Windows;

namespace Terra.GameStates
{
    public class UpgradeGameState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            TimeManager.Instance.PauseTime();
            TimeManager.Instance.AddPauseLock(this);
            InputsManager.Instance.SetAllTimeControlsState(true);
            UIWindowManager.Instance.OpenWindow<RewardWindow>();
            HUDManager.Instance.ShowUpgradeHUD();
        }

        public override void OnExit()
        {
            base.OnExit();
            
            InputsManager.Instance.SetAllTimeControlsState(false);
            TimeManager.Instance.RemovePauseLock(this);
            TimeManager.Instance.ResumeTime();
        }
    }
}
