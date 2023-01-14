using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagRotationResolver : MonoBehaviour
{
    [ColorHeader("Dependencies")] 
    [SerializeField] private Transform targetPhysicsBody;
    [SerializeField] private ProtagPhysicsState physicsState;
    [SerializeField] private MovementProfileSO movementProfile;


    public void GroundedRotationResolve(float timeStep, float gravityAccelMag)
    {
        LerpToOrientation(movementProfile.groundedRightingSpeed * timeStep);
    }

    public void AirborneRotationResolve(float timeStep, float gravityAccelMag)
    {
        float t = Mathf.InverseLerp(
            movementProfile.airborneRightingGravityScaleRange.x,
            movementProfile.airborneRightingGravityScaleRange.y,
            gravityAccelMag);

        float rightingSpeed = movementProfile.airborneRightingSpeed *
                      movementProfile.airborneRightingGravityScaleCurve.Evaluate(t);
        
        LerpToOrientation(rightingSpeed * timeStep);
    }

    private void LerpToOrientation(float amount)
    {
        var cRot = targetPhysicsBody.rotation;
        var targetRot = physicsState.OrientationMtx.rotation;
        cRot = Quaternion.Lerp(cRot, targetRot, amount);
        targetPhysicsBody.rotation = Quaternion.RotateTowards(cRot, targetRot, amount * 10f);
    }
    
}
