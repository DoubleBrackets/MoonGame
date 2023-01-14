using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagMover : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ProtagPhysicsState physicsState;
    [SerializeField] private MovementProfileSO movementProfile;
    [SerializeField] private Rigidbody rigidBody;

    public void GroundedMoveTowards(Vector2 inputDirection, float timeStep)
    {
        if (inputDirection == Vector2.zero) return;

        // We only want to move along the ground plane, so use projects to calculate how much to move
        Vector3 cVel = rigidBody.velocity;
        Vector3 groundedVel = physicsState.ProjectOnOrienationGround(cVel);
        Vector3 targetVel = physicsState.OrientationMtx.MultiplyVector(inputDirection.SwizzleXZ()).normalized * movementProfile.maxGroundedSpeed;

        Vector3 newVel = Vector3.MoveTowards(groundedVel, targetVel, timeStep * movementProfile.groundedAcceleration);

        Vector3 accel = newVel - groundedVel;

        rigidBody.velocity += accel;
    }

    public void GroundedFriction(float timeStep)
    {
        Vector3 cVel = rigidBody.velocity;
        Vector3 groundedVel = physicsState.ProjectOnOrienationGround(cVel);

        Vector3 newVel = Vector3.MoveTowards(groundedVel, Vector3.zero, timeStep * movementProfile.groundedFriction);

        Vector3 accel = newVel - groundedVel;

        rigidBody.velocity += accel;
    }

    public void GroundedSnap()
    {
        Vector3 vel = rigidBody.velocity;
        Vector3 velDir = vel.normalized;
        Vector3 normal = physicsState.GroundNormal;

        float dot = Vector3.Dot(velDir, normal);
        if (dot > 0.15f)
        {
            // snap
            rigidBody.velocity = physicsState.ProjectOnOrienationGround(vel);
        }
    }
    
    public void AirborneMoveTowards(Vector2 direction, Matrix4x4 dirTransform, float timeStep)
    {
        var newDir = dirTransform.MultiplyVector(direction.SwizzleXZ());
        rigidBody.velocity = newDir * movementProfile.maxGroundedSpeed * timeStep;
    }
    
    //private void Movement(Vector2 direction, Matrix4x4 transform, )

    public void Jump()
    {
        rigidBody.velocity += physicsState.GroundNormal * movementProfile.jumpVelocity;
    }
}
