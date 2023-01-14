using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagRotationResolver : MonoBehaviour
{
    [ColorHeader("Dependencies")] 
    [SerializeField] private Transform targetPhysicsBody;
    [SerializeField] private ProtagPhysicsState physicsState;
    [SerializeField] private MovementProfileSO movementProfile;


    public void GroundedRotationResolve(float timeStep)
    {
        LerpToOrientation(movementProfile.groundedRightingSpeed * timeStep);
    }

    public void AirborneRotationResolve(float timeStep)
    {
        LerpToOrientation(movementProfile.groundedRightingSpeed * timeStep);
    }

    private void LerpToOrientation(float amount)
    {
        var cRot = targetPhysicsBody.rotation;
        var targetRot = physicsState.OrientationMtx.rotation;
        targetPhysicsBody.rotation = Quaternion.Lerp(cRot, targetRot, amount);
    }
    
}
