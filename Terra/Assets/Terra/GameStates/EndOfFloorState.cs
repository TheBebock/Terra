using Cysharp.Threading.Tasks;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.InputSystem;
using Terra.Managers;
using Terra.Player;
using Terra.UI.HUD;

namespace Terra.GameStates
{
    public class EndOfFloorState : GameState
    {
        public override void OnEnter() 
        {
            base.OnEnter();
            InputsManager.Instance.SetPlayerControlsState(false);
            InputsManager.Instance.SetAllTimeControlsState(false);
            
            CameraManager.Instance.SetCameraBlendStyle();
            CameraManager.Instance.SpriteMask.SetActive(false);
            
            HUDManager.Instance.HideGameplayHUD();
            
            _ = StartAnimation();
        }

        //TODO: Move all the different values for the animations into the GameManager
        private async UniTaskVoid StartAnimation()
        {
            //AudioManger.Instance.PlaySFX("...")

            await HUDManager.Instance.ElevatorDoors.CloseDoors();
            CameraManager.Instance.ChangeToElevatorCamera();
            await CameraManager.Instance.StartElevatorAnimation();
            await HUDManager.Instance.FadeInDarkScreen(1.5f);
            EventsAPI.Invoke<EndOfElevatorAnimationEvent>();
            EventsAPI.Invoke<PerformCleanupEvent>();
            GameManager.Instance.SwitchToGameState<UpgradeGameState>();
            
        }

        public override void OnExit()
        {
            base.OnExit();
            
            PlayerManager.Instance.MovePlayerToStartingPosition();
            CameraManager.Instance.SpriteMask.SetActive(true);
            CameraManager.Instance.ForceSetElevatorCameraPosition(0f, true);
        }
    }
}
