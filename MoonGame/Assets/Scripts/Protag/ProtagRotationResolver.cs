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
        var cRot = targetPhysicsBody.rotation;
        var targetRot = physicsState.OrientationMtx.rotation;
        targetPhysicsBody.rotation = Quaternion.Lerp(cRot, targetRot, movementProfile.groundedRightingSpeed * timeStep);
    }

    public void AirborneRotationResolve(float timeStep)
    {
        var cRot = targetPhysicsBody.rotation;
        var targetRot = physicsState.OrientationMtx.rotation;
        targetPhysicsBody.rotation = Quaternion.Lerp(cRot, targetRot, movementProfile.airborneRightingSpeed * timeStep);
    }
    
}
