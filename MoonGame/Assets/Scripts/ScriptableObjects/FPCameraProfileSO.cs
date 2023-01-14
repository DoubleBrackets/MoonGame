using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/FPCameraProfile", fileName = "New FPCamera Profile")]
public class FPCameraProfileSO : ScriptableObject
{
    [ColorHeader("Sensitivity")]
    public float horizontalSensitivity;
    public float verticalSensitivity;

    [ColorHeader("Damping")]
    public float horizontalLerpSpeed;
    public float verticalLerpSpeed;

    [ColorHeader("Angle clamping")]
    public float maxUpAngle;
    public float maxDownAngle;
}
