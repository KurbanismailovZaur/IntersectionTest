using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] private MeshRenderer _a;
    [SerializeField] private MeshRenderer _b;
    
    private void Update()
    {
        var aBounds = _a.bounds;
        var bBounds = _b.bounds;
        
        print(AABBUtility.Intersects(aBounds.min, aBounds.max, bBounds.min, bBounds.max));
    }
}
