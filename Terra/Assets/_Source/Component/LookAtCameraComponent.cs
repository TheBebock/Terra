using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraComponent : MonoBehaviour
{
    [SerializeField] private LookAtType lookAtType = LookAtType.CameraForward;

    [Header("Lock Rotation")]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    private Vector3 originalRotation;

    enum LookAtType { LookAtCamera, CameraForward };

    private void Awake()
    {
        originalRotation = transform.rotation.eulerAngles;
    }

    //TODO: Change Camera.main to CameraManager.Instance.CurrentCamera
    void LateUpdate()
    {
        switch (lookAtType)
        {
            case LookAtType.LookAtCamera:
                transform.LookAt(Camera.main.transform.position, Vector3.up);
                break;
            case LookAtType.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            default:
                break;
        }
        
        Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX) { rotation.x = originalRotation.x; }
        if (lockY) { rotation.y = originalRotation.y; }
        if (lockZ) { rotation.z = originalRotation.z; }
        transform.rotation = Quaternion.Euler(rotation);
    }
}
