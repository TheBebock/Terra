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
            TimeManager.Instance.AddPauseLock(this);
            TimeManager.Instance.PauseTime();
            UIWindowManager.Instance.OpenWindow<RewardWindow>();
            HUDManager.Instance.StatsDisplay.Show();
            HUDManager.Instance.ShowAmmoCounter();
            HUDManager.Instance.ShowCrystalCounter();
        }

        public override void OnExit()
        {
            base.OnExit();
            TimeManager.Instance.RemovePauseLock(this);
            TimeManager.Instance.ResumeTime();
        }
    }
}
