using Cinemachine;
using Cysharp.Threading.Tasks;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.InputSystem;
using Terra.Managers;
using Terra.UI.HUD;
using UnityEngine;

namespace Terra.GameStates
{
    public class StartOfFloorState : GameState
    {
        public override void OnEnter() 
        {
            base.OnEnter(); 
            TimeManager.Instance.ResumeTime();
            InputsManager.Instance.SetPlayerControlsState(false);
            InputsManager.Instance.SetAllTimeControlsState(false);
            CameraManager.Instance.SpriteMask.SetActive(false);
            CameraManager.Instance.SetCameraBlendStyle(CinemachineBlendDefinition.Style.Cut);
            HUDManager.Instance.HideGameplayHUD();
            
            HUDManager.Instance.ForceSetDarkScreenAlpha(1f);
            HUDManager.Instance.ElevatorDoors.ForceSetDoorOpenPercentage(0);
            CameraManager.Instance.ChangeToElevatorCamera();
            
            string audioClipName;
            string otherAudioClipName;

            if (Random.Range(0, 2) == 0)
            {
                audioClipName = "track_1";
                otherAudioClipName = "track_2";
            }
            else
            {
                audioClipName = "track_2";
                otherAudioClipName = "track_1";
            }
            if (AudioManager.Instance.IsMusicPlayingClip(audioClipName))
            {
                AudioManager.Instance.PlayMusic(otherAudioClipName);
            }
            else
            {
                AudioManager.Instance.PlayMusic(audioClipName);
            }
            
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
            
            HUDManager.Instance.StatsDisplay.ResetObjectOpacityToDefault();
            HUDManager.Instance.PassiveItemsDisplay.ResetObjectOpacityToDefault();
            
            EventsAPI.Invoke<ElevatorGeneratorStoppedEvent>();
            CameraManager.Instance?.SpriteMask.SetActive(true);
            GameManager.Instance?.SwitchToGameState<GameplayState>();
        }
    }
}
