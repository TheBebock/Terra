using Cinemachine;
using Cysharp.Threading.Tasks;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.InputSystem;
using Terra.Managers;
using Terra.UI.HUD;

namespace Terra.GameStates
{
    public class StartOfFloorState : GameState
    {
        public override void OnEnter() 
        {
            base.OnEnter(); 
            TimeManager.Instance.ResumeTime();
            InputManager.Instance.SetPlayerControlsState(false);
            InputManager.Instance.SetAllTimeControlsState(false);
            CameraManager.Instance.SpriteMask.SetActive(false);
            CameraManager.Instance.SetCameraBlendStyle(CinemachineBlendDefinition.Style.Cut);
            HUDManager.Instance.ForceSetDarkScreenAlpha(1f);
            HUDManager.Instance.ElevatorDoors.ForceSetDoorOpenPercentage(0);
            HUDManager.Instance.HideGameplayHUD();
            CameraManager.Instance.ChangeToElevatorCamera();
            AudioManager.Instance.PlayMusic("track_1");
            EventsAPI.Invoke<StartOfNewFloorEvent>();
            _ = StartAnimation();
        }

        //TODO: Move all the different values for the animations into the GameManager
        private async UniTaskVoid StartAnimation()
        {
            
            await HUDManager.Instance.FadeOutDarkScreen(1.5f);
            await CameraManager.Instance.StartElevatorAnimation(useUpwardsPath:true);
            
            CameraManager.Instance.ChangeToFollowPlayerCamera();
            await HUDManager.Instance.ElevatorDoors.OpenDoors();
            
            EventsAPI.Invoke<ElevatorGeneratorStoppedEvent>();
            CameraManager.Instance?.SpriteMask.SetActive(true);
            GameManager.Instance?.SwitchToGameState<GameplayState>();
        }
    }
}
