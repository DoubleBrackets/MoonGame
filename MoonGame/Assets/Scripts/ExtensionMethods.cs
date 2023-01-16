using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    public static Vector3 SwizzleXZ(this Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }

    public static Vector3 WorldPos(this SphereCollider coll)
    {
        return coll.transform.localToWorldMatrix.MultiplyPoint(coll.center);
    }

    public static void DrawMtxGizmo(this Matrix4x4 mtx)
    {
        var forwardZ = mtx.MultiplyVector(Vector3.forward).normalized;
        var rightX = mtx.MultiplyVector(Vector3.right).normalized;
        var upY = mtx.MultiplyVector(Vector3.up).normalized;
        var pos = mtx.GetPosition();
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pos, pos + forwardZ * 2f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos, pos + rightX * 2f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, pos + upY * 2f);
    }
    
    public static void DrawMtxGizmo(this Matrix4x4 mtx, Vector3 pos)
    {
        if (!mtx.ValidTRS()) return;
        var translated = Matrix4x4.TRS(pos, mtx.rotation, Vector3.one);
        translated.DrawMtxGizmo();
    }
}
