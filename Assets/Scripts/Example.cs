using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] private MeshRenderer _a;
    [SerializeField] private MeshRenderer _b;
    [SerializeField] private Transform _lineA;
    [SerializeField] private Transform _lineB;
    
    private void Update()
    {
        var aBounds = _a.bounds;
        var bBounds = _b.bounds;
        
        // print(AABBUtility.Intersects(aBounds.min, aBounds.max, bBounds.min, bBounds.max));
        var result = AABBUtility.IntersectMovingAABB(aBounds.min, aBounds.max, bBounds.min, bBounds.max, _lineA.position, _lineB.position);

        Debug.DrawLine(_lineA.position, _lineB.position, Color.yellow);
        
        if (!result.wasHit)
            print(false);
        else
        {
            print($"true {result.tEnter} {result.point}");
            var intersectBounds = new Bounds(result.point, aBounds.size);
            DrawWireAABB(intersectBounds.min, intersectBounds.max);
        }
    }

    private void DrawWireAABB(Vector3 min, Vector3 max)
    {
        var p = new Vector3[8];

        p[0] = new Vector3(min.x, min.y, min.z);
        p[1] = new Vector3(max.x, min.y, min.z);
        p[2] = new Vector3(max.x, max.y, min.z);
        p[3] = new Vector3(min.x, max.y, min.z);

        p[4] = new Vector3(min.x, min.y, max.z);
        p[5] = new Vector3(max.x, min.y, max.z);
        p[6] = new Vector3(max.x, max.y, max.z);
        p[7] = new Vector3(min.x, max.y, max.z);

        Debug.DrawLine(p[0], p[1], Color.yellow);
        Debug.DrawLine(p[1], p[2], Color.yellow);
        Debug.DrawLine(p[2], p[3], Color.yellow);
        Debug.DrawLine(p[3], p[0], Color.yellow);

        Debug.DrawLine(p[4], p[5], Color.yellow);
        Debug.DrawLine(p[5], p[6], Color.yellow);
        Debug.DrawLine(p[6], p[7], Color.yellow);
        Debug.DrawLine(p[7], p[4], Color.yellow);

        Debug.DrawLine(p[0], p[4], Color.yellow);
        Debug.DrawLine(p[1], p[5], Color.yellow);
        Debug.DrawLine(p[2], p[6], Color.yellow);
        Debug.DrawLine(p[3], p[7], Color.yellow);
    }

}
