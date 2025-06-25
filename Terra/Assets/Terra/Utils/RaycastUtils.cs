using UnityEngine;

namespace Terra.Utils
{
    public static class RaycastExtension 
    {

        public static Vector3 GetPositionInCircle(Vector3 position, float maxRandomRange,
            LayerMask obstacleMask, int targetLayerIndex, Vector3 resultOffset = default, int maxAttempts = 10)
        {
            Vector3 result = position;

            for (int i = 0; i < maxAttempts; i++)
            {
                Vector2 randomDir2D = Random.insideUnitCircle.normalized;
                float randomDistance = Random.Range(0.1f, maxRandomRange);
                Vector3 horizontalOffset = new Vector3(randomDir2D.x, 0, randomDir2D.y) * randomDistance;
                Vector3 origin = position + horizontalOffset + Vector3.up;
            
                if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 50, obstacleMask))
                {
                    if (hit.collider.gameObject.layer == targetLayerIndex)
                    {
                        result = hit.point + resultOffset;
                        break;
                    }
                }
            }

            return result;
        }

    }
}
