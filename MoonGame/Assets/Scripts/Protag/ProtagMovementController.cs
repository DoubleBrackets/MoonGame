using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagMovementController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ProtagPhysicsState physicsState;
    [SerializeField] private MovementProfileSO movementProfile;
    [SerializeField] private Rigidbody rigidBody;

    public void GroundedMoveTowards(Vector2 inputDirection, float timeStep)
    {
        if (inputDirection == Vector2.zero) return;
        SimpleHorizontalMovement(
            inputDirection, 
            timeStep * movementProfile.groundedAcceleration, 
            movementProfile.maxGroundedSpeed,
            timeStep * movementProfile.groundedFriction);
    }

    public void GroundedFriction(float timeStep)
    {
        Vector3 cVel = rigidBody.velocity;
        Vector3 groundedVel = physicsState.ProjectOnOrienationGround(cVel);

        Vector3 newVel = Vector3.MoveTowards(groundedVel, Vector3.zero, timeStep * movementProfile.groundedFriction);

        Vector3 accel = newVel - groundedVel;

        rigidBody.velocity += accel;
    }

    // Snap velocity to ground plane if its "lifting" (prevents launching over bumps)
    public void GroundedSnap()
    {
        Vector3 vel = rigidBody.velocity;
        Vector3 velDir = vel.normalized;
        Vector3 normal = physicsState.GroundNormal;

        float dot = Vector3.Dot(velDir, normal);
        if (dot > 0.15f)
        {
            // snap :D
            rigidBody.velocity = physicsState.ProjectOnOrienationGround(vel);
        }
    }
    
    public void AirborneMoveTowards(Vector2 inputDirection, float timeStep)
    {
        if (inputDirection == Vector2.zero) return;
        // Airborne movement doesn't slow down if over 
        SimpleHorizontalMovement(
            inputDirection, 
            movementProfile.airAcceleration * timeStep, 
            movementProfile.maxAirSpeed);
    }

    /// <summary>
    /// Simple horizontal movement along orientation plane
    /// </summary>
    /// <param name="inputDirection">Direction to move</param>
    /// <param name="accelerationStep">Max velocity change</param>
    /// <param name="frictionStep">If the current velocity is over the max speed then apply this to slow down</param>
    private void SimpleHorizontalMovement(Vector2 inputDirection, float accelerationStep, float maxSpeed, float frictionStep = 0f)
    {
        // We only want to move along the ground plane, so use projects to calculate how much to move
        Vector3 cVel = rigidBody.velocity;
        Vector3 groundedVel = physicsState.ProjectOnOrienationGround(cVel);
        Vector3 targetVel = physicsState.OrientationMtx.MultiplyVector(inputDirection.SwizzleXZ()).normalized * maxSpeed;

        Vector3 newVel;
        
        // Speed is over
        if (cVel.magnitude > maxSpeed)
        {
            newVel = Vector3.MoveTowards(groundedVel, targetVel, frictionStep);
        }
        else
        {
            newVel = Vector3.MoveTowards(groundedVel, targetVel, accelerationStep);
        }

        Vector3 accel = newVel - groundedVel;

        rigidBody.velocity += accel;
    }

    public void Jump()
    {
        rigidBody.velocity += physicsState.GroundNormal * movementProfile.jumpVelocity;
    }

    public void JetpackMovement(Vector3 direction, float timeStep)
    {
        rigidBody.velocity += direction * timeStep * movementProfile.jetpackAccel;
    }
}
