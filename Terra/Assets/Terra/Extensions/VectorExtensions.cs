using UnityEngine;

namespace Terra.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 Clamp(this Vector3 value, float min, float max)
        {
            if (value.x < min || value.y < min || value.z < min ||
                value.x > max || value.y > max || value.z > max)
            {
                value.x = Mathf.Clamp(value.x, min, max);
                value.y = Mathf.Clamp(value.y, min, max);
                value.z = Mathf.Clamp(value.z, min, max);
            }

            return value;
        }

        public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
        {
            if (value.x < min.x || value.y < min.y || value.z < min.z ||
                value.x > max.x || value.y > max.y || value.z > max.z)
            {
                value.x = Mathf.Clamp(value.x, min.x, max.x);
                value.y = Mathf.Clamp(value.y, min.y, max.y);
                value.z = Mathf.Clamp(value.z, min.z, max.z);
            }

            return value;
        }
    }
}