using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    // Smoothness of the camera movement
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        // Check if a target is assigned
        if (target == null) return;

        // Calculate desired position with the offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between current position and desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the smoothed position to the camera
        transform.position = smoothedPosition;

        // Optional: Ensure the camera is always looking at the target
        // transform.LookAt(target);
    }
}
