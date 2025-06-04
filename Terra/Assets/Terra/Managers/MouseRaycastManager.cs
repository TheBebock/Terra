using Terra.Core.Generics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Managers
{
    public class MouseRaycastManager : MonoBehaviourSingleton<MouseRaycastManager>
    {
        private readonly Vector3 _defaultY = new( 0f, 100f, 0f );
        public Vector3 GetMousePositionInWorldPosition()
        {
            Ray ray = CameraManager.Instance.MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.point;
            }
            Debug.Log($"{nameof(MouseRaycastManager)}: Couldn't hit any colliders, proceeding to hit on default Y");
            
            Plane plane = new Plane(Vector3.up, _defaultY);
            if (!plane.Raycast(ray, out float enter))
            {
                Debug.LogError($"{nameof(MouseRaycastManager)}: No target found");
                return Vector3.zero;
            }
            Vector3 worldClickPosition = ray.GetPoint(enter);
            return worldClickPosition;
        }

        public Vector3 GetDirectionTowardsMousePosition(Vector3 sourcePosition)
        {
            return (GetMousePositionInWorldPosition() - sourcePosition).normalized;
        }
    }
}
