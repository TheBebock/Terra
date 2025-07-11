using Terra.Core.Generics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Managers
{
    public class MouseRaycastManager : MonoBehaviourSingleton<MouseRaycastManager>
    {
        private readonly Vector3 _defaultY = new( 0f, 100f, 0f );
        public Vector3 GetMousePositionInWorldPosition(Vector3 resultOffset = default) 
        {
            Ray ray = CameraManager.Instance.MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    return GetFallBackPosition(ray, resultOffset);
                }
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    return hit.point + resultOffset;
                }
                return hit.point;
            }
            Debug.Log($"{nameof(MouseRaycastManager)}: Couldn't hit any colliders, proceeding to hit on default Y");
           
            return GetFallBackPosition(ray, resultOffset);
           
        }

        private Vector3 GetFallBackPosition(Ray ray,Vector3 resultOffset = default)
        {
            Plane plane = new Plane(Vector3.up, _defaultY);
            if (!plane.Raycast(ray, out float enter))
            {
                Debug.LogError($"{nameof(MouseRaycastManager)}: No target found");
                return Vector3.zero;
            }
            Vector3 worldClickPosition = ray.GetPoint(enter);
            return worldClickPosition + resultOffset;
        }
        public Vector3 GetDirectionTowardsMousePosition(Vector3 sourcePosition, Vector3 resultOffset = default)
        {
            return (GetMousePositionInWorldPosition(resultOffset) - sourcePosition).normalized;
        }
    }
}
