using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] private MeshRenderer _a;
    [SerializeField] private Vector3 _aPivotOffset;
    [SerializeField] private MeshRenderer _b;
    [SerializeField] private Transform[] _linePoints;
    
    private void Update()
    {
        var aBounds = _a.bounds;
        aBounds.center = Vector3.zero;
        var bBounds = _b.bounds;

        var points = _linePoints.Select(p => p.position).ToArray();

        for (var i = 0; i < points.Length - 1; i++)
            Debug.DrawLine(points[i], points[i + 1], Color.yellow);    
        
        if (!AABBUtility.IntersectsAlongPolyline(aBounds.min, aBounds.max, _aPivotOffset, bBounds.min, bBounds.max, points, out var hitInfo))
            print(false);
        else
        {
            print($"true {hitInfo.normalizedDistance} {hitInfo.collisionPoint}");
            var intersectBounds = new Bounds(hitInfo.collisionPoint, aBounds.size);
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
