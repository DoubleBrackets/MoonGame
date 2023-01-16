using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityDisplay : MonoBehaviour
{
    [ColorHeader("Listening - Ask Set Velocity Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private Vector3EventChannelSO askSetVelocity;
    
    [ColorHeader("Dependencies")] 
    [SerializeField] private LineRenderer velocityLine;
    [SerializeField] private Transform targetLineTransform;

    [ColorHeader("Config")] 
    [SerializeField] private float velocityLineMaxLength;
    [SerializeField] private float maxDisplayVelocity;

    private void OnEnable()
    {
        askSetVelocity.OnRaised += SetVelocity;
    }
    
    private void OnDisable()
    {
        askSetVelocity.OnRaised -= SetVelocity;
    }

    private void SetVelocity(Vector3 vel)
    {
        Vector3 dir = targetLineTransform.InverseTransformVector(vel.normalized);
        float length = velocityLineMaxLength * Mathf.InverseLerp(0f, maxDisplayVelocity, vel.magnitude);
        
        velocityLine.SetPosition(1, dir * length);
    }
}
