using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Terra.Managers;
using Terra.UI.HUD;

namespace Terra.GameStates
{
    public class StartOfFloorState : GameState
    {
        private CancellationToken _cancellationTokentoken;
        public StartOfFloorState(CancellationToken cancellationToken)
        {
            _cancellationTokentoken = cancellationToken;
        }
        public override void OnEnter() 
        {
            base.OnEnter();
            InputManager.Instance.SetPlayerControlsState(false);
            InputManager.Instance.SetPlayerControlsState(false);
            CameraManager.Instance.SpriteMask.SetActive(false);
            CameraManager.Instance.SetCameraBlendStyle(CinemachineBlendDefinition.Style.Cut);
            HUDManager.Instance.ForceSetDarkScreenAlpha(1f);
            HUDManager.Instance.ElevatorDoors.ForceSetDoorOpenPercentage(0);
            HUDManager.Instance.HideGameplayHUD();
            CameraManager.Instance.ChangeToElevatorCamera();

            _ = StartAnimation();
        }

        //TODO: Move all the different values for the animations into the GameManager
        private async UniTaskVoid StartAnimation()
        {
            
            await HUDManager.Instance.FadeOutDarkScreen(1.5f);
            await CameraManager.Instance.StartElevatorAnimation(useUpwardsPath:true);
            
            CameraManager.Instance.ChangeToFollowPlayerCamera();
            await HUDManager.Instance.ElevatorDoors.OpenDoors();
            
            
            CameraManager.Instance?.SpriteMask.SetActive(true);
            GameManager.Instance?.SwitchToGameState<GameplayState>();
        }
    }
}
