using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terra.Components
{

    public class LookAtCameraComponent : MonoBehaviour
    {
        [SerializeField] private LookAtType lookAtType = LookAtType.CameraForward;

        [Header("Lock Rotation")]
        [SerializeField] private bool lockX;
        [SerializeField] private bool lockY = true;
        [SerializeField] private bool lockZ = true;

        private Vector3 originalRotation;

        private Camera lookAtCamera;

        enum LookAtType
        {
            LookAtCamera,
            CameraForward
        };

        private void Awake()
        {
            originalRotation = transform.rotation.eulerAngles;
        }
        
        //TODO: Change Camera.main to CameraManager.Instance.CurrentCamera
        private void Start()
        {
            lookAtCamera = Camera.main;
        }

        void LateUpdate()
        {
            switch (lookAtType)
            {
                case LookAtType.LookAtCamera:
                    transform.LookAt(lookAtCamera.transform.position, Vector3.up);
                    break;
                case LookAtType.CameraForward:
                    transform.forward = lookAtCamera.transform.forward;
                    break;
                default:
                    break;
            }

            Vector3 rotation = transform.rotation.eulerAngles;
            if (lockX)
            {
                rotation.x = originalRotation.x;
            }

            if (lockY)
            {
                rotation.y = originalRotation.y;
            }

            if (lockZ)
            {
                rotation.z = originalRotation.z;
            }

            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}