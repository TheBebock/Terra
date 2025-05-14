using System;
using Terra.Extensions;
using UnityEngine;

namespace Terra.CameraController
{

    [Serializable]
    public class FollowTargetState : CameraState
    {
        public CameraConfig config;

        public FollowTargetState(Transform cameraTransform, Transform targetTransform, CameraConfig config) : base(cameraTransform, targetTransform)
        {
            this.config = config;
        }

        public override void Update()
        {
            if (targetTransform == null) return;

            // Calculate desired value
            Vector3 desiredPosition = targetTransform.position + config.offset;
            
            // Clamp value to account for constrains
            desiredPosition = desiredPosition.Clamp(config.minConstraints, config.maxConstraints);

            // Smoothly interpolate between current position and desired position
            Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, config.smoothSpeed);

            cameraTransform.position = smoothedPosition;
        }
        
    }
}