using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagPhysicsState : MonoBehaviour
{
    [ColorHeader("Dependencies")] 
    [SerializeField] private GravityBody playerGravBody;
    [SerializeField] private Transform playerPhysicsBody;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MovementProfileSO movementProfile;
    [SerializeField] private Transform defaultParent;
    
    [ColorHeader("Grounded Cast Config", ColorHeaderColor.Config)]
    [SerializeField] private LayerMask groundedMask;
    [SerializeField] private Transform groundCastTransform;
    [SerializeField, Range(0f, 5f)] private float groundCastLength;
    [SerializeField, Range(0f, 5f)] private float groundCastRadius;

    [ColorHeader("Debug Info")]
    [SerializeField, ReadOnly] private bool isGrounded;
    public bool IsGrounded => isGrounded;
    [SerializeField, ReadOnly] private Vector3 groundPos;
    [SerializeField, ReadOnly] private Vector3 groundNormal;

    public Vector3 GroundNormal => groundNormal;
    public Vector3 GravityNormal => -playerGravBody.GravityDirection;
    public Vector3 GravityAcceleration => playerGravBody.GravityAcceleration;
    public float GravityAccelMag => playerGravBody.GravityAccelMag;

    public Vector3 CurrentVel => rb.velocity;

    public float VerticalSpeed => Mathf.Abs(Vector3.Dot(OrientationNormal, rb.velocity));
    public float Speed => rb.velocity.magnitude;

    // Sticking info
    private Transform groundObjectTransform;
    //private Transform prevObjectTransform;
    private Vector3 prevPos;
    
    // Basis Info
    [SerializeField, ReadOnly] private Vector3 orientationY;
    [SerializeField, ReadOnly] private Vector3 orientationX;
    [SerializeField, ReadOnly] private Vector3 orientationZ;
    [SerializeField, ReadOnly] private Matrix4x4 orientationMtx = Matrix4x4.identity;

    public Matrix4x4 OrientationMtx => orientationMtx;
    public Vector3 OrientationNormal => orientationY;

    private void FixedUpdate()
    {
        PerformGroundedCast();
        UpdateOrientationBasis();
        StickToObjects();
    }

    private void StickToObjects()
    {
        if (isGrounded)
        {
            Vector3 pos = groundObjectTransform.position;
            Vector3 offset = pos - prevPos;
            playerPhysicsBody.position += offset;
            prevPos = pos;
        }
        else
        {
            if (groundObjectTransform != null)
            {
                // Add the object velocity to rb when jumping off an object
                Vector3 pos = groundObjectTransform.position;
                Vector3 offset = pos - prevPos;
                rb.velocity += offset / Time.fixedDeltaTime;
            }
            groundObjectTransform = null;
        }
    }

    // Generate Orientation information based on some normal (ground or gravity)
    private void UpdateOrientationBasis()
    {
        // Choose up based on whether the controller is airborne or not
        Vector3 normal;
        if (isGrounded)
        {
            normal = groundNormal;
        }
        else
        {
            normal = -playerGravBody.GravityDirection;
        }

        if (normal == Vector3.zero) return;

        // Orthonormalize
        Vector3 forward = playerPhysicsBody.forward;
        // Basis 'Right'
        Vector3 tangent = Vector3.Cross(normal, forward).normalized;
        // Basis 'Forward'
        Vector3 coTangent = Vector3.Cross(tangent, normal).normalized;

        orientationZ = coTangent;
        orientationX = tangent;
        orientationY = normal;

        orientationMtx = BasicUtil.BasisVecToMtx(orientationX, orientationY, orientationZ);
    }

    private void PerformGroundedCast()
    {
        var startPos = groundCastTransform.position;
        var castDir = -groundCastTransform.up;

        isGrounded = false;
        
        if (Physics.SphereCast(
                startPos, 
                groundCastRadius, 
                castDir, 
                out RaycastHit hitInfo, 
                groundCastLength, 
                groundedMask))
        {
            groundNormal = hitInfo.normal.normalized;
            groundPos = hitInfo.point;
            float gravityAlignment = Vector3.Dot(groundNormal, GravityNormal);
            isGrounded = gravityAlignment >= movementProfile.minGroundedDot;
            if (isGrounded && groundObjectTransform != hitInfo.transform)
            {
                groundObjectTransform = hitInfo.transform;
                prevPos = groundObjectTransform.position;
            }
        }
    }

    public Vector3 ProjectOnOrienationGround(Vector3 vec)
    {
        return Vector3.ProjectOnPlane(vec, orientationY);
    }

    private void OnDrawGizmos()
    {
        // Draw grounded cast info
        Gizmos.color = Color.blue;
        var pos = groundCastTransform.position;
        var endPos = pos + groundCastLength * -groundCastTransform.transform.up;
        Gizmos.DrawLine(pos, endPos);
        Gizmos.DrawWireSphere(pos, groundCastRadius);
        Gizmos.DrawWireSphere(endPos, groundCastRadius);
        
        // Draw grounded hit info
        if (IsGrounded)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundPos, groundPos + groundNormal * 2f);
        }
        
        orientationMtx.DrawMtxGizmo(playerPhysicsBody.transform.position);
    }
}
