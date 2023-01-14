using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BasicUtil
{
    public static Matrix4x4 BasisVecToMtx(Vector3 x, Vector3 y, Vector3 z)
    {
        var matrix = Matrix4x4.identity;
        matrix.SetColumn(0, x);
        matrix.SetColumn(1, y);
        matrix.SetColumn(2, z);
        return matrix;
    }
}
