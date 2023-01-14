using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    [SerializeField] private Rigidbody targetRb;
    [SerializeField] private GravitySource source;
    [SerializeField] private float gravityAccel;
    
    public Vector3 GravityDirection;

    private void FixedUpdate()
    {
        GravityDirection = (source.transform.position - targetRb.position).normalized;
        targetRb.AddForce(GravityDirection.normalized * gravityAccel, ForceMode.Acceleration);
    }
}
