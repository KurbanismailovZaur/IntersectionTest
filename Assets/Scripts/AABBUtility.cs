using UnityEngine;

namespace DefaultNamespace
{
    public static class AABBUtility
    {
        public static HitInfo IntersectMovingAABBWithOffset(Vector3 aMin, Vector3 aMax, Vector3 bMin, Vector3 bMax, 
            Vector3 lineStart, Vector3 lineEnd, Vector3 pivotOffset)
        {
            HitInfo result = new HitInfo
            {
                wasHit = false,
                normalizedDistance = 0f,
                collisionPoint = Vector3.zero
            };

            Vector3 d = lineEnd - lineStart;
            Vector3 centerStart = lineStart - pivotOffset;
            Vector3 worldAMin = aMin + centerStart;
            Vector3 worldAMax = aMax + centerStart;

            float tEnter = 0f;
            float tExit  = 1f;

            if (!AxisCheck(worldAMin.x, worldAMax.x, bMin.x, bMax.x, d.x, ref tEnter, ref tExit))
                return result;

            if (!AxisCheck(worldAMin.y, worldAMax.y, bMin.y, bMax.y, d.y, ref tEnter, ref tExit))
                return result;

            if (!AxisCheck(worldAMin.z, worldAMax.z, bMin.z, bMax.z, d.z, ref tEnter, ref tExit))
                return result;

            if (tEnter < 0f || tEnter > 1f)
                return result;

            result.wasHit = true;
            result.normalizedDistance = tEnter;

            Vector3 pivotHit  = lineStart + d * tEnter;
            Vector3 centerHit = pivotHit - pivotOffset;

            result.collisionPoint = centerHit;

            return result;
        }

        
        static bool AxisCheck(float aMin, float aMax, float bMin, float bMax, float d, ref float tEnter, ref float tExit)
        {
            if (Mathf.Approximately(d, 0f))
            {
                if (aMax < bMin || aMin > bMax)
                    return false;

                return true;
            }

            float t1 = (bMin - aMax) / d;
            float t2 = (bMax - aMin) / d;

            float enter = Mathf.Min(t1, t2);
            float exit  = Mathf.Max(t1, t2);

            tEnter = Mathf.Max(tEnter, enter);
            tExit  = Mathf.Min(tExit,  exit);

            if (tEnter > tExit)
                return false;

            return true;
        }
        
        public static bool IntersectsAlongPolyline(Vector3 aMin, Vector3 aMax, Vector3 aPivotOffset, Vector3 bMin, Vector3 bMax, Vector3[] polyline, out HitInfo hitInfo)
        {
            hitInfo = default;

            if (polyline == null || polyline.Length < 2)
                return false;

            for (var i = 0; i < polyline.Length - 1; i++)
            {
                Vector3 start = polyline[i];
                Vector3 end   = polyline[i + 1];

                var hit = IntersectMovingAABBWithOffset(aMin, aMax, bMin, bMax, start, end, aPivotOffset);

                if (hit.wasHit)
                {
                    hitInfo = hit;
                    return true;
                }
            }

            return false;
        }
    }

    public struct HitInfo
    {
        public bool wasHit;
        public float normalizedDistance;
        public Vector3 collisionPoint;
    }
}