using UnityEngine;

namespace DefaultNamespace
{
    public static class AABBUtility
    {
        public static bool Intersects(Vector3 minA, Vector3 maxA, Vector3 minB, Vector3 maxB)
        {
            return (minA.x <= maxB.x && maxA.x >= minB.x) &&
                   (minA.y <= maxB.y && maxA.y >= minB.y) &&
                   (minA.z <= maxB.z && maxA.z >= minB.z);
        }
    }
}