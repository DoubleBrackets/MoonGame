using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/MovementProfile", fileName = "NewMovementProfile")]
public class MovementProfileSO : ScriptableObject
{
    [ColorHeader("Grounded Stats")]
    public float maxGroundedSpeed;
    public float groundedAcceleration;
    public float groundedFriction;

    [ColorHeader("Airborne Stats")] 
    public float jumpVelocity;

    [ColorHeader("Rotation Righting Stats")]
    public float groundedRightingSpeed;
    public float airborneRightingSpeed;
}
