using UnityEngine;

namespace DefaultNamespace
{
    public static class AABBUtility
    {
        public static HitInfo CheckIntersection(Vector3 syringeMin, Vector3 syringeMax, Vector3 syringePivotOffset, Vector3 laserMin, 
            Vector3 laserMax, Vector3 lineStart, Vector3 lineEnd)
        {
            var result = new HitInfo
            {
                wasHit = false,
                normalizedDistance = 0f,
                collisionPoint = Vector3.zero
            };

            var d = lineEnd - lineStart;
            var centerStart = lineStart - syringePivotOffset;
            var worldAMin = syringeMin + centerStart;
            var worldAMax = syringeMax + centerStart;

            var tEnter = 0f;
            var tExit  = 1f;

            if (!CheckIntersectionByAxis(worldAMin.x, worldAMax.x, laserMin.x, laserMax.x, d.x, ref tEnter, ref tExit))
                return result;

            if (!CheckIntersectionByAxis(worldAMin.y, worldAMax.y, laserMin.y, laserMax.y, d.y, ref tEnter, ref tExit))
                return result;

            if (!CheckIntersectionByAxis(worldAMin.z, worldAMax.z, laserMin.z, laserMax.z, d.z, ref tEnter, ref tExit))
                return result;

            if (tEnter < 0f || tEnter > 1f)
                return result;

            result.wasHit = true;
            result.normalizedDistance = tEnter;

            var pivotHit  = lineStart + d * tEnter;
            var centerHit = pivotHit - syringePivotOffset;

            result.collisionPoint = centerHit;

            return result;
        }

        
        static bool CheckIntersectionByAxis(float aMin, float aMax, float bMin, float bMax, float d, ref float tEnter, ref float tExit)
        {
            const double EPS = 1e-9;

            if (Mathf.Abs(d) < EPS)
            {
                if (aMax < bMin || aMin > bMax)
                    return false;

                return true;
            }

            var t1 = (bMin - aMax) / d;
            var t2 = (bMax - aMin) / d;

            var enter = Mathf.Min(t1, t2);
            var exit  = Mathf.Max(t1, t2);

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
                var start = polyline[i];
                var end   = polyline[i + 1];

                var hit = CheckIntersection(aMin, aMax, aPivotOffset, bMin, bMax, start, end);

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