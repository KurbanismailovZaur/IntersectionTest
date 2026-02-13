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

        public static HitInfo IntersectMovingAABB(Vector3 aMin, Vector3 aMax, Vector3 bMin, Vector3 bMax, Vector3 lineA, Vector3 lineB)
        {
            var result = new HitInfo
            {
                wasHit = false,
                tEnter = 0f,
                point = Vector3.zero,
                normal = Vector3.zero
            };

            var d = lineB - lineA;

            var tEnter = 0f;
            var tExit = 1f;

            var enterNormal = Vector3.zero;

            if (!AxisCheck( aMin.x, aMax.x, bMin.x, bMax.x, d.x, Vector3.right, ref tEnter, ref tExit, ref enterNormal))
                return result;

            if (!AxisCheck(aMin.y, aMax.y, bMin.y, bMax.y, d.y, Vector3.up, ref tEnter, ref tExit, ref enterNormal))
                return result;

            if (!AxisCheck( aMin.z, aMax.z, bMin.z, bMax.z, d.z, Vector3.forward, ref tEnter, ref tExit, ref enterNormal))
                return result;

            if (tEnter < 0f || tEnter > 1f)
                return result;

            // Есть столкновение.
            result.wasHit = true;
            result.tEnter = tEnter;

            var contactCenter = lineA + d * tEnter;

            // Точка контакта (ближайшая точка A к B в момент входа)
            var aCenterOffset = (aMin + aMax) * 0.5f - lineA;
            result.point = contactCenter + aCenterOffset;

            result.normal = enterNormal;

            return result;
        }

        static bool AxisCheck(float aMin, float aMax, float bMin, float bMax, float d, Vector3 axisNormal, ref float tEnter, ref float tExit, ref Vector3 enterNormal)
        {
            if (Mathf.Approximately(d, 0f))
            {
                if (aMax < bMin || aMin > bMax)
                    return false;

                return true;
            }

            var t1 = (bMin - aMax) / d;
            var t2 = (bMax - aMin) / d;

            var enter = Mathf.Min(t1, t2);
            var exit = Mathf.Max(t1, t2);

            if (enter > tEnter)
            {
                tEnter = enter;
                enterNormal = (t1 < t2) ? -axisNormal : axisNormal;
            }

            tExit = Mathf.Min(tExit, exit);

            if (tEnter > tExit)
                return false;

            return true;
        }

        public struct HitInfo
        {
            public bool wasHit;
            public float tEnter;
            public Vector3 point;
            public Vector3 normal;
        }
    }
}