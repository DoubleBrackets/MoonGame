using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SphereGravitySource : GravitySource
{
    [ColorHeader("Dependencies")]
    [SerializeField] private SphereCollider orbitCollider;
    
    [ColorHeader("Gravity Source Config", ColorHeaderColor.Config)]
    [SerializeField] private float baseAcceleration;
    [SerializeField] private AnimationCurve accelerationDistanceCurve;


    private void OnTriggerEnter(Collider other)
    {
        var body = other.GetComponent<GravityBody>();
        if (body != null)
        {
            askLinkGravityBody.Raise(body, this);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var body = other.GetComponent<GravityBody>();
        if (body != null)
        {
            askUnlinkGravityBody.Raise(body, this);
        }
    }

    public override Vector3 CalculateAcceleration(GravityBody body)
    {
        Vector3 dir = orbitCollider.WorldPos() - body.transform.position;
        float t = Mathf.InverseLerp(orbitCollider.radius, 0, dir.magnitude);
        float accel = baseAcceleration * accelerationDistanceCurve.Evaluate(t);
        return dir.normalized * accel;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(orbitCollider.WorldPos(), orbitCollider.radius);
    }
}
