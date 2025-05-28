using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Terra.Core.Generics;
using UnityEngine;

namespace Terra.Managers
{
    
    public class CameraManager : MonoBehaviourSingleton<CameraManager>
    {
        [SerializeField] private GameObject _spriteMask;
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private CinemachineVirtualCamera _followPlayerCamera;
        [SerializeField] private CinemachineVirtualCamera _elevatorCamera;
        [SerializeField] private AnimationCurve _elevatorCurve;
        [SerializeField] private CinemachinePathBase _upwardsPath;
        [SerializeField] private CinemachinePathBase _downwardsPath;
        [SerializeField] private Transform _pathTransform;
        
        [SerializeField] private float _dollyDuration;
        
        [Foldout("Debug"), ReadOnly][SerializeField] private float _currentElevatorPathProgress;
        [Foldout("Debug"), ReadOnly] [SerializeField] private Camera _mainCamera;
        
        public Camera MainCamera => _mainCamera;
        public GameObject SpriteMask => _spriteMask;
        
        private CinemachineTrackedDolly _elevatorTrackedDolly;
        private const int StandardPriority = 10;
        private const int TopPriority = 15;
        private Vector3 _startingLevelPosition;
        protected override void Awake()
        {
            base.Awake();
            _elevatorTrackedDolly = _elevatorCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
            if (_elevatorTrackedDolly == null)
            {
                Debug.LogError($"{this}: {_elevatorCamera.name} should have a Tracked Dolly body");
            }
            _startingLevelPosition = _pathTransform.transform.position;
            _elevatorTrackedDolly.m_PathPosition = 0.0f;
        }
        
        public void ChangeToFollowPlayerCamera()
        {
            SetNewVCamTopPriority(_followPlayerCamera);
        }

        public void SetCameraBlendStyle(CinemachineBlendDefinition.Style newBlendStyle = CinemachineBlendDefinition.Style.EaseInOut, float time = 0.5f)
        {
            _cinemachineBrain.m_DefaultBlend =  new CinemachineBlendDefinition(newBlendStyle, time);
        }
        
        public void ChangeToElevatorCamera()
        {
            SetNewVCamTopPriority(_elevatorCamera);
        }

        public void ForceSetElevatorCameraPosition(float normalizedPathProgress, bool useUpwardsPath = false)
        {
            normalizedPathProgress = Mathf.Clamp01(normalizedPathProgress);
            _currentElevatorPathProgress = normalizedPathProgress;
            
            _elevatorTrackedDolly.m_Path = useUpwardsPath ? _upwardsPath : _downwardsPath;
            
            _elevatorTrackedDolly.m_PathPosition = _currentElevatorPathProgress;
        }
        public async UniTask StartElevatorAnimation(bool useUpwardsPath = false)
        {
            _currentElevatorPathProgress = 0;
            
            _elevatorTrackedDolly.m_Path = useUpwardsPath ? _upwardsPath : _downwardsPath;
            _pathTransform.position = useUpwardsPath ? _startingLevelPosition
                : _followPlayerCamera.transform.position;
            
            SetNewVCamTopPriority(_elevatorCamera);
            
            await StartCameraDollyTrack().AttachExternalCancellation(CancellationToken);
        }
        
        private async UniTask StartCameraDollyTrack()
        {
            
            DOTween.Kill(_elevatorTrackedDolly);


            Tween tween = DOTween.To(() => _currentElevatorPathProgress, 
                    x => {
                        _currentElevatorPathProgress = x;
                        _elevatorTrackedDolly.m_PathPosition = x;
                    }, 
                    1f, 
                    _dollyDuration)
                .SetEase(_elevatorCurve);

            await tween.AwaitForComplete(cancellationToken: CancellationToken);
        }




        private void SetNewVCamTopPriority(ICinemachineCamera vCam)
        {
            _cinemachineBrain.ActiveVirtualCamera.Priority = StandardPriority;
            vCam.Priority = TopPriority;
        }
        

        private void OnValidate()
        {
            if (!_mainCamera) _mainCamera = GetComponent<Camera>();
            if(!_cinemachineBrain) _cinemachineBrain = GetComponent<CinemachineBrain>();
        }
    }
}