using System.Threading;
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
            CameraManager.Instance.SpriteMask.SetActive(false);
            _ = StartAnimation();
        }

        //TODO: Move all the different values for the animations into the GameManager
        private async UniTaskVoid StartAnimation()
        {
            HUDManager.Instance.HideGameplayHUD();
            CameraManager.Instance.ChangeToElevatorCamera();
            await HUDManager.Instance.FadeOutDarkScreen(1.5f);
            await CameraManager.Instance.StartElevatorAnimation(true);
            CameraManager.Instance?.ChangeToFollowPlayerCamera();
            
            if (HUDManager.Instance)
            {
                await HUDManager.Instance.ElevatorDoors.OpenDoors();
            }
            
            CameraManager.Instance?.SpriteMask.SetActive(true);
            GameManager.Instance?.SwitchToGameState<GameplayState>();
        }
    }
}
