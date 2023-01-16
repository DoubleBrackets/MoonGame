using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaneGravitySource : GravitySource
{
    [ColorHeader("Dependencies")]
    [SerializeField] private BoxCollider fieldCollider;
    
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
        Vector3 dir = -transform.up;
        var plane = new Plane(transform.up, transform.position);
        float dist = plane.GetDistanceToPoint(body.transform.position);
        float t = Mathf.InverseLerp(fieldCollider.size.y, 0, dist);
        float accel = baseAcceleration * accelerationDistanceCurve.Evaluate(t);
        return dir.normalized * accel;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var endPos = transform.position + transform.up * fieldCollider.size.y;
        Gizmos.DrawLine(transform.position, endPos);
        Gizmos.DrawWireSphere(endPos, 1f);
    }
}
