using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    [SerializeField] private Rigidbody targetRb;

    private Vector3 currentGravityAccel;
    private Vector3 currentGravityDir;
    private float currentGravityAccelMag;

    public Vector3 GravityDirection => currentGravityDir;
    public Vector3 GravityAcceleration => currentGravityAccel;
    public float GravityAccelMag => currentGravityAccelMag;

    public void ResetGravityCache()
    {
        currentGravityAccel = Vector3.zero;
        currentGravityDir = Vector3.zero;
    }

    public void AddAccel(Vector3 val)
    {
        currentGravityAccel += val;
    }

    public void FinishCalculatingAccel()
    {
        currentGravityDir = currentGravityAccel.normalized;
        currentGravityAccelMag = currentGravityAccel.magnitude;
    }

    public void ApplyGravity(float timeStep)
    {
        targetRb.velocity += currentGravityAccel * timeStep;
    }

    private void OnDrawGizmos()
    {
        Vector3 lineStart = targetRb.position;
        Vector3 lineEnd = lineStart + currentGravityDir * 2f;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(lineStart, lineEnd);
        Handles.color = Color.blue;
        Handles.Label(lineEnd, currentGravityAccelMag.ToString());
    }
}
