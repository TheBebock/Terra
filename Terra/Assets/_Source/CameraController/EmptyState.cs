using Terra.CameraController;
using UnityEngine;

public class EmptyCameraState : CameraState
{
    public EmptyCameraState(Transform cameraTransform, Transform targetTransform) : base(cameraTransform, targetTransform)
    {
    }
}
