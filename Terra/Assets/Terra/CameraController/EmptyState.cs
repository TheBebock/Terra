using UnityEngine;

namespace Terra.CameraController
{
    public class EmptyCameraState : CameraState
    {
        public EmptyCameraState(Transform cameraTransform, Transform targetTransform) : base(cameraTransform, targetTransform)
        {
        }
    }
}
