using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagPhysicsState : MonoBehaviour
{
    [ColorHeader("Dependencies")] 
    [SerializeField] private GravityBody playerGravBody;
    [SerializeField] private FPCameraController cameraController;
    [SerializeField] private GameObject playerPhysicsBody;
    
    [ColorHeader("Grounded Cast Config")]
    [SerializeField] private LayerMask groundedMask;
    [SerializeField] private Transform groundCastTransform;
    [SerializeField, Range(0f, 5f)] private float groundCastLength;
    [SerializeField, Range(0f, 5f)] private float groundCastRadius;

    // Grounded info
    [SerializeField, ReadOnly] private bool isGrounded;
    public bool IsGrounded => isGrounded;
    [SerializeField, ReadOnly] private Vector3 groundPos;
    [SerializeField, ReadOnly] private Vector3 groundNormal;
    public Vector3 GroundNormal => groundNormal;
    public Vector3 GravityNormal => -playerGravBody.GravityDirection;

    // Basis Info
    private Vector3 orientationY;
    private Vector3 orientationX;
    private Vector3 orientationZ;
    private Matrix4x4 orientationMtx;

    public Matrix4x4 OrientationMtx => orientationMtx;

    private void FixedUpdate()
    {
        PerformGroundedCast();
        UpdateOrientationBasis();
    }

    // Generate a basis vector/matrix based on camera forward and current active normal
    private void UpdateOrientationBasis()
    {
        // Basis 'Up'
        Vector3 normal;
        if (isGrounded)
        {
            normal = groundNormal;
        }
        else
        {
            normal = -playerGravBody.GravityDirection;
        }

        Vector3 forward = cameraController.Forward;
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
            isGrounded = true;
            groundNormal = hitInfo.normal.normalized;
            groundPos = hitInfo.point;
        }
    }

    public Vector3 ProjectOntoOrienationGround(Vector3 vec)
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
