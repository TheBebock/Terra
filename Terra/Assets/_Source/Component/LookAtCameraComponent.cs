using System.Collections.Generic;
using Terra.CameraController;
using UnityEngine;

namespace Terra.Components
{

    public class LookAtCameraComponent : MonoBehaviour
    {
        [SerializeField] private LookAtType lookAtType = LookAtType.CameraForward;
        [SerializeField] private List<Transform> targetTransforms = null;
        
        [Header("Lock Rotation")]
        [SerializeField] private bool lockX;
        [SerializeField] private bool lockY = true;
        [SerializeField] private bool lockZ = true;

        private List<Vector3> originalRotations = new ();
        private Transform lookAtCamera;

        enum LookAtType
        {
            LookAtCamera,
            CameraForward
        };

        private void Awake()
        {
            // If there are no targets set, set singular target - this.transform
            if (targetTransforms.IsNullOrEmpty())
            {
                targetTransforms.Add(transform);
            }
            
            // Get original rotation of all targets
            for (int i = 0; i < targetTransforms.Count; i++)
            {
                originalRotations.Add(targetTransforms[i].rotation.eulerAngles);
            }

        }
        private void Start()
        {
            // Get refenrece to camera
            if(CameraManager.Instance)
                lookAtCamera = CameraManager.Instance?.transform;
            else
                Debug.LogError("No camera manager found.");
            
        }

        void LateUpdate()
        {
            for (int i = 0; i < targetTransforms.Count; i++)
            {
                UpdateTransformRotation(targetTransforms[i], originalRotations[i]);
            }
        }

        
        /// <summary>
        /// Handles rotation update of given target 
        /// </summary>
        private void UpdateTransformRotation(Transform targetTransform, Vector3 originalRotation)
        {
            // Set rotation based on method
            switch (lookAtType)
            {
                case LookAtType.LookAtCamera:
                    targetTransform.LookAt(lookAtCamera.transform.position, Vector3.up);
                    break;
                case LookAtType.CameraForward:
                    targetTransform.forward = lookAtCamera.transform.forward;
                    break;
            }

            // Lock respective rotation
            Vector3 rotation = targetTransform.rotation.eulerAngles;
            if (lockX) rotation.x = originalRotation.x;
            
            if (lockY) rotation.y = originalRotation.y;
            
            if (lockZ) rotation.z = originalRotation.z;
            
            // Set rotation accounting for locked rotations
            targetTransform.rotation = Quaternion.Euler(rotation);
        }
    }
}