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
    public float maxAirSpeed;
    public float airAcceleration;

    [ColorHeader("Jetpack Stats")] 
    public float jetpackAccel;
    public float jetpackMaxFuel;
    public float jetpackFuelRegen;

    [ColorHeader("Rotation Righting Stats")]
    public float groundedRightingSpeed;
    public float airborneRightingSpeed;
    [RangeSlider(0f, 100f)] public Vector2 airborneRightingGravityScaleRange;
    public AnimationCurve airborneRightingGravityScaleCurve;

}
