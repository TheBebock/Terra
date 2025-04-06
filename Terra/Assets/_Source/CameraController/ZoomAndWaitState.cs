using System;
using System.Collections;
using System.Collections.Generic;
using Terra.CameraController;
using UnityEngine;

[Serializable]
public class ZoomAndWaitState : CameraState
{
    public ZoomAndWaitState(Transform cameraTransform, Transform targetTransform) : base(cameraTransform, targetTransform)
    {
        
    }
}
