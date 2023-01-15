using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    [ColorHeader("Invoking - On Gravity Body Enable/Disable Channels", ColorHeaderColor.TriggeringEvents)] 
    [SerializeField]private GravityBodyEventChannelSO onBodyEnabled;
    [SerializeField] private GravityBodyEventChannelSO onBodyDisabled;
    
    [ColorHeader("Dependencies")]
    [SerializeField] private Rigidbody targetRb;

    [ColorHeader("Debug")]
    [SerializeField, ReadOnly] private Vector3 currentGravityAccel;
    [SerializeField, ReadOnly] private Vector3 currentGravityDir;
    [SerializeField, ReadOnly] private float currentGravityAccelMag;

    public Vector3 GravityDirection => currentGravityDir;
    public Vector3 GravityAcceleration => currentGravityAccel;
    public float GravityAccelMag => currentGravityAccelMag;

    private void OnEnable()
    {
        onBodyEnabled.Raise(this);
    }
    
    private void OnDisable()
    {
        onBodyDisabled.Raise(this);
    }

    public void ResetGravityCache()
    {
        currentGravityAccel = Vector3.zero;
        currentGravityDir = Vector3.zero;
        currentGravityAccelMag = 0;
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
        Vector3 lineEnd = lineStart + currentGravityDir * 4f;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(lineStart, lineEnd);
        Handles.color = Color.blue;
        Handles.Label(lineEnd, currentGravityAccelMag.ToString());
    }
}
