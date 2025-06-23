using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.Extensions;
using Terra.Interfaces;
using Terra.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Components
{

    public class LookAtCameraComponent : InGameMonobehaviour, IWithSetUp
    {
        [FormerlySerializedAs("lookAtType")] [SerializeField] private LookAtType _lookAtType = LookAtType.CameraForward;
        [FormerlySerializedAs("targetTransforms")] [SerializeField] private List<Transform> _targetTransforms = new ();
        
        [FormerlySerializedAs("lockX")]
        [Header("Lock Rotation")]
        [SerializeField] private bool _lockX;
        [FormerlySerializedAs("lockY")] [SerializeField] private bool _lockY = true;
        [FormerlySerializedAs("lockZ")] [SerializeField] private bool _lockZ = true;

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
        }

        public void SetUp()
        {
            // Get refenrece to camera
            if(CameraManager.Instance) _lookAtCamera = CameraManager.Instance?.transform;
            else
            {
                Debug.LogError($"No camera manager found. {this} is turning off");   
                this.enabled = false;
                return;
            }
        }
        
        void LateUpdate()
        {
            for (int i = 0; i < _targetTransforms.Count; i++)
            {
                UpdateTransformRotation(_targetTransforms[i], _originalRotations[i]);
            }
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



        public void TearDown()
        {
            //Noop
        }
    }
}