using System;
using UnityEngine;

namespace Terra.CameraController
{
    [Serializable]
    public class ZoomAndWaitState : CameraState
    {
        public ZoomAndWaitState(Transform cameraTransform, Transform targetTransform) : base(cameraTransform, targetTransform)
        {
        
        }
    }
}
