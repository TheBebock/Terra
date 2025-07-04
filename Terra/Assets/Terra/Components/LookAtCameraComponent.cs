using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Extensions;
using Terra.Interfaces;
using Terra.Managers;
using Terra.Particles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Components
{
    public class LookAtCameraComponent : InGameMonobehaviour, IWithSetUp
    {
        [FormerlySerializedAs("lookAtType")] [SerializeField] private LookAtType _lookAtType = LookAtType.CameraForward;
        [FormerlySerializedAs("targetTransforms")] [SerializeField] private List<Transform> _targetTransforms = new ();
        [SerializeField, ReadOnly] private List<VFXController> _targetVFXControllers = new ();
        
        [FormerlySerializedAs("lockX")]
        [Header("Lock Rotation")]
        [SerializeField] private bool _lockX;
        [FormerlySerializedAs("lockY")] [SerializeField] private bool _lockY = true;
        [FormerlySerializedAs("lockZ")] [SerializeField] private bool _lockZ = true;

        [SerializeField] private bool _fadeIfCloseToCamera;
        [ShowIf(nameof(_fadeIfCloseToCamera))] [SerializeField] private float _fadeValue = 0.1f;
        [ShowIf(nameof(_fadeIfCloseToCamera))] [SerializeField] private float _fadeDuration = 0.25f;
        [ShowIf(nameof(_fadeIfCloseToCamera))] [SerializeField] private float _maxDistanceFromCamera = 5f;

        private List<Vector3> _originalRotations = new ();
        private Transform _lookAtCamera;

        enum LookAtType
        {
            LookAtCamera,
            CameraForward
        };

        private void Awake()
        {
            // If there are no targets set, set singular target - this.transform
            if (_targetTransforms.IsNullOrEmpty())
            {
                _targetTransforms.Add(transform);
            }

            // Get original rotation of all targets
            for (int i = 0; i < _targetTransforms.Count; i++)
            {
                _originalRotations.Add(_targetTransforms[i].rotation.eulerAngles);
            }
            
            // Get VFXControllers
            if (_targetVFXControllers.IsNullOrEmpty())
            {
                if(!_targetTransforms[0].TryGetComponent(out VFXController vfxController)) return;
                _targetVFXControllers.Add(vfxController);
            }
        }

        public void SetUp()
        {
            // Get reference to camera
            if(CameraManager.Instance) _lookAtCamera = CameraManager.Instance?.transform;
            else
            {
                Debug.LogError($"No camera manager found. {this} is turning off");   
                this.enabled = false;
            }
        }
        
        void LateUpdate()
        {
            for (int i = 0; i < _targetTransforms.Count; i++)
            {
                UpdateTransformRotation(_targetTransforms[i], _originalRotations[i]);
            }
            
            if(!_fadeIfCloseToCamera) return;
            
            for (int i = 0; i < _targetVFXControllers.Count; i++)
            {
                TryFadingDueToCamera(_targetVFXControllers[i]);
            }
        }

        private void TryFadingDueToCamera(VFXController controller)
        {
            if (!IsTargetTooCloseToCamera())
            {
                controller.DoFadeModel(1, _fadeDuration);
                return;
            }
            
            controller.DoFadeModel(_fadeValue, _fadeDuration);

        }
        
        private bool IsTargetTooCloseToCamera()
        { 
            return Vector3.Distance(transform.position, _lookAtCamera.position) < _maxDistanceFromCamera;
        }
        
        /// <summary>
        /// Handles rotation update of given target 
        /// </summary>
        private void UpdateTransformRotation(Transform targetTransform, Vector3 originalRotation)
        {

            // Set rotation based on method
            switch (_lookAtType)
            {
                case LookAtType.LookAtCamera:
                    targetTransform.LookAt(_lookAtCamera.transform.position, Vector3.up);
                    break;
                case LookAtType.CameraForward:
                    targetTransform.forward = _lookAtCamera.transform.forward;
                    break;
            }

            // Lock respective rotation
            Vector3 rotation = targetTransform.rotation.eulerAngles;
            if (_lockX) rotation.x = originalRotation.x;
            
            if (_lockY) rotation.y = originalRotation.y;
            
            if (_lockZ) rotation.z = originalRotation.z;
            
            // Set rotation accounting for locked rotations
            targetTransform.rotation = Quaternion.Euler(rotation);
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            _targetVFXControllers.Clear();

            for (int i = 0; i < _targetTransforms.Count; i++)
            {
                if (_targetTransforms[i] != null)
                {
                    if(!_targetTransforms[i].TryGetComponent(out VFXController vc)) return;
                    _targetVFXControllers.Add(vc);
                }
            }
        }
#endif

        public void TearDown()
        {
            //Noop
        }
    }
}